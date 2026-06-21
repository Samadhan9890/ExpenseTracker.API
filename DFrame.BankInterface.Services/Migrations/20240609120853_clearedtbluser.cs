using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Services.Migrations
{
    /// <inheritdoc />
    public partial class clearedtbluser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "APPROVAL_LIMIT",
                schema: "dbo",
                table: "TBL_USER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "APPROVAL_LIMIT",
                schema: "dbo",
                table: "TBL_USER",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
