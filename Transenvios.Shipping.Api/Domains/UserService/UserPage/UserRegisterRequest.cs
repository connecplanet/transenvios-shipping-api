using System.ComponentModel.DataAnnotations;

namespace Transenvios.Shipping.Api.Domains.UserService.UserPage
{
    public class UserRegisterRequest
    {
        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
