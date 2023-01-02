using System.ComponentModel.DataAnnotations;

namespace Transenvios.Shipping.Api.Domains.UserService
{
    public class UserTokenRequest
    {
        [Required]
        public string? AccessToken { get; set; }
    }
}
