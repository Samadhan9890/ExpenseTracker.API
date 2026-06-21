using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Services.Migrations
{
    /// <inheritdoc />
    public partial class tblroles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TBL_ROLE",
                schema: "dbo",
                columns: table => new
                {
                    ROLE_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ROLE_NAME = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    ENTRY_ID = table.Column<int>(type: "int", nullable: true),
                    MODIFY_ID = table.Column<int>(type: "int", nullable: true),
                    ENTRY_DATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    MODIFY_DATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    DELETE_ID = table.Column<int>(type: "int", nullable: true),
                    DELETE_DATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    STATUS = table.Column<bool>(type: "bit", nullable: true),
                    BU_ID = table.Column<int>(type: "int", nullable: true),
                    ROLE_DESCRIPTION = table.Column<string>(type: "varchar(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBL_ROLE", x => x.ROLE_ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TBL_ROLE",
                schema: "dbo");
        }
    }
}
