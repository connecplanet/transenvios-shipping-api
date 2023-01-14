namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.Requests
{
    public class ShipmentOrderPersonRequest
    {
        public string? DocumentType { get; set; }
        public string? DocumentId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? CountryCode { get; set; }
        public string? Phone { get; set; }
        public bool IsClient { get; set; }
    }
}
