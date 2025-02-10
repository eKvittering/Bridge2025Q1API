using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class activityEmployeeNull2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_AbpUsers_EmployeeId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "test",
                table: "Activities");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Activities",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_EmployeeId",
                table: "Activities",
                newName: "IX_Activities_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_AbpUsers_UserId",
                table: "Activities",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_AbpUsers_UserId",
                table: "Activities");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Activities",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_UserId",
                table: "Activities",
                newName: "IX_Activities_EmployeeId");

            migrationBuilder.AddColumn<string>(
                name: "test",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_AbpUsers_EmployeeId",
                table: "Activities",
                column: "EmployeeId",
                principalTable: "AbpUsers",
                principalColumn: "Id");
        }
    }
}
