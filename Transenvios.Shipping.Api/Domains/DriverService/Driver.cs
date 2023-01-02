using System.Text.Json.Serialization;

namespace Transenvios.Shipping.Api.Domains.DriverService
{
    public class Driver : BaseEntity<Guid>
    {
        public string? DocumentType { get; set; }
        public long? DocumentId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public long? CountryCode { get; set; }
        public string? Phone { get; set; }
        public Guid? PickUpCityId { get; set; }
        public string? PickUpAddress { get; set; }
    }
}
