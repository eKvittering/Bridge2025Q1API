using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class activity_responsibles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Faults",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ActivityResponsibles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityId = table.Column<int>(type: "int", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    GroupId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityResponsibles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityResponsibles_AbpUsers_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActivityResponsibles_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActivityResponsibles_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityResponsibles_ActivityId",
                table: "ActivityResponsibles",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityResponsibles_EmployeeId",
                table: "ActivityResponsibles",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityResponsibles_GroupId",
                table: "ActivityResponsibles",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityResponsibles");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Faults");
        }
    }
}
