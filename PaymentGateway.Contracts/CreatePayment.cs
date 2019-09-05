namespace PaymentGateway.Contracts
{
    public class CreatePayment
    {
        public string CardNumber { get; set; }
        public short ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public int CVV { get; set; }
    }
}