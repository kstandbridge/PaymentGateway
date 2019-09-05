using System;
using System.Threading.Tasks;
using PaymentGateway.Contracts;
using PaymentGateway.Data;
using PaymentGateway.Processing;

namespace PaymentGateway.Domain
{
    public class PaymentManager : IPaymentManager
    {
        private readonly ICommandQueue<SubmitPaymentCommand> _submitPaymentCommandQueue;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentManager(
            ICommandQueue<SubmitPaymentCommand> submitPaymentCommandQueue,
            IPaymentRepository paymentRepository)
        {
            _submitPaymentCommandQueue = submitPaymentCommandQueue;
            _paymentRepository = paymentRepository;
        }

        public async Task<GetPayment> GetByIdAsync(Guid id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            
            if (payment == null) return null;

            return new GetPayment(payment);
        }

        public async Task<GetPayment> CreateAsync(CreatePayment createPayment)
        {
            var payment = await _paymentRepository.CreateAsync(createPayment);
            _submitPaymentCommandQueue.QueueCommand(new SubmitPaymentCommand(payment.Id));
            return new GetPayment(payment);
        }
    }
}