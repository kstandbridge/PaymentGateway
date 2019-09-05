using System;

namespace PaymentGateway.ServiceClients
{
    public class BankPaymentResponse
    {
        public BankPaymentResponse()
        {
            Id = Guid.Empty;
        }

        public Guid Id { get; set; }
        public bool IsSuccessful { get; set; }
    }
}