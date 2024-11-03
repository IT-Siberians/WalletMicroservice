using System.Collections.Generic;

namespace Auction.Common.Application.L2.Interfaces.Answers;

public class BadValues(IDictionary<string, string[]> errors) : IBadBaseAnswer
{
    public IDictionary<string, string[]> Errors { get; } = errors;

    public BadValues<TResult> ToBadValues<TResult>() => new(Errors);

    public IAnswer<TResult> ToIAnswer<TResult>() => new BadValues<TResult>(Errors);
}

public class BadValues<TResult>(IDictionary<string, string[]> errors)
    : BadValues(errors),
    IBadBaseAnswer<TResult>;
