using System;

namespace PaymentGateway.Contracts
{
    public class Payment
    {
        public Payment(DateTime createdAt)
        {
            Id = Guid.NewGuid();
            CreatedAt = createdAt;
            LastUpdatedAt = createdAt;
        }

        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string CardNumber { get; set; }
        public short ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public int CVV { get; set; }
        public Guid BankTransactionId { get; set; }

        public string GetKey()
        {
            return GenerateKey(Id);
        }

        public static string GenerateKey(Guid id)
        {
            return $"{nameof(Payment)}-{id}";
        }
    }
}
