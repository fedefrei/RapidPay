using System.ComponentModel.DataAnnotations;

namespace RapidPay.API.Models.Payment
{
    public class APIResponse<T>
	{
        public bool Succeeded { get; set; } = false;
        
        public string Message { get; set; } = string.Empty;
        
        public T Value { get; set; }
    }
}
