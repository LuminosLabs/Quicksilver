using System.ComponentModel.DataAnnotations;

namespace EPiServer.Reference.Commerce.Site.Features.Payment.Models
{
    public class ValidateMerchantSessionModel
    {
        [DataType(DataType.Url)]
        [Required]
        public string ValidationUrl { get; set; }
    }
}