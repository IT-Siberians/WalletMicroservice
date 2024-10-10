using System;

namespace Auction.Common.Application.Models.Base;

public interface IModel<out TKey>
    where TKey : struct, IEquatable<TKey>
{
    public TKey Id { get; }
}
