using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NETAPI.Migrations
{
    /// <inheritdoc />
    public partial class TransactionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Wallets_WalletId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_WalletId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "WalletId",
                table: "Transaction");

            migrationBuilder.AddColumn<int>(
                name: "DestWalletId",
                table: "Transaction",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SourceWalletId",
                table: "Transaction",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_DestWalletId",
                table: "Transaction",
                column: "DestWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_SourceWalletId",
                table: "Transaction",
                column: "SourceWalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Wallets_DestWalletId",
                table: "Transaction",
                column: "DestWalletId",
                principalTable: "Wallets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Wallets_SourceWalletId",
                table: "Transaction",
                column: "SourceWalletId",
                principalTable: "Wallets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Wallets_DestWalletId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Wallets_SourceWalletId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_DestWalletId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_SourceWalletId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "DestWalletId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "SourceWalletId",
                table: "Transaction");

            migrationBuilder.AddColumn<int>(
                name: "WalletId",
                table: "Transaction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_WalletId",
                table: "Transaction",
                column: "WalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Wallets_WalletId",
                table: "Transaction",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
