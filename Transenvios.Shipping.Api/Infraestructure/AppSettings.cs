namespace Transenvios.Shipping.Api.Infraestructure
{
    public class AppSettings
    {
        public AuthSettings Auth { get; set; }
        public EmailSettings Email { get; set; }
        public ShipmentSettings Shipment { get; set; }
    }

    public class AuthSettings
    {
        public string Secret { get; set; }
    }

    public class EmailSettings
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
    }

    public class ShipmentSettings
    {
        public decimal InsuredAmountRatio { get; set; }
        public decimal UrgentAmountRatio { get; set; }
        public decimal FragileAmountRatio { get; set; }
        public decimal TaxAmountRatio { get; set; }
    }
}
