using BankSystem.Application.DTOs;
using FluentValidation;

namespace BankSystem.Application.Validators
{
    public class WithdrawDtoValidator : AbstractValidator<WithdrawDto>
    {
        public WithdrawDtoValidator()
        {
            RuleFor(x => x.AccountNumber).NotEmpty();
            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }
}
