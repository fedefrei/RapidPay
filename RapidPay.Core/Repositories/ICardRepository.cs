using RapidPay.Core.Entities;

namespace RapidPay.Core.Repositories
{
	public interface ICardRepository
	{
		public Task<Card> CreateCardAsync(Card card);
		public Task<Card> GetCardAsync(Guid userId, string cardHash);
		
		/// <summary>
		/// Updates the balance of the card
		/// </summary>
		/// <param name="cardId"></param>
		/// <param name="amount"></param>
		public Task UpdateCardBalance(Guid cardId, double amount);
	}
}
