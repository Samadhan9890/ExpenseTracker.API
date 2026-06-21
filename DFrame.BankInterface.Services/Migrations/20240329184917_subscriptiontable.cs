using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Services.Migrations
{
    /// <inheritdoc />
    public partial class subscriptiontable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SUBSCRIPTION",
                columns: table => new
                {
                    SUBSCRIPTION_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SUBSCRIPTION_TYPE = table.Column<string>(type: "varchar(255)", nullable: false),
                    OLD_SUBSCRIPTION_ID = table.Column<int>(type: "int", nullable: true),
                    CLIENT_ID = table.Column<int>(type: "int", nullable: false),
                    DATE_OF_INVESTMENT = table.Column<DateTime>(type: "datetime", nullable: false),
                    PLAN_CODE = table.Column<string>(type: "varchar(30)", nullable: false),
                    PLAN_NAME = table.Column<string>(type: "varchar(50)", nullable: false),
                    INVESTMENT_AMOUNT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PAYOUT_FREQUENCY = table.Column<int>(type: "int", nullable: false),
                    TOTAL_INTEREST = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PAYOUT_FREQUENCY_INTEREST_RATE = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    MATURITY_DATE = table.Column<DateTime>(type: "datetime", nullable: false),
                    APPROVED_BY = table.Column<string>(type: "varchar(255)", nullable: false),
                    APPROVED_DATE = table.Column<DateTime>(type: "datetime", nullable: false),
                    BORROW_LETTER_STATUS = table.Column<string>(type: "varchar(255)", nullable: false),
                    PAYOUT_METHOD = table.Column<string>(type: "varchar(255)", nullable: false),
                    PAYOUT_BANK_NAME = table.Column<string>(type: "varchar(255)", nullable: true),
                    PAYOUT_BANK_ACCOUNT_NO = table.Column<int>(type: "int", nullable: true),
                    PAYOUT_BANK_IFSC_CODE = table.Column<string>(type: "varchar(255)", nullable: true),
                    PAYOUT_BANK_ACCOUNT_HOLDER_NAME = table.Column<string>(type: "varchar(255)", nullable: true),
                    UPI_ID = table.Column<string>(type: "varchar(255)", nullable: true),
                    NOTES = table.Column<string>(type: "varchar(255)", nullable: true),
                    STATUS = table.Column<int>(type: "int", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    CREATED_BY = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SUBSCRIPTION", x => x.SUBSCRIPTION_ID);
                    table.ForeignKey(
                        name: "FK_SUBSCRIPTION_CLIENT_MASTER_CLIENT_ID",
                        column: x => x.CLIENT_ID,
                        principalTable: "CLIENT_MASTER",
                        principalColumn: "CLIENT_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SUBSCRIPTION_PLAN_MASTER_PLAN_CODE",
                        column: x => x.PLAN_CODE,
                        principalTable: "PLAN_MASTER",
                        principalColumn: "PLAN_CODE",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SUBSCRIPTION_CLIENT_ID",
                table: "SUBSCRIPTION",
                column: "CLIENT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SUBSCRIPTION_PLAN_CODE",
                table: "SUBSCRIPTION",
                column: "PLAN_CODE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SUBSCRIPTION");
        }
    }
}
