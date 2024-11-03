using Auction.Common.Application.L1.Models;
using Auction.Common.Domain.ValueObjects.String;
using FluentValidation;

namespace Auction.Common.Presentation.Validation;

public class LotInfoModelValidator : AbstractValidator<LotInfoModel>
{
    public LotInfoModelValidator()
    {
        RuleFor(e => e.Id)
            .NotEmpty();
        RuleFor(e => e.Title)
            .NotEmpty()
            .MinimumLength(Title.MinLength)
            .MaximumLength(Title.MaxLength);
        RuleFor(e => e.Description)
            .NotEmpty()
            .MinimumLength(1);
    }
}
