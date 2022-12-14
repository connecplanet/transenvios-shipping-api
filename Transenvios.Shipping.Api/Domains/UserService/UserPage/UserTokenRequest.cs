using System.ComponentModel.DataAnnotations;

namespace Transenvios.Shipping.Api.Domains.UserService.UserPage
{
    public class UserTokenRequest
    {
        [Required]
        public string? AccessToken { get; set; }
    }
}
