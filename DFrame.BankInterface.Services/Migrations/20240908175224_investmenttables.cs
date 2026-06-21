using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Services.Migrations
{
    /// <inheritdoc />
    public partial class investmenttables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientBankingDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ClientGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvestmentId = table.Column<int>(type: "int", nullable: false),
                    InvestmentGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Mode = table.Column<string>(type: "varchar(10)", nullable: false),
                    AccountNoOrUpiId = table.Column<string>(type: "varchar(50)", nullable: false),
                    BankName = table.Column<string>(type: "varchar(40)", nullable: true),
                    AccountHolderName = table.Column<string>(type: "varchar(50)", nullable: true),
                    IFSCCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "varchar(30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientBankingDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentReceivedDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessId = table.Column<int>(type: "int", nullable: false),
                    InvestmentId = table.Column<int>(type: "int", nullable: false),
                    InvestmentGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Mode = table.Column<string>(type: "varchar(10)", nullable: false),
                    BankName = table.Column<string>(type: "varchar(50)", nullable: true),
                    AccountNumberOrUpiId = table.Column<string>(type: "varchar(30)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Comments = table.Column<string>(type: "varchar(255)", nullable: true),
                    AddedBy = table.Column<string>(type: "varchar(30)", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "current_timestamp")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentReceivedDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Investments",
                columns: table => new
                {
                    InvestmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10000, 1"),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ClientName = table.Column<string>(type: "varchar(200)", nullable: true),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    PlanName = table.Column<string>(type: "varchar(50)", nullable: true),
                    InvestmentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InvestmentStartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    InvestmentEndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PayoutFrequency = table.Column<int>(type: "int", nullable: false),
                    TotalProfitPercent = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PayoutFrequencyProfitRatePercent = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    InvestmentTenure = table.Column<int>(type: "int", nullable: false),
                    IsPaymentScheduleAvailable = table.Column<bool>(type: "bit", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "current_timestamp"),
                    CreatedBy = table.Column<string>(type: "varchar(30)", nullable: false),
                    IsTdsApplicable = table.Column<bool>(type: "bit", nullable: false),
                    TdsPercent = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    IsReferralBonusApplicable = table.Column<bool>(type: "bit", nullable: false),
                    ReferralFirstBonusPercent = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ReferralLastBonusPercent = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    IsMaturityBonusApplicable = table.Column<bool>(type: "bit", nullable: false),
                    BonusTime = table.Column<string>(type: "varchar(10)", nullable: false, defaultValue: "end"),
                    BonusPercent = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CapitalReturnPeriod = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investments", x => x.InvestmentId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientBankingDetails");

            migrationBuilder.DropTable(
                name: "InvestmentReceivedDetails");

            migrationBuilder.DropTable(
                name: "Investments");
        }
    }
}
