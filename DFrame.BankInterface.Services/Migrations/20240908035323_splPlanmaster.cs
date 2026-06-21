using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Services.Migrations
{
    /// <inheritdoc />
    public partial class splPlanmaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SPL_PLAN_MASTER",
                columns: table => new
                {
                    PlanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PLAN_CODE = table.Column<string>(type: "varchar(30)", nullable: false),
                    PLAN_NAME = table.Column<string>(type: "varchar(50)", nullable: false),
                    DESCRIPTION = table.Column<string>(type: "varchar(100)", nullable: true),
                    MIN_INVESTMENT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PLAN_PERIOD = table.Column<int>(type: "int", nullable: false),
                    PAYOUT_FREQUENCY = table.Column<int>(type: "int", nullable: false),
                    PROFIT_RATE = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    IS_BONUS_APPLICABLE = table.Column<bool>(type: "bit", nullable: false),
                    BONUS_PAYOUT_TIME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BONUS_PERCENT = table.Column<int>(type: "int", nullable: false),
                    CAPITAL_RETURN_PERIOD = table.Column<int>(type: "int", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    STATUS = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SPL_PLAN_MASTER", x => x.PlanId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SPL_PLAN_MASTER");
        }
    }
}
