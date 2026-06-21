using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Services.Migrations
{
    /// <inheritdoc />
    public partial class createDepartmentLocationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TBL_CUSTOM_REPORT_MASTER",
                schema: "dbo",
                columns: table => new
                {
                    CUSTOM_REPORT_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CUSTOM_REPORT_CODE = table.Column<string>(type: "varchar(200)", nullable: false),
                    CUSTOM_REPORT_NAME = table.Column<string>(type: "varchar(200)", nullable: false),
                    CUSTOM_REPORT_DESC = table.Column<string>(type: "varchar(max)", nullable: false),
                    CUSTOM_REPORT_QUERY = table.Column<string>(type: "varchar(max)", nullable: false),
                    STATUS = table.Column<int>(type: "int", nullable: true),
                    ENTRY_ID = table.Column<int>(type: "int", nullable: true),
                    ENTRY_DATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    MODIFY_ID = table.Column<int>(type: "int", nullable: true),
                    MODIFY_DATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    DELETE_ID = table.Column<int>(type: "int", nullable: true),
                    DELETE_DATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    ROLE_ACCESS = table.Column<string>(type: "varchar(max)", nullable: false),
                    ISDAG = table.Column<int>(type: "int", nullable: true),
                    DATE_FILTER = table.Column<string>(type: "varchar(200)", nullable: true),
                    ORDER_FILTER = table.Column<string>(type: "varchar(max)", nullable: true),
                    DAG_DESC = table.Column<string>(type: "varchar(50)", nullable: true),
                    COLUMN_FILTER = table.Column<string>(type: "varchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBL_CUSTOM_REPORT_MASTER", x => x.CUSTOM_REPORT_ID);
                });

            migrationBuilder.CreateTable(
                name: "TBL_DEPARTMENT",
                schema: "dbo",
                columns: table => new
                {
                    DEPARTMENT_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DEPARTMENT_CODE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DEPARTMENT_DESCRIPTION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SUN_ACCOUNT_CODE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    STATUS = table.Column<bool>(type: "bit", nullable: true),
                    ENTRY_ID = table.Column<int>(type: "int", nullable: true),
                    ENTRY_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MODIFY_ID = table.Column<int>(type: "int", nullable: true),
                    MODIFY_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DELETE_ID = table.Column<int>(type: "int", nullable: true),
                    DELETE_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    L_DEP_ONLY = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBL_DEPARTMENT", x => x.DEPARTMENT_ID);
                });

            migrationBuilder.CreateTable(
                name: "TBL_LOC",
                schema: "dbo",
                columns: table => new
                {
                    LOCATION_ID = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LOCATION_CODE = table.Column<string>(type: "VARCHAR(50)", nullable: true),
                    LOCATION_NAME = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    ADDRESS1 = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    ADDRESS2 = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    ADDRESS3 = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    ADDRESS4 = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    ADDRESS5 = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    BUSINESS_UNIT = table.Column<int>(type: "int", nullable: true),
                    ENTRY_ID = table.Column<int>(type: "int", nullable: true),
                    ENTRY_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MODIFY_ID = table.Column<int>(type: "int", nullable: true),
                    MODIFY_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DELETE_ID = table.Column<int>(type: "int", nullable: true),
                    DELETE_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    STATUS = table.Column<int>(type: "int", nullable: true),
                    PRINT_ADDRESS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PINCODE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EMAIL_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CITY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    STATE_ID = table.Column<int>(type: "int", nullable: true),
                    TELEPHONE = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBL_LOC", x => x.LOCATION_ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TBL_CUSTOM_REPORT_MASTER",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TBL_DEPARTMENT",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TBL_LOC",
                schema: "dbo");
        }
    }
}
