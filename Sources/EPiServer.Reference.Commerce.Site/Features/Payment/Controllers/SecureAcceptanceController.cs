using System.Collections.Generic;
using System.Web.Mvc;
using LL.EpiserverCyberSourceConnector.Site.Payments.CreditCard;

namespace EPiServer.Reference.Commerce.Site.Features.Payment.Controllers
{
    public class SecureAcceptanceController : Controller
    {
        private readonly SecureAcceptanceSecurity _secureAcceptanceSecurity;

        public SecureAcceptanceController(SecureAcceptanceSecurity secureAcceptanceSecurity)
        {
            _secureAcceptanceSecurity = secureAcceptanceSecurity;
        }

        [HttpPost]
        public ActionResult SignFields(IDictionary<string, string> fields)
        {
            var result = _secureAcceptanceSecurity.Sign(fields);

            return Json(new
            {
                result
            });
        }
    }
}