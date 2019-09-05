using System.Threading.Tasks;
using PaymentGateway.Processors;

namespace PaymentGateway.ServiceClients
{
    public class FakeBankServiceClient : IBankServiceClient
    {
        public Task<BankPaymentResponse> CreateOrderAsync(SubmitPaymentCommand submitPaymentCommand)
        {
            return Task.FromResult(new BankPaymentResponse { IsSuccessful = true });
        }
    }
}