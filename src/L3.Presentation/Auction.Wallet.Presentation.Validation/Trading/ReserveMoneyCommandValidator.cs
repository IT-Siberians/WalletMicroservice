using Auction.Common.Application.L1.Models;
using Auction.Wallet.Application.L2.Interfaces.Commands.Trading;
using FluentValidation;

namespace Auction.Wallet.Presentation.Validation.Trading;

public class ReserveMoneyCommandValidator : AbstractValidator<ReserveMoneyCommand>
{
    public ReserveMoneyCommandValidator(IValidator<LotInfoModel> lotInfoModelValidator)
    {
        RuleFor(e => e.BuyerId)
            .NotEmpty();
        RuleFor(e => e.Price)
            .GreaterThan(0);
        RuleFor(e => e.Lot)
            .NotEmpty()
            .SetValidator(lotInfoModelValidator!);
    }
}
