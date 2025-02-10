using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class ProductResponsibleGroupdbcontext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductResponsibleGroup_Groups_GroupId",
                table: "ProductResponsibleGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductResponsibleGroup_products_ProductId",
                table: "ProductResponsibleGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductResponsibleGroup",
                table: "ProductResponsibleGroup");

            migrationBuilder.RenameTable(
                name: "ProductResponsibleGroup",
                newName: "ProductResponsibleGroups");

            migrationBuilder.RenameIndex(
                name: "IX_ProductResponsibleGroup_ProductId",
                table: "ProductResponsibleGroups",
                newName: "IX_ProductResponsibleGroups_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductResponsibleGroup_GroupId",
                table: "ProductResponsibleGroups",
                newName: "IX_ProductResponsibleGroups_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductResponsibleGroups",
                table: "ProductResponsibleGroups",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductResponsibleGroups_Groups_GroupId",
                table: "ProductResponsibleGroups",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductResponsibleGroups_products_ProductId",
                table: "ProductResponsibleGroups",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductResponsibleGroups_Groups_GroupId",
                table: "ProductResponsibleGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductResponsibleGroups_products_ProductId",
                table: "ProductResponsibleGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductResponsibleGroups",
                table: "ProductResponsibleGroups");

            migrationBuilder.RenameTable(
                name: "ProductResponsibleGroups",
                newName: "ProductResponsibleGroup");

            migrationBuilder.RenameIndex(
                name: "IX_ProductResponsibleGroups_ProductId",
                table: "ProductResponsibleGroup",
                newName: "IX_ProductResponsibleGroup_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductResponsibleGroups_GroupId",
                table: "ProductResponsibleGroup",
                newName: "IX_ProductResponsibleGroup_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductResponsibleGroup",
                table: "ProductResponsibleGroup",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductResponsibleGroup_Groups_GroupId",
                table: "ProductResponsibleGroup",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductResponsibleGroup_products_ProductId",
                table: "ProductResponsibleGroup",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
