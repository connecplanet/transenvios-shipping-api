using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Domains.DriverService
{
    public class DriverProcessor
    {
        private readonly IDriverStorage _iDriver;

        public DriverProcessor(IDriverStorage getDriver)
        {
            _iDriver = getDriver
                ?? throw new ArgumentNullException(nameof(getDriver));
        }

        public async Task<IList<Driver>> GetDriversAsync()
        {
            return await _iDriver.GetAllAsync();
        }

        private async Task<Driver> GetDriverAsync(Guid id)
        {
            var user = await _iDriver.GetAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("Driver not found");
            }
            return user;
        }

        public async Task<DriverStateResponse> UpdateAsync(Guid id, Driver model)
        {
            var currentDriver = await GetDriverAsync(id);

            var items = await _iDriver.UpdateAsync(model);

            return new DriverStateResponse
            {
                Id = currentDriver.Id,
                Items = items,
                Message = "Driver updated successfully"
            };
        }

        public async Task<DriverStateResponse> RegisterAsync(Driver model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.DocumentType) || string.IsNullOrWhiteSpace(model.Email) ||
                    string.IsNullOrWhiteSpace(model.Phone) || string.IsNullOrWhiteSpace(model.PickUpCityId) ||
                    string.IsNullOrWhiteSpace(model.PickUpAddress) || string.IsNullOrWhiteSpace(model.FirstName))
                {
                    throw new AppException("Some required fields are missing");
                }

                var invalidEmail = model.Email != null && await _iDriver.Exists(model.Email);

                if (invalidEmail)
                {
                    throw new AppException($"Email '{model.Email}' is already registered");
                }

                var invalidDocument = model.DocumentId != null && await _iDriver.Exists(model.DocumentId);

                if (invalidDocument)
                {
                    throw new AppException($"Document ID '{model.DocumentId}' is already registered");
                }
                model.Id = Guid.NewGuid();
                var items = await _iDriver.AddAsync(model);

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
            var user = await GetDriverAsync(id);
            var items = await _iDriver.DeleteAsync(user);

            return new DriverStateResponse
            {
                Id = user.Id,
                Items = items,
                Message = "Driver deleted successfully"
            };
        }
    }
}
