using System.Threading.Tasks;

namespace MassageManagementSystem.Services
{
    public class PayPalService : IPaymentService
    {
        public async Task<string> ProcessPayment(decimal amount)
        {
            // Simulate a delay for API processing.
            await Task.Delay(500);
            // Return a dummy transaction ID.
            return "PAYPAL_TXN_12345";
        }
    }
}
