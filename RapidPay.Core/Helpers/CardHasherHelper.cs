using System.Security.Cryptography;
using System.Text;

namespace RapidPay.Core.Helpers
{
	internal static class CardHasherHelper
	{
		private static string ByteArrayToString(byte[] arrInput)
		{
			int i;
			StringBuilder sOutput = new StringBuilder(arrInput.Length);
			for (i = 0; i < arrInput.Length; i++)
			{
				sOutput.Append(arrInput[i].ToString("X2"));
			}
			return sOutput.ToString();
		}

		public static string GetHashedCardNumber(string cardNumber)
		{
			byte[] numberBytes = Encoding.UTF8.GetBytes(cardNumber + DateTime.Now.Ticks);
			var tmpHash = MD5.Create().ComputeHash(numberBytes);

			return ByteArrayToString(tmpHash);
		}
	}
}
