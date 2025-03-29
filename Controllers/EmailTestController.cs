using MassageManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MassageManagementSystem.Controllers
{
    public class EmailTestController : Controller
    {
        private readonly IEmailService _emailService;

        public EmailTestController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        // GET: /EmailTest/SendTestEmail
        [HttpGet]
        public async Task<IActionResult> SendTestEmail()
        {
            // Replace "yourtestemail@example.com" with the email you want to test receiving the email.
            await _emailService.SendEmailAsync("yourtestemail@example.com", "Test Email", "<p>This is a test email from Massage Management System.</p>");
            return Content("Test email sent. Check your inbox.");
        }
    }
}
