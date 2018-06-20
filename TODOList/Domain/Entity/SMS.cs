using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TODOList.Domain.Entity {
    public class SMS {
        public long? SMSID { get; set; }
        public string NumberTo { get; set; }
        public string NumberFrom { get; set; }
        public string Message { get; set; }

        public void LoadFrom(SMS other) {
            NumberTo = other.NumberTo;
            NumberFrom = other.NumberFrom;
            Message = other.Message;
        }
    }
}