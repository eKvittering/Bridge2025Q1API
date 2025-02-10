using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class SubCustomerAddedInCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubCustomerId",
                table: "Invoices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_SubCustomerId",
                table: "Invoices",
                column: "SubCustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_SubCustomers_SubCustomerId",
                table: "Invoices",
                column: "SubCustomerId",
                principalTable: "SubCustomers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_SubCustomers_SubCustomerId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_SubCustomerId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "SubCustomerId",
                table: "Invoices");
        }
    }
}
