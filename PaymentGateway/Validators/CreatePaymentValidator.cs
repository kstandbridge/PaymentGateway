using FluentValidation;
using NodaTime;
using PaymentGateway.Contracts;

namespace PaymentGateway.Validators
{
    public class CreatePaymentValidator : AbstractValidator<CreatePayment>
    {
        public CreatePaymentValidator(IClock clock)
        {
            var now = clock.GetCurrentInstant().ToDateTimeUtc();

            RuleFor(payment => payment.CardNumber).NotNull();
            RuleFor(payment => payment.ExpiryDate).GreaterThan(now);
            RuleFor(payment => payment.Amount).GreaterThan(0);
            RuleFor(payment => payment.Currency).NotNull().Length(3);
            RuleFor(payment => payment.CVV).InclusiveBetween(100, 999);
        }
    }
}