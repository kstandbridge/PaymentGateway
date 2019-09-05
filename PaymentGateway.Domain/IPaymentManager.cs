using System;
using System.Threading.Tasks;
using PaymentGateway.Contracts;

namespace PaymentGateway.Domain
{
    public interface IPaymentManager
    {
        Task<GetPayment> GetByIdAsync(Guid id);
        Task<GetPayment> CreateAsync(CreatePayment createPayment);
    }
}