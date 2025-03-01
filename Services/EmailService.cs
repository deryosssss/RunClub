using MailKit.Net.Smtp; // Provides functionality for sending emails using SMTP.
using MailKit.Security; // Defines security options for SMTP connections.
using MimeKit; // Handles email message formatting (headers, body, attachments).
using Microsoft.Extensions.Options; // Used for retrieving configuration settings.

// Namespace defining the service layer of the application.
namespace RunClubAPI.Services
{    
    /// <summary>
    /// EmailService is responsible for sending emails using an SMTP server.
    /// </summary>
    public class EmailService
    {
        private readonly EmailSettings _emailSettings; // Stores SMTP configuration details.

        /// <summary>
        /// Constructor initializes email settings using dependency injection.
        /// </summary>
        /// <param name="emailSettings">Injected configuration options for email settings.</param>
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        /// <summary>
        /// Sends an email to the specified recipient with the given subject and body.
        /// </summary>
        /// <param name="toEmail">Recipient's email address.</param>
        /// <param name="subject">Email subject line.</param>
        /// <param name="body">Email body content.</param>
        public void SendEmail(string toEmail, string subject, string body)
        {
            var message = new MimeMessage(); // Creates a new email message.

            // Sets the sender email address.
            message.From.Add(new MailboxAddress("Support Student App", _emailSettings.SmtpUsername));

            // Adds the recipient email address.
            message.To.Add(new MailboxAddress("Receiver Name", toEmail));

            message.Subject = subject; // Sets the subject of the email.

            // Creates a plain text email body.
            var textPart = new TextPart("plain")
            {
                Text = body
            };
            message.Body = textPart; // Assigns the body to the message.

            using (var client = new SmtpClient()) // Initializes the SMTP client.
            {
                // Connects to the SMTP server with secure communication.
                client.Connect(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);

                // Authenticates the SMTP client using the stored credentials.
                client.Authenticate(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);

                // Sends the email message.
                client.Send(message);

                // Disconnects from the SMTP server to release resources.
                client.Disconnect(true);
            }
        }
    }
}

/* The EmailService class is designed to handle email notifications within the RunClubAPI, leveraging the MailKit library for sending messages via an SMTP (Simple Mail Transfer Protocol) server. The service follows a structured approach, utilizing dependency injection to retrieve SMTP configuration settings dynamically from the application’s configuration. The SendEmail method constructs an email using MIME (Multipurpose Internet Mail Extensions), ensuring proper message formatting. The SMTP client establishes a secure connection using STARTTLS encryption, authenticates with the SMTP server, and sends the message. By implementing a modular email service, the system enhances communication capabilities, allowing automated email notifications such as password resets, account confirmations, or event updates. The use of asynchronous email delivery could further improve performance by preventing blocking operations in the main application workflow. */