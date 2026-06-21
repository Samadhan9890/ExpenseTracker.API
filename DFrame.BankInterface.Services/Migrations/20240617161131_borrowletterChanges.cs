using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Services.Migrations
{
    /// <inheritdoc />
    public partial class borrowletterChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SENT_DATE",
                table: "TBL_BORROW_LETTER_DETAILS",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CHEQUE_DATE",
                table: "TBL_BORROW_LETTER_DETAILS",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SENT_DATE",
                table: "TBL_BORROW_LETTER_DETAILS",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CHEQUE_DATE",
                table: "TBL_BORROW_LETTER_DETAILS",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true);
        }
    }
}
