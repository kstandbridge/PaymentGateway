using System;

namespace PaymentGateway.Contracts
{
    /// <summary>
    /// Represents a payment.
    /// </summary>
    public class Payment
    {
        public Payment(DateTime createdAt)
        {
            Id = Guid.NewGuid();
            CreatedAt = createdAt;
            LastUpdatedAt = createdAt;
        }

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the created at timestamp.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the lasted updated at timestamp.
        /// </summary>
        public DateTime LastUpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the payment status.
        /// </summary>
        public PaymentStatus PaymentStatus { get; set; }

        /// <summary>
        /// Gets or sets the card number.
        /// </summary>
        public string CardNumber { get; set; }

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
        /// Gets or sets the CVV.
        /// </summary>
        public int CVV { get; set; }

        /// <summary>
        /// Gets or sets the bank transaction id.
        /// </summary>
        public Guid BankTransactionId { get; set; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <returns>The key.</returns>
        public string GetKey()
        {
            return GenerateKey(Id);
        }

        /// <summary>
        /// Generates a key.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <returns>The key.</returns>
        public static string GenerateKey(Guid id)
        {
            return $"{nameof(Payment)}-{id}";
        }
    }
}
