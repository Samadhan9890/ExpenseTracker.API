using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Services.Migrations
{
    /// <inheritdoc />
    public partial class approvenullcolumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SUBSCRIPTION_CLIENT_MASTER_ClientMasterClientId",
                table: "SUBSCRIPTION");

            migrationBuilder.AlterColumn<int>(
                name: "ClientMasterClientId",
                table: "SUBSCRIPTION",
                type: "int",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "APPROVED_BY",
                table: "SUBSCRIPTION",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)");            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SUBSCRIPTION_CLIENT_MASTER_ClientMasterClientId",
                table: "SUBSCRIPTION");           

            migrationBuilder.AlterColumn<int>(
                name: "ClientMasterClientId",
                table: "SUBSCRIPTION",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "APPROVED_BY",
                table: "SUBSCRIPTION",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true);
          
        }
    }
}
