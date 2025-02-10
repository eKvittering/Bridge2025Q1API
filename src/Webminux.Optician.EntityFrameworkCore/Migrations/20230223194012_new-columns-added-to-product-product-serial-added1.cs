using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class newcolumnsaddedtoproductproductserialadded1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSerialNo_products_ProductId",
                table: "ProductSerialNo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSerialNo",
                table: "ProductSerialNo");

            migrationBuilder.RenameTable(
                name: "ProductSerialNo",
                newName: "ProductSerialNos");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSerialNo_ProductId",
                table: "ProductSerialNos",
                newName: "IX_ProductSerialNos_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSerialNos",
                table: "ProductSerialNos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSerialNos_products_ProductId",
                table: "ProductSerialNos",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSerialNos_products_ProductId",
                table: "ProductSerialNos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSerialNos",
                table: "ProductSerialNos");

            migrationBuilder.RenameTable(
                name: "ProductSerialNos",
                newName: "ProductSerialNo");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSerialNos_ProductId",
                table: "ProductSerialNo",
                newName: "IX_ProductSerialNo_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSerialNo",
                table: "ProductSerialNo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSerialNo_products_ProductId",
                table: "ProductSerialNo",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
