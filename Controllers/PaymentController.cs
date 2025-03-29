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
        public async Task<IActionResult> Process(PaymentRequest paymentRequest)
        {
            // call your payment service
            var transactionId = await _paymentService.ProcessPayment(paymentRequest.Amount);
            return Ok(new { TransactionId = transactionId });
        }
    }
}
