using BankSystem.Application.DTOs;
using FluentValidation;

namespace BankSystem.Application.Validators
{
    public class DepositDtoValidator : AbstractValidator<DepositDto>
    {
        public DepositDtoValidator()
        {
            RuleFor(x => x.AccountId).NotEmpty();
            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }
}
