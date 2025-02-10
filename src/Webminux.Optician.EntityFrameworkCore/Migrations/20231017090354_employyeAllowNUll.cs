using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class employyeAllowNUll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_AbpUsers_EmployeeId",
                table: "Activities");

            migrationBuilder.AlterColumn<long>(
                name: "EmployeeId",
                table: "Activities",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_AbpUsers_EmployeeId",
                table: "Activities",
                column: "EmployeeId",
                principalTable: "AbpUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_AbpUsers_EmployeeId",
                table: "Activities");

            migrationBuilder.AlterColumn<long>(
                name: "EmployeeId",
                table: "Activities",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_AbpUsers_EmployeeId",
                table: "Activities",
                column: "EmployeeId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
