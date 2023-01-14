using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Transenvios.Shipping.Api.Domains.UserService;
using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Adapters.UserService
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly AppSettings _appSettings;
        private readonly UserProcessor _userProcessor;

        public UsersController(
            ILogger<UsersController> logger,
            IOptions<AppSettings> appSettings,
            UserProcessor userProcessor
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userProcessor = userProcessor ?? throw new ArgumentNullException(nameof(userProcessor));
            _appSettings = appSettings.Value ?? throw new ArgumentNullException(nameof(appSettings));
        }

        /// <summary>
        /// Authenticate the user as a Employee or a Customer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous, HttpPost("Authenticate")]
        public async Task<IActionResult> SignInAsync(UserAuthenticateRequest model)
        {
            var response = await _userProcessor.SignInAsync(model);
            return Ok(response);
        }

        /// <summary>
        /// Register the user as a Employee or a Customer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous, HttpPost]
        public async Task<ActionResult<UserStateResponse>> SignUpAsync(UserRegisterRequest model)
        {
            var response = await _userProcessor.SignUpAsync(model);
            return Ok(response);
        }

        [AllowAnonymous, HttpPost("ForgotPassword")]
        public async Task<ActionResult<UserStateResponse>> ForgotPassword(UserAuthenticateRequest? data)
        {
            if (data == null || string.IsNullOrWhiteSpace(data.Email))
            {
                return BadRequest();
            }

            var response = await _userProcessor.PasswordResetAsync(data.Email);
            return Ok(response);
        }

        [AllowAnonymous, HttpPost("reset-password")]
        public async Task<ActionResult<UserStateResponse>> ResetPassword(UserAuthenticateRequest data)
        {
            throw new NotImplementedException();
        }

        [HttpPost("sign-in-with-token")]
        public async Task<ActionResult<UserSignInResponse>> SignInWithToken(UserTokenRequest data)
        {
            var response = await _userProcessor.SignInWithTokenAsync(data);
            return Ok(response);
        }

        [AllowAnonymous, HttpPost("unlock-session")]
        public async Task<ActionResult<UserStateResponse>> UnlockSession(string accessToken)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<ActionResult<IList<UserAuthenticateResponse>>> GetAllAsync()
        {
            var users = await _userProcessor.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserAuthenticateResponse>> GetAsync(Guid id)
        {
            var response = await _userProcessor.GetAsync(id);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserStateResponse>> UpdateAsync(Guid id, UserUpdateRequest model)
        {
            var response = await _userProcessor.UpdateAsync(id, model);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<UserStateResponse>> DeleteAsync(Guid id)
        {
            var response = await _userProcessor.DeleteAsync(id);
            return Ok(response);
        }
    }
}