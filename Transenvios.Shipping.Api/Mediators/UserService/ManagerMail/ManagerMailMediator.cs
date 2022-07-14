using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using Transenvios.Shipping.Api.Domains.UserService.UserPage;
using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Mediators.UserService.ManagerMail
{
    public class ManagerMailMediator : IPasswordReset
    {
        private SmtpClient cliente;
        private readonly AppSettings _appSettings;
        private MailMessage email;
        public ManagerMailMediator(
            IOptions<AppSettings> appSettings)
        {


            _appSettings = appSettings.Value;

            cliente = new SmtpClient(_appSettings.EmailHost, Int32.Parse(_appSettings.EmailPort))
            {
                EnableSsl = _appSettings.EmailEnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                //Port = Int32.Parse(_appSettings.EmailPort),

                Credentials = new NetworkCredential(_appSettings.EmailUser, _appSettings.EmailPassword)
            };
        }
        public bool EnviarCorreo(string destinatario, string asunto, string mensaje, bool esHtlm = false)
        {
            try
            {
                email = new MailMessage(_appSettings.EmailUser, destinatario, asunto, mensaje);
                email.IsBodyHtml = esHtlm;
                cliente.Send(email);

            }
            catch (Exception)
            {

                throw;
            }

            return true;
        }
        public void EnviarCorreo(MailMessage message)
        {
            cliente.Send(message);
        }
        public async Task EnviarCorreoAsync(MailMessage message)
        {
            await cliente.SendMailAsync(message);
        }

        public async Task<UserStateResponse> PasswordResetAsync(string mail)
        {
            try
            {
              var sendValue=  EnviarCorreo(mail,
                                       "Cambio contraseña TransEnvios",
                                       "<h1>Mensaje en HTML<h1><p>Contenido</p>",
                                       true);

                return new UserStateResponse
                {
                    Message = "Mail send is " + sendValue
                };
            }
            catch (Exception)
            {
                throw;
            }
          }

        public bool ManangerMail(string parameterReset)
            {
                throw new NotImplementedException();
            }


            //public Task<UserStateResponse> PasswordResetAsync(UserRegisterRequest data)
            //{
            //    var user = await _passwordReset.PasswordResetAsync(model);

            //    // validate
            //    if (user == null)
            //    {
            //        throw new AppException("Username not exit");
            //    }

            //    // authentication successful
            //    var response = _passwordReset.ManangerMail(data.Email);

            //    return new UserStateResponse
            //    {
            //        Message = "User deleted successfully"
            //    };
            //}

            //public bool ManangerMail(string parameterReset)
            //{
            //    return  EnviarCorreo("inforfercho@gmail.com",
            //                               "Prueba",
            //                               "<h1>Mensaje en HTML<h1><p>Contenido</p>",
            //                               true);
            //}
        }
    }
