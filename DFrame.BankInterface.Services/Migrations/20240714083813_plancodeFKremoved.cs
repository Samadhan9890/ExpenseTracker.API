using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Services.Migrations
{
    /// <inheritdoc />
    public partial class plancodeFKremoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SUBSCRIPTION_PLAN_MASTER_PLAN_CODE",
                table: "SUBSCRIPTION");

            migrationBuilder.DropIndex(
                name: "IX_SUBSCRIPTION_PLAN_CODE",
                table: "SUBSCRIPTION");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SUBSCRIPTION_PLAN_CODE",
                table: "SUBSCRIPTION",
                column: "PLAN_CODE");

            migrationBuilder.AddForeignKey(
                name: "FK_SUBSCRIPTION_PLAN_MASTER_PLAN_CODE",
                table: "SUBSCRIPTION",
                column: "PLAN_CODE",
                principalTable: "PLAN_MASTER",
                principalColumn: "PLAN_CODE",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
