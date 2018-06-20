using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TODOList.Domain.Entity.Identity;

namespace TODOList.Domain.Entity {
    public class TodoTask {
        public long? TaskID { get; set; }
        public string TaskName { get; set; }

        public long? ListID { get; set; }
        public virtual TodoList ParentList { get; set; }

        public string Description { get; set; }
        public string Colour { get; set; }

        public void LoadFrom(TodoTask other) {
            this.ListID = other.ListID;
            this.TaskName = other.TaskName;
            this.Description = other.Description;
            this.Colour = other.Colour;
        }
    }
}