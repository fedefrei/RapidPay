using System.ComponentModel.DataAnnotations;

namespace RapidPay.API.Models.Payment
{
    public class PaymentRequest
    {
        [Required]
        public string Cardhash { get; set; }

        [Range(100, 999)]
        public int CCV { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public double Amount { get; set; }
    }
}
