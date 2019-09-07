using AutoFixture;
using Moq;
using PaymentGateway.Contracts;
using PaymentGateway.Data;
using PaymentGateway.Processing.Queues;

namespace PaymentGateway.Domain.Tests.PaymentManagerTests
{
    public abstract class PaymentManagerFixture
    {
        protected Mock<ICommandQueue<SubmitPaymentCommand>> CommandQueueMock = new Mock<ICommandQueue<SubmitPaymentCommand>>();
        protected Mock<IPaymentRepository> PaymentRepositoryMock = new Mock<IPaymentRepository>();

        protected Fixture Fixture = new Fixture();
        protected PaymentManager SUT;

        protected PaymentManagerFixture()
        {
            SUT = new PaymentManager(
                CommandQueueMock.Object,
                PaymentRepositoryMock.Object);
        }
    }
}
