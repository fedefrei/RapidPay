using RapidPay.Core.Entities;
using RapidPay.Core.Helpers;
using RapidPay.Core.Models;
using RapidPay.Core.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace RapidPay.Core
{
	public class CardManagementService
	{
		private readonly ICardRepository _cardRepository;
		private readonly IPaymentRepository _paymentRepository;
		private readonly UFEService _ufeService;

		public CardManagementService(ICardRepository cardRepository, IPaymentRepository paymentRepository, UFEService ufeService)
		{
			this._cardRepository = cardRepository ?? throw new ArgumentNullException(nameof(cardRepository));
			_paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
			_ufeService = ufeService ?? throw new ArgumentNullException(nameof(paymentRepository));
		}

		public static CardValidationResult ValidateCard(Card card)
		{
			//Checks on the card data can be done here
			var nextMonth = DateTime.Today.AddMonths(1);

			var startOfNextMonth = new DateTime(nextMonth.Year, nextMonth.Month, 1);

			if (new DateTime(card.ValidUntilYear, card.ValidUntilMonth, 1) < startOfNextMonth)
			{
				return new() { Succeeded = false, Reason = "expired-card" };
			}

			return new() { Succeeded = true };
		}
		
		public static CardValidationResult ValidateAmount(double amount)
		{
			if (amount <= 0)
			{
				return new()
				{
					Succeeded = false,
					Reason = "invalid-amount"
				};
			}

			return new() { Succeeded = true };
		}

		private double GetCardLimitForUser(Guid userId)
		{
			//Here we can do whatever we want if we need more validation
			return 200000;
		}

		/// <summary>
		/// Validates the provided card and persists it if correct.
		/// Assigns the balance and the card limit.
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		public async Task<CardServiceResult<Card>> AddCardAsync(Card card)
		{
			var validationResult = ValidateCard(card);

			if (!validationResult.Succeeded)
			{
				return new()
				{
					Succeeded = false,
					Message = validationResult.Reason
				};
			}

			card.Hash = CardHasherHelper.GetHashedCardNumber(card.CardNumber);
			card.Balance = card.CardLimit = GetCardLimitForUser(card.UserId);

			var addedCard = await _cardRepository.CreateCardAsync(card);

			return new()
			{
				Succeeded = true,
				Value = addedCard
			};
		}

		/// <summary>
		/// Attempts to do a charge to the Credit Card
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="cardHash"></param>
		/// <param name="amount"></param>
		/// <returns>A CardServiceResult with the result of the operation. If successful ir return the balance in the card.</returns>
		public async Task<CardServiceResult<double>> ProcessPayment(Guid userId, string cardHash, int ccv, double amount)
		{
			var card = await _cardRepository.GetCardAsync(userId, cardHash);

			if (card == null)
			{
				return new() { Succeeded = false, Message = "invalid-card" };
			}
			
			if (card.CCV != ccv)
			{
				return new() { Succeeded = false, Message = "invalid-ccv" };
			}

			if (amount <= 0)
			{
				return new() { Succeeded = false, Message = "invalid-amount" };
			}

			//Proceed to pay

			var fee = _ufeService.GetFeeExchange(amount);
			double amountToCharge = amount + (amount * fee / 100);

			var result = ValidateCardAcceptsPayment(card, amountToCharge);

			if (!result.Succeeded)
			{
				return new() { Succeeded = false, Message = result.Reason };
			}

			//Register the payment
			await _paymentRepository.AddPayment(card.Id, amount, fee);

			//Deduct the balance from the card and update
			double remainingBalance = card.Balance - amountToCharge;
			await _cardRepository.UpdateCardBalance(card.Id, remainingBalance);

			return new()
			{
				Succeeded = true,
				Value = remainingBalance
			};
		}
		
		/// <summary>
		/// Attempts to do a charge to the Credit Card
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="cardHash"></param>
		/// <param name="amount"></param>
		/// <returns>A CardServiceResult with the result of the operation. If successful ir return the balance in the card.</returns>
		public async Task<CardServiceResult<double>> GetCardBalance(Guid userId, string cardHash, int ccv)
		{
			var card = await _cardRepository.GetCardAsync(userId, cardHash);

			if (card == null || card.CCV != ccv)
			{
				return new()
				{
					Succeeded = false,
					Message = "invalid-card"
				};
			}

			//Revalidate the card
			var cardValidation = ValidateCard(card);

			if (!cardValidation.Succeeded)
			{
				return new()
				{
					Succeeded = false,
					Message = cardValidation.Reason
				};
			}

			return new()
			{
				Succeeded = true,
				Value = card.Balance
			};
		}

		private static CardValidationResult ValidateCardAcceptsPayment(Card card, double amount)
		{
			//Revalidate the card
			var cardValidation = ValidateCard(card);

			if (!cardValidation.Succeeded)
			{
				return new()
				{
					Succeeded = false,
					Reason = cardValidation.Reason
				};
			}

			bool hasEnoughBalance = card.Balance - amount > 0;

			if (hasEnoughBalance)
			{
				return new() { Succeeded = true };
			}
			else
			{
				return new() { Succeeded = false, Reason = "not-enough-limit" };
			}
		}
	}
}
