using Auction.Wallet.Application.L2.Interfaces.Commands.Traiding;
using FluentValidation;

namespace Auction.Wallet.Presentation.Validation.Traiding;

public class RealeaseMoneyCommandValidator : AbstractValidator<RealeaseMoneyCommand>
{
    public RealeaseMoneyCommandValidator()
    {
        RuleFor(e => e.BuyerId)
            .NotEmpty();
        RuleFor(e => e.LotId)
            .NotEmpty();
        RuleFor(e => e.Price)
            .GreaterThan(0);
    }
}
