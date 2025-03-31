using MassageManagementSystem.Models;
using MassageManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MassageManagementSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // POST: /Payment/Process
        [HttpPost("Process")]
        public async Task<IActionResult> Process([FromBody] PaymentRequest paymentRequest)
        {
            if (paymentRequest.Amount <= 0)
                return BadRequest("Invalid payment amount.");

            var approvalUrl = await _paymentService.ProcessPayment(paymentRequest.Amount);
            return Ok(new { RedirectUrl = approvalUrl });
        }

        [HttpGet("success")]
        public IActionResult Success(string paymentId, string token, string PayerID)
        {
            // Return a view that triggers JS to send booking + PayPal data
            ViewBag.PaymentId = paymentId;
            ViewBag.PayerID = PayerID;
            return View("Success");
        }


        [HttpGet("cancel")]
        public IActionResult Cancel()
        {
            return View("Cancel"); // Renders Views/Payment/Cancel.cshtml
        }


    }
}
