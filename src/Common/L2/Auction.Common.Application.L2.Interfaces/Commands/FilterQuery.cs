namespace Auction.Common.Application.L2.Interfaces.Commands;

public record FilterQuery(
    string? With,
    string? Without)
{
    public static FilterQuery? NewOrNull(string? with, string? without)
    {
        var withNotEmpty = string.IsNullOrWhiteSpace(with) ? null : with;
        var withoutNotEmpty = string.IsNullOrWhiteSpace(without) ? null : without;

        if (withNotEmpty is null && withoutNotEmpty is null)
        {
            return null;
        }

        return new FilterQuery(withNotEmpty, withoutNotEmpty);
    }
}
