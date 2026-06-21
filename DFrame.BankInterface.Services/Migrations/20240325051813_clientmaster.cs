using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Services.Migrations
{
    /// <inheritdoc />
    public partial class clientmaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CLIENT_MASTER",
                columns: table => new
                {
                    CLIENT_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10000, 1"),
                    NAME = table.Column<string>(type: "varchar(255)", nullable: false),
                    AADHAR_NO = table.Column<string>(type: "varchar(20)", nullable: false),
                    PAN_NO = table.Column<string>(type: "varchar(10)", nullable: false),
                    DOB = table.Column<DateTime>(type: "date", nullable: false),
                    REFERRED_BY = table.Column<string>(type: "varchar(30)", nullable: false),
                    PER_ADDRESS_LINE1 = table.Column<string>(type: "varchar(60)", nullable: false),
                    PER_ADDRESS_LINE2 = table.Column<string>(type: "varchar(60)", nullable: false),
                    PER_ADDRESS_LINE3 = table.Column<string>(type: "varchar(60)", nullable: false),
                    PER_STATE = table.Column<string>(type: "varchar(50)", nullable: false),
                    PER_CITY = table.Column<string>(type: "varchar(50)", nullable: false),
                    PER_PIN_CODE = table.Column<int>(type: "int", nullable: false),
                    MAIL_ADDRESS_LINE1 = table.Column<string>(type: "varchar(60)", nullable: false),
                    MAIL_ADDRESS_LINE2 = table.Column<string>(type: "varchar(60)", nullable: false),
                    MAIL_ADDRESS_LINE3 = table.Column<string>(type: "varchar(60)", nullable: false),
                    MAIL_STATE = table.Column<string>(type: "varchar(50)", nullable: false),
                    MAIL_CITY = table.Column<string>(type: "varchar(50)", nullable: false),
                    MAIL_PIN_CODE = table.Column<int>(type: "int", nullable: false),
                    PHONE = table.Column<string>(type: "varchar(15)", nullable: false),
                    MOBILE = table.Column<string>(type: "varchar(15)", nullable: false),
                    EMAIL = table.Column<string>(type: "varchar(80)", nullable: false),
                    AADHAR_ATTACHMENT_PATH = table.Column<string>(type: "varchar(255)", nullable: false),
                    PAN_ATTACHMENT_PATH = table.Column<string>(type: "varchar(255)", nullable: false),
                    PROFILE_IMAGE_ATTACHMENT_PATH = table.Column<string>(type: "varchar(255)", nullable: false),
                    FAMILY_TAG = table.Column<string>(type: "varchar(100)", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "current_timestamp"),
                    CREATED_BY = table.Column<string>(type: "varchar(50)", nullable: false),
                    STATUS = table.Column<bool>(type: "bit", nullable: false),
                    NOTES = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLIENT_MASTER", x => x.CLIENT_ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CLIENT_MASTER");
        }
    }
}
