using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Services.Migrations
{
    /// <inheritdoc />
    public partial class investmentid_column_change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SPL_PAYMENT_SCHEDULE_CLIENT_MASTER_CLIENT_ID",
                table: "SPL_PAYMENT_SCHEDULE");

            migrationBuilder.DropForeignKey(
                name: "FK_SPL_PAYMENT_SCHEDULE_Investments_SUBSCRIPTION_ID",
                table: "SPL_PAYMENT_SCHEDULE");

            migrationBuilder.DropIndex(
                name: "IX_SPL_PAYMENT_SCHEDULE_CLIENT_ID",
                table: "SPL_PAYMENT_SCHEDULE");

            migrationBuilder.RenameColumn(
                name: "SUBSCRIPTION_ID",
                table: "SPL_PAYMENT_SCHEDULE",
                newName: "INVESTMENT_ID");

            migrationBuilder.RenameIndex(
                name: "IX_SPL_PAYMENT_SCHEDULE_SUBSCRIPTION_ID",
                table: "SPL_PAYMENT_SCHEDULE",
                newName: "IX_SPL_PAYMENT_SCHEDULE_INVESTMENT_ID");

            migrationBuilder.AddColumn<int>(
                name: "ClientMasterClientId",
                table: "SPL_PAYMENT_SCHEDULE",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ExternalPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransferredFrom = table.Column<string>(type: "varchar(500)", nullable: false),
                    TransferredTo = table.Column<string>(type: "varchar(500)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DrCr = table.Column<string>(type: "varchar(10)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalPayments", x => x.Id);
                });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SPL_PAYMENT_SCHEDULE_CLIENT_MASTER_ClientMasterClientId",
                table: "SPL_PAYMENT_SCHEDULE");

            migrationBuilder.DropForeignKey(
                name: "FK_SPL_PAYMENT_SCHEDULE_Investments_INVESTMENT_ID",
                table: "SPL_PAYMENT_SCHEDULE");

            migrationBuilder.DropTable(
                name: "ExternalPayments");

            migrationBuilder.DropIndex(
                name: "IX_SPL_PAYMENT_SCHEDULE_ClientMasterClientId",
                table: "SPL_PAYMENT_SCHEDULE");

            migrationBuilder.DropColumn(
                name: "ClientMasterClientId",
                table: "SPL_PAYMENT_SCHEDULE");

            migrationBuilder.RenameColumn(
                name: "INVESTMENT_ID",
                table: "SPL_PAYMENT_SCHEDULE",
                newName: "SUBSCRIPTION_ID");

            migrationBuilder.RenameIndex(
                name: "IX_SPL_PAYMENT_SCHEDULE_INVESTMENT_ID",
                table: "SPL_PAYMENT_SCHEDULE",
                newName: "IX_SPL_PAYMENT_SCHEDULE_SUBSCRIPTION_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SPL_PAYMENT_SCHEDULE_CLIENT_ID",
                table: "SPL_PAYMENT_SCHEDULE",
                column: "CLIENT_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_SPL_PAYMENT_SCHEDULE_CLIENT_MASTER_CLIENT_ID",
                table: "SPL_PAYMENT_SCHEDULE",
                column: "CLIENT_ID",
                principalTable: "CLIENT_MASTER",
                principalColumn: "CLIENT_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SPL_PAYMENT_SCHEDULE_Investments_SUBSCRIPTION_ID",
                table: "SPL_PAYMENT_SCHEDULE",
                column: "SUBSCRIPTION_ID",
                principalTable: "Investments",
                principalColumn: "InvestmentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
