using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Services.Migrations
{
    /// <inheritdoc />
    public partial class Create_PaymentProof : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TBL_PAYMENT_PROOF",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PAYMENT_SCHEDULE_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TREFERENCE = table.Column<string>(type: "varchar(50)", nullable: false),
                    NOTES = table.Column<string>(type: "varchar(500)", nullable: false),
                    ATTACHMENT = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime", nullable: false),
                    CREATED_BY = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBL_PAYMENT_PROOF", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TBL_PAYMENT_PROOF");
        }
    }
}
