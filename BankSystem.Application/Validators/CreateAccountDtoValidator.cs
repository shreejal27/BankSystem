using FluentValidation;
using BankSystem.Application.DTOs;

namespace BankSystem.Application.Validators
{
    public class AccountDtoValidator : AbstractValidator<AccountDto>
    {
        public AccountDtoValidator()
        {
            RuleFor(x => x.AccountNumber)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.Balance)
                .GreaterThanOrEqualTo(0).WithMessage("Initial balance must be non-negative.");
        }
    }
}
