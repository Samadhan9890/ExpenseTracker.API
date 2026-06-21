using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Services.Migrations
{
    /// <inheritdoc />
    public partial class clear_user_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BRANCH",
                schema: "dbo",
                table: "TBL_USER");

            migrationBuilder.DropColumn(
                name: "BU_SUN_CODE",
                schema: "dbo",
                table: "TBL_USER");

            migrationBuilder.DropColumn(
                name: "DESIGNATION_ID",
                schema: "dbo",
                table: "TBL_USER");

            migrationBuilder.DropColumn(
                name: "DESIGNATION_NAME",
                schema: "dbo",
                table: "TBL_USER");

            migrationBuilder.DropColumn(
                name: "DIVISION_ID",
                schema: "dbo",
                table: "TBL_USER");

            migrationBuilder.DropColumn(
                name: "EMP_PEOPLESOFT_CODE",
                schema: "dbo",
                table: "TBL_USER");

            migrationBuilder.DropColumn(
                name: "EMP_SUN_CODE",
                schema: "dbo",
                table: "TBL_USER");

            migrationBuilder.DropColumn(
                name: "ENTITY_ID",
                schema: "dbo",
                table: "TBL_USER");

            migrationBuilder.DropColumn(
                name: "ENTITY_NAME",
                schema: "dbo",
                table: "TBL_USER");

            migrationBuilder.DropColumn(
                name: "IS_AD",
                schema: "dbo",
                table: "TBL_USER");

            migrationBuilder.DropColumn(
                name: "IS_SHARED_ADMIN_STAFF",
                schema: "dbo",
                table: "TBL_USER");

            migrationBuilder.DropColumn(
                name: "IS_SHARED_SERVICE_STAFF",
                schema: "dbo",
                table: "TBL_USER");

            migrationBuilder.DropColumn(
                name: "JOB_CODE",
                schema: "dbo",
                table: "TBL_USER");

            migrationBuilder.DropColumn(
                name: "LOCATION_NAME",
                schema: "dbo",
                table: "TBL_USER");

            migrationBuilder.DropColumn(
                name: "MAX_ADV_OUT_LIMIT",
                schema: "dbo",
                table: "TBL_USER");

            migrationBuilder.DropColumn(
                name: "REIMBURSEMENT_ACCESS",
                schema: "dbo",
                table: "TBL_USER");

            migrationBuilder.DropColumn(
                name: "T1",
                schema: "dbo",
                table: "TBL_USER");

            migrationBuilder.DropColumn(
                name: "T2",
                schema: "dbo",
                table: "TBL_USER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BRANCH",
                schema: "dbo",
                table: "TBL_USER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BU_SUN_CODE",
                schema: "dbo",
                table: "TBL_USER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DESIGNATION_ID",
                schema: "dbo",
                table: "TBL_USER",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DESIGNATION_NAME",
                schema: "dbo",
                table: "TBL_USER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DIVISION_ID",
                schema: "dbo",
                table: "TBL_USER",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EMP_PEOPLESOFT_CODE",
                schema: "dbo",
                table: "TBL_USER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EMP_SUN_CODE",
                schema: "dbo",
                table: "TBL_USER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ENTITY_ID",
                schema: "dbo",
                table: "TBL_USER",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ENTITY_NAME",
                schema: "dbo",
                table: "TBL_USER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IS_AD",
                schema: "dbo",
                table: "TBL_USER",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IS_SHARED_ADMIN_STAFF",
                schema: "dbo",
                table: "TBL_USER",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IS_SHARED_SERVICE_STAFF",
                schema: "dbo",
                table: "TBL_USER",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JOB_CODE",
                schema: "dbo",
                table: "TBL_USER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LOCATION_NAME",
                schema: "dbo",
                table: "TBL_USER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MAX_ADV_OUT_LIMIT",
                schema: "dbo",
                table: "TBL_USER",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "REIMBURSEMENT_ACCESS",
                schema: "dbo",
                table: "TBL_USER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "T1",
                schema: "dbo",
                table: "TBL_USER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "T2",
                schema: "dbo",
                table: "TBL_USER",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
