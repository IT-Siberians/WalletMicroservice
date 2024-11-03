using Auction.Common.Application.L2.Interfaces.Commands;
using Auction.Common.Domain.ValueObjects.String;
using FluentValidation;

namespace Auction.Common.Presentation.Validation;

public class CreatePersonCommandValidator : AbstractValidator<CreatePersonCommand>
{
    public CreatePersonCommandValidator()
    {
        RuleFor(e => e.Id)
            .NotEmpty();
        RuleFor(e => e.Username)
            .NotEmpty()
            .MinimumLength(Username.MinLength)
            .MaximumLength(Username.MaxLength)
            .Matches(Username.Pattern);
    }
}
