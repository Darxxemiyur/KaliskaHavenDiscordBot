using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KaliskaHaven.Database.Migrations
{
	/// <inheritdoc/>
	public partial class Hehe1 : Migration
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

			migrationBuilder.CreateIndex(
				name: "IX_DbCurrencies_WalletID",
				schema: "discordbottie",
				table: "DbCurrencies",
				column: "WalletID");

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
				name: "DbCurrencies",
				schema: "discordbottie");

			migrationBuilder.DropTable(
				name: "ShopItems",
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
