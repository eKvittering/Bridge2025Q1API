using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class packageTYpe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PackageType_ActivityTypes_ActivityId",
                table: "PackageType");

            migrationBuilder.DropForeignKey(
                name: "FK_PackageType_Faults_FaultId",
                table: "PackageType");

            migrationBuilder.DropIndex(
                name: "IX_PackageType_ActivityId",
                table: "PackageType");

            migrationBuilder.DropIndex(
                name: "IX_PackageType_FaultId",
                table: "PackageType");

            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "PackageType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateIndex(
                name: "IX_PackageType_FaultId",
                table: "PackageType",
                column: "FaultId");

            migrationBuilder.AddForeignKey(
                name: "FK_PackageType_ActivityTypes_ActivityId",
                table: "PackageType",
                column: "ActivityId",
                principalTable: "ActivityTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PackageType_Faults_FaultId",
                table: "PackageType",
                column: "FaultId",
                principalTable: "Faults",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
