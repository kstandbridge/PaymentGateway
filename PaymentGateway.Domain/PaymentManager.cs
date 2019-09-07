using System;
using System.Threading.Tasks;
using PaymentGateway.Contracts;
using PaymentGateway.Data;
using PaymentGateway.Processing.Queues;

namespace PaymentGateway.Domain
{
    /// <summary>
    /// The Payment manager
    /// Following a domain driven design, this is mainly responsible for ensuring a conversion is done from internal Payment models to external GetPayment requests
    /// </summary>
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

        /// <summary>
        /// Gets an existing payment using its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <returns>The get payment response.</returns>
        public async Task<GetPayment> GetByIdAsync(Guid id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            
            if (payment == null) return null;

            return new GetPayment(payment);
        }

        /// <summary>
        /// Creates a new payment in the system to be handled by backend processors.
        /// </summary>
        /// <param name="createPayment">The create payment request.</param>
        /// <returns>The get payment response.</returns>
        public async Task<GetPayment> CreateAsync(CreatePayment createPayment)
        {
            var payment = await _paymentRepository.CreateAsync(createPayment);
            _submitPaymentCommandQueue.QueueCommand(new SubmitPaymentCommand(payment.Id));
            return new GetPayment(payment);
        }
    }
}