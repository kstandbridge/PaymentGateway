using System;
using PaymentGateway.Extensions;

namespace PaymentGateway.Models
{
    public class GetPayment
    {
        public GetPayment(Payment payment)
        {
            Id = payment.Id;
            PaymentStatus = payment.PaymentStatus;
            MaskedCardNumber = payment.CardNumber.Mask();
            ExpiryMonth = payment.ExpiryMonth;
            ExpiryYear = payment.ExpiryYear;
            Amount = payment.Amount;
            Currency = payment.Currency;
        }

        public Guid Id { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string MaskedCardNumber { get; set; }
        public short ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
    }
}