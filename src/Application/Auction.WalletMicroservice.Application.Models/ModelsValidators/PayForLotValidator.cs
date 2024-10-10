using Auction.Common.Application.ModelsValidators;
using Auction.Common.Domain.ValueObjects.Numeric;
using Auction.WalletMicroservice.Application.Models.Traiding;
using System;
using System.Collections.Generic;

namespace Auction.WalletMicroservice.Application.Models.ModelsValidators;

public class PayForLotValidator : IModelValidator<PayForLotModel>
{
    public string[]? GetErrors(PayForLotModel model)
    {
        var errors = new List<string>();

        if (model.BuyerId == Guid.Empty)
        {
            errors.Add("Id покупателя не может быть равен Guid.Empty");
        }

        if (model.SellerId == Guid.Empty)
        {
            errors.Add("Id продавца не может быть равен Guid.Empty");
        }

        if (model.SellerId == model.BuyerId && model.BuyerId != Guid.Empty)
        {
            errors.Add($"Id покупателя и продавца не может совпадать ({model.SellerId})");
        }

        if (model.LotId == Guid.Empty)
        {
            errors.Add("Id лота не может быть равен Guid.Empty");
        }

        if (!Price.IsValid(model.HammerPrice))
        {
            errors.Add("Неправильная цена лота");
        }

        if (errors.Count == 0)
        {
            return null;
        }

        return errors.ToArray();
    }
}
