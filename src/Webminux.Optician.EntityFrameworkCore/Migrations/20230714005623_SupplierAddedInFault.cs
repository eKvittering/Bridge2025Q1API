using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class SupplierAddedInFault : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "Faults",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Faults_SupplierId",
                table: "Faults",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Faults_Suppliers_SupplierId",
                table: "Faults",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faults_Suppliers_SupplierId",
                table: "Faults");

            migrationBuilder.DropIndex(
                name: "IX_Faults_SupplierId",
                table: "Faults");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "Faults");
        }
    }
}
