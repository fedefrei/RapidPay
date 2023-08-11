using System.ComponentModel.DataAnnotations;

namespace RapidPay.Core.Entities
{
	public class Card : BaseEntity
	{
		public Guid UserId { get; set; }

		[MaxLength(15)] 
		public string CardNumber { get; set; }
		public string NameOnCard { get; set; }
		
		[Range(1, 12)]
		public int ValidUntilMonth { get; set; }
		
		[Range(1980, 2079)]
		public int ValidUntilYear { get; set; }

		[Range(100, 999)]
		public int CCV { get; set; }

		public string Hash { get; internal set; }

		[Range(0, double.MaxValue)]
		public double CardLimit { get; set; }

		public double Balance { get; set; }
	}
}
