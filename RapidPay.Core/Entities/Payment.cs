using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RapidPay.Core.Entities
{
	public class Payment : BaseEntity
	{
		[ForeignKey("CardId")]
		public Card Card { get; set; }

		public Guid CardId { get; set; }
		
		public double Amount { get; set; }
		
		public double Fee { get; set; }
	}
}
