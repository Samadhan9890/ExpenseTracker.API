using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Services.Migrations
{
    /// <inheritdoc />
    public partial class joiningbonus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CAPITAL_LOCKING_PERIOD",
                table: "SPL_PLAN_MASTER",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IS_JOINING_BONUS_APPLICABLE",
                table: "SPL_PLAN_MASTER",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "JOINING_BONUS_PERCENT",
                table: "SPL_PLAN_MASTER",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CapitalLockingReturnPeriod",
                table: "Investments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsJoiningBonusApplicable",
                table: "Investments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "JoiningBonusPercent",
                table: "Investments",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CAPITAL_LOCKING_PERIOD",
                table: "SPL_PLAN_MASTER");

            migrationBuilder.DropColumn(
                name: "IS_JOINING_BONUS_APPLICABLE",
                table: "SPL_PLAN_MASTER");

            migrationBuilder.DropColumn(
                name: "JOINING_BONUS_PERCENT",
                table: "SPL_PLAN_MASTER");

            migrationBuilder.DropColumn(
                name: "CapitalLockingReturnPeriod",
                table: "Investments");

            migrationBuilder.DropColumn(
                name: "IsJoiningBonusApplicable",
                table: "Investments");

            migrationBuilder.DropColumn(
                name: "JoiningBonusPercent",
                table: "Investments");
        }
    }
}
