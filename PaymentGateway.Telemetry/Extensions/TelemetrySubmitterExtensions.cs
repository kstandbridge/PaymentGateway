using PaymentGateway.Telemetry.Models;
using PaymentGateway.Telemetry.Submitters;

namespace PaymentGateway.Telemetry.Extensions
{
    public static class TelemetrySubmitterExtensions
    {
        /// <summary>
        /// Returns a new TimedOperation.
        /// </summary>
        /// <typeparam name="T">The type of telemetry message.</typeparam>
        /// <param name="submitter">The telemetry submitter.</param>
        /// <param name="message">The telemetry message.</param>
        /// <returns></returns>
        public static TimedOperation<T> BeginTimedOperation<T>(this ITelemetrySubmitter submitter, T message) where T : TimedOperationBase
        {
            return new TimedOperation<T>(submitter, message);
        }
    }
}