using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PaymentGateway.Telemetry.Submitters;

namespace PaymentGateway.Telemetry
{
    public class TelemetryManager : ITelemetrySubmitter
    {
        private readonly ILogger<TelemetryManager> _logger;
        private readonly List<ITelemetrySubmitter> _telemetrySubmitters;

        public TelemetryManager(
            List<ITelemetrySubmitter> telemetrySubmitters, 
            ILogger<TelemetryManager> logger)
        {
            _telemetrySubmitters = telemetrySubmitters;
            _logger = logger;
        }

        public async Task<bool> SubmitAsync(object message)
        {
            try
            {
                foreach (var telemetrySubmitter in _telemetrySubmitters)
                {
                    await telemetrySubmitter.SubmitAsync(message);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to submit telemetry");
                return false;
            }

            return true;
        }
    }
}