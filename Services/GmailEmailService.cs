using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
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
            var fromAddress = _configuration["Email:FromAddress"];
            var smtpServer = _configuration["Email:SmtpServer"];
            var smtpPort = int.Parse(_configuration["Email:SmtpPort"]);
            var username = _configuration["Email:Username"];
            var password = _configuration["Email:Password"];

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Massage Management System", fromAddress));
            emailMessage.To.Add(MailboxAddress.Parse(to));
            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body,
                TextBody = StripHtmlTags(body) // fallback for non-HTML clients
            };

            emailMessage.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            try
            {
                // Use STARTTLS which Gmail expects on port 587
                await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(username, password);
                await client.SendAsync(emailMessage);
            }
            catch (Exception ex)
            {
                // Log or handle error as needed
                throw new InvalidOperationException("Failed to send email.", ex);
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }

        private string StripHtmlTags(string html)
        {
            // Basic fallback to plain text
            return System.Text.RegularExpressions.Regex.Replace(html, "<.*?>", string.Empty);
        }
    }
}
