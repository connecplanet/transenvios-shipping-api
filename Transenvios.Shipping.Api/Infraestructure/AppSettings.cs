namespace Transenvios.Shipping.Api.Infraestructure
{
    public class AppSettings
    {
        public AuthConfig Auth { get; set; }
        public EmailConfig Email { get; set; }
        public ShipmentConfig Shipment { get; set; }
    }

    public class AuthConfig
    {
        public string Secret { get; set; }
    }

    public class EmailConfig
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
    }

    public class ShipmentConfig
    {
        public decimal InsuredAmountRatio { get; set; }
        public decimal UrgentAmountRatio { get; set; }
        public decimal FragileAmountRatio { get; set; }
        public decimal TaxRatio { get; set; }
    }
}
