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
        
        //private readonly UserMediator UserMediator;
        private readonly IMapper _mapper;


        public MailProcessor(IPasswordReset passwordReset
            //, UserMediator _userMediator
               ,  IMapper mapper
            , IGetUser getUser


            )
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _getUser= getUser ?? throw new ArgumentNullException(nameof(getUser));
            //UserMediator = _userMediator;
            _passwordReset = passwordReset ?? throw new ArgumentNullException(nameof(passwordReset));
        }
        public async Task<UserStateResponse> PasswordResetAsync(string email)
        {
            

            var emailUser = await _passwordReset.PasswordResetAsync(email);

            //var response = await UserMediator.UpdateAsync(emailUser);

            var user1 = await _getUser.GetByEmailAsync(email);

            var user = await _passwordReset.PasswordResetAsync("");

            // validate
            if (user == null)
            {
                throw new AppException("Username not exit");
            }

            // authentication successful
            var response1 = _passwordReset.ManangerMail(email);

            return new UserStateResponse
            {
                Message = "User deleted successfully"
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
