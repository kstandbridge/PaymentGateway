using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PaymentGateway.Contracts;
using PaymentGateway.Telemetry.Models;
using Xunit;

namespace PaymentGateway.Tests.Controllers.PaymentsControllerTests
{
    public class Create : PaymentsControllerFixture
    {
        [Fact]
        public async Task Get_HappyPath_ReturnsPayment()
        {
            // arrange
            var createPayment = Fixture.Create<CreatePayment>();
            var expected = Fixture.Create<GetPayment>();
            CreatePaymentValidatorMock
                .Setup(m => m.ValidateAsync(It.IsAny<ValidationContext>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            PaymentManagerMock
                .Setup(m => m.CreateAsync(createPayment))
                .ReturnsAsync(expected);

            // act
            IActionResult actual = await SUT.Create(createPayment);
            
            // assert
            actual.GetObject<GetPayment>().Should().Be(expected);
        }

        [Fact]
        public async Task Get_HappyPath_ReportsToTelemetry()
        {
            // arrange
            var createPayment = Fixture.Create<CreatePayment>();
            var expected = Fixture.Create<GetPayment>();
            CreatePaymentValidatorMock
                .Setup(m => m.ValidateAsync(It.IsAny<ValidationContext>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            PaymentManagerMock
                .Setup(m => m.CreateAsync(createPayment))
                .ReturnsAsync(expected);

            // act
            await SUT.Create(createPayment);

            // assert
            TelemetrySubmitterMock.Verify(m => m.SubmitAsync(It.IsAny<ServiceOperation>()));
        }

        [Fact]
        public async Task Get_NullRequest_ReturnsBadRequest()
        {
            // arrange
            const HttpStatusCode expected = HttpStatusCode.BadRequest;

            // act
            var actual = await SUT.Create(null);

            // assert
            actual.AssertStatusCode(expected);
        }

        [Fact]
        public async Task Get_ManagerError_Throws()
        {
            // arrange
            var createPayment = Fixture.Create<CreatePayment>();
            CreatePaymentValidatorMock
                .Setup(m => m.ValidateAsync(It.IsAny<ValidationContext>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            PaymentManagerMock
                .Setup(m => m.CreateAsync(createPayment))
                .ThrowsAsync(new InvalidOperationException());

            // act

            // assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => SUT.Create(createPayment));
        }

        [Fact]
        public async Task Get_ManagerError_ReportsToTelemetry()
        {
            // arrange
            var createPayment = Fixture.Create<CreatePayment>();
            CreatePaymentValidatorMock
                .Setup(m => m.ValidateAsync(It.IsAny<ValidationContext>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            PaymentManagerMock
                .Setup(m => m.CreateAsync(createPayment))
                .ThrowsAsync(new InvalidOperationException());

            // act
            await Assert.ThrowsAsync<InvalidOperationException>(() => SUT.Create(createPayment));

            // assert
            TelemetrySubmitterMock.Verify(m => m.SubmitAsync(It.Is<ServiceOperation>(o => o.IsFaulted == 1)));
        }
    }
}