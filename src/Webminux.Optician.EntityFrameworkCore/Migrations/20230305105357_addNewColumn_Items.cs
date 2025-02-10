using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class addNewColumn_Items : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "SerialNumber",
                table: "UserTypes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "SerialNumber",
                table: "products",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "SerialNumber",
                table: "productGroups",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ProductSerialNumber",
                table: "InvoiceLines",
                newName: "ProductSerialNoId");

            migrationBuilder.RenameColumn(
                name: "SerialNumber",
                table: "BookingActivityTypes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "SerialNumber",
                table: "ActivityTypes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "SerialNumber",
                table: "ActivityArts",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "SerialNumber",
                table: "Activities",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ProductItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "InvoiceLines",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLines_ProductSerialNoId",
                table: "InvoiceLines",
                column: "ProductSerialNoId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceLines_ProductItems_ProductSerialNoId",
                table: "InvoiceLines",
                column: "ProductSerialNoId",
                principalTable: "ProductItems",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceLines_ProductItems_ProductSerialNoId",
                table: "InvoiceLines");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceLines_ProductSerialNoId",
                table: "InvoiceLines");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ProductItems");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "InvoiceLines");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "UserTypes",
                newName: "SerialNumber");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "products",
                newName: "SerialNumber");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "productGroups",
                newName: "SerialNumber");

            migrationBuilder.RenameColumn(
                name: "ProductSerialNoId",
                table: "InvoiceLines",
                newName: "ProductSerialNumber");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "BookingActivityTypes",
                newName: "SerialNumber");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ActivityTypes",
                newName: "SerialNumber");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ActivityArts",
                newName: "SerialNumber");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Activities",
                newName: "SerialNumber");

            migrationBuilder.AddColumn<int>(
                name: "ProductItemId",
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
    }
}
