using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TODOList.Domain.Abstract;
using TODOList.Domain.Entity;

namespace TODOList.Domain.Concrete.EF {
    public class EFSMSRepository : AbstractSMSRepository {
        private readonly DBContext _context = DBContext.Create();

        public override async Task SaveAsync(SMS entity) {
            if (entity.SMSID.HasValue) {
                // update
                SMS inDatabaseSMS = await _context.SMS.FindAsync(entity.SMSID);
                inDatabaseSMS.LoadFrom(entity);
            } else {
                // create
                _context.SMS.Add(entity);
            }

            await _context.SaveChangesAsync();
        }

        public override async Task<SMS> GetAsync(long id) {
            return await _context.SMS.FindAsync(id);
        }

        public override async Task DeleteAsync(long id) {
            await DeleteAsync(await GetAsync(id));
        }

        public override async Task DeleteAsync(SMS entity) {
            _context.SMS.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public override async Task<IList<SMS>> GetAllAsync() {
            return await _context.SMS.ToListAsync();
        }
    }
}

