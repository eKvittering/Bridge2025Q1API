using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class invoiceChnageRelationSerialNo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "InvoiceLines");

            migrationBuilder.AddColumn<int>(
                name: "ProductItemId",
                table: "InvoiceLines",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductSerialNumber",
                table: "InvoiceLines",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLines_ProductItemId",
                table: "InvoiceLines",
                column: "ProductItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceLines_ProductItems_ProductItemId",
                table: "InvoiceLines",
                column: "ProductItemId",
                principalTable: "ProductItems",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceLines_ProductItems_ProductItemId",
                table: "InvoiceLines");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceLines_ProductItemId",
                table: "InvoiceLines");

            migrationBuilder.DropColumn(
                name: "ProductItemId",
                table: "InvoiceLines");

            migrationBuilder.DropColumn(
                name: "ProductSerialNumber",
                table: "InvoiceLines");

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "InvoiceLines",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
