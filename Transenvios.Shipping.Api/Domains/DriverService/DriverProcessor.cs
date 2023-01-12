using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Domains.UserService;
using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Domains.DriverService
{
    public class DriverProcessor
    {
        private readonly IDriverStorage _driverMediator;
        private readonly ICatalogStorage<City> _cityMediator;

        public DriverProcessor(IDriverStorage driverMediator, ICatalogStorage<City> cityMediator)
        {
            _driverMediator = driverMediator ?? throw new ArgumentNullException(nameof(driverMediator));
            _cityMediator = cityMediator ?? throw new ArgumentNullException(nameof(cityMediator));
        }

        public async Task<IList<Driver>> GetAsync()
        {
            return await _driverMediator.GetAllAsync();
        }

        public async Task<Driver> GetAsync(Guid id)
        {
            var user = await _driverMediator.GetAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("Driver not found");
            }
            return user;
        }

        public async Task<DriverStateResponse> UpdateAsync(Guid id, Driver model)
        {
            var currentDriver = await GetAsync(id);

            var items = await _driverMediator.UpdateAsync(model);

            return new DriverStateResponse
            {
                Id = currentDriver.Id,
                Items = items,
                Message = "Driver updated successfully"
            };
        }

        public async Task<DriverStateResponse> AddAsync(DriverRequest modelDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(modelDto.DocumentType) || string.IsNullOrWhiteSpace(modelDto.Email) ||
                    string.IsNullOrWhiteSpace(modelDto.Phone) || string.IsNullOrEmpty(modelDto.PickUpCityId) ||
                    string.IsNullOrWhiteSpace(modelDto.PickUpAddress) || string.IsNullOrWhiteSpace(modelDto.FirstName))
                {
                    throw new AppException("Some required fields are missing");
                }

                var invalidEmail = modelDto.Email != null && await _driverMediator.Exists(modelDto.Email);
                if (invalidEmail)
                {
                    throw new AppException($"Email '{modelDto.Email}' is already registered");
                }

                var invalidDocument = modelDto.DocumentId != null && await _driverMediator.Exists(modelDto.DocumentId);
                if (invalidDocument)
                {
                    throw new AppException($"Document ID '{modelDto.DocumentId}' is already registered");
                }

                if (!Guid.TryParse(modelDto.PickUpCityId, out var pickUpCityId))
                {
                    var pickUpCity = await _cityMediator.GetAsync(modelDto.PickUpCityId);
                    if (pickUpCity == null)
                    {
                        throw new AppException($"City '{modelDto.PickUpCityId}' is not registered");
                    }
                    pickUpCityId = pickUpCity.Id;
                }

                var countryNumber = (modelDto.CountryCode == UserConstants.ColombiaCode
                    ? UserConstants.ColombiaNumber
                    : modelDto.CountryCode)?.Replace(UserConstants.DialPrefix, string.Empty);
                
                if(!int.TryParse(countryNumber, out var countryId))
                {
                    countryId = int.Parse(UserConstants.ColombiaNumber);
                }

                if (!int.TryParse(modelDto.DocumentId, out var documentId))
                {
                    documentId = 0;
                }

                var model = new Driver
                {
                    Id = Guid.NewGuid(),
                    FirstName = modelDto.FirstName,
                    LastName = modelDto.LastName,
                    Email = modelDto.Email,
                    CountryCode = countryId.ToString(),
                    DocumentType = modelDto.DocumentType,
                    DocumentId = documentId.ToString(),
                    Phone = modelDto.Phone,
                    PickUpAddress = modelDto.PickUpAddress,
                    PickUpCityId = pickUpCityId
                };

                var itemsAffected = await _driverMediator.AddAsync(model);

                return new DriverStateResponse
                {
                    Id = model.Id,
                    Items = itemsAffected,
                    Message = "Registration successful"
                };
            }
            catch (Exception ex)
            {
                return new DriverStateResponse
                {
                    Message = ex.GetBaseException().Message
                };
            }
        }

        public async Task<DriverStateResponse> DeleteAsync(Guid id)
        {
            var user = await GetAsync(id);
            var items = await _driverMediator.DeleteAsync(user);

            return new DriverStateResponse
            {
                Id = user.Id,
                Items = items,
                Message = "Driver deleted successfully"
            };
        }
    }
}
