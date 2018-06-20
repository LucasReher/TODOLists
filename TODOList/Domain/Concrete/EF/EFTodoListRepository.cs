using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TODOList.Domain.Abstract;
using TODOList.Domain.Entity;

namespace TODOList.Domain.Concrete.EF {
    public class EFTodoListRepository : AbstractTodoListRepository {
        private DBContext _context = new DBContext();

        public override async Task DeleteAsync(long id) {
            await DeleteAsync(await GetAsync(id));
        }

        public override async Task DeleteAsync(TodoList entity) {
            _context.TodoLists.Remove(entity);
            await _context.SaveChangesAsync();
        }
        public override async Task<ICollection<TodoList>> GetAllAsync() {
            return await _context.TodoLists.ToListAsync();
        }

        public override async Task<TodoList> GetAsync(long id) {
            return await _context.TodoLists.FindAsync(id);
        }

        public override async Task SaveAsync(TodoList entity) {
            if (entity.ListID.HasValue) {
                TodoList inDatabaseTodoList = await _context.TodoLists.FindAsync(entity);
                inDatabaseTodoList.LoadFrom(entity);
            } else {
                _context.TodoLists.Add(entity);
            }
            await _context.SaveChangesAsync();
        }
    }
}