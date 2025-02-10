using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class newcolumnsaddedtoproductproductserialadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ProductGroupNumber",
                table: "products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<long>(
                name: "EmployeeId",
                table: "products",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductSerialNo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSerialNo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSerialNo_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_products_EmployeeId",
                table: "products",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_products_SupplierId",
                table: "products",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSerialNo_ProductId",
                table: "ProductSerialNo",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_products_AbpUsers_EmployeeId",
                table: "products",
                column: "EmployeeId",
                principalTable: "AbpUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_products_Suppliers_SupplierId",
                table: "products",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_AbpUsers_EmployeeId",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "FK_products_Suppliers_SupplierId",
                table: "products");

            migrationBuilder.DropTable(
                name: "ProductSerialNo");

            migrationBuilder.DropIndex(
                name: "IX_products_EmployeeId",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_products_SupplierId",
                table: "products");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "products");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "products");

            migrationBuilder.AlterColumn<int>(
                name: "ProductGroupNumber",
                table: "products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
