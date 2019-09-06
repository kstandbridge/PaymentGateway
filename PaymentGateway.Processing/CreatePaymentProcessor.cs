using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PaymentGateway.BankService;
using PaymentGateway.Contracts;
using PaymentGateway.Data;
using PaymentGateway.Telemetry.Extensions;
using PaymentGateway.Telemetry.Models;
using PaymentGateway.Telemetry.Submitters;

namespace PaymentGateway.Processing
{
    public class CreatePaymentProcessor : BackgroundService
    {
        private readonly ICommandQueue<SubmitPaymentCommand> _commandQueue;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBankServiceClient _bankServiceClient;
        private readonly ITelemetrySubmitter _telemetrySubmitter;
        private readonly ILogger<CreatePaymentProcessor> _logger;

        public CreatePaymentProcessor(
            ICommandQueue<SubmitPaymentCommand> commandQueue, 
            IPaymentRepository paymentRepository,
            IBankServiceClient bankServiceClient,
            ITelemetrySubmitter telemetrySubmitter,
            ILogger<CreatePaymentProcessor> logger)
        {
            _commandQueue = commandQueue;
            _paymentRepository = paymentRepository;
            _bankServiceClient = bankServiceClient;
            _telemetrySubmitter = telemetrySubmitter;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var submitPaymentCommand = await _commandQueue.DequeueAsync(stoppingToken);

                _logger.LogInformation("Processing payment Id: {0}", submitPaymentCommand.PaymentId);

                using (var operation = _telemetrySubmitter.BeginTimedOperation(new ServiceOperation(nameof(CreatePaymentProcessor), nameof(ExecuteAsync))))
                {
                    try
                    {
                        var payment = await _paymentRepository.GetByIdAsync(submitPaymentCommand.PaymentId);

                        var result = await _bankServiceClient.CreateOrderAsync(submitPaymentCommand);
                        payment.PaymentStatus = result.IsSuccessful ? PaymentStatus.Success : PaymentStatus.Failed;

                        _logger.LogInformation("Setting payment Id: {0} as {1}", submitPaymentCommand.PaymentId, payment.PaymentStatus);

                        payment.BankTransactionId = result.Id;

                        await _paymentRepository.UpdateAsync(payment);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to CreatePayment due to {0}", ex.Message);
                        operation.SetFaulted(ex);
                    }
                }
            }
        }
    }
}