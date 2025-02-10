using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class noteaddedinactivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notes_ActivityId",
                table: "Notes");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_ActivityId",
                table: "Notes",
                column: "ActivityId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notes_ActivityId",
                table: "Notes");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_ActivityId",
                table: "Notes",
                column: "ActivityId");
        }
    }
}
