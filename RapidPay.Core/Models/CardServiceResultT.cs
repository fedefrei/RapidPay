namespace RapidPay.Core.Models
{
	public class CardServiceResult<T> : CardServiceResult
	{
		public T Value { get; set; }
	}
}
