namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService
{
    public class ShipmentOrderPerson
    {
        public string? DocumentType { get; set; }
        public int? DocumentId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public int? CountryCode { get; set; }
        public string? Phone { get; set; }
        public bool IsClient { get; set; }
    }
}
