using BankSystem.Application.DTOs;
using FluentValidation;

namespace BankSystem.Application.Validators
{
    public class TransferDtoValidator : AbstractValidator<TransferDto>
    {
        public TransferDtoValidator()
        {
            RuleFor(x => x.FromAccountNumber).NotEmpty();
            RuleFor(x => x.ToAccountNumber).NotEmpty().NotEqual(x => x.FromAccountNumber);
            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }
}
