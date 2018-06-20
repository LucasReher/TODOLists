using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using TODOList.Domain.Entity;
using TODOList.Domain.Entity.Identity;

namespace TODOList.Domain.Concrete.EF {
    public class DBContext : IdentityDbContext<ApplicationUser, ApplicationRole, long, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim> {
        public DBContext() : base("DBContextRemote") { }
        public DbSet<TodoList> TodoLists { get; set; }
        public DbSet<TodoTask> TodoTasks { get; set; }
        public DbSet<SMS> SMS { get; set; }

        public static DBContext Create() {
            return new DBContext();
        }

        protected override void OnModelCreating(DbModelBuilder mb) {
            base.OnModelCreating(mb);

            mb.Conventions.Remove<PluralizingTableNameConvention>();
            mb.Conventions.Remove<PluralizingEntitySetNameConvention>();

            mb.Entity<ApplicationUser>().HasMany(au => au.TodoLists).WithRequired(tl => tl.User)
                .HasForeignKey(tl => tl.UserID);

            mb.Entity<TodoList>().HasKey(tl => tl.ListID);
            mb.Entity<TodoList>().HasMany(tl => tl.TodoTasks).WithRequired(tt => tt.ParentList)
                .HasForeignKey(tt => tt.ListID);
            mb.Entity<TodoList>().HasRequired(tl => tl.User).WithMany(au => au.TodoLists)
                .HasForeignKey(tl => tl.UserID);

            mb.Entity<TodoTask>().HasKey(tt => tt.TaskID);
            mb.Entity<TodoTask>().HasRequired(tt => tt.ParentList).WithMany(tl => tl.TodoTasks)
                .HasForeignKey(tt => tt.ListID);

            mb.Entity<SMS>().HasKey(sms => sms.SMSID);
        }
    }
}