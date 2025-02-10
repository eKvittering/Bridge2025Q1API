using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class dda : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Groups_GroupId",
                table: "Packages");

            migrationBuilder.DropIndex(
                name: "IX_Packages_GroupId",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Packages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Packages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Packages_GroupId",
                table: "Packages",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Groups_GroupId",
                table: "Packages",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }
    }
}
