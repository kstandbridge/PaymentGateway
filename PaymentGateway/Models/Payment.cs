using System;

namespace PaymentGateway.Models
{
    public class Payment
    {
        public Guid Id { get; set; }
        public DateTime DateOccured { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string CardNumber { get; set; }
        public short ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public int CVV { get; set; }
    }
}
