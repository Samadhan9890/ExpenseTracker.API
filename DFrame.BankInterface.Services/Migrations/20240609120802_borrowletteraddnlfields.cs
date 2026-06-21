using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Services.Migrations
{
    /// <inheritdoc />
    public partial class borrowletteraddnlfields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SENT_DATE",
                table: "TBL_BORROW_LETTER_DETAILS",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TRACKING_NO",
                table: "TBL_BORROW_LETTER_DETAILS",
                type: "varchar(50)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SENT_DATE",
                table: "TBL_BORROW_LETTER_DETAILS");

            migrationBuilder.DropColumn(
                name: "TRACKING_NO",
                table: "TBL_BORROW_LETTER_DETAILS");
        }
    }
}
