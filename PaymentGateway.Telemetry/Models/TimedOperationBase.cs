namespace PaymentGateway.Telemetry.Models
{
    public abstract class TimedOperationBase
    {
        public double Duration { get; set; }
        public int IsFaulted { get; set; }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
    }
}