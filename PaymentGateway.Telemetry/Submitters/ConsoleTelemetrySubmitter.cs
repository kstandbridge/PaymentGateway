using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PaymentGateway.Telemetry.Submitters
{
    /// <summary>
    /// The console telemetry submitter.
    /// </summary>
    public class ConsoleTelemetrySubmitter : ITelemetrySubmitter
    {
        /// <summary>
        /// Writes out to console the message as a json string
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public Task<bool> SubmitAsync(object message)
        {
            var json = JsonConvert.SerializeObject(message);
            
            Console.WriteLine($"Telemetry Action: {message.GetType()}, Payload: {json}");

            return Task.FromResult(true);
        }
    }
}