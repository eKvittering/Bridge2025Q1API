using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class tenantmediaupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "TenantMedias",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "TenantMedias",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "TenantMedias",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "TenantMedias",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TenantMedias",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "TenantMedias",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "TenantMedias",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "TenantMedias");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "TenantMedias");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "TenantMedias");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "TenantMedias");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TenantMedias");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "TenantMedias");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "TenantMedias");
        }
    }
}
