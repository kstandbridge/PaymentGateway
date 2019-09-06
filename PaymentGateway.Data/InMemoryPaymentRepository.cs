using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using NodaTime;
using PaymentGateway.Contracts;

namespace PaymentGateway.Data
{
    public class InMemoryPaymentRepository : IPaymentRepository
    {
        private readonly ConcurrentDictionary<string, Payment> _paymentData = new ConcurrentDictionary<string, Payment>();

        private static volatile ConcurrentDictionary<string, object> _locks = new ConcurrentDictionary<string, object>();

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

            var lockObj = _locks.GetOrAdd(payment.GetKey(), new object());

            lock (lockObj)
            {
                try
                {
                    payment = _paymentData.GetOrAdd(payment.GetKey(), payment);
                }
                finally
                {
                    _locks.TryRemove(payment.GetKey(), out _);
                }
            }

            return Task.FromResult<Payment>(payment);
        }

        public Task<Payment> UpdateAsync(Payment updatePayment)
        {
            updatePayment.LastUpdatedAt = _clock.GetCurrentInstant().ToDateTimeUtc();
            var key = Payment.GenerateKey(updatePayment.Id);

            var lockObj = _locks.GetOrAdd(key, new object());

            lock (lockObj)
            {
                try
                {
                    _paymentData.AddOrUpdate(key, updatePayment, (k, v) => updatePayment);
                }
                finally
                {
                    _locks.TryRemove(key, out _);
                }
            }

            return GetByIdAsync(updatePayment.Id);
        }

    }
}