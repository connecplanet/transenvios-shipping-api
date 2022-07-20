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
                Credentials = new NetworkCredential(_appSettings.EmailUser, _appSettings.EmailPassword)
            };
        }
        public bool EnviarCorreo(string destinatario, string asunto, string mensaje, bool esHtlm = false)
        {
            email = new MailMessage(_appSettings.EmailUser, destinatario, asunto, mensaje);
            email.IsBodyHtml = esHtlm;
            cliente.Send(email);
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

        public async Task<UserStateResponse> PasswordResetAsync(string mail, string newPassword)
        {
            try
            {
                var sendValue = EnviarCorreo(mail,
                                         "Cambio contraseña TransEnvios",
                                         "<p>Nueva Contraseña</p>" +
                                         "<h1>" + newPassword + "<h1>",
                                         true);

                return new UserStateResponse
                {
                    Message = "Mail send is " + sendValue
                };
            }
            catch (Exception ex)
            {
                return new UserStateResponse
                {
                    Message = ex.InnerException.ToString()
                };
            }
        }
    }
}
