using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Services.Migrations
{
    /// <inheritdoc />
    public partial class paymentscheduleAvailableflag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IS_PAYMENT_SCEDULE_AVAILABLE",
                table: "SUBSCRIPTION",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IS_PAYMENT_SCEDULE_AVAILABLE",
                table: "SUBSCRIPTION",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
