using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class asdf3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Packages_AbpUsers_SendId",
                table: "Packages");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_PackageType_packageTypeId",
                table: "Packages");

            migrationBuilder.DropForeignKey(
                name: "FK_PackageType_Activities_FollowUpTypeId",
                table: "PackageType");

            migrationBuilder.DropIndex(
                name: "IX_PackageType_FollowUpTypeId",
                table: "PackageType");

            migrationBuilder.DropIndex(
                name: "IX_Packages_packageTypeId",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "PackageId",
                table: "Packages");

            migrationBuilder.RenameColumn(
                name: "packageTypeId",
                table: "Packages",
                newName: "PackageTypeId");

            migrationBuilder.RenameColumn(
                name: "SendId",
                table: "Packages",
                newName: "SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Packages_SendId",
                table: "Packages",
                newName: "IX_Packages_SenderId");

            migrationBuilder.AlterColumn<int>(
                name: "PackageTypeId",
                table: "Packages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_AbpUsers_SenderId",
                table: "Packages",
                column: "SenderId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Packages_AbpUsers_SenderId",
                table: "Packages");

            migrationBuilder.RenameColumn(
                name: "PackageTypeId",
                table: "Packages",
                newName: "packageTypeId");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Packages",
                newName: "SendId");

            migrationBuilder.RenameIndex(
                name: "IX_Packages_SenderId",
                table: "Packages",
                newName: "IX_Packages_SendId");

            migrationBuilder.AlterColumn<int>(
                name: "packageTypeId",
                table: "Packages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PackageId",
                table: "Packages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PackageType_FollowUpTypeId",
                table: "PackageType",
                column: "FollowUpTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_packageTypeId",
                table: "Packages",
                column: "packageTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_AbpUsers_SendId",
                table: "Packages",
                column: "SendId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_PackageType_packageTypeId",
                table: "Packages",
                column: "packageTypeId",
                principalTable: "PackageType",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PackageType_Activities_FollowUpTypeId",
                table: "PackageType",
                column: "FollowUpTypeId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
