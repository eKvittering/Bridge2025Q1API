using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class tenant_media_change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "TenantMedias",
                newName: "HomeVideo");

            migrationBuilder.RenameColumn(
                name: "MediaType",
                table: "TenantMedias",
                newName: "HomeImage6");

            migrationBuilder.AddColumn<string>(
                name: "HomeImage1",
                table: "TenantMedias",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeImage2",
                table: "TenantMedias",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeImage3",
                table: "TenantMedias",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeImage4",
                table: "TenantMedias",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeImage5",
                table: "TenantMedias",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HomeImage1",
                table: "TenantMedias");

            migrationBuilder.DropColumn(
                name: "HomeImage2",
                table: "TenantMedias");

            migrationBuilder.DropColumn(
                name: "HomeImage3",
                table: "TenantMedias");

            migrationBuilder.DropColumn(
                name: "HomeImage4",
                table: "TenantMedias");

            migrationBuilder.DropColumn(
                name: "HomeImage5",
                table: "TenantMedias");

            migrationBuilder.RenameColumn(
                name: "HomeVideo",
                table: "TenantMedias",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "HomeImage6",
                table: "TenantMedias",
                newName: "MediaType");
        }
    }
}
