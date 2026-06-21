using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Services.Migrations
{
    /// <inheritdoc />
    public partial class fieldInvestmentReceivedMethodAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PAYOUT_METHOD",
                table: "SUBSCRIPTION",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.AddColumn<string>(
                name: "RECEIVED_INVESTMENT_METHOD",
                table: "SUBSCRIPTION",
                type: "Varchar(50)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RECEIVED_INVESTMENT_METHOD",
                table: "SUBSCRIPTION");

            migrationBuilder.AlterColumn<string>(
                name: "PAYOUT_METHOD",
                table: "SUBSCRIPTION",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true);
        }
    }
}
