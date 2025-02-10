using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class SubCustomerMadeFullAudited : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "SubCustomers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "SubCustomers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "SubCustomers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "SubCustomers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SubCustomers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "SubCustomers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "SubCustomers",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "SubCustomers");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "SubCustomers");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "SubCustomers");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "SubCustomers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SubCustomers");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "SubCustomers");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "SubCustomers");
        }
    }
}
