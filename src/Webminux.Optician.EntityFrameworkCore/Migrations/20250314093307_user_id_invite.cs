using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class user_id_invite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invites_Customers_CustomerId",
                table: "Invites");

            migrationBuilder.DropIndex(
                name: "IX_Invites_CustomerId",
                table: "Invites");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Invites");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Invites",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invites_UserId",
                table: "Invites",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invites_AbpUsers_UserId",
                table: "Invites",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invites_AbpUsers_UserId",
                table: "Invites");

            migrationBuilder.DropIndex(
                name: "IX_Invites_UserId",
                table: "Invites");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Invites");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Invites",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invites_CustomerId",
                table: "Invites",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invites_Customers_CustomerId",
                table: "Invites",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }
    }
}
