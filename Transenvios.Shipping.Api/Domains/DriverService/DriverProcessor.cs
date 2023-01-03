using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Domains.DriverService
{
    public class DriverProcessor
    {
        private readonly IDriverStorage _storage;

        public DriverProcessor(IDriverStorage getDriver)
        {
            _storage = getDriver ?? throw new ArgumentNullException(nameof(getDriver));
        }

        public async Task<IList<Driver>> GetAsync()
        {
            return await _storage.GetAllAsync();
        }

        private async Task<Driver> GetAsync(Guid id)
        {
            var user = await _storage.GetAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("Driver not found");
            }
            return user;
        }

        public async Task<DriverStateResponse> UpdateAsync(Guid id, Driver model)
        {
            var currentDriver = await GetAsync(id);

            var items = await _storage.UpdateAsync(model);

            return new DriverStateResponse
            {
                Id = currentDriver.Id,
                Items = items,
                Message = "Driver updated successfully"
            };
        }

        public async Task<DriverStateResponse> AddAsync(Driver model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.DocumentType) || string.IsNullOrWhiteSpace(model.Email) ||
                    string.IsNullOrWhiteSpace(model.Phone) || Guid.Empty == model.PickUpCityId ||
                    string.IsNullOrWhiteSpace(model.PickUpAddress) || string.IsNullOrWhiteSpace(model.FirstName))
                {
                    throw new AppException("Some required fields are missing");
                }

                var invalidEmail = model.Email != null && await _storage.Exists(model.Email);

                if (invalidEmail)
                {
                    throw new AppException($"Email '{model.Email}' is already registered");
                }

                var invalidDocument = model.DocumentId != null && await _storage.Exists(model.DocumentId);

                if (invalidDocument)
                {
                    throw new AppException($"Document ID '{model.DocumentId}' is already registered");
                }
                model.Id = Guid.NewGuid();
                var items = await _storage.AddAsync(model);

                return new DriverStateResponse
                {
                    Id = model.Id,
                    Items = items,
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
            var items = await _storage.DeleteAsync(user);

            return new DriverStateResponse
            {
                Id = user.Id,
                Items = items,
                Message = "Driver deleted successfully"
            };
        }
    }
}
