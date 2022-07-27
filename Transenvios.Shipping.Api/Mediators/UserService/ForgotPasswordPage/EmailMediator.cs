using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using Transenvios.Shipping.Api.Domains.UserService.UserPage;
using Transenvios.Shipping.Api.Infraestructure;

namespace Transenvios.Shipping.Api.Mediators.UserService.ForgotPasswordPage
{
    public class EmailMediator : IPasswordReset
    {
        private SmtpClient cliente;
        private readonly AppSettings _appSettings;
        private MailMessage email;
        public EmailMediator(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;

            cliente = new SmtpClient(_appSettings.Email.Host, int.Parse(_appSettings.Email.Port))
            {
                EnableSsl = _appSettings.Email.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_appSettings.Email.User, _appSettings.Email.Password)
            };
        }
        public bool SendEmail(string mailTo, string subject, string body, bool isHtml = false)
        {
            email = new MailMessage(_appSettings.Email.User, mailTo, subject, body)
            {
                IsBodyHtml = isHtml
            };
            cliente.Send(email);
            return true;
        }
        public void SendEmail(MailMessage message)
        {
            cliente.Send(message);
        }
        public async Task SendEmailAsync(MailMessage message)
        {
            await cliente.SendMailAsync(message);
        }

        public async Task<UserStateResponse> PasswordResetAsync(string mail, string newPassword)
        {
            try
            {
                var sendValue = SendEmail(mail,
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
                    Message = ex.GetBaseException().Message
                };
            }
        }
    }
}
