using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Transenvios.Shipping.Api.Adapters.UserService.AuthorizationEntity;
using Transenvios.Shipping.Api.Domains.MailService.MailPage;
using Transenvios.Shipping.Api.Domains.UserService.UserPage;
using Transenvios.Shipping.Api.Infraestructure;
using Transenvios.Shipping.Api.Mediators.UserService.ManagerMail;

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
        
        private readonly MailProcessor _mailProcessor;


        public UsersController(
            ILogger<UsersController> logger,
            IOptions<AppSettings> appSettings,
            UserProcessor userProcessor
            ,MailProcessor mailProcessor
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userProcessor = userProcessor ?? throw new ArgumentNullException(nameof(userProcessor));
            _mailProcessor = mailProcessor ?? throw new ArgumentNullException(nameof(mailProcessor)); 
            _appSettings = appSettings.Value ?? throw new ArgumentNullException(nameof(appSettings));
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(UserAuthenticateRequest model)
        {
            var response = await _userProcessor.AuthenticateAsync(model);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost()]
        public async Task<ActionResult<UserStateResponse>> RegisterAsync(UserRegisterRequest model)
        {
            if (string.IsNullOrEmpty(model.Role))
            {
                model.Role = UserConstants.Requester;
            }

            var response = await _userProcessor.RegisterAsync(model);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("forgotPassword")]
        public async Task<ActionResult<UserStateResponse>> ForgotPassword(UserAuthenticateRequest data)
        {

            var response = await _mailProcessor.PasswordResetAsync(data.Email);
            return Ok(response);

            //return null;
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