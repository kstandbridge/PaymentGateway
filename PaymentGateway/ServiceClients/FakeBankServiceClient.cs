using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using NodaTime;
using PaymentGateway.Models;

namespace PaymentGateway.ServiceClients
{
    public class FakeBankServiceClient : IBankServiceClient
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IClock _clock;

        public FakeBankServiceClient(IMemoryCache memoryCache, IClock clock)
        {
            _memoryCache = memoryCache;
            _clock = clock;
        }

        public async Task<GetPayment> GetByIdAsync(Guid id)
        {
            await Task.Yield(); // Simulate a task

            if (_memoryCache.TryGetValue(id, out Payment payment))
            {
                return new GetPayment(payment);
            }

            return null;
        }

        public async Task<GetPayment> CreateAsync(CreatePayment createPayment)
        {
            await Task.Yield(); // Simulate a task

            var payment = new Payment
            {
                CardNumber = createPayment.CardNumber,
                Amount = createPayment.Amount,
                CVV = createPayment.CVV,
                Currency = createPayment.Currency,
                ExpiryMonth = createPayment.ExpiryMonth,
                ExpiryYear = createPayment.ExpiryYear
            };

            // Mock the payment being successful
            payment.Id = Guid.NewGuid();
            payment.PaymentStatus = PaymentStatus.Success;
            payment.DateOccured = _clock.GetCurrentInstant().ToDateTimeUtc();
            
            _memoryCache.Set(payment.Id, payment);

            return new GetPayment(payment);
        }
    }
}
