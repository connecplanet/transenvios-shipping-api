using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Domains.CatalogService.IdTypePage;

namespace Transenvios.Shipping.Api.Mediators.CatalogService.IdTypePage
{
    public class IdTypeMediator : IGetCatalog<IdType>
    {
        public async Task<IList<IdType>> GetAllAsync()
        {
            var items = new List<IdType>
            {
                new IdType
                {
                    Id = Guid.NewGuid(),
                    Code = "C.C.",
                    Name = "Cédula",
                    Active = true
                },
                new IdType
                {
                    Id = Guid.NewGuid(),
                    Code = "C.E.",
                    Name = "Cédula de Extranjería",
                    Active = true
                },
                new IdType
                {
                    Id = Guid.NewGuid(),
                    Code = "NIT",
                    Name = "NIT",
                    Active = true
                }
            };
            return items;
        }
    }
}
