using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using TODOList.Domain.Concrete.EF;
using TODOList.Domain.Entity;
using TODOList.Domain.Entity.Identity;

namespace TODOList.Migrations {

    internal sealed class Configuration : DbMigrationsConfiguration<TODOList.Domain.Concrete.EF.DBContext> {
        public bool doSeed = true;
        public Configuration() {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DBContext c) {
            if (doSeed) {
                ApplicationUser appUser = new ApplicationUser() {
                    AccessFailedCount = 0,
                    Email = "lucas.reher@gmail.com",
                    UserName = "lucas.reher@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumber = "0426825175",
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false,
                    PasswordHash = new PasswordHasher().HashPassword("mypassword"),
                    SecurityStamp = $"{Guid.NewGuid():N}",
                };

                c.Users.Add(appUser);
                c.SaveChanges();

                TodoList shoppingList = new TodoList {
                    ListName = "My Groceries",
                    UserID = appUser.Id,
                    LeftPositoned = true,
                    DateCreated = DateTime.Now,   
                };

                TodoList exerciseList = new TodoList {
                    ListName = "Workout List",
                    UserID = appUser.Id,
                    LeftPositoned = true,
                    DateCreated = DateTime.Now,
                };

                TodoList placesToVisitList = new TodoList {
                    ListName = "Places to Visit",
                    UserID = appUser.Id,
                    LeftPositoned = false,
                    DateCreated = DateTime.Now,
                };

                c.TodoLists.Add(shoppingList);
                c.TodoLists.Add(exerciseList);
                c.TodoLists.Add(placesToVisitList);
                c.SaveChanges();

                TodoTask buyLettuce = new TodoTask {
                    TaskName = "Buy Lettuce",
                    Description = "I really need to buy some lettuce tomorow's party",
                    ListID = shoppingList.ListID,
                    Colour = "green",
                };

                TodoTask buyCarrots = new TodoTask {
                    TaskName = "Buy Carrots",
                    Description = "I want to make that delicious carrot cake",
                    ListID = shoppingList.ListID,
                    Colour = "orange",
                };

                TodoTask workoutLegs = new TodoTask {
                    TaskName = "Workout Legs",
                    Description = "Never skip leg day.",
                    ListID = exerciseList.ListID,
                    Colour = "blue",
                };

                TodoTask morrocanPlace = new TodoTask {
                    TaskName = "Morrocan Place",
                    Description = "That place you went last week next to the dominos.",
                    ListID = placesToVisitList.ListID,
                    Colour = "grey",
                };

                TodoTask iceSkatingRing = new TodoTask {
                    TaskName = "Ice Skating Place",
                    Description = "",
                    ListID = placesToVisitList.ListID,
                    Colour = "grey",
                };

                TodoTask bookShop = new TodoTask {
                    TaskName = "That Book Store",
                    Description = "That book shop at toowong",
                    ListID = placesToVisitList.ListID,
                    Colour = "grey",
                };

                c.TodoTasks.Add(buyLettuce);
                c.TodoTasks.Add(buyCarrots);

                c.TodoTasks.Add(workoutLegs);

                c.TodoTasks.Add(morrocanPlace);
                c.TodoTasks.Add(iceSkatingRing);
                c.TodoTasks.Add(bookShop);
                c.SaveChanges();
            }
        }
    }
}