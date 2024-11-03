using Auction.Wallet.Application.L2.Interfaces.Commands.Owners;
using FluentValidation;

namespace Auction.Wallet.Presentation.Validation.Owners;

public class GetWalletBalanceQueryValidator : AbstractValidator<GetWalletBalanceQuery>
{
    public GetWalletBalanceQueryValidator()
    {
        RuleFor(e => e.OwnerId)
            .NotEmpty();
    }
}
