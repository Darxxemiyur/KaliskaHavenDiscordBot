using KaliskaHaven.Database.Economy;
using KaliskaHaven.Database.Entities;

using Wallet = KaliskaHaven.Glue.Economy.Wallet;

namespace KaliskaHaven.DiscordUI.EconomyUI;

public record class TransferDetails(string Locale, Wallet From, Wallet To, DbCurrency Currency);
public record class DepositDetails(string Locale, Wallet To, DbCurrency Currency);
public record class WithdrawDetails(string Locale, Wallet From, DbCurrency Currency);

public record class AskForDepositPermissions(string Locale, Person Person);
