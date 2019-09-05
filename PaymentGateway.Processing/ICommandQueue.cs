using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Processing
{
    public interface ICommandQueue<T>
    {
        void QueueCommand(T command);
        Task<T> DequeueAsync(CancellationToken stoppingToken);
    }
}