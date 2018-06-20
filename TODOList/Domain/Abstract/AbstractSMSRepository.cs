using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TODOList.Domain.Entity;

namespace TODOList.Domain.Abstract {
    public abstract class AbstractSMSRepository :AbstractRepository<SMS> {
        public abstract Task<IList<SMS>> GetAllAsync();
        
    }
}