using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Services.Migrations
{
    /// <inheritdoc />
    public partial class officeFiledsCLientMAster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OFFICE_ADDRESS_LINE1",
                table: "CLIENT_MASTER",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OFFICE_ADDRESS_LINE2",
                table: "CLIENT_MASTER",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OFFICE_ADDRESS_LINE3",
                table: "CLIENT_MASTER",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OFFICE_CITY",
                table: "CLIENT_MASTER",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OFFICE_PIN_CODE",
                table: "CLIENT_MASTER",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OFFICE_STATE",
                table: "CLIENT_MASTER",
                type: "varchar(255)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OFFICE_ADDRESS_LINE1",
                table: "CLIENT_MASTER");

            migrationBuilder.DropColumn(
                name: "OFFICE_ADDRESS_LINE2",
                table: "CLIENT_MASTER");

            migrationBuilder.DropColumn(
                name: "OFFICE_ADDRESS_LINE3",
                table: "CLIENT_MASTER");

            migrationBuilder.DropColumn(
                name: "OFFICE_CITY",
                table: "CLIENT_MASTER");

            migrationBuilder.DropColumn(
                name: "OFFICE_PIN_CODE",
                table: "CLIENT_MASTER");

            migrationBuilder.DropColumn(
                name: "OFFICE_STATE",
                table: "CLIENT_MASTER");
        }
    }
}
