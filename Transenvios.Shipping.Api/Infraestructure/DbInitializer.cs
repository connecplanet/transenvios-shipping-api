namespace Transenvios.Shipping.Api.Infraestructure
{
    public class DbInitializer
    {
        public static void Initialize(DataContext context, IWebHostEnvironment env)
        {
            context.Database.EnsureCreated();
        }
    }
}
