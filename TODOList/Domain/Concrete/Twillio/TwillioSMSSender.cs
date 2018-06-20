using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TODOList.Domain.Abstract;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace TODOList.Domain.Concrete.Twillio {
    public class TwillioSMSSender : AbstractSMSSender {
        private string _accountSID;
        private string _authToken;
        private string _outgoingNumber;
        private PhoneNumber _outgoingPhone;
        private readonly AbstractSMSRepository _smsRepository;

        public TwillioSMSSender(AbstractSMSRepository smsRepository) {
            _smsRepository = smsRepository;

            _accountSID = ConfigurationManager.AppSettings["Twilio:AccountSID"];
            _authToken = ConfigurationManager.AppSettings["Twilio:AuthToken"];
            _outgoingNumber = ConfigurationManager.AppSettings["Twilio:OutgoingNumber"];

            _outgoingPhone = new PhoneNumber(_outgoingNumber);

            TwilioClient.Init(_accountSID, _authToken);
        }

        public override async Task SendSMSAsync(string number, string message) {
            PhoneNumber to = new PhoneNumber(number);
            MessageResource messageResource = MessageResource.Create(
                to: to,
                from: _outgoingPhone,
                body: message);

            await _smsRepository.SaveAsync(new Entity.SMS {
                Message = message,
                NumberFrom = _outgoingNumber,
                NumberTo = number
            });
        }
    }
}