using System;
using System.Threading.Tasks;
using PaymentGateway.Contracts;

namespace PaymentGateway.Data
{
    public interface IPaymentRepository
    {
        Task<Payment> GetByIdAsync(Guid id);
        Task<Payment> CreateAsync(CreatePayment createPayment);
        Task<Payment> UpdateAsync(Payment payment);
    }
}
