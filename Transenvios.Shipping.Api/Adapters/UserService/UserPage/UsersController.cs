using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Transenvios.Shipping.Api.Adapters.UserService.AuthorizationEntity;
using Transenvios.Shipping.Api.Domains.UserService.UserPage;
using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Adapters.UserService.UserPage
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

        [AllowAnonymous, HttpPost("Authenticate")] // sign-in
        public async Task<IActionResult> AuthenticateAsync(UserAuthenticateRequest model)
        {
            var response = await _userProcessor.AuthenticateAsync(model);
            return Ok(response);
        }

        [AllowAnonymous, HttpPost] // sign-up
        public async Task<ActionResult<UserStateResponse>> RegisterAsync(UserRegisterRequest model)
        {
            var response = await _userProcessor.RegisterAsync(model);
            return Ok(response);
        }

        [AllowAnonymous, HttpPost("ForgotPassword")] // forgot-password
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
        public async Task<ActionResult<UserAuthenticateResponse>> GetByIdAsync(Guid id)
        {
            var user = await _userProcessor.GetByIdAsync(id);

            if (user == null) 
            {
                return NotFound();
            }

            return Ok(user);
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