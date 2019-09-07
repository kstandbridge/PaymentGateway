using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using PaymentGateway.Contracts;
using PaymentGateway.Processing.Managers;
using PaymentGateway.Processing.Queues;

namespace PaymentGateway.Processing
{
    public class CreatePaymentProcessor : BackgroundService
    {
        private readonly ICommandQueue<SubmitPaymentCommand> _commandQueue;
        private readonly ICreatePaymentManager _createPaymentManager;


        public CreatePaymentProcessor(
            ICommandQueue<SubmitPaymentCommand> commandQueue,
            ICreatePaymentManager createPaymentManager)
        {
            _commandQueue = commandQueue;
            _createPaymentManager = createPaymentManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var submitPaymentCommand = await _commandQueue.DequeueAsync(stoppingToken);

                await _createPaymentManager.ExecuteAsync(submitPaymentCommand);
            }
        }
    }
}