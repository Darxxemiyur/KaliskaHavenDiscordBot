using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KaliskaHaven.Database.Migrations
{
	/// <inheritdoc/>
	public partial class Hehe2 : Migration
	{
		/// <inheritdoc/>
		protected override void Up(MigrationBuilder migrationBuilder)
		{
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
		}

		/// <inheritdoc/>
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "TransactionRecords",
				schema: "discordbottie");
		}
	}
}
