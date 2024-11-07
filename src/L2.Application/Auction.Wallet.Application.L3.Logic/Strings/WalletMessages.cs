namespace Auction.Wallet.Application.L3.Logic.Strings;

public class WalletMessages
{
    public const string ThereIsNotEnoughFreeMoneyInBill = "На счёте недостаточно свободных денег";
    public const string ThereIsNotEnoughReservedMoneyInBill = "На счёте недостаточно зарезервированных денег";

    public const string MoneySuccessfullyAddedToWallet = "Деньги успешно добавлены в кошелёк";
    public const string MoneyWasSuccessfullyWithdrawnToExternalBill = "Деньги успешно выведены на внешний счёт";

    public const string BuyerAndSellerIdsCannotMatch = "Id покупателя и продавца не могут совпадать ({0})";
    public const string NotEnoughMoneyReserved = "Зарезервировано недостаточно денег";
    public const string FailedToPayForLot = "Не удалось оплатить лот";
    public const string PaymentForLotWasSuccessful = "Оплата лота прошла успешно";

    public const string MoneySuccessfullyUnfrozen = "Деньги успешно разморожены";
    public const string MoneySuccessfullyReserved = "Деньги успешно зарезервированы";
}
