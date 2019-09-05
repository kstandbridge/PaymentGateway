using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using PaymentGateway.Models;
using PaymentGateway.Repositories;
using PaymentGateway.ServiceClients;

namespace PaymentGateway.Processors
{
    public class CreatePaymentProcessor : BackgroundService
    {
        private readonly ICommandQueue<SubmitPaymentCommand> _commandQueue;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBankServiceClient _bankServiceClient;

        public CreatePaymentProcessor(
            ICommandQueue<SubmitPaymentCommand> commandQueue, 
            IPaymentRepository paymentRepository,
            IBankServiceClient bankServiceClient)
        {
            _commandQueue = commandQueue;
            _paymentRepository = paymentRepository;
            _bankServiceClient = bankServiceClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var submitPaymentCommand = await _commandQueue.DequeueAsync(stoppingToken);
                var payment = await _paymentRepository.GetByIdAsync(submitPaymentCommand.PaymentId);

                var result = await _bankServiceClient.CreateOrderAsync(submitPaymentCommand);
                payment.PaymentStatus = result.IsSuccessful ? PaymentStatus.Success : PaymentStatus.Failed;
                payment.BankTransactionId = result.Id;

                await _paymentRepository.UpdateAsync(payment);
            }
        }
    }
}