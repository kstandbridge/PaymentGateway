using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PaymentGateway.BankService;
using PaymentGateway.Contracts;
using PaymentGateway.Data;
using PaymentGateway.Telemetry.Extensions;
using PaymentGateway.Telemetry.Models;
using PaymentGateway.Telemetry.Submitters;

namespace PaymentGateway.Processing.Managers
{
    public class CreatePaymentManager : ICreatePaymentManager
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBankServiceClient _bankServiceClient;
        private readonly ITelemetrySubmitter _telemetrySubmitter;
        private readonly ILogger<CreatePaymentManager> _logger;

        public CreatePaymentManager(
            IPaymentRepository paymentRepository,
            IBankServiceClient bankServiceClient, 
            ITelemetrySubmitter telemetrySubmitter, 
            ILogger<CreatePaymentManager> logger)
        {
            _paymentRepository = paymentRepository;
            _bankServiceClient = bankServiceClient;
            _telemetrySubmitter = telemetrySubmitter;
            _logger = logger;
        }


        public async Task ExecuteAsync(SubmitPaymentCommand submitPaymentCommand)
        {
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