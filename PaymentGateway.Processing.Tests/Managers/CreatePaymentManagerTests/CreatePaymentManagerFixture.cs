using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentGateway.BankService;
using PaymentGateway.Data;
using PaymentGateway.Processing.Managers;
using PaymentGateway.Telemetry.Submitters;

namespace PaymentGateway.Processing.Tests.Managers.CreatePaymentManagerTests
{
    public abstract class CreatePaymentManagerFixture
    {
        protected Mock<IPaymentRepository> PaymentRepositoryMock = new Mock<IPaymentRepository>();
        protected Mock<IBankServiceClient> BankServiceClientMock = new Mock<IBankServiceClient>();
        protected Mock<ITelemetrySubmitter> TelemetrySubmitterMock = new Mock<ITelemetrySubmitter>();
        protected Mock<ILogger<CreatePaymentManager>> LoggerMock = new Mock<ILogger<CreatePaymentManager>>();

        protected Fixture Fixture = new Fixture();
        
        protected CreatePaymentManager SUT;

        protected CreatePaymentManagerFixture()
        {
            SUT = new CreatePaymentManager(
                PaymentRepositoryMock.Object,
                BankServiceClientMock.Object,
                TelemetrySubmitterMock.Object,
                LoggerMock.Object);
        }
    }
}
