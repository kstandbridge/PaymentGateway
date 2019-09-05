using System;
using System.Threading.Tasks;
using PaymentGateway.Models;

namespace PaymentGateway.ServiceClients
{
    public interface IBankServiceClient
    {
        Task<GetPayment> GetByIdAsync(Guid id);
        Task<GetPayment> CreateAsync(CreatePayment createPayment);
    }
}