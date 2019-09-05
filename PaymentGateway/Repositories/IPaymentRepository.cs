using System;
using System.Threading.Tasks;
using PaymentGateway.Models;

namespace PaymentGateway.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment> GetByIdAsync(Guid id);
        Task<Payment> CreateAsync(CreatePayment createPayment);
        Task<Payment> UpdateAsync(Payment payment);
    }
}
