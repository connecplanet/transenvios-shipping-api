namespace Transenvios.Shipping.Api.Infraestructure
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string EmailHost { get; set; }
        public string EmailPort { get; set; }
        public string EmailUser { get; set; }
        public string EmailPassword { get; set; }
        public bool EmailEnableSsl { get; set; }

        public string EmailFrom { get; set; }
        
    }
}
