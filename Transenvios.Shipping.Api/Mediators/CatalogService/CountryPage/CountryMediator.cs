using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Domains.CatalogService.CountryPage;

namespace Transenvios.Shipping.Api.Mediators.CatalogService.CountryPage
{
    public class CountryMediator : IGetCatalog<Country>
    {
        public async Task<IList<Country>> GetAllAsync()
        {
            var items = new List<Country>
            {
                new Country
                {
                    Id = Guid.NewGuid(),
                    Code = "+57",
                    Name = "Colombia",
                    Active = true
                }
            };
            return items;
        }
    }
}
