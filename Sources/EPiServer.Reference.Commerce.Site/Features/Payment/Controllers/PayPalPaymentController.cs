using System.Linq;
using System.Web.Mvc;
using EPiServer.Commerce.Order;
using EPiServer.Editor;
using EPiServer.Reference.Commerce.Site.Features.Cart.Services;
using LL.EpiserverCyberSourceConnector.Payments.PayPal;

namespace EPiServer.Reference.Commerce.Site.Features.Payment.Controllers
{
    public class PayPalPaymentController : Controller
    {
        private ICart _cart;
        private readonly ICartService _cartService;
        private readonly IOrderRepository _orderRepository;

        private ICart Cart => _cart ?? (_cart = _cartService.LoadCart(_cartService.DefaultCartName));

        public PayPalPaymentController(ICartService cartService, IOrderRepository orderRepository)
        {
            _cartService = cartService;
            _orderRepository = orderRepository;
        }

        public ActionResult Purchase(string payerId)
        {
            if (PageEditing.PageIsInEditMode)
            {
                return new EmptyResult();
            }

            var payment = Cart.GetFirstForm().Payments.FirstOrDefault();
            var payPalPayment = payment as ICyberSourcePayPalPayment;

            if (payPalPayment != null && !string.IsNullOrEmpty(payerId))
            {
                payPalPayment.CyberSourcePayPalPayerId = payerId;
                _orderRepository.Save(Cart);
            }

            return RedirectToAction("PlaceOrder", "Checkout");
        }
    }
}