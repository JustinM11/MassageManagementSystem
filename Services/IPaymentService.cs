using System.Threading.Tasks;

namespace MassageManagementSystem.Services
{
    public interface IPaymentService
    {
        Task<string> ProcessPayment(decimal amount);
    }
}
