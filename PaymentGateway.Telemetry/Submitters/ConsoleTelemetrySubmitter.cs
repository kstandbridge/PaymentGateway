using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PaymentGateway.Telemetry.Submitters
{
    public class ConsoleTelemetrySubmitter : ITelemetrySubmitter
    {
        public Task<bool> SubmitAsync(object message)
        {
            var json = JsonConvert.SerializeObject(message);
            
            Console.WriteLine(json);

            return Task.FromResult(true);
        }
    }
}