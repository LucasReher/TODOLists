using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TODOList.Domain.Abstract;
using TODOList.Domain.Entity;

namespace TODOList.Domain.Concrete.Twillio {
    public class TestSMSSender : AbstractSMSSender {
        private readonly AbstractSMSRepository _smsRepository;

        public override async Task SendSMSAsync(string number, string message) {
            SMS sms = new SMS {
                NumberTo = number,
                Message = message,
                NumberFrom = ConfigurationManager.AppSettings["Twilio:OutgoingNumber"]
            };
            await _smsRepository.SaveAsync(sms);
        }

        public TestSMSSender(AbstractSMSRepository smsRepository) {
            _smsRepository = smsRepository;
        }

    }
}