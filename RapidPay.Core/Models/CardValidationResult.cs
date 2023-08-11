namespace RapidPay.Core.Models
{
	public class CardValidationResult
	{
		public bool Succeeded { get; set; }
		public string Reason { get; set; } = string.Empty;
	}
}
