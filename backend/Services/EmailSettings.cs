namespace RunClubAPI.Services
{
    /// <summary>
    /// Represents the SMTP email configuration settings required for sending emails.
    /// </summary>
    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
    }
}


/* The EmailSettings class serves as a configuration model for storing SMTP (Simple Mail Transfer Protocol) settings required for sending emails within the RunClubAPI. This class follows the options pattern, which allows for structured configuration management in ASP.NET Core applications. The SMTP server address, port, username, and password are typically stored in configuration files (e.g., appsettings.json) or secure environment variables. These settings are injected into the application using dependency injection with IOptions<EmailSettings>, ensuring flexibility and security. By separating configuration from implementation, this approach promotes maintainability, scalability, and ease of reconfiguration without modifying the core application logic. Additionally, sensitive information such as SMTP credentials should be managed using secrets management tools or environment variables to prevent security risks.*/