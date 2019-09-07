namespace PaymentGateway.Telemetry.Models
{
    /// <summary>
    /// Represents an timed operation
    /// </summary>
    public abstract class TimedOperationBase
    {
        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// Gets or sets is faulted.
        /// We use a number instead of a bool (1,0) as we can simply check the average of this number to see a failure rate percentage.
        /// </summary>
        public int IsFaulted { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the stack trace.
        /// </summary>
        public string StackTrace { get; set; }
    }
}