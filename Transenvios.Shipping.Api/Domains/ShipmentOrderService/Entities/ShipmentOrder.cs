using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Domains.DriverService;
using Transenvios.Shipping.Api.Domains.UserService;

namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService.Entities
{
    public class ShipmentOrder
    {
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

        public Guid? PickUpCityId { get; set; }
        public virtual City? PickUpCity { get; set; }

        public Guid? DropOffCityId { get; set; }
        public virtual City? DropOffCity { get; set; }

        public Guid? TransporterId { get; set; }
        public virtual Driver? Transporter { get; set; }

        public Guid CustomerId { get; set; }
        public virtual User? Customer { get; set; }

        public Guid? ModifyUserId { get; set; }
        [JsonIgnore]
        public virtual User? ModifyUser { get; set; }
        
        public virtual ICollection<ShipmentOrderItem>? Packages { get; set; }
    }
}