using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class ticketCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tickets_AbpUsers_CustomerId",
                table: "tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_tickets_AbpUsers_EmployeeId",
                table: "tickets");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "tickets",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_tickets_EmployeeId",
                table: "tickets",
                newName: "IX_tickets_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "tickets",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tickets_AbpUsers_UserId",
                table: "tickets",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tickets_Customers_CustomerId",
                table: "tickets",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tickets_AbpUsers_UserId",
                table: "tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_tickets_Customers_CustomerId",
                table: "tickets");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "tickets",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_tickets_UserId",
                table: "tickets",
                newName: "IX_tickets_EmployeeId");

            migrationBuilder.AlterColumn<long>(
                name: "CustomerId",
                table: "tickets",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tickets_AbpUsers_CustomerId",
                table: "tickets",
                column: "CustomerId",
                principalTable: "AbpUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tickets_AbpUsers_EmployeeId",
                table: "tickets",
                column: "EmployeeId",
                principalTable: "AbpUsers",
                principalColumn: "Id");
        }
    }
}
