using AutoMapper;
using Transenvios.Shipping.Api.Domains.UserService.UserPage;
using Transenvios.Shipping.Api.Infraestructure;
using Transenvios.Shipping.Api.Mediators.UserService.UserPage;

namespace Transenvios.Shipping.Api.Domains.MailService.MailPage
{

    public class MailProcessor
    {
        private readonly IPasswordReset _passwordReset;
        private readonly IGetUser _getUser;
        private readonly IUpdateUser _updateUser;
        private readonly IMapper _mapper;

        public MailProcessor(IPasswordReset passwordReset
            ,IMapper mapper
            , IGetUser getUser
            , IUpdateUser updateUser
            )
        {
            _updateUser = updateUser ?? throw new ArgumentNullException(nameof(updateUser));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _getUser = getUser ?? throw new ArgumentNullException(nameof(getUser));
            
            _passwordReset = passwordReset ?? throw new ArgumentNullException(nameof(passwordReset));
        }
        public async Task<UserStateResponse> PasswordResetAsync(string email)
        {
            var user = await _getUser.GetByEmailAsync(email);

            // validate
            if (user == null)
            {
                throw new AppException("Username not exit");
            }
            var random = new Random();

            var value = random.Next(0, 100);

            string newPassword = string.Concat(user.FirstName, user.FirstName,value);

            user.PasswordHash= BCrypt.Net.BCrypt.HashPassword(newPassword);

            var update = _updateUser.UpdateAsync(user);

            var emailUser = await _passwordReset.PasswordResetAsync(email, newPassword);

            // authentication successful
            return new UserStateResponse
            {
                Message = emailUser.Message
            };
        }

        public void ManangerMail(string parameterReset)
        {
            //return EnviarCorreo("inforfercho@gmail.com",
            //                           "Prueba",
            //                           "<h1>Mensaje en HTML<h1><p>Contenido</p>",
            //                           true);
        }

    }
}
