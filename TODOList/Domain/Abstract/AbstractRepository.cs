using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TODOList.Domain.Abstract {
    public abstract class AbstractRepository <TType, TKey> {
        public abstract Task SaveAsync(TType type);
        public abstract Task<TType> GetAsync(TKey id);
        public abstract Task DeleteAsync(TKey id);
        public abstract Task DeleteAsync(TType type);
    }

    public abstract class AbstractRepository<TType> : AbstractRepository<TType, long> { }
}