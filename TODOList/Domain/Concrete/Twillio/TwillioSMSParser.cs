using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using TODOList.Domain.Abstract;
using TODOList.Domain.Concrete.Owin;
using TODOList.Domain.Entity;
using TODOList.Domain.Entity.Identity;

namespace TODOList.Domain.Concrete.Twillio {
    public static class SMSParsingExtensions {
        public static bool IsReminderIntent(this string message, out string reminder) {
            string msg = message.ToLowerInvariant().Trim();
            string rawReminder;
            if (msg.RemoveIfStartsWith(out rawReminder, "remind me")) {
                if (rawReminder.RemoveIfStartsWith(out rawReminder, "to", "that")) {
                    reminder = rawReminder.CapitalizeFirstLetter();
                    return true;
                }
                reminder = rawReminder.CapitalizeFirstLetter();
                return true;
            }
            reminder = message;
            return false;
        }

        public static bool IsLookupIntent(this string message) {
            string _;
            return message.ToLowerInvariant().RemoveIfStartsWith(out _, "my reminder", "reminder", "what are my reminder");
        }

        public static bool IsHelpIntent(this string message) {
            string _;
            return message.ToLowerInvariant().RemoveIfStartsWith(out _, "how", "how to", "how do");
        }

        public static bool IsResetPasswordIntent(this string message) {
            string _;
            return message.ToLowerInvariant().RemoveIfStartsWith(out _, "reset password");
        }

        public static bool RemoveIfStartsWith(this string raw, out string processed, params string[] toRemove) {
            foreach (string remove in toRemove) {
                if (raw.StartsWith(remove)) {
                    if (raw.Length == remove.Length) {
                        processed = raw;
                    } else {
                        processed = raw.Substring(remove.Length + 1);
                    }

                    return true;
                }
            }

            processed = raw;
            return false;
        }

        public static string CapitalizeFirstLetter(this string input) {
            return $"{input.Substring(0, 1).ToUpperInvariant()}{input.Substring(1)}";
        }
    }
    public class TwillioSMSParser : AbstractSMSParser {
        private readonly AbstractSMSSender _smsSender;
        private readonly UserManager<ApplicationUser, long> _userManager;
        private readonly AbstractSMSRepository _smsRepository;
        private readonly AbstractTodoListRepository _todoListRepository;
        private readonly AbstractTodoTaskRepository _todoTaskRepository;

        public static string SimplifyNumber(string number) {
            string simplifiedNumber = number.Replace(" ", "").Replace("+", "");
            if (simplifiedNumber.StartsWith("61")) {
                simplifiedNumber = simplifiedNumber.Substring(2);
            }
            return simplifiedNumber;
        }

        public override async Task ProcessSMSAsync(string number, string message) {

            try {
                await _smsRepository.SaveAsync(new Entity.SMS() {
                    Message = message,
                    NumberFrom = number,
                    NumberTo = ConfigurationManager.AppSettings["Twilio:OutgoingNumber"]
                });

                string simplifiedNumber = SimplifyNumber(number);
                ApplicationUser user = await _userManager.FindByNameAsync(simplifiedNumber);

                if (user == null) {
                    string password = Guid.NewGuid().ToString().Substring(0, 6).ToLowerInvariant();
                    IdentityResult ir = await _userManager.CreateAsync(new ApplicationUser() {
                        UserName = simplifiedNumber,
                        PhoneNumber = number,
                        PhoneNumberConfirmed = true,
                        EmailConfirmed = true,
                        Email = $"{simplifiedNumber}@foreverly.cloud",
                        SecurityStamp = $"{Guid.NewGuid():N}"
                    }, password);

                    await _smsSender.SendSMSAsync(number,
                        $"Your account on foreverly.cloud has been created!\r\nUsername: {simplifiedNumber}\r\nPassword: {password}");
                    user = await _userManager.FindByNameAsync(simplifiedNumber);
                }

                string reminder = null;
                if (message.IsReminderIntent(out reminder)) {
                    TodoList tl = user.TodoLists?.FirstOrDefault(x => x.ListName.Equals("Reminders"));

                    if (tl == null) {
                        tl = new TodoList {
                            ListName = "Reminders",
                            DateCreated = DateTime.Now,
                            UserID = user.Id,
                            LeftPositoned = true
                        };
                        await _todoListRepository.SaveAsync(tl);
                    }

                    TodoTask tt = new TodoTask {
                        TaskName = $"Reminder {(tl.TodoTasks?.Count ?? 0) + 1}",
                        Description = reminder,
                        Colour = "black",
                        ListID = tl.ListID
                    };

                    await _todoTaskRepository.SaveAsync(tt);

                    await _smsSender.SendSMSAsync(number, $"We've created your reminder under {tt.TaskName}");
                } else if (message.IsLookupIntent()) {
                    TodoList tl = user.TodoLists?.FirstOrDefault(x => x.ListName.Equals("Reminders"));
                    int remindersCount = tl?.TodoTasks?.Count ?? 0;
                    if (remindersCount == 0) {
                        await _smsSender.SendSMSAsync(number, "You have no reminders");
                    } else {
                        int remindersToSend = Math.Min(remindersCount, 5);

                        if (remindersToSend == 1) {
                            await _smsSender.SendSMSAsync(number, $"Your last reminder was:");
                        } else {
                            await _smsSender.SendSMSAsync(number, $"Your last {remindersToSend} reminders were:");
                        }

                        IEnumerable<TodoTask> localReminders =
                                tl.TodoTasks.OrderByDescending(x => x.TaskID).Take(remindersToSend);
                        foreach (TodoTask tt in localReminders) {
                            await _smsSender.SendSMSAsync(number, $"{tt.TaskName}: {tt.Description}");
                        }

                    }
                } else if (message.IsHelpIntent()) {
                    await _smsSender.SendSMSAsync(number, $"Say \"remind me to ...\" to set a reminder and \"reminders\" to get your last 5 reminders");
                } else if (message.IsResetPasswordIntent()) {
                    string password = Guid.NewGuid().ToString().Substring(0, 6).ToLowerInvariant();
                    
                    await ((ApplicationUserManager)_userManager).ChangePasswordAsync(user.Id, password);

                    await _smsSender.SendSMSAsync(number, $"Your new account details\r\nUsername: {user.UserName}\r\nPassword: {password}");
                } else {
                    await _smsSender.SendSMSAsync(number, $"I'm sorry, I don't understand that, type \"how ...\" for valid commands");
                }
            } catch {
                await _smsSender.SendSMSAsync(number, $"Something went wrong, we were unable to complete your action");
            }

        }

        public TwillioSMSParser(AbstractSMSSender smsSender,
            UserManager<ApplicationUser, long> userManager,
            AbstractSMSRepository smsRepository,
            AbstractTodoListRepository todoListRepository,
            AbstractTodoTaskRepository todoTaskRepository) {
            _smsSender = smsSender;
            _userManager = userManager;
            _smsRepository = smsRepository;
            _todoListRepository = todoListRepository;
            _todoTaskRepository = todoTaskRepository;
        }
    }
}