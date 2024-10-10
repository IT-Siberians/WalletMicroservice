using Auction.Common.Application.ModelsValidators;
using Auction.Common.Domain.ValueObjects.Numeric;
using Auction.WalletMicroservice.Application.Models.Traiding;
using System;
using System.Collections.Generic;

namespace Auction.WalletMicroservice.Application.Models.ModelsValidators;

public class RealeaseMoneyValidator : IModelValidator<RealeaseMoneyModel>
{
    public string[]? GetErrors(RealeaseMoneyModel model)
    {
        var errors = new List<string>();

        if (model.BuyerId == Guid.Empty)
        {
            errors.Add("Id покупателя не может быть равен Guid.Empty");
        }

        if (!Price.IsValid(model.Price))
        {
            errors.Add("Неправильная цена лота");
        }

        if (model.LotId == Guid.Empty)
        {
            errors.Add("Id лота не может быть равен Guid.Empty");
        }

        if (errors.Count == 0)
        {
            return null;
        }

        return errors.ToArray();
    }
}
