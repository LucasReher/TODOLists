using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TODOList.Domain.Abstract;
using TODOList.Domain.Entity;

namespace TODOList.Domain.Concrete.EF {
    public class EFTodoTaskRepository : AbstractTodoTaskRepository {
        private DBContext _context = new DBContext();
        public override async Task DeleteAsync(TodoTask entity) {
            _context.TodoTasks.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public override async Task DeleteAsync(long id) {
            await DeleteAsync(await GetAsync(id));
        }

        public override async Task<ICollection<TodoTask>> GetAllAsync() {
            return await _context.TodoTasks.ToListAsync();
        }

        public override async Task<TodoTask> GetAsync(long id) {
            return await _context.TodoTasks.FindAsync(id);
        }

        public override async Task SaveAsync(TodoTask entity) {
            if (entity.TaskID.HasValue) {
                TodoTask inDatabaseTodoTask = await _context.TodoTasks.FindAsync(entity);
                inDatabaseTodoTask.LoadFrom(entity);
            } else {
                _context.TodoTasks.Add(entity);
            }
            await _context.SaveChangesAsync();
        }
    }
}