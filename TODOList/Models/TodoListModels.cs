using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TODOList.Models {

    public class TodoListsModel {
        public string NewListName { get; set; }
        public bool NewListPositon { get; set; }
        public List<TodoListModel> TodoLists { get; set; }
    }

    public class TodoListModel {
        public long? ListID { get; set; }
        public string ListName { get; set; }
        public bool LeftPositioned { get; set;}
        public DateTime DateCreated { get; set; }
        public List<TodoTaskModel> TodoTasks { get; set; }
    }

    public class TodoTaskModel {
        public long? TaskID { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public string Colour { get; set; }
    }

    public class CreateTaskModel {
        public long ListID { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
    }

}