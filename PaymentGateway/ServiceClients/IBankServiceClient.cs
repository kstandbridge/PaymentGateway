using System.Threading.Tasks;
using PaymentGateway.Models;
using PaymentGateway.Processors;

namespace PaymentGateway.ServiceClients
{
    public interface IBankServiceClient
    {
        Task<BankPaymentResponse> CreateOrderAsync(SubmitPaymentCommand submitPaymentCommand);
    }
}