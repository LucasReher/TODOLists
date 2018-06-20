using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using TODOList.Domain.Concrete.EF;
using TODOList.Domain.Entity.Identity;

namespace TODOList.Domain.Concrete.Owin {
    public class ApplicationUserManager : UserManager<ApplicationUser, long> {
        private readonly DBContext _ctx = DBContext.Create();
        public ApplicationUserManager(IUserStore<ApplicationUser, long> store) : base(store) {
         
        }

        public async Task<IdentityResult> ChangePasswordAsync(long userId, string newPassword) {
            ApplicationUser au = _ctx.Users.Find(userId);
            au.PasswordHash = new PasswordHasher().HashPassword(newPassword);
            await _ctx.SaveChangesAsync();

            return IdentityResult.Success;
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context) {
            ApplicationUserManager manager = new ApplicationUserManager(new UserStore<ApplicationUser, ApplicationRole, long, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>(context.Get<DBContext>()));

            // Validation Logic for Usernames
            manager.UserValidator = new UserValidator<ApplicationUser, long>(manager) {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Validation for Passwords
            manager.PasswordValidator = new PasswordValidator {
                RequiredLength = 5,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // User lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            return manager;
        }
    }
}