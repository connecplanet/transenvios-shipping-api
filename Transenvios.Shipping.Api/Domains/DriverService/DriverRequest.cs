namespace Transenvios.Shipping.Api.Domains.DriverService
{
    public class DriverRequest
    {
        public string? DocumentType { get; set; }
        public string? DocumentId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? CountryCode { get; set; }
        public string? Phone { get; set; }
        public string? PickUpCityId { get; set; }
        public string? PickUpAddress { get; set; }
    }
}