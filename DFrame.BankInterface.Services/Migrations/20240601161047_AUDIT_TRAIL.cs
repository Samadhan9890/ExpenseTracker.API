using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Services.Migrations
{
    /// <inheritdoc />
    public partial class AUDIT_TRAIL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TBL_AUDIT_TRAIL",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PROCESS_ID = table.Column<int>(type: "int", nullable: true),
                    PROCESS_DESC = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    UNQ_ID = table.Column<int>(type: "int", nullable: true),
                    ACTION_DESC = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    ACTION_DATE = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    COMMENTS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    USER_ID = table.Column<int>(type: "int", nullable: true),
                    USER_NAME = table.Column<string>(type: "nvarchar(500)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBL_AUDIT_TRAIL", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TBL_AUDIT_TRAIL");
        }
    }
}
