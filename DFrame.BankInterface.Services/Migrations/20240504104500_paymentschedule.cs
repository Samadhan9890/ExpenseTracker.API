using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Services.Migrations
{
    /// <inheritdoc />
    public partial class paymentschedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PAYMENT_SCHEDULE",
                columns: table => new
                {
                    SCHEDULE_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    SubscriptionId = table.Column<int>(type: "int", nullable: false),
                    DUE_DATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    PAYABLE_AMOUNT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AMOUNT_PAID = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PAYMENT_DATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    PAYMENT_MODE = table.Column<string>(type: "varchar(30)", nullable: true),
                    PAYMENT_UTR = table.Column<string>(type: "varchar(30)", nullable: true),
                    PAYMENT_PROOF_ATTACHMENT = table.Column<string>(type: "varchar(255)", nullable: true),
                    STATUS = table.Column<int>(type: "int", nullable: true),
                    NOTES = table.Column<string>(type: "varchar(255)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime", nullable: false),
                    CREATED_BY = table.Column<string>(type: "varchar(50)", nullable: true),
                    INTEREST_RATE = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    INVESTED_AMOUNT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DAY = table.Column<string>(type: "varchar(20)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PAYMENT_SCHEDULE", x => x.SCHEDULE_ID);
                    table.ForeignKey(
                        name: "FK_PAYMENT_SCHEDULE_CLIENT_MASTER_ClientId",
                        column: x => x.ClientId,
                        principalTable: "CLIENT_MASTER",
                        principalColumn: "CLIENT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PAYMENT_SCHEDULE_SUBSCRIPTION_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "SUBSCRIPTION",
                        principalColumn: "SUBSCRIPTION_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PAYMENT_SCHEDULE_ClientId",
                table: "PAYMENT_SCHEDULE",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_PAYMENT_SCHEDULE_SubscriptionId",
                table: "PAYMENT_SCHEDULE",
                column: "SubscriptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PAYMENT_SCHEDULE");
        }
    }
}
