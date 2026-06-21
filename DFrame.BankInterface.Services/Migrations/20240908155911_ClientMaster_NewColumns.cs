using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Services.Migrations
{
    /// <inheritdoc />
    public partial class ClientMaster_NewColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BLOOD_RELATION",
                table: "CLIENT_MASTER",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IS_AADHAR_PAN_LINKED",
                table: "CLIENT_MASTER",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IS_REFERRAL_BONUS_APPLICABLE",
                table: "CLIENT_MASTER",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BLOOD_RELATION",
                table: "CLIENT_MASTER");

            migrationBuilder.DropColumn(
                name: "IS_AADHAR_PAN_LINKED",
                table: "CLIENT_MASTER");

            migrationBuilder.DropColumn(
                name: "IS_REFERRAL_BONUS_APPLICABLE",
                table: "CLIENT_MASTER");
        }
    }
}
