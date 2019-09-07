using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using PaymentGateway.Contracts;
using Xunit;

namespace PaymentGateway.Domain.Tests.PaymentManagerTests
{
    public class GetByIdAsync : PaymentManagerFixture
    {
        [Fact]
        public async Task GetByIdAsync_HappyPath_ReturnsGetPayment()
        {
            // arrange
            var id = new Guid();
            var payment = Fixture.Create<Payment>();
            var expected = new GetPayment(payment);
            PaymentRepositoryMock
                .Setup(m => m.GetByIdAsync(id))
                .ReturnsAsync(payment);
            
            // act
            var actual = await SUT.GetByIdAsync(id);

            // assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetByIdAsync_NotFound_ReturnsNull()
        {
            // arrange
            var id = new Guid();
            PaymentRepositoryMock
                .Setup(m => m.GetByIdAsync(id))
                .ReturnsAsync((Payment)null);
            
            // act
            var actual = await SUT.GetByIdAsync(id);

            // assert
            actual.Should().BeNull();
        }
    }
}