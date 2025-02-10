using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class invoiceandinvoicelineadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InvoiceId",
                table: "ProductItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InvoiceLineId",
                table: "ProductItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductItems_InvoiceId",
                table: "ProductItems",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductItems_InvoiceLineId",
                table: "ProductItems",
                column: "InvoiceLineId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductItems_InvoiceLines_InvoiceLineId",
                table: "ProductItems",
                column: "InvoiceLineId",
                principalTable: "InvoiceLines",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductItems_Invoices_InvoiceId",
                table: "ProductItems",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductItems_InvoiceLines_InvoiceLineId",
                table: "ProductItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductItems_Invoices_InvoiceId",
                table: "ProductItems");

            migrationBuilder.DropIndex(
                name: "IX_ProductItems_InvoiceId",
                table: "ProductItems");

            migrationBuilder.DropIndex(
                name: "IX_ProductItems_InvoiceLineId",
                table: "ProductItems");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "ProductItems");

            migrationBuilder.DropColumn(
                name: "InvoiceLineId",
                table: "ProductItems");
        }
    }
}
