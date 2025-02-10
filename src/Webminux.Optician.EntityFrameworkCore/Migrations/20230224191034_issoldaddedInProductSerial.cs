using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class issoldaddedInProductSerial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSold",
                table: "ProductSerialNos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ProductSerialNoId",
                table: "InvoiceLines",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLines_ProductSerialNoId",
                table: "InvoiceLines",
                column: "ProductSerialNoId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceLines_ProductSerialNos_ProductSerialNoId",
                table: "InvoiceLines",
                column: "ProductSerialNoId",
                principalTable: "ProductSerialNos",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceLines_ProductSerialNos_ProductSerialNoId",
                table: "InvoiceLines");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceLines_ProductSerialNoId",
                table: "InvoiceLines");

            migrationBuilder.DropColumn(
                name: "IsSold",
                table: "ProductSerialNos");

            migrationBuilder.DropColumn(
                name: "ProductSerialNoId",
                table: "InvoiceLines");
        }
    }
}
