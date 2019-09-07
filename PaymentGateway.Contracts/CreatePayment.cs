using System;

namespace PaymentGateway.Contracts
{
    /// <summary>
    /// Request to create a payment
    /// </summary>
    public class CreatePayment
    {
        /// <summary>
        /// Gets or sets the card number.
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// Gets or sets the expiry date.
        /// </summary>
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the CVV.
        /// </summary>
        public int CVV { get; set; }
    }
}