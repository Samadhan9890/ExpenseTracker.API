using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Services.Migrations
{
    /// <inheritdoc />
    public partial class bdadditionlcolmns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "TBL_BUSINESS_DEV_TEAM",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsClient",
                table: "TBL_BUSINESS_DEV_TEAM",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "TBL_BUSINESS_DEV_TEAM");

            migrationBuilder.DropColumn(
                name: "IsClient",
                table: "TBL_BUSINESS_DEV_TEAM");
        }
    }
}
