using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Services.Migrations
{
    /// <inheritdoc />
    public partial class userMFA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IS_MFA",
                schema: "dbo",
                table: "TBL_USER",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LAST_OTP",
                schema: "dbo",
                table: "TBL_USER",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IS_MFA",
                schema: "dbo",
                table: "TBL_USER");

            migrationBuilder.DropColumn(
                name: "LAST_OTP",
                schema: "dbo",
                table: "TBL_USER");
        }
    }
}
