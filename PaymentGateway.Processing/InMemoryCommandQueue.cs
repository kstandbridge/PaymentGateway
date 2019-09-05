using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Processing
{
    public class InMemoryCommandQueue<T> : ICommandQueue<T>
    {
        private readonly ConcurrentQueue<T> _concurrentQueue = new ConcurrentQueue<T>();
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(0);

        public void QueueCommand(T command)
        {
            _concurrentQueue.Enqueue(command);
            _semaphoreSlim.Release();
        }

        public async Task<T> DequeueAsync(CancellationToken stoppingToken)
        {
            await _semaphoreSlim.WaitAsync(stoppingToken);

            _concurrentQueue.TryDequeue(out var command);

            return command;
        }
    }
}