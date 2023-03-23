﻿using DisCatSharp.Entities;

using KaliskaHaven.Database;
using KaliskaHaven.Database.Economy;
using KaliskaHaven.Database.Entities;
using KaliskaHaven.Economy;

using Microsoft.EntityFrameworkCore;

using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Glue.Economy
{
	public sealed class Wallet : IDbWallet
	{
		public static async Task<(Person, Wallet)> EnsureCreated(KaliskaDB db, DiscordUser user)
		{
			var person = await db.Persons.FirstOrDefaultAsync(x => x.DiscordId == user.Id);
			if (person == null)
			{
				person = new Person {
					DiscordId = user.Id,
				};

				await using var tr = await db.Database.BeginTransactionAsync();
				await db.Persons.AddAsync(person);
				await db.SaveChangesAsync();
				await tr.CommitAsync();
			}

			var wallet = await EnsureCreated(db, person);

			return (person, wallet);
		}

		public static async Task<Wallet> EnsureCreated(KaliskaDB db, Person person)
		{
			var walletT = await db.Wallets.FirstOrDefaultAsync(x => x.Owner == person);

			await using var tr = await db.Database.BeginTransactionAsync();
			if (walletT != null)
			{
				await tr.CommitAsync();
				return new Wallet(db, walletT);
			}

			var wallet = new Database.Economy.Wallet();
			person.Wallet = wallet;
			wallet.Owner = person;

			await db.Wallets.AddAsync(wallet);
			await db.SaveChangesAsync();

			await tr.CommitAsync();

			return new Wallet(db, wallet);
		}

		private readonly KaliskaDB _db;
		private readonly Database.Economy.Wallet _wallet;

		public Wallet(KaliskaDB db, Database.Economy.Wallet wallet)
		{
			_db = db;
			_wallet = wallet;
		}

		public long ID => _wallet.ID;

		public ICollection<DbCurrency> DbCurrencies => _wallet.DbCurrencies;

		public Person Owner => _wallet.Owner;

		public IIdentity? Identity => _wallet.Identity;

		public IWallet? Identifyable => _wallet.Identifyable;

		public Type Type => _wallet.Type;

		public async Task EnsureFullyLoaded()
		{
			var entry = _db.Entry(_wallet);
			await entry.Collection(x => x.DbCurrencies).LoadAsync();
			await entry.Reference(x => x.Owner).LoadAsync();
			await _db.SaveChangesAsync();
		}

		public async Task<IIdentifiable<ITransactionLog>> Deposit(Currency currency)
		{
			await this.EnsureFullyLoaded();
			var transaction = await _wallet.Deposit(currency);
			await _db.SaveChangesAsync();
			return transaction;
		}

		public bool Equals<TId>(IIdentifiable<TId> to) => to is IIdentifiable<IWallet> wa && wa is IDbWallet dw && dw.ID == ID;

		public async Task<Currency?> Get(CurrencyType currency)
		{
			await this.EnsureFullyLoaded();
			return await _wallet.Get(currency);
		}

		public async IAsyncEnumerable<Currency> GetAllCurrencies()
		{
			await this.EnsureFullyLoaded();
			await foreach (var curr in _wallet.GetAllCurrencies())
				yield return curr;
		}

		public async Task<IIdentifiable<ITransactionLog>> Withdraw(Currency currency)
		{
			await this.EnsureFullyLoaded();
			var transaction = await _wallet.Withdraw(currency);
			await _db.SaveChangesAsync();
			return transaction;
		}
	}
}
