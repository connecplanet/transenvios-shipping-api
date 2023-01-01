namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService
{
    public class ShipmentOrder
    {
        public int Id { get; set; }
        public string? PickUpCityId { get; set; }
        public string? PickUpAddress { get; set; }
        public string? DropOffCityId { get; set; }
        public string? DropOffAddress { get; set; }
        public decimal? InitialPrice { get; set; }
        public decimal Taxes { get; set; }
        public decimal TotalPrice { get; set; }
        public string? PaymentState { get; set; }
        public string? ShipmentState { get; set; }
        public string? TransporterId { get; set; }
        public string? ApplicantId { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public string? ModifyUserId { get; set; }
        public string? SenderDocumentType { get; set; }
        public int? SenderDocumentId { get; set; }
        public string? SenderFirstName { get; set; }
        public string? SenderLastName { get; set; }
        public string? SenderEmail { get; set; }
        public int? SenderCountryCode { get; set; }
        public string? SenderPhone { get; set; }
        public string? RecipientDocumentType { get; set; }
        public int? RecipientDocumentId { get; set; }
        public string? RecipientFirstName { get; set; }
        public string? RecipientLastName { get; set; }
        public string? RecipientEmail { get; set; }
        public int? RecipientCountryCode { get; set; }
        public string? RecipientPhone { get; set; }
    }
}
