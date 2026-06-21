using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Services.Migrations
{
    /// <inheritdoc />
    public partial class planMaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PLAN_MASTER",
                columns: table => new
                {
                    PLAN_CODE = table.Column<string>(type: "varchar(30)", nullable: false),
                    PlanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PLAN_NAME = table.Column<string>(type: "varchar(50)", nullable: false),
                    DESCRIPTION = table.Column<string>(type: "varchar(100)", nullable: false),
                    PAYOUT_FREQUENCY = table.Column<int>(type: "int", nullable: false),
                    INTEREST_RATE = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    MIN_INVESTMENT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "current_timestamp"),
                    STATUS = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLAN_MASTER", x => x.PLAN_CODE);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PLAN_MASTER");
        }
    }
}
