using Auction.Wallet.Application.L2.Interfaces.Commands.Owners;
using FluentValidation;

namespace Auction.Wallet.Presentation.Validation.Owners;

public class PutMoneyInWalletCommandValidator : AbstractValidator<PutMoneyInWalletCommand>
{
    public PutMoneyInWalletCommandValidator()
    {
        RuleFor(e => e.OwnerId)
            .NotEmpty();
        RuleFor(e => e.Money)
            .GreaterThan(0);
    }
}
