using Transenvios.Shipping.Api.Domains.CatalogService;

namespace Transenvios.Shipping.Api.Mediators.CatalogService
{
    public class CountryMediator : ICatalogQuery<Country>
    {
        public async Task<IList<Country>> GetAllAsync()
        {
            IList<Country> items = new List<Country>
            {
                new Country
                {
                    Id = Guid.NewGuid(),
                    Code = "+57",
                    Name = "ColombiaId",
                    Active = true
                }
            };
            return items;
        }
    }
}
