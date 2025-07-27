using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Project.BLL.Interface;

namespace Project.BLL
{
    public class EmailService : IEmailService
    {
        private readonly string _fromEmail;
        private readonly string _password;

        public EmailService(IConfiguration configuration)
        {
            _fromEmail = configuration["EmailSettings:FromEmail"];
            _password = configuration["EmailSettings:Password"];
        }
        public async Task SendWinnerEmailAsync(string winnerEmail, string subject, string body)
        {
            var fromAddress = new MailAddress(_fromEmail, "Chedva Basket Raffle");
            var toAddress = new MailAddress(winnerEmail);
            var fromPassword = _password;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com", // השרת SMTP שלך, למשל smtp.gmail.com
                Port = 587, // בדרך כלל 587 או 465
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = false, // או true אם יש לך תוכן HTML
            };

            await smtp.SendMailAsync(message);
        }
    }


}


