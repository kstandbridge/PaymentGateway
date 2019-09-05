using System;

namespace PaymentGateway.Processors
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
