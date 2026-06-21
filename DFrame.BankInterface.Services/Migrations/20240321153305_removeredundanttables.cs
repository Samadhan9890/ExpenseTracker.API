using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Services.Migrations
{
    /// <inheritdoc />
    public partial class removeredundanttables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentDetails_BankFileGenDetails_BankFileBatchId",
                table: "PaymentDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentDetails_BankMasters_BankCode",
                table: "PaymentDetails");

            migrationBuilder.DropTable(
                name: "BankFileGenDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentDetails",
                table: "PaymentDetails");

            migrationBuilder.DropIndex(
                name: "IX_PaymentDetails_BankCode",
                table: "PaymentDetails");

            migrationBuilder.DropIndex(
                name: "IX_PaymentDetails_BankFileBatchId",
                table: "PaymentDetails");

            migrationBuilder.RenameTable(
                name: "PaymentDetails",
                newName: "TblPaymentDetails");

            migrationBuilder.AlterColumn<Guid>(
                name: "GUID",
                table: "TblPaymentDetails",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "NEWID()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EntryDate",
                table: "TblPaymentDetails",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TblPaymentDetails",
                table: "TblPaymentDetails",
                column: "Id");

            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropPrimaryKey(
                name: "PK_TblPaymentDetails",
                table: "TblPaymentDetails");

            migrationBuilder.RenameTable(
                name: "TblPaymentDetails",
                newName: "PaymentDetails");

            migrationBuilder.AlterColumn<Guid>(
                name: "GUID",
                table: "PaymentDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EntryDate",
                table: "PaymentDetails",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentDetails",
                table: "PaymentDetails",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BankFileGenDetails",
                columns: table => new
                {
                    BankFileBatchId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    BankFileExt = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BankFileGenBy = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    BankFileGenDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BankFilePath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankFileGenDetails", x => x.BankFileBatchId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDetails_BankCode",
                table: "PaymentDetails",
                column: "BankCode");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDetails_BankFileBatchId",
                table: "PaymentDetails",
                column: "BankFileBatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentDetails_BankFileGenDetails_BankFileBatchId",
                table: "PaymentDetails",
                column: "BankFileBatchId",
                principalTable: "BankFileGenDetails",
                principalColumn: "BankFileBatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentDetails_BankMasters_BankCode",
                table: "PaymentDetails",
                column: "BankCode",
                principalTable: "BankMasters",
                principalColumn: "BankCode");
        }
    }
}
