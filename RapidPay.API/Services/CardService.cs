using RapidPay.API.Models.Card;
using RapidPay.API.Models.Payment;
using RapidPay.Core;
using RapidPay.Core.Entities;

namespace RapidPay.API.Services
{
    public class CardService
	{
		private readonly CardManagementService _cardManagementService;

        public CardService(CardManagementService cardManagementService)
        {
			_cardManagementService = cardManagementService ?? throw (new ArgumentNullException(nameof(cardManagementService)));

		}

        public async Task<APIResponse<CardDto>> CreateCard(Guid userId, CardCreateRequest request)
		{
			//Check that the card are all digits

			var cardToAdd = new Card
			{
				CardNumber = request.CardNumber,
				CCV = request.CCV,
				DateLastUpdate = DateTime.Now,
				NameOnCard = request.NameOnCard,
				UserId = userId,
				ValidUntilMonth = request.ValidUntilMonth,
				ValidUntilYear = request.ValidUntilYear,
			};

			var result = await _cardManagementService.AddCardAsync(cardToAdd);

			if (result.Succeeded)
			{
				var cardDto = new CardDto
				{
					Balance = result.Value.Balance,
					CardHash = result.Value.Hash,
					NameOnCard = result.Value.NameOnCard,
					Expires = $"{result.Value.ValidUntilMonth}/{result.Value.ValidUntilYear}"
				};

				return new() { Succeeded = true, Value = cardDto };
			}

			return new() { Message = result.Message };
		}

		internal async Task<APIResponse<double>> AttemptPayment(Guid userId, PaymentRequest request)
		{
			var result = await _cardManagementService.ProcessPayment(userId, request.Cardhash, request.CCV, request.Amount);

			if (result.Succeeded)
			{
				return new() { Succeeded = true, Value = result.Value };
			}

			return new() { Message = result.Message };
		}
		
		internal async Task<APIResponse<double>> GetCardBalance(Guid userId, CardBalanceRequest request)
		{
			var result = await _cardManagementService.GetCardBalance(userId, request.Cardhash, request.CCV);

			if (result.Succeeded)
			{
				return new() { Succeeded = true, Value = result.Value };
			}

			return new() { Message = result.Message };
		}
	}
}
