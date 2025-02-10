using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class asdf3aa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActivityId",
                table: "PackageType",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PackageType_ActivityId",
                table: "PackageType",
                column: "ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_PackageType_ActivityTypes_ActivityId",
                table: "PackageType",
                column: "ActivityId",
                principalTable: "ActivityTypes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PackageType_ActivityTypes_ActivityId",
                table: "PackageType");

            migrationBuilder.DropIndex(
                name: "IX_PackageType_ActivityId",
                table: "PackageType");

            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "PackageType");
        }
    }
}
