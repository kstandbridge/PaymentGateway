using AutoFixture;
using Moq;
using PaymentGateway.Contracts;
using PaymentGateway.Processing.Managers;
using PaymentGateway.Processing.Queues;

namespace PaymentGateway.Processing.Tests.CreatePaymentProcessorTests
{
    public abstract class CreatePaymentProcessorFixture
    {
        protected Mock<ICommandQueue<SubmitPaymentCommand>> CommandQueueMock = new Mock<ICommandQueue<SubmitPaymentCommand>>();

        protected Mock<ICreatePaymentManager> CreatePaymentManagerMock = new Mock<ICreatePaymentManager>();
        protected Fixture Fixture = new Fixture();

        protected CreatePaymentProcessor SUT;

        protected CreatePaymentProcessorFixture()
        {
            SUT = new CreatePaymentProcessor(
                CommandQueueMock.Object,
                CreatePaymentManagerMock.Object);
        }
    }
}