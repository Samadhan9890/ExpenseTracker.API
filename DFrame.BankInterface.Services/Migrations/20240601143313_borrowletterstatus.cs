using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Services.Migrations
{
    /// <inheritdoc />
    public partial class borrowletterstatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CLOSED_BY",
                table: "SUBSCRIPTION",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CLOSING_DATE",
                table: "SUBSCRIPTION",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "INVESTMENT_RCVD_DATE",
                table: "SUBSCRIPTION",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "INVESTMENT_RECVD_DETAILS",
                table: "SUBSCRIPTION",
                type: "varchar(300)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TBL_BORROW_LETTER_DETAILS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SUBSCRIPTION_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SUBSCRIPTION_ID = table.Column<int>(type: "int", nullable: false),
                    CHEQUE_NO = table.Column<string>(type: "varchar(50)", nullable: true),
                    CHEQUE_DATE = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBL_BORROW_LETTER_DETAILS", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TBL_BORROW_LETTER_DETAILS");

            migrationBuilder.DropColumn(
                name: "CLOSED_BY",
                table: "SUBSCRIPTION");

            migrationBuilder.DropColumn(
                name: "CLOSING_DATE",
                table: "SUBSCRIPTION");

            migrationBuilder.DropColumn(
                name: "INVESTMENT_RCVD_DATE",
                table: "SUBSCRIPTION");

            migrationBuilder.DropColumn(
                name: "INVESTMENT_RECVD_DETAILS",
                table: "SUBSCRIPTION");
        }
    }
}
