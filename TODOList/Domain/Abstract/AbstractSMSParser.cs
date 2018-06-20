using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TODOList.Domain.Abstract {
    public abstract class AbstractSMSParser {
        public abstract Task ProcessSMSAsync(string number, string message);
    }
}