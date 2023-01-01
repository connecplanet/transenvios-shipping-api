using Transenvios.Shipping.Api.Domains.CatalogService;

namespace Transenvios.Shipping.Api.Mediators.CatalogService
{
    public class IdTypeMediator : ICatalogQuery<IdType>
    {
        public async Task<IList<IdType>> GetAllAsync()
        {
            var items = new List<IdType>
            {
                new() { Id = Guid.NewGuid(), Code = "C.C.", Name = "Cédula", Active = true },
                new() { Id = Guid.NewGuid(), Code = "C.E.", Name = "Cédula de Extranjería", Active = true },
                new() { Id = Guid.NewGuid(), Code = "NIT", Name = "NIT", Active = true }
            };
            return await Task.FromResult(items);
        }
    }
}
