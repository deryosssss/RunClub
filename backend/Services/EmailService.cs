using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace RunClubAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(toEmail))
                {
                    _logger.LogWarning("Recipient email is null or empty.");
                    return false;
                }

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Support Team", _emailSettings.SmtpUsername));
                message.To.Add(new MailboxAddress(toEmail, toEmail));
                message.Subject = subject;
                message.Body = new TextPart("html") { Text = body }; // ✅ HTML support enabled

                using var client = new SmtpClient();
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation($"✅ Email successfully sent to {toEmail}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"❌ Failed to send email: {ex.Message}");
                return false;
            }
        }


    }
}



/* The EmailService class is designed to handle email notifications within the RunClubAPI, leveraging the MailKit library for sending messages via an SMTP (Simple Mail Transfer Protocol) server. The service follows a structured approach, utilizing dependency injection to retrieve SMTP configuration settings dynamically from the application’s configuration. The SendEmail method constructs an email using MIME (Multipurpose Internet Mail Extensions), ensuring proper message formatting. The SMTP client establishes a secure connection using STARTTLS encryption, authenticates with the SMTP server, and sends the message. By implementing a modular email service, the system enhances communication capabilities, allowing automated email notifications such as password resets, account confirmations, or event updates. The use of asynchronous email delivery could further improve performance by preventing blocking operations in the main application workflow. */