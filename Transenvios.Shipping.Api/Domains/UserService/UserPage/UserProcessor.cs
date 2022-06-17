using AutoMapper;
using Transenvios.Shipping.Api.Domains.UserService.AuthorizationEntity;
using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Domains.UserService.UserPage
{
    public class UserProcessor
    {
        private readonly IMapper _mapper;
        private readonly IJwtUtils _jwtUtils;
        private readonly IRegisterUser _registerUser;
        private readonly IGetUser _getUser;        
        private readonly IUpdateUser _updateUser;
        private readonly IRemoveUser _removeUser;

        public UserProcessor(
            IMapper mapper,
            IJwtUtils jwtUtils,
            IRegisterUser registerUser,
            IGetUser getUser,
            IUpdateUser updateUser,
            IRemoveUser removeUser)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _jwtUtils = jwtUtils ?? throw new ArgumentNullException(nameof(jwtUtils));
            _registerUser = registerUser ?? throw new ArgumentNullException(nameof(registerUser));
            _getUser = getUser ?? throw new ArgumentNullException(nameof(getUser));
            _updateUser = updateUser ?? throw new ArgumentNullException(nameof(updateUser));
            _removeUser = removeUser ?? throw new ArgumentNullException(nameof(removeUser));
        }

        public async Task<UserStateResponse> RegisterAsync(UserRegisterRequest model)
        {

            try
            {

            // validate
            if (await _getUser.ExistsEmail(model.Email))
            {
                throw new AppException($"Email '{model.Email}' is already registered");
            }

            // map model to new user object
            var user = _mapper.Map<User>(model);

            // hash password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
            var items = await _registerUser.RegisterAsync(user);

            return new UserStateResponse
            {
                Id = user.Id,
                Items = items,
                Message = "Registration successful"
            };


            }
            catch (Exception ex)
            {

                return new UserStateResponse
                {
                    Message = ex.Message
                };
            }
        }

        public async Task<IList<UserAuthenticateResponse>> GetAllAsync()
        {
            var response = await _getUser.GetAllAsync();
            var users = _mapper.Map<IList<UserAuthenticateResponse>>(response);
            return users;
        }

        public async Task<UserAuthenticateResponse> GetByIdAsync(Guid id)
        {
            var user = await GetUserAsync(id);
            var response = _mapper.Map<UserAuthenticateResponse>(user);
            return response;
        }

        public async Task<UserAuthenticateResponse> AuthenticateAsync(UserAuthenticateRequest model)
        {
            var user = await _getUser.GetByEmailAsync(model.Email);

            // validate
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash)) {
                throw new AppException("Username or password is incorrect");
            }

            // authentication successful
            var response = _mapper.Map<UserAuthenticateResponse>(user);
            response.Token = _jwtUtils.GenerateToken(user);
            return response;
        }

        public async Task<UserStateResponse> UpdateAsync(Guid id, UserUpdateRequest model)
        {
            var currentUser = await GetUserAsync(id);
            var emailUser = await _getUser.GetByEmailAsync(model.Email);

            // validate
            if (emailUser != null && currentUser.Id != emailUser.Id)
            {
                throw new AppException($"Email '{model.Email}' is already taken");
            }

            // hash password if it was entered
            if (!string.IsNullOrEmpty(model.Password))
            {
                currentUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
            }

            // copy model to user and save
            _mapper.Map(model, currentUser);
            var items = await _updateUser.UpdateAsync(currentUser);
            
            return new UserStateResponse
            {
                Id = currentUser.Id,
                Items = items,
                Message = "User updated successfully"
            };
        }

        public async Task<UserStateResponse> DeleteAsync(Guid id)
        {
            var user = await GetUserAsync(id);
            var items = await _removeUser.RemoveAsync(user);

            return new UserStateResponse
            {
                Id = user.Id,
                Items = items,
                Message = "User deleted successfully"
            };
        }

        private async Task<User> GetUserAsync(Guid id)
        {
            var user = await _getUser.GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            return user;
        }
    }
}