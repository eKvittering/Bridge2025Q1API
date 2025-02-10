using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class custmertypeadded2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_CustomerType_CustomeTypeId",
                table: "Customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerType",
                table: "CustomerType");

            migrationBuilder.RenameTable(
                name: "CustomerType",
                newName: "CustomerTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerTypes",
                table: "CustomerTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_CustomerTypes_CustomeTypeId",
                table: "Customers",
                column: "CustomeTypeId",
                principalTable: "CustomerTypes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_CustomerTypes_CustomeTypeId",
                table: "Customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerTypes",
                table: "CustomerTypes");

            migrationBuilder.RenameTable(
                name: "CustomerTypes",
                newName: "CustomerType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerType",
                table: "CustomerType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_CustomerType_CustomeTypeId",
                table: "Customers",
                column: "CustomeTypeId",
                principalTable: "CustomerType",
                principalColumn: "Id");
        }
    }
}
