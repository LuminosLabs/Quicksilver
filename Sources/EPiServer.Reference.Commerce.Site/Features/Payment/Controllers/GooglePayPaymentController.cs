using EPiServer.Commerce.Order;
using EPiServer.Reference.Commerce.Site.Features.Cart.Services;
using System.Linq;
using System.Web.Mvc;
using LL.EpiserverCyberSourceConnector.Core.Payments.GooglePay;

namespace EPiServer.Reference.Commerce.Site.Features.Payment.Controllers
{
    public class GooglePayPaymentController : Controller
    {
        private ICart _cart;
        private readonly ICartService _cartService;
        private readonly IOrderRepository _orderRepository;
        private ICart Cart => _cart ?? (_cart = _cartService.LoadCart(_cartService.DefaultCartName));

        public GooglePayPaymentController(ICartService cartService, IOrderRepository orderRepository)
        {
            _cartService = cartService;
            _orderRepository = orderRepository;
        }

        [HttpPost]
        public ActionResult ProcessGooglePayPayment(string paymentData)
        {
            if (string.IsNullOrEmpty(paymentData))
            {
                return Json(false);
            }

            var payment = Cart.GetFirstForm().Payments.FirstOrDefault();
            var googlePayPayment = (ICyberSourceGooglePayPayment)payment;
            if (googlePayPayment != null)
            {
                googlePayPayment.CyberSourceGooglePayData = paymentData;

                _orderRepository.Save(Cart);
                return Json(true);
            }

            return Json(false);
        }
    }
}