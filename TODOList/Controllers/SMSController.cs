using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TODOList.Domain.Abstract;

namespace TODOList.Controllers
{
    public class SMSController : Controller
    {
        private readonly AbstractSMSRepository _smsRepository;
        private readonly AbstractSMSParser _smsParser;

        public SMSController(AbstractSMSRepository smsRepository, AbstractSMSParser smsParser) {
            _smsRepository = smsRepository;
            _smsParser = smsParser;
        }

        // GET: SMS
        public async Task<ActionResult> Index() {
            return View(await _smsRepository.GetAllAsync());
        }

        public async Task<ActionResult> TestSend(string phone, string message) {
            await _smsParser.ProcessSMSAsync(phone, message);
            return RedirectToAction("Index");
        }

        [AllowAnonymous, HttpPost]
        public async Task<ActionResult> Recieve() {
            string fromNumber = Request.Form["From"];
            string body = Request.Form["Body"];

            if (fromNumber == null) {
                return null;
            }

            await _smsParser.ProcessSMSAsync(fromNumber, body);
            return null;
        }
    }
}