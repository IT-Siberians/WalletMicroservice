using Auction.Common.Domain.Entities;
using Auction.Common.Domain.EntitiesExceptions;
using Auction.Common.Domain.ValueObjects.Numeric;
using Auction.Common.Domain.ValueObjects.String;
using System;

namespace Auction.WalletMicroservice.Domain.Entities;

/// <summary>
/// Владелец счёта
/// </summary>
public class Owner : AbstractPerson<Guid>
{
    /// <summary>
    /// Уникальный идентификатор счёта
    /// </summary>
    public Guid BillId { get; }

    /// <summary>
    /// Счёт владельца
    /// </summary>
    public Bill Bill { get; }

    /// <summary>
    /// Количество денег на счёте
    /// </summary>
    public Money Balance => Bill.Money;

    /// <summary>
    /// Конструктор для EF
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected Owner() : base() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    /// <summary>
    /// Конструктор создания владельца счёта
    /// </summary>
    /// <param name="id">Уникальный идентификатор владельца</param>
    /// <param name="username">Имя владельца</param>
    /// <param name="bill">Счёт владельца</param>
    /// <exception cref="ArgumentNullValueException">Если аргумент null</exception>
    public Owner(Guid id, Username username)
        : base(id, username)
    {
        GuidEmptyValueException.ThrowIfEmpty(id);

        Bill = new Bill(Guid.NewGuid(), this);
        BillId = Bill.Id;
    }

    /// <summary>
    /// Основной конструктор владельца счёта
    /// </summary>
    /// <param name="id">Уникальный идентификатор владельца</param>
    /// <param name="username">Имя владельца</param>
    /// <param name="bill">Счёт владельца</param>
    /// <exception cref="ArgumentNullValueException">Если аргумент null</exception>
    public Owner(Guid id, Username username, Bill bill)
        : base(id, username)
    {
        GuidEmptyValueException.ThrowIfEmpty(id);

        Bill = bill ?? throw new ArgumentNullValueException(nameof(bill));
        BillId = bill.Id;
    }

    /// <summary>
    /// Добавляет заданную сумм на кошелёк пользователя (перевод извне, с внешнего счёта)
    /// </summary>
    /// <param name="money">Количество денег</param>
    public void PutMoneyInWallet(Money money)
    {
        ArgumentNullValueException.ThrowIfNull(money, nameof(money));

        Bill.PutMoney(money);
        Bill.AddTransferTo(new Transfer(Guid.NewGuid(), money, null, Bill));
    }

    /// <summary>
    /// Выводит заданную сумм с кошелька пользователя (перевод на внешний счёт)
    /// </summary>
    /// <param name="money">Количество денег</param>
    /// <returns>true если на счёте достаточно свободных денег, иначе false</returns>
    public bool WithdrawMoneyFromWallet(Money money)
    {
        ArgumentNullValueException.ThrowIfNull(money, nameof(money));

        var isWithdrawn = Bill.WithdrawMoney(money);
        if (!isWithdrawn)
        {
            return false;
        }

        Bill.AddTransferFrom(new Transfer(Guid.NewGuid(), money, Bill, null));
        return true;
    }

    /// <summary>
    /// Оплачивает покупку лота
    /// </summary>
    /// <param name="price">Цена лота</param>
    /// <param name="seller">Продавец лота</param>
    /// <param name="lot">Данные лота</param>
    /// <returns>true если на счёте достаточно зарезервированных денег, иначе false</returns>
    public bool PayForLot(
        Price price,
        Owner seller,
        Lot lot)
    {
        ArgumentNullValueException.ThrowIfNull(price, nameof(price));
        ArgumentNullValueException.ThrowIfNull(seller, nameof(seller));
        ArgumentNullValueException.ThrowIfNull(lot, nameof(lot));

        if (!Bill.PayForLot(price))
        {
            return false;
        }

        Bill.AddTransferFrom(new Transfer(Guid.NewGuid(), price, Bill, seller.Bill, lot));
        seller.Bill.PutMoney(price);

        return true;
    }

    /// <summary>
    /// Резервирует сумму для оплаты лота
    /// </summary>
    /// <param name="price">Текущая цена лота</param>
    /// <param name="lot">Данные лота</param>
    /// <returns>true если на счёте достаточно свободных денег, иначе false</returns>
    public bool ReserveMoney(
        Price price,
        Lot lot)
    {
        ArgumentNullValueException.ThrowIfNull(price, nameof(price));
        ArgumentNullValueException.ThrowIfNull(lot, nameof(lot));

        if (!Bill.ReserveMoney(price))
        {
            return false;
        }

        Bill.AddFreezing(new Freezing(Guid.NewGuid(), Bill, price, lot, false));

        return true;
    }

    /// <summary>
    /// Размораживает заданную сумму, если на счёте есть достаточная замороженная сумма
    /// </summary>
    /// <param name="price">Текущая цена лота</param>
    /// <param name="lot">Данные лота</param>
    /// <returns>true если на счёте достаточно замороженных денег, иначе false</returns>
    public bool RealeaseMoney(
        Price price,
        Lot lot)
    {
        ArgumentNullValueException.ThrowIfNull(price, nameof(price));
        ArgumentNullValueException.ThrowIfNull(lot, nameof(lot));

        if (!Bill.RealeaseMoney(price))
        {
            return false;
        }

        Bill.AddFreezing(new Freezing(Guid.NewGuid(), Bill, price, lot, true));

        return true;
    }

    /// <summary>
    /// Проверяет наличие заданного количества свободных денег
    /// </summary>
    /// <param name="money">Количество денег</param>
    /// <returns>true если на счёте достаточно свободных денег, иначе false</returns>
    public bool HasFreeMoney(Money money)
    {
        ArgumentNullValueException.ThrowIfNull(money, nameof(money));

        return Bill.FreeMoney >= money;
    }

    /// <summary>
    /// Проверяет наличие заданного количества замороженных денег
    /// </summary>
    /// <param name="money">Количество денег</param>
    /// <returns>true если на счёте достаточно замороженных денег, иначе false</returns>
    public bool HasFrozenMoney(Money money)
    {
        ArgumentNullValueException.ThrowIfNull(money, nameof(money));

        return Bill.FrozenMoney >= money;
    }
}
