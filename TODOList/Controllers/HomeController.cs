using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TODOList.Domain.Abstract;
using TODOList.Domain.Entity;
using Microsoft.AspNet.Identity;
using Ninject;
using TODOList.Models;

namespace TODOList.Controllers {
    public class HomeController : Controller {
        private AbstractTodoListRepository _todoListRepository;
        private AbstractTodoTaskRepository _todoTaskRepository;

        public HomeController(AbstractTodoListRepository todoListRepository, AbstractTodoTaskRepository todoTaskRepository) {
            _todoListRepository = todoListRepository;
            _todoTaskRepository = todoTaskRepository;
        }

        [HttpGet]
        public async Task<ActionResult> Index() {
            if (!User.Identity.IsAuthenticated) {
                return RedirectToAction("Login", "Account");
            }

            var UserID = User.Identity.GetUserId<long>();
            ICollection<TodoList> lists = await _todoListRepository.GetAllAsync();
            ICollection<TodoTask> tasks = await _todoTaskRepository.GetAllAsync();

            TodoListsModel listsModel = new TodoListsModel();
            listsModel.TodoLists = new List<TodoListModel>();

            foreach (TodoList list in lists) {
                if (list.UserID == UserID) {
                    TodoListModel listModel = new TodoListModel {
                        ListID = list.ListID,
                        ListName = list.ListName,
                        LeftPositioned = list.LeftPositoned,
                        DateCreated = list.DateCreated,
                        TodoTasks = new List<TodoTaskModel> { }
                    };
                    foreach (TodoTask task in tasks) {
                        if (task.ListID == list.ListID) {
                            TodoTaskModel taskModel = new TodoTaskModel {
                                TaskID = task.TaskID,
                                TaskName = task.TaskName,
                                Description = task.Description,
                                Colour = task.Colour
                            };
                            listModel.TodoTasks.Add(taskModel);
                        }
                    }
                    listsModel.TodoLists.Add(listModel);
                }
            }

            return View(listsModel);
        }

        [HttpPost]
        public async Task<ActionResult> CreateList(TodoListsModel model) {
            var UserID = User.Identity.GetUserId<long>();

            TodoList newTodoList = new TodoList {
                UserID = UserID,
                ListName = model.NewListName,
                DateCreated = DateTime.Now,
                LeftPositoned = model.NewListPositon,
                TodoTasks = new List<TodoTask> { }
            };

            await _todoListRepository.SaveAsync(newTodoList);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> CreateTask(CreateTaskModel data) {

            TodoTask todoTask = new TodoTask {
                ListID = data.ListID,
                TaskName = data.TaskName,
                Description = data.Description,
                Colour = "black",
            };

            await _todoTaskRepository.SaveAsync(todoTask);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteList(long listID) {
            await _todoListRepository.DeleteAsync(listID);
            return Json(new { deleted = true });
        }

        public async Task<ActionResult> DeleteTask(long taskID) {
            await _todoTaskRepository.DeleteAsync(taskID);
            return Json(new {deleted = true});
        }
    }
}