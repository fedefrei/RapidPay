using Microsoft.EntityFrameworkCore;
using RapidPay.Core.Entities;

namespace RapidPay.Core.Repositories
{
	public class CardRepository : ICardRepository
	{
		private readonly RapidPayContext _context;

		public CardRepository(RapidPayContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public async Task<Card> CreateCardAsync(Card card)
		{
			_context.Cards.Add(card);

			await _context.SaveChangesAsync();

			return card;
		}
		
		public async Task UpdateCardBalance(Guid cardId, double newBalance)
		{
			var card = await _context.Cards.FirstOrDefaultAsync(c => c.Id == cardId);

			card.Balance = newBalance;

			await _context.SaveChangesAsync();
		}

		public Task<Card> GetCardAsync(Guid userId, string cardHash) =>
			_context.Cards
			.AsNoTracking()
			.FirstOrDefaultAsync(c => c.UserId == userId && c.Hash == cardHash);
	}
}
