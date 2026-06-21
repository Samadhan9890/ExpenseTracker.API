using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Services.Migrations
{
    /// <inheritdoc />
    public partial class subplanmasterchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "INTEREST_RATE",
                table: "PLAN_MASTER");

            migrationBuilder.DropColumn(
                name: "MIN_INVESTMENT",
                table: "PLAN_MASTER");

            migrationBuilder.DropColumn(
                name: "PAYOUT_FREQUENCY",
                table: "PLAN_MASTER");

            migrationBuilder.CreateTable(
                name: "SUB_PLANS_MASTER",
                columns: table => new
                {
                    SUB_PLANS_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    PLAN_CODE = table.Column<string>(type: "varchar(30)", nullable: false),
                    PAYOUT_FREQUENCY_IN_MONTHS = table.Column<int>(type: "int", nullable: false),
                    INTEREST_RATE = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    MIN_INVESTMENT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "current_timestamp")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SUB_PLANS_MASTER", x => x.SUB_PLANS_ID);
                });

            migrationBuilder.CreateTable(
                name: "TBL_BUSINESS_DEV_TEAM",
                columns: table => new
                {
                    BDID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "varchar(255)", nullable: false),
                    ADDRESS = table.Column<string>(type: "varchar(255)", nullable: false),
                    JOINING_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    ENTRY_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    STATUS = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBL_BUSINESS_DEV_TEAM", x => x.BDID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SUB_PLANS_MASTER");

            migrationBuilder.DropTable(
                name: "TBL_BUSINESS_DEV_TEAM");

            migrationBuilder.AddColumn<decimal>(
                name: "INTEREST_RATE",
                table: "PLAN_MASTER",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MIN_INVESTMENT",
                table: "PLAN_MASTER",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "PAYOUT_FREQUENCY",
                table: "PLAN_MASTER",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
