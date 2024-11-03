using Auction.Wallet.Application.L2.Interfaces.Commands.Owners;
using FluentValidation;

namespace Auction.Wallet.Presentation.Validation.Owners;

public class GetWalletTransactionsQueryValidator : AbstractValidator<GetWalletTransactionsQuery>
{
    public GetWalletTransactionsQueryValidator()
    {
        RuleFor(e => e.OwnerId)
            .NotEmpty();
    }
}
