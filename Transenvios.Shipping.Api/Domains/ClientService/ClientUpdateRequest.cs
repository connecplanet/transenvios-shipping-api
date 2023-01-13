namespace Transenvios.Shipping.Api.Domains.ClientService
{
    public class ClientUpdateRequest
    {
        public string? DocumentType { get; set; }
        public string? DocumentId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? CountryCode { get; set; }
        public string? Phone { get; set; }
        public string? Role { get; set; }
        public string? PasswordHash { get; set; }

    }
}
