using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using PaymentGateway.BankService.Contracts;
using PaymentGateway.Contracts;
using PaymentGateway.Telemetry.Models;
using Xunit;

namespace PaymentGateway.Processing.Tests.Managers.CreatePaymentManagerTests
{
    public class ProcessPaymentAsync : CreatePaymentManagerFixture
    {
        [Fact]
        public async Task ProcessPaymentAsync_HappyPath_CallsBank()
        {
            // arrange
            var command = Fixture.Create<SubmitPaymentCommand>();
            var payment = Fixture.Create<Payment>();
            PaymentRepositoryMock
                .Setup(m => m.GetByIdAsync(command.PaymentId))
                .ReturnsAsync(payment);
            BankServiceClientMock
                .Setup(m => m.CreateOrderAsync(command))
                .ReturnsAsync(new BankPaymentResponse {IsSuccessful = true, Id = Guid.NewGuid()});

            // act
            await SUT.ProcessPaymentAsync(command);

            // assert
            BankServiceClientMock
                .Verify(m => m.CreateOrderAsync(It.Is<SubmitPaymentCommand>(c => c.PaymentId == command.PaymentId)));
        }

        [Fact]
        public async Task ProcessPaymentAsync_HappyPath_SetsOrderStatus()
        {
            // arrange
            var command = Fixture.Create<SubmitPaymentCommand>();
            var payment = Fixture.Create<Payment>();
            PaymentRepositoryMock
                .Setup(m => m.GetByIdAsync(command.PaymentId))
                .ReturnsAsync(payment);
            BankServiceClientMock
                .Setup(m => m.CreateOrderAsync(command))
                .ReturnsAsync(new BankPaymentResponse {IsSuccessful = true, Id = Guid.NewGuid()});

            // act
            await SUT.ProcessPaymentAsync(command);

            // assert
            PaymentRepositoryMock
                .Verify(m=>m.UpdateAsync(It.Is<Payment>(p=>p.Id == payment.Id && p.PaymentStatus == PaymentStatus.Success)));
        }

        [Fact]
        public async Task ProcessPaymentAsync_HappyPath_SetsTransactionId()
        {
            // arrange
            var command = Fixture.Create<SubmitPaymentCommand>();
            var payment = Fixture.Create<Payment>();
            var bankPaymentResponse = new BankPaymentResponse {IsSuccessful = true, Id = Guid.NewGuid()};

            PaymentRepositoryMock
                .Setup(m => m.GetByIdAsync(command.PaymentId))
                .ReturnsAsync(payment);
            BankServiceClientMock
                .Setup(m => m.CreateOrderAsync(command))
                .ReturnsAsync(bankPaymentResponse);

            // act
            await SUT.ProcessPaymentAsync(command);

            // assert
            PaymentRepositoryMock
                .Verify(m=>m.UpdateAsync(It.Is<Payment>(p=>p.Id == payment.Id && p.BankTransactionId == bankPaymentResponse.Id)));
        }

        [Fact]
        public async Task ProcessPaymentAsync_HappyPath_ReportsToTelemetry()
        {
            // arrange
            var command = Fixture.Create<SubmitPaymentCommand>();
            var payment = Fixture.Create<Payment>();
            var bankPaymentResponse = new BankPaymentResponse {IsSuccessful = true, Id = Guid.NewGuid()};

            PaymentRepositoryMock
                .Setup(m => m.GetByIdAsync(command.PaymentId))
                .ReturnsAsync(payment);
            BankServiceClientMock
                .Setup(m => m.CreateOrderAsync(command))
                .ReturnsAsync(bankPaymentResponse);

            // act
            await SUT.ProcessPaymentAsync(command);

            // assert
            TelemetrySubmitterMock
                .Verify(m=>m.SubmitAsync(It.IsAny<ServiceOperation>()));
        }

        [Fact]
        public async Task ProcessPaymentAsync_OnBankFail_SetsOrderStatus()
        {
            // arrange
            var command = Fixture.Create<SubmitPaymentCommand>();
            var payment = Fixture.Create<Payment>();
            PaymentRepositoryMock
                .Setup(m => m.GetByIdAsync(command.PaymentId))
                .ReturnsAsync(payment);
            BankServiceClientMock
                .Setup(m => m.CreateOrderAsync(command))
                .ReturnsAsync(new BankPaymentResponse {IsSuccessful = false, Id = Guid.NewGuid()});

            // act
            await SUT.ProcessPaymentAsync(command);

            // assert
            PaymentRepositoryMock
                .Verify(m=>m.UpdateAsync(It.Is<Payment>(p=>p.Id == payment.Id && p.PaymentStatus == PaymentStatus.Failed)));
        }

        [Fact]
        public async Task ProcessPaymentAsync_OnBankError_ReportsToTelemetry()
        {
            // arrange
            var command = Fixture.Create<SubmitPaymentCommand>();
            var payment = Fixture.Create<Payment>();
            var bankPaymentResponse = new BankPaymentResponse {IsSuccessful = true, Id = Guid.NewGuid()};

            PaymentRepositoryMock
                .Setup(m => m.GetByIdAsync(command.PaymentId))
                .ReturnsAsync(payment);
            BankServiceClientMock
                .Setup(m => m.CreateOrderAsync(command))
                .ThrowsAsync(new InvalidOperationException());

            // act
            await SUT.ProcessPaymentAsync(command);

            // assert
            TelemetrySubmitterMock
                .Verify(m => m.SubmitAsync(It.Is<ServiceOperation>(o => o.IsFaulted == 1)));
        }
    }
}