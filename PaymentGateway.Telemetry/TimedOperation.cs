using System;
using System.Diagnostics;
using PaymentGateway.Telemetry.Models;
using PaymentGateway.Telemetry.Submitters;

namespace PaymentGateway.Telemetry
{
    public class TimedOperation<T> : IDisposable where T : TimedOperationBase
    {
        private readonly ITelemetrySubmitter _telemetrySubmitter;
        private readonly T _message;
        private readonly Stopwatch _stopwatch;

        public TimedOperation(ITelemetrySubmitter telemetrySubmitter, T message)
        {
            _telemetrySubmitter = telemetrySubmitter;
            _message = message;
            _stopwatch = Stopwatch.StartNew();
        }

        public void SetFaulted(Exception ex)
        {
            _message.IsFaulted = 1;
            _message.ErrorMessage = ex.Message;
            _message.StackTrace = ex.StackTrace;
        }

        public void Dispose()
        {
            _message.Duration = _stopwatch.ElapsedMilliseconds;
            _telemetrySubmitter.SubmitAsync(_message);
        }
    }
}
