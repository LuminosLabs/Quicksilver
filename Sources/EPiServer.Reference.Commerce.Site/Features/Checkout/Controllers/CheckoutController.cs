using EPiServer.Commerce.Order;
using EPiServer.Core;
using EPiServer.Data;
using EPiServer.Reference.Commerce.Site.Features.Cart.Services;
using EPiServer.Reference.Commerce.Site.Features.Checkout.Pages;
using EPiServer.Reference.Commerce.Site.Features.Checkout.Services;
using EPiServer.Reference.Commerce.Site.Features.Checkout.ViewModelFactories;
using EPiServer.Reference.Commerce.Site.Features.Checkout.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.Market.Services;
using EPiServer.Reference.Commerce.Site.Features.Recommendations.Services;
using EPiServer.Reference.Commerce.Site.Features.Shared.Services;
using EPiServer.Reference.Commerce.Site.Infrastructure.Attributes;
using EPiServer.Web.Mvc;
using EPiServer.Web.Mvc.Html;
using EPiServer.Web.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EPiServer.Reference.Commerce.Site.Features.AddressBook.Services;
using EPiServer.Reference.Commerce.Site.Features.Start.Pages;
using EPiServer.ServiceLocation;
using LL.EpiserverCyberSourceConnector.Core.Models;
using LL.EpiserverCyberSourceConnector.Core.Payments;
using LL.EpiserverCyberSourceConnector.Core.Payments.CreditCard;
using LL.EpiserverCyberSourceConnector.Site.Payments.CreditCard;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace EPiServer.Reference.Commerce.Site.Features.Checkout.Controllers
{
    public class CheckoutController : PageController<CheckoutPage>
    {
        private readonly ICurrencyService _currencyService;
        private readonly ControllerExceptionHandler _controllerExceptionHandler;
        private readonly CheckoutViewModelFactory _checkoutViewModelFactory;
        private readonly OrderSummaryViewModelFactory _orderSummaryViewModelFactory;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartService _cartService;
        private readonly IRecommendationService _recommendationService;
        private readonly OrderValidationService _orderValidationService;
        private ICart _cart;
        private readonly CheckoutService _checkoutService;
        private readonly IDatabaseMode _databaseMode;
        private readonly SecureAcceptanceSecurity _secureAcceptanceSecurity;
        private readonly IAddressBookService _addressBookService;

        public CheckoutController(
            ICurrencyService currencyService,
            ControllerExceptionHandler controllerExceptionHandler,
            IOrderRepository orderRepository,
            CheckoutViewModelFactory checkoutViewModelFactory,
            ICartService cartService,
            OrderSummaryViewModelFactory orderSummaryViewModelFactory,
            IRecommendationService recommendationService,
            CheckoutService checkoutService,
            OrderValidationService orderValidationService,
            IDatabaseMode databaseMode,
            SecureAcceptanceSecurity secureAcceptanceSecurity,
            IAddressBookService addressBookService)
        {
            _currencyService = currencyService;
            _controllerExceptionHandler = controllerExceptionHandler;
            _orderRepository = orderRepository;
            _checkoutViewModelFactory = checkoutViewModelFactory;
            _cartService = cartService;
            _orderSummaryViewModelFactory = orderSummaryViewModelFactory;
            _recommendationService = recommendationService;
            _checkoutService = checkoutService;
            _orderValidationService = orderValidationService;
            _databaseMode = databaseMode;
            _secureAcceptanceSecurity = secureAcceptanceSecurity;
            _addressBookService = addressBookService;
        }

        [HttpGet]
        [OutputCache(Duration = 0, NoStore = true)]
        public async Task<ActionResult> Index(CheckoutPage currentPage)
        {
            if (CartIsNullOrEmpty())
            {
                return View("EmptyCart");
            }

            var viewModel = CreateCheckoutViewModel(currentPage);

            Cart.Currency = _currencyService.GetCurrentCurrency();

            _checkoutService.UpdateShippingAddresses(Cart, viewModel);
            _checkoutService.UpdateShippingMethods(Cart, viewModel.Shipments);

            _cartService.ApplyDiscounts(Cart);
            _orderRepository.Save(Cart);

            await _recommendationService.TrackCheckoutAsync(HttpContext);

            _checkoutService.ProcessPaymentCancel(viewModel, TempData, ControllerContext);

            return View(viewModel.ViewName, viewModel);
        }

        [HttpGet]
        public ActionResult SingleShipment(CheckoutPage currentPage)
        {
            if (!CartIsNullOrEmpty())
            {
                _cartService.MergeShipments(Cart);
                _orderRepository.Save(Cart);
            }

            return RedirectToAction("Index", new { node = currentPage.ContentLink });
        }

        [HttpPost]
        public ActionResult ChangeAddress(UpdateAddressViewModel addressViewModel)
        {
            ModelState.Clear();

            var viewModel = CreateCheckoutViewModel(addressViewModel.CurrentPage);

            // Set random value for Name/Id if null.
            if (addressViewModel.BillingAddress.AddressId == null)
            {
                addressViewModel.BillingAddress.Name = addressViewModel.BillingAddress.AddressId = Guid.NewGuid().ToString();
            }

            foreach (var shipment in addressViewModel.Shipments.Where(x => x.Address.AddressId == null))
            {
                shipment.Address.Name = shipment.Address.AddressId = Guid.NewGuid().ToString();
            }

            _checkoutService.CheckoutAddressHandling.ChangeAddress(viewModel, addressViewModel);

            _checkoutService.UpdateShippingAddresses(Cart, viewModel);

            _orderRepository.Save(Cart);

            var addressViewName = addressViewModel.ShippingAddressIndex > -1 ? "SingleShippingAddress" : "BillingAddress";

            return PartialView(addressViewName, viewModel);
        }

        // CyberSource Connector code changes
        [HttpPost]
        [Authorize]
        public ActionResult GetSelectedBillingAddress(string addressId)
        {
            var customerAddresses = _addressBookService.List();

            return Json(new { address = customerAddresses.FirstOrDefault(x => x.AddressId == addressId) });
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult OrderSummary()
        {
            var viewModel = _orderSummaryViewModelFactory.CreateOrderSummaryViewModel(Cart);
            return PartialView(viewModel);
        }

        [HttpPost]
        [AllowDBWrite]
        public ActionResult AddCouponCode(CheckoutPage currentPage, string couponCode)
        {
            if (_cartService.AddCouponCode(Cart, couponCode))
            {
                _orderRepository.Save(Cart);
            }
            var viewModel = CreateCheckoutViewModel(currentPage);
            return View(viewModel.ViewName, viewModel);
        }

        [HttpPost]
        [AllowDBWrite]
        public ActionResult RemoveCouponCode(CheckoutPage currentPage, string couponCode)
        {
            _cartService.RemoveCouponCode(Cart, couponCode);
            _orderRepository.Save(Cart);
            var viewModel = CreateCheckoutViewModel(currentPage);
            return View(viewModel.ViewName, viewModel);
        }

        // CyberSource Connector code changes
        [HttpPost]
        [AllowDBWrite]
        public ActionResult AddPaymentOnCart(CheckoutViewModel viewModel, IPaymentMethod paymentMethod)
        {
            if (CartIsNullOrEmpty())
            {
                return Redirect(Url.ContentUrl(ContentReference.StartPage));
            }

            viewModel.Payment = paymentMethod;
            viewModel.IsAuthenticated = User.Identity.IsAuthenticated;
            _checkoutService.CheckoutAddressHandling.UpdateUserAddresses(viewModel);

            if (!_checkoutService.ValidateOrder(ModelState, viewModel, _orderValidationService.ValidateOrder(Cart)))
            {
                return View(viewModel);
            }

            if (!paymentMethod.ValidateData())
            {
                return View(viewModel);
            }

            _checkoutService.UpdateShippingAddresses(Cart, viewModel);
            _checkoutService.CreateAndAddPaymentToCart(Cart, viewModel);

            return new EmptyResult();
        }

        // CyberSource Connector code changes
        [AllowDBWrite]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult PlaceOrder()
        {
            var payment = Cart.GetFirstForm().Payments.FirstOrDefault();

            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            var startPage = contentRepository.Get<StartPage>(ContentReference.StartPage);
            var checkoutPage = contentRepository.Get<CheckoutPage>(startPage.CheckoutPage);
            var viewModel = _checkoutViewModelFactory.CreateEmptyCheckoutViewModel(checkoutPage);

            if (!ConfigureCreditCardProperties(payment))
            {
                return View(viewModel);
            }

            AddDecisionManagerInformation(payment);

            string redirectUrl;
            var purchaseOrder = _checkoutService.PlaceOrder(Cart, ModelState, out redirectUrl);

            if (!string.IsNullOrEmpty(redirectUrl))
            {
                return Redirect(redirectUrl);
            }

            if (purchaseOrder == null)
            {
                return View(viewModel);
            }

            // commented order confirmation email sending because SMTP server is not configured
            var confirmationSentSuccessfully = true; //_checkoutService.SendConfirmation(viewModel, purchaseOrder);
            var billingEmail = Cart.GetFirstShipment().ShippingAddress.Email;

            return Redirect(_checkoutService.BuildRedirectionUrl(purchaseOrder, billingEmail, confirmationSentSuccessfully));
        }

        // CyberSource Connector code changes
        [HttpPost]
        public ActionResult Purchase(CheckoutViewModel viewModel, IPaymentMethod paymentMethod)
        {
            if (CartIsNullOrEmpty())
            {
                return Redirect(Url.ContentUrl(ContentReference.StartPage));
            }

            viewModel.Payment = paymentMethod;
            viewModel.IsAuthenticated = User.Identity.IsAuthenticated;
            _checkoutService.CheckoutAddressHandling.UpdateUserAddresses(viewModel);

            if (!_checkoutService.ValidateOrder(ModelState, viewModel, _orderValidationService.ValidateOrder(Cart)))
            {
                return View(viewModel);
            }

            if (!paymentMethod.ValidateData())
            {
                return View(viewModel);
            }

            _checkoutService.UpdateShippingAddresses(Cart, viewModel);
            _checkoutService.CreateAndAddPaymentToCart(Cart, viewModel);

            var payment = Cart.GetFirstForm().Payments.FirstOrDefault();
            AddDecisionManagerInformation(payment);

            string redirectUrl;
            var purchaseOrder = _checkoutService.PlaceOrder(Cart, ModelState, out redirectUrl);
            if (!string.IsNullOrEmpty(redirectUrl))
            {
                return Redirect(redirectUrl);
            }

            if (purchaseOrder == null)
            {
                return View(viewModel);
            }

            // commented order confirmation email sending because SMTP server is not configured
            var confirmationSentSuccessfully = true; //_checkoutService.SendConfirmation(viewModel, purchaseOrder);
            var billingEmail = Cart.GetFirstShipment().ShippingAddress.Email;

            return Redirect(_checkoutService.BuildRedirectionUrl(purchaseOrder, billingEmail, confirmationSentSuccessfully));
        }

        public ActionResult OnPurchaseException(ExceptionContext filterContext)
        {
            var currentPage = filterContext.RequestContext.GetRoutedData<CheckoutPage>();
            if (currentPage == null)
            {
                return new EmptyResult();
            }

            var viewModel = CreateCheckoutViewModel(currentPage);
            ModelState.AddModelError("Purchase", filterContext.Exception.Message);

            return View(viewModel.ViewName, viewModel);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            _controllerExceptionHandler.HandleRequestValidationException(filterContext, "purchase", OnPurchaseException);
        }

        // CyberSource Connector added code
        private bool ConfigureCreditCardProperties(IPayment payment)
        {
            var cyberSourcePayment = payment as ICyberSourceCreditCardPayment;

            if (cyberSourcePayment == null) return true;

            if (!IsSignatureChecked())
            {
                ModelState.AddModelError("SignatureCheck", "Secure Acceptance Signature check fail. Please try again.");

                return false;
            }

            if (Request.Form["decision"] != "ACCEPT")
            {
                ModelState.AddModelError("SecureAcceptanceResponse",
                    $"Secure acceptance request failed with Message: {Request.Form["message"]}. Invalid fields: {Request.Form["invalid_fields"]}");

                return false;
            }

            cyberSourcePayment.CyberSourceToken = Request.Form["payment_token"];
            cyberSourcePayment.CyberSourceRequestId = Request.Form["transaction_id"];

            return true;
        }

        // CyberSource Connector code changes
        private bool IsSignatureChecked()
        {
            var parameters = new Dictionary<string, string>();
            foreach (var key in Request.Form.AllKeys)
            {
                parameters.Add(key, Request.Params[key]);
            }
            var computedSignature = _secureAcceptanceSecurity.Sign(parameters);
            var requestSignatureValue = Request.Form["signature"];

            return computedSignature == requestSignatureValue;
        }

        // CyberSource Connector code changes
        private void AddDecisionManagerInformation(IPayment payment)
        {
            var cyberSourcePayment = payment as IBaseCyberSourcePayment;

            if (cyberSourcePayment == null) return;

            var decisionInformation = new DecisionManagerInformation
            {
                CustomerIpAddress = Request.UserHostAddress,
                IsHttpBrowserCookiesAccepted = true,
                CustomerId = User.Identity.GetUserId(),
                HttpBrowserType = Request.UserAgent,
                HostName = Request.UserHostName
            };

            var decisionManagerJson = JsonConvert.SerializeObject(decisionInformation);
            cyberSourcePayment.DecisionManagerInformation = decisionManagerJson;
        }

        private ViewResult View(CheckoutViewModel checkoutViewModel)
        {
            return View(checkoutViewModel.ViewName, CreateCheckoutViewModel(checkoutViewModel.CurrentPage, checkoutViewModel.Payment));
        }

        private CheckoutViewModel CreateCheckoutViewModel(CheckoutPage currentPage, IPaymentMethod paymentMethod = null)
        {
            var checkoutViewModel = _checkoutViewModelFactory.CreateCheckoutViewModel(Cart, currentPage, paymentMethod);
            return checkoutViewModel;
        }

        private ICart Cart => _cart ?? (_cart = _cartService.LoadCart(_cartService.DefaultCartName));

        private bool CartIsNullOrEmpty()
        {
            return Cart == null || !Cart.GetAllLineItems().Any();
        }
    }
}
