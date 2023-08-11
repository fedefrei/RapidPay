using System.ComponentModel.DataAnnotations;

namespace RapidPay.API.Models.Card
{
    public class CardDto
	{
        public string CardHash { get; set; }
        public string NameOnCard { get; set; }
        public string Expires { get; set; }
        public double Balance { get; set; }
    }
}
