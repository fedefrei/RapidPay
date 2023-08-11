namespace RapidPay.Core
{
	public class UFEService
	{
		private static readonly object _lock = new object();

		private DateTime LastApplication = DateTime.MinValue;
		private double FeeToApply;
		private double LastFeeAmount;

		public double GetFeeExchange(double amount)
		{
			lock (_lock)
			{
				var currentTime = DateTime.Now;
				LastFeeAmount = amount;

				if (currentTime.Day != LastApplication.Day && currentTime.Hour != LastApplication.Hour)
				{
					LastApplication = currentTime;
					FeeToApply = LastFeeAmount * Random.Shared.NextDouble() * 2;
				}

				return FeeToApply;
			}
		}
	}
}
