using System;

namespace PaymentGateway.Contracts
{
    /// <summary>
    /// Represents a submit payment command
    /// </summary>
    public class SubmitPaymentCommand
    {
        
        public SubmitPaymentCommand(Guid paymentId)
        {
            PaymentId = paymentId;
        }

        /// <summary>
        /// The payment unique identifier.
        /// </summary>
        public Guid PaymentId { get; }
    }
}
