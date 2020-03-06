using Newtonsoft.Json;

namespace EPiServer.Reference.Commerce.Site.Features.Payment.Models
{
    public class MerchantSessionRequest
    {
        [JsonProperty(PropertyName = "merchantIdentifier")]
        public string MerchantIdentifier { get; set; }

        [JsonProperty(PropertyName = "displayName")]
        public string DisplayName { get; set; }

        [JsonProperty(PropertyName = "initiative")]
        public string Initiative { get; set; }

        [JsonProperty(PropertyName = "initiativeContext")]
        public string InitiativeContext { get; set; }
    }
}