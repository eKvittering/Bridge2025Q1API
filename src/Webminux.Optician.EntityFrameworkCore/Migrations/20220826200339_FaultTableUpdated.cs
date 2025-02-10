using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class FaultTableUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Faults",
                newName: "Comment");

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "InvoiceLines",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActivityId",
                table: "Faults",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Faults",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Faults",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImagePublicKey",
                table: "Faults",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Faults",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ResponsibleEmployeeId",
                table: "Faults",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Faults_ActivityId",
                table: "Faults",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Faults_ResponsibleEmployeeId",
                table: "Faults",
                column: "ResponsibleEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Faults_AbpUsers_ResponsibleEmployeeId",
                table: "Faults",
                column: "ResponsibleEmployeeId",
                principalTable: "AbpUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Faults_Activities_ActivityId",
                table: "Faults",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faults_AbpUsers_ResponsibleEmployeeId",
                table: "Faults");

            migrationBuilder.DropForeignKey(
                name: "FK_Faults_Activities_ActivityId",
                table: "Faults");

            migrationBuilder.DropIndex(
                name: "IX_Faults_ActivityId",
                table: "Faults");

            migrationBuilder.DropIndex(
                name: "IX_Faults_ResponsibleEmployeeId",
                table: "Faults");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "InvoiceLines");

            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "Faults");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Faults");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Faults");

            migrationBuilder.DropColumn(
                name: "ImagePublicKey",
                table: "Faults");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Faults");

            migrationBuilder.DropColumn(
                name: "ResponsibleEmployeeId",
                table: "Faults");

            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "Faults",
                newName: "Description");
        }
    }
}
