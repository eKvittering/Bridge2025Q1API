using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class ppp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubPackages_Activities_ActivityId",
                table: "SubPackages");

            migrationBuilder.DropForeignKey(
                name: "FK_SubPackages_Groups_GroupId",
                table: "SubPackages");

            migrationBuilder.DropIndex(
                name: "IX_SubPackages_ActivityId",
                table: "SubPackages");

            migrationBuilder.DropIndex(
                name: "IX_SubPackages_GroupId",
                table: "SubPackages");

            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "SubPackages");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "SubPackages");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "SubPackages");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "SubPackages");

            migrationBuilder.AddColumn<int>(
                name: "ActivityId",
                table: "Packages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Packages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Packages_ActivityId",
                table: "Packages",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_GroupId",
                table: "Packages",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Activities_ActivityId",
                table: "Packages",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Groups_GroupId",
                table: "Packages",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Activities_ActivityId",
                table: "Packages");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Groups_GroupId",
                table: "Packages");

            migrationBuilder.DropIndex(
                name: "IX_Packages_ActivityId",
                table: "Packages");

            migrationBuilder.DropIndex(
                name: "IX_Packages_GroupId",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Packages");

            migrationBuilder.AddColumn<int>(
                name: "ActivityId",
                table: "SubPackages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EmployeeId",
                table: "SubPackages",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "SubPackages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "SubPackages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SubPackages_ActivityId",
                table: "SubPackages",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_SubPackages_GroupId",
                table: "SubPackages",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubPackages_Activities_ActivityId",
                table: "SubPackages",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubPackages_Groups_GroupId",
                table: "SubPackages",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }
    }
}
