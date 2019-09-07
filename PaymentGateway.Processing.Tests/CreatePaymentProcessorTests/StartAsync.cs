using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using PaymentGateway.Contracts;
using Xunit;

namespace PaymentGateway.Processing.Tests.CreatePaymentProcessorTests
{
    public class StartAsync : CreatePaymentProcessorFixture
    {
        [Fact]
        public async Task StartAsync_HappyPath_CallsManager()
        {
            // arrange
            SubmitPaymentCommand command = Fixture.Create<SubmitPaymentCommand>();

            CommandQueueMock.Setup(m => m.DequeueAsync(It.IsAny<CancellationToken>())).ReturnsAsync(command);
            CreatePaymentManagerMock
                .Setup(m => m.ExecuteAsync(command))
                .Callback<SubmitPaymentCommand>(t => SUT.Dispose())
                .Returns(Task.CompletedTask);

            // act
            await SUT.StartAsync(CancellationToken.None);

            // assert
            CreatePaymentManagerMock.Verify(m=>m.ExecuteAsync(command));
        }
    }
}