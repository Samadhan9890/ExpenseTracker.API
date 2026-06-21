using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Services.Migrations
{
    /// <inheritdoc />
    public partial class SplPaymentSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SPL_PAYMENT_SCHEDULE",
                columns: table => new
                {
                    SCHEDULE_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CLIENT_ID = table.Column<int>(type: "int", nullable: false),
                    SUBSCRIPTION_ID = table.Column<int>(type: "int", nullable: false),
                    PAYMENT_TYPE = table.Column<int>(type: "int", nullable: false),
                    DUE_DATE = table.Column<DateOnly>(type: "date", nullable: false),
                    PROFIT_AMOUNT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TDS = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PAYABLE_AMOUNT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AMOUNT_PAID = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PAYMENT_DATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    PAYMENT_MODE = table.Column<string>(type: "varchar(20)", nullable: true),
                    PAYMENT_UTR = table.Column<string>(type: "varchar(100)", nullable: true),
                    PAYMENT_PROOF_ATTACHMENT = table.Column<string>(type: "varchar(255)", nullable: true),
                    STATUS = table.Column<int>(type: "int", nullable: true),
                    NOTES = table.Column<string>(type: "varchar(500)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    CREATED_BY = table.Column<string>(type: "varchar(50)", nullable: true),
                    INTEREST_RATE = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    INVESTED_AMOUNT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DAY = table.Column<string>(type: "varchar(10)", nullable: true),
                    CLIENT_NAME = table.Column<string>(type: "varchar(200)", nullable: true),
                    PAID_BY = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SPL_PAYMENT_SCHEDULE", x => x.SCHEDULE_ID);
                    table.ForeignKey(
                        name: "FK_SPL_PAYMENT_SCHEDULE_CLIENT_MASTER_CLIENT_ID",
                        column: x => x.CLIENT_ID,
                        principalTable: "CLIENT_MASTER",
                        principalColumn: "CLIENT_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SPL_PAYMENT_SCHEDULE_Investments_SUBSCRIPTION_ID",
                        column: x => x.SUBSCRIPTION_ID,
                        principalTable: "Investments",
                        principalColumn: "InvestmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SPL_PAYMENT_SCHEDULE_CLIENT_ID",
                table: "SPL_PAYMENT_SCHEDULE",
                column: "CLIENT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SPL_PAYMENT_SCHEDULE_SUBSCRIPTION_ID",
                table: "SPL_PAYMENT_SCHEDULE",
                column: "SUBSCRIPTION_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SPL_PAYMENT_SCHEDULE");
        }
    }
}
