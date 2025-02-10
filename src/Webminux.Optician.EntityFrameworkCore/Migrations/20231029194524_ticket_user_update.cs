using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class ticket_user_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketsUsers_AbpUsers_UserId",
                table: "TicketsUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketsUsers_tickets_TicketId",
                table: "TicketsUsers");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "TicketsUsers",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "TicketId",
                table: "TicketsUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketsUsers_AbpUsers_UserId",
                table: "TicketsUsers",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketsUsers_tickets_TicketId",
                table: "TicketsUsers",
                column: "TicketId",
                principalTable: "tickets",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketsUsers_AbpUsers_UserId",
                table: "TicketsUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketsUsers_tickets_TicketId",
                table: "TicketsUsers");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "TicketsUsers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TicketId",
                table: "TicketsUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketsUsers_AbpUsers_UserId",
                table: "TicketsUsers",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketsUsers_tickets_TicketId",
                table: "TicketsUsers",
                column: "TicketId",
                principalTable: "tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
