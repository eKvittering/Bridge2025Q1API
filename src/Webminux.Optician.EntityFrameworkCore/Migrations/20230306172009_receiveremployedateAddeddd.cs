using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class receiveremployedateAddeddd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecieverEmployee",
                table: "ProductItems");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "ProductItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "ProductItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "ProductItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "ProductItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProductItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "ProductItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "ProductItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RecieverEmployeeId",
                table: "ProductItems",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_ProductItems_RecieverEmployeeId",
                table: "ProductItems",
                column: "RecieverEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductItems_AbpUsers_RecieverEmployeeId",
                table: "ProductItems",
                column: "RecieverEmployeeId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductItems_AbpUsers_RecieverEmployeeId",
                table: "ProductItems");

            migrationBuilder.DropIndex(
                name: "IX_ProductItems_RecieverEmployeeId",
                table: "ProductItems");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "ProductItems");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "ProductItems");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "ProductItems");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "ProductItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProductItems");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "ProductItems");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "ProductItems");

            migrationBuilder.DropColumn(
                name: "RecieverEmployeeId",
                table: "ProductItems");

            migrationBuilder.AddColumn<int>(
                name: "RecieverEmployee",
                table: "ProductItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
