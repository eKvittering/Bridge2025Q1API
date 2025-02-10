using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class RelationOfActivityRoleChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Activities_RoomId",
                table: "Activities");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_RoomId",
                table: "Activities",
                column: "RoomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Activities_RoomId",
                table: "Activities");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_RoomId",
                table: "Activities",
                column: "RoomId",
                unique: true,
                filter: "[RoomId] IS NOT NULL");
        }
    }
}
