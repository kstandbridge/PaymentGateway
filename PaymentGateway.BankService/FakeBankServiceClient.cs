using System.Threading.Tasks;
using PaymentGateway.BankService.Contracts;
using PaymentGateway.Contracts;

namespace PaymentGateway.BankService
{
    public class FakeBankServiceClient : IBankServiceClient
    {
        public Task<BankPaymentResponse> CreateOrderAsync(SubmitPaymentCommand submitPaymentCommand)
        {
            return Task.FromResult(new BankPaymentResponse { IsSuccessful = true });
        }
    }
}