using System.Threading.Tasks;
using PaymentGateway.BankService.Contracts;
using PaymentGateway.Contracts;

namespace PaymentGateway.BankService
{
    public interface IBankServiceClient
    {
        Task<BankPaymentResponse> CreateOrderAsync(SubmitPaymentCommand submitPaymentCommand);
    }
}