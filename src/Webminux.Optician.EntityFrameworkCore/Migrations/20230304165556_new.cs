using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceLines_ProductSerialNos_ProductSerialNoId",
                table: "InvoiceLines");

            migrationBuilder.DropTable(
                name: "ProductSerialNos");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceLines_ProductSerialNoId",
                table: "InvoiceLines");

            migrationBuilder.DropColumn(
                name: "ProductSerialNoId",
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

            migrationBuilder.CreateTable(
                name: "ProductItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSold = table.Column<bool>(type: "bit", nullable: false),
                    RecieverEmployee = table.Column<int>(type: "int", nullable: false),
                    ReceiverEmployeeDate = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductItems_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductItems_ProductId",
                table: "ProductItems",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductItems");

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

            migrationBuilder.AddColumn<int>(
                name: "ProductSerialNoId",
                table: "InvoiceLines",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductSerialNos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    IsSold = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSerialNos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSerialNos_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLines_ProductSerialNoId",
                table: "InvoiceLines",
                column: "ProductSerialNoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSerialNos_ProductId",
                table: "ProductSerialNos",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceLines_ProductSerialNos_ProductSerialNoId",
                table: "InvoiceLines",
                column: "ProductSerialNoId",
                principalTable: "ProductSerialNos",
                principalColumn: "Id");
        }
    }
}
