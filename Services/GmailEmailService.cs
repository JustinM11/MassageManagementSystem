using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace MassageManagementSystem.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }

    public class GmailEmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public GmailEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var emailMessage = new MimeMessage();

            // Set the sender (From) address
            emailMessage.From.Add(new MailboxAddress("Massage Management System", _configuration["Email:FromAddress"]));
            // Set the recipient (To) address
            emailMessage.To.Add(new MailboxAddress("", to));
            emailMessage.Subject = subject;
            // You can use "plain" or "html" for text type
            emailMessage.Body = new TextPart("html") { Text = body };

            using (var client = new SmtpClient())
            {
                // Connect to Gmail's SMTP server using the settings from configuration.
                await client.ConnectAsync(_configuration["Email:SmtpServer"], int.Parse(_configuration["Email:SmtpPort"]), false);
                // Authenticate with the Gmail account
                await client.AuthenticateAsync(_configuration["Email:Username"], _configuration["Email:Password"]);
                // Send the email
                await client.SendAsync(emailMessage);
                // Disconnect and close the connection
                await client.DisconnectAsync(true);
            }
        }
    }
}
