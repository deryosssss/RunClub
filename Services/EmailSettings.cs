namespace RunClubAPI.Services
{
    /// <summary>
    /// Represents the SMTP email configuration settings required for sending emails.
    /// </summary>
    public class EmailSettings
    {
        /// <summary>
        /// The SMTP server address used for sending emails (e.g., smtp.gmail.com).
        /// </summary>
        public string SmtpServer { get; set; }

        /// <summary>
        /// The port number used for SMTP communication (e.g., 587 for TLS, 465 for SSL).
        /// </summary>
        public int SmtpPort { get; set; }

        /// <summary>
        /// The username for authenticating with the SMTP server (typically an email address).
        /// </summary>
        public string SmtpUsername { get; set; }

        /// <summary>
        /// The password or app-specific authentication token for SMTP authentication.
        /// </summary>
        public string SmtpPassword { get; set; }
    }
}

/* The EmailSettings class serves as a configuration model for storing SMTP (Simple Mail Transfer Protocol) settings required for sending emails within the RunClubAPI. This class follows the options pattern, which allows for structured configuration management in ASP.NET Core applications. The SMTP server address, port, username, and password are typically stored in configuration files (e.g., appsettings.json) or secure environment variables. These settings are injected into the application using dependency injection with IOptions<EmailSettings>, ensuring flexibility and security. By separating configuration from implementation, this approach promotes maintainability, scalability, and ease of reconfiguration without modifying the core application logic. Additionally, sensitive information such as SMTP credentials should be managed using secrets management tools or environment variables to prevent security risks.*/