using System.Threading.Tasks;

namespace MassageManagementSystem.Services
{
    public class GmailEmailService : IEmailService
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            // Simulate email sending delay.
            await Task.Delay(500);
            // In production, integrate with the Gmail API or another email provider.
        }
    }
}
