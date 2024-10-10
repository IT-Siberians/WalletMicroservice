using Auction.Common.Application.Models.Common;
using Auction.Common.Domain.ValueObjects.String;
using System;
using System.Collections.Generic;

namespace Auction.Common.Application.ModelsValidators;

public class LotInfoValidator : IModelValidator<LotInfoModel>
{
    public string[]? GetErrors(LotInfoModel model)
    {
        var errors = new List<string>();

        if (model.Id == Guid.Empty)
        {
            errors.Add("Id лота не может быть равен Guid.Empty");
        }

        if (!Title.IsValid(model.Title))
        {
            errors.Add("Неправильное значение заголовка");
        }

        if (!Text.IsValid(model.Description))
        {
            errors.Add("Неправильное значение описания");
        }

        if (errors.Count == 0)
        {
            return null;
        }

        return errors.ToArray();
    }
}
