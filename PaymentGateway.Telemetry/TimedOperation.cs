using System;
using System.Diagnostics;
using PaymentGateway.Telemetry.Models;
using PaymentGateway.Telemetry.Submitters;

namespace PaymentGateway.Telemetry
{
    /// <summary>
    /// Represents a disposable timed operation.
    /// Upon creation a timer will start, upon disposable the timer will stop and the message will be passed to the telemetry submitter.
    /// </summary>
    /// <typeparam name="T">The type of timed operation.</typeparam>
    public class TimedOperation<T> : IDisposable where T : TimedOperationBase
    {
        private readonly ITelemetrySubmitter _telemetrySubmitter;
        private readonly T _message;
        private readonly Stopwatch _stopwatch;

        /// <summary>
        /// Starts the timer
        /// </summary>
        /// <param name="telemetrySubmitter">The telemetry submitter.</param>
        /// <param name="message">The message.</param>
        public TimedOperation(ITelemetrySubmitter telemetrySubmitter, T message)
        {
            _telemetrySubmitter = telemetrySubmitter;
            _message = message;
            _stopwatch = Stopwatch.StartNew();
        }

        /// <summary>
        /// Adds error message and stack trace information to the message.
        /// </summary>
        /// <param name="ex">The exception.</param>
        public void SetFaulted(Exception ex)
        {
            _message.IsFaulted = 1;
            _message.ErrorMessage = ex.Message;
            _message.StackTrace = ex.StackTrace;
        }

        /// <summary>
        /// Stops the timer and passes message to telemetry submitter.
        /// </summary>
        public void Dispose()
        {
            _message.Duration = _stopwatch.ElapsedMilliseconds;
            _telemetrySubmitter.SubmitAsync(_message);
        }
    }
}
