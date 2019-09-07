using AutoFixture;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentGateway.Contracts;
using PaymentGateway.Controllers;
using PaymentGateway.Domain;
using PaymentGateway.Telemetry.Submitters;

namespace PaymentGateway.Tests.Controllers.PaymentsControllerTests
{
    public abstract class PaymentsControllerFixture
    {
        protected Mock<IPaymentManager> PaymentManagerMock = new Mock<IPaymentManager>();
        protected Mock<IValidator<CreatePayment>> CreatePaymentValidatorMock = new Mock<IValidator<CreatePayment>>();
        protected Mock<ITelemetrySubmitter> TelemetrySubmitterMock = new Mock<ITelemetrySubmitter>();
        protected Mock<ILogger<PaymentsController>> LoggerMock = new Mock<ILogger<PaymentsController>>();
        
        protected Fixture Fixture = new Fixture();
        protected PaymentsController SUT;

        protected PaymentsControllerFixture()
        {
            SUT = new PaymentsController(
                PaymentManagerMock.Object,
                CreatePaymentValidatorMock.Object,
                TelemetrySubmitterMock.Object,
                LoggerMock.Object);
        }
    }
}
