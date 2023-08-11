using RapidPay.Core.Entities;

namespace RapidPay.Core.Repositories
{
	public interface IPaymentRepository
	{
		public Task AddPayment(Guid cardId, double amount, double fee);
	}
}
