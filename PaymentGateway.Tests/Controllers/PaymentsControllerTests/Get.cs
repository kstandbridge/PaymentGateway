using System;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using PaymentGateway.Contracts;
using PaymentGateway.Telemetry.Models;
using Xunit;

namespace PaymentGateway.Tests.Controllers.PaymentsControllerTests
{
    public class Get : PaymentsControllerFixture
    {
        [Fact]
        public async Task Get_HappyPath_ReturnsPayment()
        {
            // arrange
            var expected = Fixture.Create<GetPayment>();
            PaymentManagerMock
                .Setup(m => m.GetByIdAsync(expected.Id))
                .ReturnsAsync(expected);

            // act
            var actual = await SUT.Get(expected.Id);
            
            // assert
            actual.GetObject<GetPayment>().Should().Be(expected);
        }

        [Fact]
        public async Task Get_HappyPath_ReportsToTelemetry()
        {
            // arrange
            var expected = Fixture.Create<GetPayment>();
            PaymentManagerMock
                .Setup(m => m.GetByIdAsync(expected.Id))
                .ReturnsAsync(expected);

            // act
            await SUT.Get(expected.Id);
            
            // assert
            TelemetrySubmitterMock.Verify(m=>m.SubmitAsync(It.IsAny<ServiceOperation>()));
        }

        [Fact]
        public async Task Get_EmptyId_ReturnsBadRequest()
        {
            // arrange
            var id = Guid.Empty;
            const HttpStatusCode expected = HttpStatusCode.BadRequest;

            // act
            var actual = await SUT.Get(id);

            // assert
            actual.AssertStatusCode(expected);
        }

        [Fact]
        public async Task Get_NotExist_ReturnsNotFound()
        {
            // arrange
            var id = Guid.NewGuid();
            const HttpStatusCode expected = HttpStatusCode.NotFound;

            PaymentManagerMock
                .Setup(m => m.GetByIdAsync(id))
                .ReturnsAsync((GetPayment)null);

            // act
            var actual = await SUT.Get(id);

            // assert
            actual.AssertStatusCode(expected);
        }

        [Fact]
        public async Task Get_ManagerError_ShouldThrow()
        {
            // arrange
            var id = Guid.NewGuid();
            PaymentManagerMock
                .Setup(m => m.GetByIdAsync(id))
                .ThrowsAsync(new InvalidOperationException());

            // act

            // assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => SUT.Get(id));
        }

        [Fact]
        public async Task Get_ManagerError_ReportsToTelemetry()
        {
            // arrange
            var id = Guid.NewGuid();
            var invalidOperationException = new InvalidOperationException();
            PaymentManagerMock
                .Setup(m => m.GetByIdAsync(id))
                .ThrowsAsync(invalidOperationException);

            // act
            await Assert.ThrowsAsync<InvalidOperationException>(() => SUT.Get(id));

            // assert
            TelemetrySubmitterMock.Verify(m => m.SubmitAsync(It.Is<ServiceOperation>(o => o.IsFaulted == 1)));
        }
    }
}