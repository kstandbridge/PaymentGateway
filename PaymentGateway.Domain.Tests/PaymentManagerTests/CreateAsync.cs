using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using PaymentGateway.Contracts;
using Xunit;

namespace PaymentGateway.Domain.Tests.PaymentManagerTests
{
    public class CreateAsync : PaymentManagerFixture
    {
        [Fact]
        public async Task CreateAsync_HappyPath_ReturnsGetPayment()
        {
            // arrange
            var createPayment = Fixture.Create<CreatePayment>();
            var payment = Fixture.Create<Payment>();
            var expected = new GetPayment(payment);
            PaymentRepositoryMock
                .Setup(m => m.CreateAsync(createPayment))
                .ReturnsAsync(payment);
            
            // act
            var actual = await SUT.CreateAsync(createPayment);

            // assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task CreateAsync_HappyPath_AddsToCommandQueue()
        {
            // arrange
            var createPayment = Fixture.Create<CreatePayment>();
            var payment = Fixture.Create<Payment>();
            PaymentRepositoryMock
                .Setup(m => m.CreateAsync(createPayment))
                .ReturnsAsync(payment);
            
            // act
            await SUT.CreateAsync(createPayment);

            // assert
            CommandQueueMock
                .Setup(m => m.QueueCommand(It.Is<SubmitPaymentCommand>(c => c.PaymentId == payment.Id)));
        }
    }
}