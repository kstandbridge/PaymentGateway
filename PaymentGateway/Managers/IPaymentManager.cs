using System;
using System.Threading.Tasks;
using PaymentGateway.Models;

namespace PaymentGateway.Managers
{
    public interface IPaymentManager
    {
        Task<GetPayment> GetByIdAsync(Guid id);
        Task<GetPayment> CreateAsync(CreatePayment createPayment);
    }
}