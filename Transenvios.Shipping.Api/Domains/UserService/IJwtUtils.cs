namespace Transenvios.Shipping.Api.Domains.UserService
{
    public interface IJwtUtils
    {
        public string GenerateToken(User user);
        public Guid? ValidateToken(string? token);
    }
}
