using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using NodaTime;
using PaymentGateway.Contracts;

namespace PaymentGateway.Data
{
    /// <summary>
    /// The InMemory payment repository
    /// This makes use of a ConcurrentDictionary to store payments, there is no data persistence between sessions
    /// Purely an example repository that could be replaced with Couchbase or similar built against the same IPaymentRepository
    /// </summary>
    public class InMemoryPaymentRepository : IPaymentRepository
    {
        private readonly ConcurrentDictionary<string, Payment> _paymentData = new ConcurrentDictionary<string, Payment>();

        private readonly IClock _clock;

        public InMemoryPaymentRepository(IClock clock)
        {
            _clock = clock;
        }

        public Task<Payment> GetByIdAsync(Guid id)
        {
            if (_paymentData.TryGetValue(Payment.GenerateKey(id), out Payment payment))
            {
                return Task.FromResult(payment);
            }

            return Task.FromResult<Payment>(null);
        }

        public Task<Payment> CreateAsync(CreatePayment createPayment)
        {
            var payment = new Payment(_clock.GetCurrentInstant().ToDateTimeUtc())
            {
                CardNumber = createPayment.CardNumber,
                Amount = createPayment.Amount,
                CVV = createPayment.CVV,
                Currency = createPayment.Currency,
                ExpiryMonth = (short)createPayment.ExpiryDate.Month,
                ExpiryYear = createPayment.ExpiryDate.Year,
                PaymentStatus = PaymentStatus.InProgress
            };


            payment = _paymentData.GetOrAdd(payment.GetKey(), payment);

            return Task.FromResult<Payment>(payment);
        }

        public Task<Payment> UpdateAsync(Payment updatePayment)
        {
            updatePayment.LastUpdatedAt = _clock.GetCurrentInstant().ToDateTimeUtc();
            var key = Payment.GenerateKey(updatePayment.Id);

            _paymentData.AddOrUpdate(key, updatePayment, (k, v) => updatePayment);

            return GetByIdAsync(updatePayment.Id);
        }

    }
}