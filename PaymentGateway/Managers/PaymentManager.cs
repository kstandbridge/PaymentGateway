using System;
using System.Threading.Tasks;
using PaymentGateway.Models;
using PaymentGateway.ServiceClients;

namespace PaymentGateway.Managers
{
    public class PaymentManager : IPaymentManager
    {
        private readonly IBankServiceClient _bankServiceClient;

        public PaymentManager(IBankServiceClient bankServiceClient)
        {
            _bankServiceClient = bankServiceClient;
        }

        public Task<GetPayment> GetByIdAsync(Guid id)
        {
            return _bankServiceClient.GetByIdAsync(id);
        }

        public Task<GetPayment> CreateAsync(CreatePayment createPayment)
        {
            return _bankServiceClient.CreateAsync(createPayment);
        }
    }
}