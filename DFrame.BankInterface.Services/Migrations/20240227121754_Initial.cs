using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Services.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "BankFileGenDetails",
                columns: table => new
                {
                    BankFileBatchId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankFilePath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BankFileGenDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BankFileGenBy = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    BankFileExt = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ProfileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankFileGenDetails", x => x.BankFileBatchId);
                });

            migrationBuilder.CreateTable(
                name: "BankMasters",
                columns: table => new
                {
                    BankCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    BankAccNo = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    BankAdd01 = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    BankAdd02 = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    BankAdd03 = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    BankAdd04 = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    BankAdd05 = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    BankAccountType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    BankIfscCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BankEmailId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BankBranchCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    BankGlCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BankClientCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SeriesDetails = table.Column<long>(type: "bigint", maxLength: 30, nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    EntryBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankMasters", x => x.BankCode);
                });

            

            migrationBuilder.CreateTable(
                name: "PaymentDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    FmsSource = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Treference = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tax1 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tax2 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tax3 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tds = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tdescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContryCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    PaymentMode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    CurrencyType = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    LocationDetails = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TransactionType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    JrnalNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    JrnalLine = table.Column<int>(type: "int", nullable: false),
                    AllocRef = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    BeneSurName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BeneName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BeneAccNo = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    BeneIfscCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BeneBankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BeneAdd01 = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    BeneAdd02 = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    BeneAdd03 = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    BeneAdd04 = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    BeneAdd05 = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    BeneState = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BenePhoneNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    BeneEmailId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BeneBranchCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    BeneMobileNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    BatchNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ReversePostingStatus = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ValidationRefNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ValidationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PreviousStatus = table.Column<int>(type: "int", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    EntryBy = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    BankCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BankFileBatchId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    UnqRefNo = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    Udf1 = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Udf2 = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Udf3 = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Udf4 = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Udf5 = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    PmtUtrDetails = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    PmtStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    PmtRemarks = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    PmtDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PmtRejectionReason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PmtUnqRefNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PmtRespFileId = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentDetails_BankFileGenDetails_BankFileBatchId",
                        column: x => x.BankFileBatchId,
                        principalTable: "BankFileGenDetails",
                        principalColumn: "BankFileBatchId");
                    table.ForeignKey(
                        name: "FK_PaymentDetails_BankMasters_BankCode",
                        column: x => x.BankCode,
                        principalTable: "BankMasters",
                        principalColumn: "BankCode");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDetails_BankCode",
                table: "PaymentDetails",
                column: "BankCode");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDetails_BankFileBatchId",
                table: "PaymentDetails",
                column: "BankFileBatchId");

           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentDetails");
          

            migrationBuilder.DropTable(
                name: "BankFileGenDetails");

            migrationBuilder.DropTable(
                name: "BankMasters");
        }
    }
}
