using PaymentGateway.Telemetry.Models;
using PaymentGateway.Telemetry.Submitters;

namespace PaymentGateway.Telemetry.Extensions
{
    public static class TelemetrySubmitterExtensions
    {
        public static TimedOperation<T> BeginTimedOperation<T>(this ITelemetrySubmitter submitter, T message) where T : TimedOperationBase
        {
            return new TimedOperation<T>(submitter, message);
        }
    }
}