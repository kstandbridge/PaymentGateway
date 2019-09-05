using System;

namespace PaymentGateway.Contracts
{
    public class SubmitPaymentCommand
    {
        
        public SubmitPaymentCommand(Guid paymentId)
        {
            PaymentId = paymentId;
        }

        public Guid PaymentId { get; }
    }
}
