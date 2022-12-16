
using Transenvios.Shipping.Api.Domains.DriverService.DriverPage;
using Transenvios.Shipping.Api.Domains.UserService.UserPage;
using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Domains.DriverService.DriverPage
{
    public class DriverProcessor
    {
        private readonly IDriver _iDriver;
        
        public DriverProcessor(IDriver getDriver)
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
            var user = await _iDriver.GetByIdAsync(id);
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

                var invalidEmail = model.Email != null && await _iDriver.ExistsEmail(model.Email);

                if (invalidEmail)
                {
                    throw new AppException($"Email '{model.Email}' is already registered");
                }

                var invalidDocument = model.DocumentId != null && await _iDriver.ExistsDocument(model.DocumentId);

                if (invalidDocument)
                {
                    throw new AppException($"Document ID '{model.DocumentId}' is already registered");
                }
                model.Id = Guid.NewGuid();
                var items = await _iDriver.RegisterAsync(model);

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
            var items = await _iDriver.RemoveAsync(user);

            return new DriverStateResponse
            {
                Id = user.Id,
                Items = items,
                Message = "Driver deleted successfully"
            };
        }
    }
}
