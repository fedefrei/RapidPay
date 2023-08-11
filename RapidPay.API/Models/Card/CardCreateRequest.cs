using System.ComponentModel.DataAnnotations;

namespace RapidPay.API.Models.Card
{
    public class CardCreateRequest
    {
        [MaxLength(15)]
        [MinLength(15)]
        public string CardNumber { get; set; }
        public string NameOnCard { get; set; }

        [Range(1, 12)]
        public int ValidUntilMonth { get; set; }

        [Range(1980, 2079)]
        public int ValidUntilYear { get; set; }

        [Range(100, 999)]
        public int CCV { get; set; }
    }
}
