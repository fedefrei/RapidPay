using Microsoft.AspNetCore.Mvc;
using RapidPay.API.Models.Card;
using RapidPay.API.Services;

namespace RapidPay.API.Controllers
{
	[Route("api/card")]
	public class CardController : AuthorizedController
	{
		private readonly CardService _cardService;
		public CardController(CardService cardService)
		{
			_cardService = cardService ?? throw (new ArgumentNullException(nameof(cardService)));
		}

		/// <summary>
		/// Attempts to create a card and if valid, it returns the card data with the hash
		/// </summary>
		/// <returns>IActionResult</returns>
		/// <response code="200">Returns the card data</response>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<CardDto>> CreateCard(CardCreateRequest request)
		{
			var createResult = await _cardService.CreateCard(UserId, request);

			if (createResult.Succeeded)
			{
				return Ok(createResult.Value);
			}

			return BadRequest(createResult.Message);
		}

		/// <summary>
		/// Attempts to get the card balance for the hash/ccv combination
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost("balance")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<double>> GetCardBalance(CardBalanceRequest request)
		{
			var result = await _cardService.GetCardBalance(UserId, request);

			if (result.Succeeded)
			{
				return Ok(result.Value);
			}

			return BadRequest(result.Message);
		}
	}
}
