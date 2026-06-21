using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Services.Migrations
{
    /// <inheritdoc />
    public partial class bdTeamAddcolumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AadharNo",
                table: "TBL_BUSINESS_DEV_TEAM",
                type: "varchar(15)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PanNo",
                table: "TBL_BUSINESS_DEV_TEAM",
                type: "varchar(12)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AadharNo",
                table: "TBL_BUSINESS_DEV_TEAM");

            migrationBuilder.DropColumn(
                name: "PanNo",
                table: "TBL_BUSINESS_DEV_TEAM");
        }
    }
}
