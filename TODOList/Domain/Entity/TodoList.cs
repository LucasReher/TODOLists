using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TODOList.Domain.Entity.Identity;

namespace TODOList.Domain.Entity {
    public class TodoList {
        public long? ListID { get; set; }
        public string ListName { get; set; }
        public DateTime DateCreated { get; set; }
        //Determines if located on left hand side of application
        public bool LeftPositoned { get; set; }

        public long UserID { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual IList<TodoTask> TodoTasks { get; set; }

        public void LoadFrom(TodoList other) {
            this.UserID = other.UserID;
            this.ListName = other.ListName;
            this.DateCreated = other.DateCreated;
            this.LeftPositoned = other.LeftPositoned;
            this.TodoTasks = new List<TodoTask>(other.TodoTasks);
        }
    }
}