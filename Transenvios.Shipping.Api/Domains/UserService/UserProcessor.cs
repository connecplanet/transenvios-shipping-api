using AutoMapper;
using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Domains.ClientService;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.Entities;
using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Domains.UserService
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
        private readonly ClientProcessor _clientProcess;

        public UserProcessor(
            IMapper mapper,
            IJwtUtils jwtUtils,
            IRegisterUser registerUser,
            IGetUser getUser,
            IUpdateUser updateUser,
            IRemoveUser removeUser,
            IGetAuthorizeUser getAuthorizeUser,
            IPasswordReset passwordReset,
            ClientProcessor clientProcess
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
            _clientProcess = clientProcess ?? throw new ArgumentNullException(nameof(clientProcess));
        }

        public async Task<UserStateResponse> SignUpAsync(UserRegisterRequest model)
        {
            try
            {
                var roleValue = int.TryParse(model.Role, out var enumValue) && Enum.IsDefined(typeof(UserRoles), enumValue)
                    ? (UserRoles)enumValue
                    : UserRoles.Customer;
                model.Role = ((int)roleValue).ToString();

                var countryCode = !string.IsNullOrWhiteSpace(model.CountryCode) &&
                                  int.TryParse(model.CountryCode.Replace("+", string.Empty), out var countryId)
                    ? countryId.ToString()
                    : UserConstants.ColombiaNumber;
                model.CountryCode = countryCode;

                var user = _mapper.Map<User>(model);
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
                user.Active = true;

                if (roleValue == UserRoles.Employee)
                {
                    return await EmployeeSignUp(model, user);
                }

                return await CustomerSignUp(user, roleValue);
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

        public async Task<UserAuthenticateResponse> GetAsync(Guid id)
        {
            var user = await GetUserAsync(id);
            var response = _mapper.Map<UserAuthenticateResponse>(user);
            return response;
        }
        public async Task<UserAuthenticateResponse> SignInAsync(UserAuthenticateRequest model)
        {
            if (string.IsNullOrWhiteSpace(model.Email))
            {
                throw new AppException("Email is required.");
            }

            var roleValue = int.TryParse(model.Role, out var enumValue) && Enum.IsDefined(typeof(UserRoles), enumValue)
                ? (UserRoles)enumValue
                : UserRoles.Customer;
            model.Role = ((int)roleValue).ToString();

            Person person;

            if (roleValue == UserRoles.Employee)
            {
                person = await _getUser.GetAsync(model.Email);
            }
            else
            {
                person = await _clientProcess.GetAsync(model.Email);
            }

            if (person == null || !BCrypt.Net.BCrypt.Verify(model.Password, person.PasswordHash))
            {
                throw new AppException("Username or password is incorrect.");
            }

            var response = _mapper.Map<UserAuthenticateResponse>(person);

            response.Token = _jwtUtils.GenerateToken(person);
            response.Avatar = "assets/images/avatars/transenvios.png";
            response.Status = "online";
            return response;
        }

        public async Task<UserStateResponse> UpdateAsync(Guid id, UserUpdateRequest model)
        {
            if (string.IsNullOrWhiteSpace(model.Email))
            {
                throw new AppException("Email is required.");
            }

            var currentUser = await GetUserAsync(id);
            var emailUser = await _getUser.GetAsync(model.Email);

            if (emailUser != null && currentUser.Id != emailUser.Id)
            {
                throw new AppException($"Email '{model.Email}' is already taken.");
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
            var items = await _removeUser.DeleteAsync(user);

            return new UserStateResponse
            {
                Id = user.Id,
                Items = items,
                Message = "User deleted successfully"
            };
        }

        public async Task<UserStateResponse> PasswordResetAsync(string email)
        {
            var user = await _getUser.GetAsync(email);

            if (user == null)
            {
                throw new AppException("Username not exit");
            }

            var random = new Random();
            var value = random.Next(0, 1000);
            var newPassword = string.Concat(user.FirstName, user.LastName, value);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            var updates = await _updateUser.UpdateAsync(user);
            var emailUser = await _passwordReset.PasswordResetAsync(email, newPassword);

            return new UserStateResponse
            {
                Items = updates,
                Message = emailUser.Message
            };
        }

        public async Task<UserSignInResponse> SignInWithTokenAsync(UserTokenRequest data)
        {
            if (data == null || string.IsNullOrWhiteSpace(data.AccessToken))
            {
                throw new AppException("Token is required.");
            }

            var userId = _jwtUtils.ValidateToken(data.AccessToken);
            if (userId == null)
            {
                throw new AppException("Token user is not valid.");
            }

            var user = await GetAsync(userId.Value);
            var response = new UserSignInResponse
            {
                User = user,
                AccessToken = data.AccessToken
            };
            if (response.User == null)
            {
                throw new AppException("Token is not valid.");
            }

            return response;
        }

        private async Task<User> GetUserAsync(Guid id)
        {
            var user = await _getAuthorizeUser.GetAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            return user;
        }

        private async Task<UserStateResponse> CustomerSignUp(User user, UserRoles roleValue)
        {
            var customer = new ClientUpdateRequest
            {
                DocumentType = user.DocumentType,
                DocumentId = user.DocumentId,
                Email = user.Email,
                CountryCode = user.CountryCode,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PasswordHash = user.PasswordHash,
                Phone = user.Phone,
                Role = ((int)roleValue).ToString(),
            };
            var addResult = await _clientProcess.AddAsync(customer);
            return new UserStateResponse
            {
                Id = addResult.Id,
                Items = addResult.Items,
                Message = addResult.Message
            };
        }

        private async Task<UserStateResponse> EmployeeSignUp(UserRegisterRequest model, User user)
        {
            var validationResult = model.Email != null && await _getUser.Exists(model.Email);

            if (validationResult)
            {
                throw new AppException($"Email '{model.Email}' is already registered");
            }

            var itemsAffected = await _registerUser.SignUpAsync(user);
            return new UserStateResponse
            {
                Id = user.Id,
                Items = itemsAffected,
                Message = "Registration successful"
            };
        }
    }
}