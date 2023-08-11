using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RapidPay.API.Models.Card;
using RapidPay.API.Models.Payment;
using RapidPay.API.Services;

namespace RapidPay.API.Controllers
{
    [Route("api/payment")]
	public class PaymentController : AuthorizedController
	{
		private readonly CardService _cardService;
        public PaymentController(CardService cardService)
        {
			_cardService = cardService ?? throw (new ArgumentNullException(nameof(cardService)));
        }
		
		/// <summary>
		/// Processes a payment for the given card hash/CCV combinations
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> ProcessPayment(PaymentRequest request)
		{
			var createResult = await _cardService.AttemptPayment(UserId, request);

			if (createResult.Succeeded)
			{
				return Ok(createResult.Value);
			}

			return BadRequest(createResult.Message);
		}
	}
}
