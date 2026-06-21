using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Services.Migrations
{
    /// <inheritdoc />
    public partial class relationclientsubs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientMasterClientId",
                table: "SUBSCRIPTION",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SUBSCRIPTION_ClientMasterClientId",
                table: "SUBSCRIPTION",
                column: "ClientMasterClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_SUBSCRIPTION_CLIENT_MASTER_ClientMasterClientId",
                table: "SUBSCRIPTION",
                column: "ClientMasterClientId",
                principalTable: "CLIENT_MASTER",
                principalColumn: "CLIENT_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SUBSCRIPTION_CLIENT_MASTER_ClientMasterClientId",
                table: "SUBSCRIPTION");

            migrationBuilder.DropIndex(
                name: "IX_SUBSCRIPTION_ClientMasterClientId",
                table: "SUBSCRIPTION");

            migrationBuilder.DropColumn(
                name: "ClientMasterClientId",
                table: "SUBSCRIPTION");
        }
    }
}
