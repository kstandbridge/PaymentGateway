using System.Threading.Tasks;

namespace PaymentGateway.Telemetry.Submitters
{
    public interface ITelemetrySubmitter
    {
        Task<bool> SubmitAsync(object message);
    }
}