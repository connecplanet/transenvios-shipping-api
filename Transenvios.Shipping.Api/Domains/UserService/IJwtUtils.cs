using Transenvios.Shipping.Api.Domains.CatalogService;

namespace Transenvios.Shipping.Api.Domains.UserService
{
    public interface IJwtUtils
    {
        public string GenerateToken(Person user);
        public Guid? ValidateToken(string? token);
    }
}
