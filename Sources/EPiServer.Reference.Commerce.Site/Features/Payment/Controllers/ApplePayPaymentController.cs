using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using EPiServer.Commerce.Order;
using EPiServer.Reference.Commerce.Site.Features.Cart.Services;
using EPiServer.Reference.Commerce.Site.Features.Payment.Models;
using EPiServer.Reference.Commerce.Site.Features.Payment.Services;
using LL.EpiserverCyberSourceConnector.Core.Payments.ApplePay;
using LL.EpiserverCyberSourceConnector.Site;
using Newtonsoft.Json;
using NuGet;
using HttpClient = System.Net.Http.HttpClient;

namespace EPiServer.Reference.Commerce.Site.Features.Payment.Controllers
{
    public class ApplePayPaymentController : Controller
    {
        private readonly ICartService _cartService;
        private ICart _cart;
        private readonly IOrderRepository _orderRepository;
        private readonly ApplePayConfiguration _applePayConfiguration;
        private ICart Cart => _cart ?? (_cart = _cartService.LoadCart(_cartService.DefaultCartName));

        public ApplePayPaymentController(ICartService cartService, IOrderRepository orderRepository)
        {
            _cartService = cartService;
            _orderRepository = orderRepository;
            _applePayConfiguration = new ApplePayConfiguration();
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult ProcessApplePayPayment(string paymentData, string paymentCardType)
        {
            if (string.IsNullOrEmpty(paymentData) || string.IsNullOrEmpty(paymentCardType))
            {
                return Json(false);
            }

            var payment = Cart.GetFirstForm().Payments.FirstOrDefault();
            var applePayPayment = payment as ICyberSourceApplePayPayment;
            if (applePayPayment != null)
            {
                applePayPayment.CyberSourceApplePayData = paymentData;

                if (CreditCardTypeStore.CardTypes.ContainsKey(paymentCardType))
                {
                    applePayPayment.CyberSourceApplePayCardType = CreditCardTypeStore.CardTypes[paymentCardType];
                }

                _orderRepository.Save(Cart);
                return Json(true);
            }

            return Json(false);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult StartApplePaySession([FromBody] ValidateMerchantSessionModel model, CancellationToken cancellationToken)
        {
            Uri requestUri;
            if (!ModelState.IsValid ||
                string.IsNullOrWhiteSpace(model?.ValidationUrl) ||
                !Uri.TryCreate(model.ValidationUrl, UriKind.Absolute, out requestUri))
            {
                return Json(false);
            }

            string result = GetMerchantSessionAsync(new Uri(model.ValidationUrl), new MerchantSessionRequest
            {
                DisplayName = _applePayConfiguration.DisplayName,
                Initiative = "web",
                InitiativeContext = _applePayConfiguration.InitiativeContext,
                MerchantIdentifier = CertificateService.GetMerchantIdentifier(
                    CertificateService.LoadMerchantCertificate(_applePayConfiguration.CertificateThumbprint))
            }).Result;

            return Json(result);
        }

        public async Task<string> GetMerchantSessionAsync(
          Uri requestUri,
          MerchantSessionRequest request,
          CancellationToken cancellationToken = default(CancellationToken))
        {
            // POST the data to create a valid Apple Pay merchant session.
            string json = JsonConvert.SerializeObject(request);

            using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                var clientHandler = GetHttpClientHandler();
                HttpClient client = new HttpClient(clientHandler);

                try
                {
                    var response = client.PostAsync(requestUri, content).Result;

                    response.EnsureSuccessStatusCode();
                    // Read the opaque merchant session JSON from the response body.
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        return stream.ReadToEnd();
                    }
                }
                catch (Exception e)
                {
                    return e.ToString();
                }
            }
        }

        private HttpClientHandler GetHttpClientHandler()
        {
            var certificate = CertificateService.LoadMerchantCertificate(_applePayConfiguration.CertificateThumbprint);

            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(certificate);

            // Apple Pay JS requires the use of at least TLS 1.2 to generate a merchange session:
            // https://developer.apple.com/documentation/applepayjs/setting_up_server_requirements
            // If you run an older operating system that does not negotiate this by default
            // set System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; 

            return handler;
        }
    }
}