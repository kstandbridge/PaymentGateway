using System;
using PaymentGateway.Shared.Extensions;

namespace PaymentGateway.Contracts
{
    /// <summary>
    /// The get payment response.
    /// </summary>
    public class GetPayment
    {
        public GetPayment(Payment payment)
        {
            Id = payment.Id;
            PaymentStatus = payment.PaymentStatus;
            MaskedCardNumber = payment.CardNumber.MaskCreditCard();
            ExpiryMonth = payment.ExpiryMonth;
            ExpiryYear = payment.ExpiryYear;
            Amount = payment.Amount;
            Currency = payment.Currency;
            BankTransactionId = payment.BankTransactionId;
        }

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the payment status.
        /// </summary>
        public PaymentStatus PaymentStatus { get; set; }

        /// <summary>
        /// Gets or sets the masked card number.
        /// </summary>
        public string MaskedCardNumber { get; set; }

        /// <summary>
        /// Gets or sets the expiry month.
        /// </summary>
        public short ExpiryMonth { get; set; }

        /// <summary>
        /// Gets or sets the expiry year.
        /// </summary>
        public int ExpiryYear { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the bank transaction unique identifier.
        /// </summary>
        public Guid BankTransactionId { get; set; }
    }
}