using Contract.Abstractions.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Command.Infrastructure.EmailServices
{
    public class EmailService : IEmailService
    {
        private readonly EmailOptions _emailOptions;
        public EmailService(IOptions<EmailOptions> emailOptions) {
            _emailOptions = emailOptions.Value;
        }

        public string GenerateTokenLink(string token, string routeTo)
        {
            var encodedToken = WebUtility.UrlEncode(token);
            var tokenLink = $"{_emailOptions.VerificationUrl}/{routeTo}/{encodedToken}";
            return tokenLink ?? throw new Exception("Không thể tạo link xác thực email.");
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using var client = new SmtpClient(_emailOptions.SmtpServer, _emailOptions.Port)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailOptions.SenderEmail, _emailOptions.SenderPassword),
                EnableSsl = _emailOptions.EnableSsl
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailOptions.SenderEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(to);

            await client.SendMailAsync(mailMessage);
        }
    }
}
