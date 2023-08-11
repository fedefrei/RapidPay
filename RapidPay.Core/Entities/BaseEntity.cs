using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RapidPay.Core.Entities
{
	public abstract class BaseEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)] //A new key will be generated when a city is added
		public Guid Id { get; set; }

		public DateTime DateCreation { get; set; } = DateTime.Now;
		
		public DateTime DateLastUpdate { get; set; } = DateTime.Now;
	}
}
