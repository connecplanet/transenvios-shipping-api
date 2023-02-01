using Microsoft.EntityFrameworkCore;
using System.Net;
using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Domains.CatalogService
{
    public class CityProcessor
    {
        private readonly ICatalogStorage<City> _mediator;

        public CityProcessor(ICatalogStorage<City> cityMediator)
        {
            _mediator = cityMediator ?? throw new ArgumentNullException(nameof(cityMediator));
        }

        public async Task<IList<City>> GetAllAsync()
        {
            return await _mediator.GetAllAsync();
        }

        public async Task<City> GetAsync(Guid id)
        {
            return await _mediator.GetAsync(id);
        }

        public async Task<CatalogStateResponse> UpdateAsync(Guid id, City modelDto)
        {
            if (modelDto == null || id != modelDto.Id)
            {
                throw new AppException(HttpStatusCode.BadRequest);
            }
            var model = await _mediator.GetAsync(id);
            if (model == null)
            {
                throw new AppException(HttpStatusCode.NotFound);
            }

            var exits = _mediator.Exists(id, modelDto.Code);
            if (exits)
            {
                throw new AppException(HttpStatusCode.UnprocessableEntity);
            }

            model.Code = modelDto.Code;
            model.Name = modelDto.Name;
            model.Active = modelDto.Active;

            int itemsAffected;
            try
            {
                itemsAffected = await _mediator.UpdateAsync(model);
            }
            catch (DbUpdateConcurrencyException) when (!_mediator.Exists(id))
            {
                throw new AppException(HttpStatusCode.NotFound);
            }

            return new CatalogStateResponse
            {
                Id = model.Id,
                Items = itemsAffected,
                Message = $"{HttpStatusCode.OK}"
            };
        }

        public async Task<CatalogStateResponse> AddAsync(City modelDto)
        {
            var id = Guid.NewGuid();
            var exits = _mediator.Exists(id, modelDto.Code);
            if (exits)
            {
                throw new AppException(HttpStatusCode.UnprocessableEntity);
            }

            var model = new City
            {
                Code = modelDto.Code,
                Name = modelDto.Name,
                Active = modelDto.Active,
                Id = id
            };

            try
            {
                var itemsAffected = await _mediator.AddAsync(model);

                return new CatalogStateResponse
                {
                    Id = model.Id,
                    Items = itemsAffected,
                    Message = $"{HttpStatusCode.OK}"
                };
            }
            catch (Exception error)
            {
                return new CatalogStateResponse
                {
                    Message = error.GetBaseException().Message
                };
            }
        }

        public async Task<CatalogStateResponse> DeleteAsync(Guid id)
        {
            var model = await _mediator.GetAsync(id);
            if (model == null)
            {
                throw new AppException(HttpStatusCode.NotFound);
            }

            var itemsAffected = await _mediator.DeleteAsync(model);

            return new CatalogStateResponse
            {
                Id = model.Id,
                Items = itemsAffected,
                Message = $"{HttpStatusCode.OK}"
            };
        }
    }
}
