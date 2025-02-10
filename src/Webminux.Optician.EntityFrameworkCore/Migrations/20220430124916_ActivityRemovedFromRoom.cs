using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class ActivityRemovedFromRoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Activities_ActivityId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_ActivityId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Activities_RoomId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "Rooms");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_RoomId",
                table: "Activities",
                column: "RoomId",
                unique: true,
                filter: "[RoomId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Activities_RoomId",
                table: "Activities");

            migrationBuilder.AddColumn<int>(
                name: "ActivityId",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_ActivityId",
                table: "Rooms",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_RoomId",
                table: "Activities",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Activities_ActivityId",
                table: "Rooms",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id");
        }
    }
}
