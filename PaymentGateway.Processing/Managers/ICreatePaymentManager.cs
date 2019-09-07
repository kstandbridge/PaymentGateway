using System.Threading.Tasks;
using PaymentGateway.Contracts;

namespace PaymentGateway.Processing.Managers
{
    public interface ICreatePaymentManager
    {
        Task ProcessPaymentAsync(SubmitPaymentCommand submitPaymentCommand);
    }
}