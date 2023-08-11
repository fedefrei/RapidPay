using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RapidPay.API.Controllers
{
	[Authorize]
	[ApiController]
	public class AuthorizedController : ControllerBase
	{
		private Guid _userId;

		public Guid UserId
		{
			get
			{
				if (_userId != Guid.Empty)
				{
					return _userId;
				}

				if (Guid.TryParse(User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value, out Guid userID))
				{
					_userId = userID;
					return _userId;
				}

				throw new Exception("invalid-user-id");
			}
		}
	}
}
