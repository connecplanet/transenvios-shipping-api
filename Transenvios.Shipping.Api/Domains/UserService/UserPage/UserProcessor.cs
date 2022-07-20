using AutoMapper;
using Transenvios.Shipping.Api.Domains.UserService.AuthorizationEntity;
using Transenvios.Shipping.Api.Infraestructure;
using Transenvios.Shipping.Api.Mediators.UserService.ForgotPasswordPage;
using Transenvios.Shipping.Api.Mediators.UserService.UserPage;

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
        private readonly IGetAuthorizeUser _getAuthorizeUser;
        private readonly IPasswordReset _passwordReset;

        public UserProcessor(
            IMapper mapper,
            IJwtUtils jwtUtils,
            IRegisterUser registerUser,
            IGetUser getUser,
            IUpdateUser updateUser,
            IRemoveUser removeUser,
            IGetAuthorizeUser getAuthorizeUser,
            IPasswordReset passwordReset
            )
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _jwtUtils = jwtUtils ?? throw new ArgumentNullException(nameof(jwtUtils));
            _registerUser = registerUser ?? throw new ArgumentNullException(nameof(registerUser));
            _getUser = getUser ?? throw new ArgumentNullException(nameof(getUser));
            _updateUser = updateUser ?? throw new ArgumentNullException(nameof(updateUser));
            _removeUser = removeUser ?? throw new ArgumentNullException(nameof(removeUser));
            _getAuthorizeUser = getAuthorizeUser ?? throw new ArgumentNullException(nameof(getAuthorizeUser));
            _passwordReset = passwordReset ?? throw new ArgumentNullException(nameof(passwordReset));
        }

        public async Task<UserStateResponse> RegisterAsync(UserRegisterRequest model)
        {
            try
            {
                var result = await _getUser.ExistsEmail(model.Email);

                if (result)
                {
                    throw new AppException($"Email '{model.Email}' is already registered");
                }

                var user = _mapper.Map<User>(model);
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
                    Message = ex.GetBaseException().Message
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

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                throw new AppException("Username or password is incorrect");
            }

            var response = _mapper.Map<UserAuthenticateResponse>(user);
            response.Token = _jwtUtils.GenerateToken(user);
            return response;
        }

        public async Task<UserStateResponse> UpdateAsync(Guid id, UserUpdateRequest model)
        {
            var currentUser = await GetUserAsync(id);
            var emailUser = await _getUser.GetByEmailAsync(model.Email);

            if (emailUser != null && currentUser.Id != emailUser.Id)
            {
                throw new AppException($"Email '{model.Email}' is already taken");
            }

            if (!string.IsNullOrEmpty(model.Password))
            {
                currentUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
            }

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
            var user = await _getAuthorizeUser.GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            return user;
        }
        public async Task<UserStateResponse> PasswordResetAsync(string email)
        {
            var user = await _getUser.GetByEmailAsync(email);

            if (user == null)
            {
                throw new AppException("Username not exit");
            }

            var random = new Random();
            var value = random.Next(0, 100);
            string newPassword = string.Concat(user.FirstName, user.FirstName, value);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            var update = _updateUser.UpdateAsync(user);
            var emailUser = await _passwordReset.PasswordResetAsync(email, newPassword);

            return new UserStateResponse
            {
                Message = emailUser.Message
            };
        }
    }
}