using Microsoft.EntityFrameworkCore;
using RapidPay.Core.Entities;

namespace RapidPay.Core.Repositories
{
	public class PaymentRepository : IPaymentRepository
	{
		private readonly RapidPayContext _context;

        public PaymentRepository(RapidPayContext context)
        {
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public async Task AddPayment(Guid cardId, double amount, double fee)
		{
			_context.Payments.Add(new Payment
			{
				Amount = amount,
				Fee = fee,
				CardId = cardId,
				DateLastUpdate = DateTime.Now
			});

			await _context.SaveChangesAsync();
		}
	}
}
