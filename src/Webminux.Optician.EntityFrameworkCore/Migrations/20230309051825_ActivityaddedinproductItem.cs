using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class ActivityaddedinproductItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActivityId",
                table: "ProductItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductItems_ActivityId",
                table: "ProductItems",
                column: "ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductItems_Activities_ActivityId",
                table: "ProductItems",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductItems_Activities_ActivityId",
                table: "ProductItems");

            migrationBuilder.DropIndex(
                name: "IX_ProductItems_ActivityId",
                table: "ProductItems");

            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "ProductItems");
        }
    }
}
