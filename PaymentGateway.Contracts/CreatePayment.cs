using System;

namespace PaymentGateway.Contracts
{
    public class CreatePayment
    {
        public string CardNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public int CVV { get; set; }
    }
}