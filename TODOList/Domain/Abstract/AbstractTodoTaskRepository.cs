using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TODOList.Domain.Entity;

namespace TODOList.Domain.Abstract {
    public abstract class AbstractTodoTaskRepository : AbstractRepository<TodoTask> {
        public abstract Task<ICollection<TodoTask>> GetAllAsync();
    }
}   