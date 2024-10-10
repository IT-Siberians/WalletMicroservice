using Auction.Common.Application.Models.Common;
using Auction.Common.Application.ModelsValidators;
using Auction.Common.Domain.ValueObjects.Numeric;
using Auction.WalletMicroservice.Application.Models.Traiding;
using System;
using System.Collections.Generic;

namespace Auction.WalletMicroservice.Application.Models.ModelsValidators;

public class ReserveMoneyValidator(IModelValidator<LotInfoModel> lotValidator)
    : IModelValidator<ReserveMoneyModel>
{
    private readonly IModelValidator<LotInfoModel> _lotValidator = lotValidator;

    public string[]? GetErrors(ReserveMoneyModel model)
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

        if (model.Lot is null)
        {
            errors.Add("Не задан лот");
        }
        else
        {
            var lotErrors = _lotValidator.GetErrors(model.Lot);
            if (lotErrors is not null)
            {
                errors.AddRange(lotErrors);
            }
        }

        if (errors.Count == 0)
        {
            return null;
        }

        return errors.ToArray();
    }
}
