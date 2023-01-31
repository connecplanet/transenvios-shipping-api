namespace Transenvios.Shipping.Api.Infraestructure
{
    public class AppSettings
    {
        public AuthSettings Auth { get; set; } = null!;
        public EmailSettings Email { get; set; } = null!;
        public PackageAdditionCharges PackageCharges { get; set; } = null!;
    }

    public class AuthSettings
    {
        public string? Secret { get; set; }
    }

    public class EmailSettings
    {
        public string? Host { get; set; }
        public string? Port { get; set; }
        public string? User { get; set; }
        public string? Password { get; set; }
        public bool EnableSsl { get; set; }
    }

    public class PackageAdditionCharges
    {
        public decimal InsuredCharge { get; set; }
        public decimal UrgentCharge { get; set; }
        public decimal FragileCharge { get; set; }
        public decimal TaxCharge { get; set; }
    }
}
