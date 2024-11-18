using Auction.Wallet.Application.L2.Interfaces.Commands.Trading;
using FluentValidation;

namespace Auction.Wallet.Presentation.Validation.Trading;

public class PayForLotCommandValidator : AbstractValidator<PayForLotCommand>
{
    public PayForLotCommandValidator()
    {
        RuleFor(e => e.BuyerId)
            .NotEmpty();
        RuleFor(e => e.SellerId)
            .NotEmpty();
        RuleFor(e => e.LotId)
            .NotEmpty();
        RuleFor(e => e.HammerPrice)
            .GreaterThan(0);
    }
}
