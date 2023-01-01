using System.ComponentModel.DataAnnotations;

namespace Transenvios.Shipping.Api.Domains.UserService
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
        public string? CountryCode { get; set; }
        public string? Phone { get; set; }
        public string? Role { get; set; }
        public string? DocumentType { get; set; }
        public string? DocumentId { get; set; }
    }
}
