using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KaliskaHaven.Database.Migrations
{
	/// <inheritdoc/>
	public partial class InitialState : Migration
	{
		/// <inheritdoc/>
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.EnsureSchema(
				name: "discordbottie");

			migrationBuilder.CreateTable(
				name: "Persons",
				schema: "discordbottie",
				columns: table => new
				{
					ID = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					DiscordId = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
				},
				constraints: table => {
					table.PrimaryKey("PK_Persons", x => x.ID);
				});

			migrationBuilder.CreateTable(
				name: "ShopItems",
				schema: "discordbottie",
				columns: table => new
				{
					ID = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
				},
				constraints: table => {
					table.PrimaryKey("PK_ShopItems", x => x.ID);
				});

			migrationBuilder.CreateTable(
				name: "Wallets",
				schema: "discordbottie",
				columns: table => new
				{
					ID = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					OwnerID = table.Column<long>(type: "bigint", nullable: true)
				},
				constraints: table => {
					table.PrimaryKey("PK_Wallets", x => x.ID);
					table.ForeignKey(
						name: "FK_Wallets_Persons_OwnerID",
						column: x => x.OwnerID,
						principalSchema: "discordbottie",
						principalTable: "Persons",
						principalColumn: "ID");
				});

			migrationBuilder.CreateTable(
				name: "DbCurrencies",
				schema: "discordbottie",
				columns: table => new
				{
					ID = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					WalletID = table.Column<long>(type: "bigint", nullable: true),
					CurrencyType = table.Column<int>(type: "integer", nullable: false),
					Quantity = table.Column<long>(type: "bigint", nullable: false)
				},
				constraints: table => {
					table.PrimaryKey("PK_DbCurrencies", x => x.ID);
					table.ForeignKey(
						name: "FK_DbCurrencies_Wallets_WalletID",
						column: x => x.WalletID,
						principalSchema: "discordbottie",
						principalTable: "Wallets",
						principalColumn: "ID");
				});

			migrationBuilder.CreateTable(
				name: "TransactionRecords",
				schema: "discordbottie",
				columns: table => new
				{
					ID = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					Kind = table.Column<int>(type: "integer", nullable: false),
					FromID = table.Column<long>(type: "bigint", nullable: true),
					ToID = table.Column<long>(type: "bigint", nullable: true),
					WithdrawnID = table.Column<long>(type: "bigint", nullable: true),
					DepositedID = table.Column<long>(type: "bigint", nullable: true)
				},
				constraints: table => {
					table.PrimaryKey("PK_TransactionRecords", x => x.ID);
					table.ForeignKey(
						name: "FK_TransactionRecords_DbCurrencies_DepositedID",
						column: x => x.DepositedID,
						principalSchema: "discordbottie",
						principalTable: "DbCurrencies",
						principalColumn: "ID");
					table.ForeignKey(
						name: "FK_TransactionRecords_DbCurrencies_WithdrawnID",
						column: x => x.WithdrawnID,
						principalSchema: "discordbottie",
						principalTable: "DbCurrencies",
						principalColumn: "ID");
					table.ForeignKey(
						name: "FK_TransactionRecords_Wallets_FromID",
						column: x => x.FromID,
						principalSchema: "discordbottie",
						principalTable: "Wallets",
						principalColumn: "ID");
					table.ForeignKey(
						name: "FK_TransactionRecords_Wallets_ToID",
						column: x => x.ToID,
						principalSchema: "discordbottie",
						principalTable: "Wallets",
						principalColumn: "ID");
				});

			migrationBuilder.CreateIndex(
				name: "IX_DbCurrencies_WalletID",
				schema: "discordbottie",
				table: "DbCurrencies",
				column: "WalletID");

			migrationBuilder.CreateIndex(
				name: "IX_TransactionRecords_DepositedID",
				schema: "discordbottie",
				table: "TransactionRecords",
				column: "DepositedID");

			migrationBuilder.CreateIndex(
				name: "IX_TransactionRecords_FromID",
				schema: "discordbottie",
				table: "TransactionRecords",
				column: "FromID");

			migrationBuilder.CreateIndex(
				name: "IX_TransactionRecords_ToID",
				schema: "discordbottie",
				table: "TransactionRecords",
				column: "ToID");

			migrationBuilder.CreateIndex(
				name: "IX_TransactionRecords_WithdrawnID",
				schema: "discordbottie",
				table: "TransactionRecords",
				column: "WithdrawnID");

			migrationBuilder.CreateIndex(
				name: "IX_Wallets_OwnerID",
				schema: "discordbottie",
				table: "Wallets",
				column: "OwnerID",
				unique: true);
		}

		/// <inheritdoc/>
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "ShopItems",
				schema: "discordbottie");

			migrationBuilder.DropTable(
				name: "TransactionRecords",
				schema: "discordbottie");

			migrationBuilder.DropTable(
				name: "DbCurrencies",
				schema: "discordbottie");

			migrationBuilder.DropTable(
				name: "Wallets",
				schema: "discordbottie");

			migrationBuilder.DropTable(
				name: "Persons",
				schema: "discordbottie");
		}
	}
}
