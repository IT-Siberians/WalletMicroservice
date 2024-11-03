using Auction.Common.Application.L2.Interfaces.Commands;
using FluentValidation;

namespace Auction.Common.Presentation.Validation;

public class GetPersonQueryValidator : AbstractValidator<GetPersonQuery>
{
    public GetPersonQueryValidator()
    {
        RuleFor(e => e.Id)
            .NotEmpty();
    }
}
