using System.Text.Json.Serialization;
using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Domains.DriverService;
using Transenvios.Shipping.Api.Domains.UserService;

namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.Entities
{
    public class ShipmentOrder
    {
        #region Properties
        public long Id { get; set; }
        public string? PickUpAddress { get; set; }
        public string? DropOffAddress { get; set; }
        public decimal? InitialPrice { get; set; }
        public decimal Taxes { get; set; }
        public decimal TotalPrice { get; set; }
        public PaymentStates PaymentState { get; set; }
        public ShipmentStates ShipmentState { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string? SenderDocumentType { get; set; }
        public string? SenderDocumentId { get; set; }
        public string? SenderFirstName { get; set; }
        public string? SenderLastName { get; set; }
        public string? SenderEmail { get; set; }
        public string? SenderCountryCode { get; set; }
        public string? SenderPhone { get; set; }
        public string? RecipientDocumentType { get; set; }
        public string? RecipientDocumentId { get; set; }
        public string? RecipientFirstName { get; set; }
        public string? RecipientLastName { get; set; }
        public string? RecipientEmail { get; set; }
        public string? RecipientCountryCode { get; set; }
        public string? RecipientPhone { get; set; }
        #endregion Properties

        #region Detail to parent relationship
        public Guid? PickUpCityId { get; set; }
        [JsonIgnore]
        public City? PickUpCity { get; set; }

        public Guid? DropOffCityId { get; set; }
        [JsonIgnore]
        public City? DropOffCity { get; set; }

        public Guid? TransporterId { get; set; }
        [JsonIgnore]
        public Driver? Transporter { get; set; }

        public Guid CustomerId { get; set; }
        [JsonIgnore]
        public User? Customer { get; set; }

        public Guid? ModifyUserId { get; set; }
        [JsonIgnore]
        public User? ModifyUser { get; set; }
        #endregion Detail to parent relationship

        #region Parent to Detail relationship
        public List<ShipmentOrderItem>? Packages { get; set; }
        #endregion Parent to Detail relationship
    }
}