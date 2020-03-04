using System.Linq;
using System.Web.Mvc;
using EPiServer.Commerce.Order;
using EPiServer.Editor;
using EPiServer.Reference.Commerce.Site.Features.Cart.Services;
using EPiServer.Reference.Commerce.Site.Features.Checkout.Services;
using LL.EpiserverCyberSourceConnector.Payments.PayPal;

namespace EPiServer.Reference.Commerce.Site.Features.Checkout.Controllers
{
    public class PayPalPaymentController : Controller
    {
        private ICart _cart;
        private readonly ICartService _cartService;
        private readonly IOrderRepository _orderRepository;
        private readonly CheckoutService _checkoutService;
        private readonly IPayPalConfiguration _payPalConfiguration;

        private ICart Cart => _cart ?? (_cart = _cartService.LoadCart(_cartService.DefaultCartName));

        public PayPalPaymentController(ICartService cartService, IOrderRepository orderRepository,
            CheckoutService checkoutService, IPayPalConfiguration payPalConfiguration)
        {
            _cartService = cartService;
            _orderRepository = orderRepository;
            _checkoutService = checkoutService;
            _payPalConfiguration = payPalConfiguration;
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

                string paymentRedirectUrl;
                var purchaseOrder = _checkoutService.PlaceOrder(Cart, ModelState, out paymentRedirectUrl);
                if (purchaseOrder != null)
                {
                    return Redirect(_checkoutService.BuildRedirectionUrl(purchaseOrder, Cart.GetFirstShipment().ShippingAddress.Email, true));
                }
            }

            string currentLanguage = Globalization.GlobalizationSettings.UICultureLanguageCode;
            string cancelUrl = _payPalConfiguration.CancelUrl;
            if (!cancelUrl.StartsWith("/"))
            {
                cancelUrl = "/" + cancelUrl;
            }

            string cancelRedirectUrl = $"/{currentLanguage}{cancelUrl}";

            return Redirect(cancelRedirectUrl);
        }
    }
}