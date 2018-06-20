using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TODOList.Domain.Abstract {
    public abstract class AbstractSMSSender {
        public abstract Task SendSMSAsync(string number, string message);
    }
}