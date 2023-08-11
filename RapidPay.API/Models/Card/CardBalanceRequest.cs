using System.ComponentModel.DataAnnotations;

namespace RapidPay.API.Models.Card
{
    public class CardBalanceRequest
    {
		[Required]
		public string Cardhash { get; set; }

		[Range(100, 999)]
		public int CCV { get; set; }

	}
}
