using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class productitemaddedinfaultinvoicelineoptionalinfault2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faults_InvoiceLines_InvoiceLineId",
                table: "Faults");

            migrationBuilder.DropIndex(
                name: "IX_ProductItems_ActivityId",
                table: "ProductItems");

            migrationBuilder.AlterColumn<int>(
                name: "InvoiceLineId",
                table: "Faults",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ProductItemId",
                table: "Faults",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductItems_ActivityId",
                table: "ProductItems",
                column: "ActivityId",
                unique: true,
                filter: "[ActivityId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Faults_ProductItemId",
                table: "Faults",
                column: "ProductItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Faults_InvoiceLines_InvoiceLineId",
                table: "Faults",
                column: "InvoiceLineId",
                principalTable: "InvoiceLines",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Faults_ProductItems_ProductItemId",
                table: "Faults",
                column: "ProductItemId",
                principalTable: "ProductItems",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faults_InvoiceLines_InvoiceLineId",
                table: "Faults");

            migrationBuilder.DropForeignKey(
                name: "FK_Faults_ProductItems_ProductItemId",
                table: "Faults");

            migrationBuilder.DropIndex(
                name: "IX_ProductItems_ActivityId",
                table: "ProductItems");

            migrationBuilder.DropIndex(
                name: "IX_Faults_ProductItemId",
                table: "Faults");

            migrationBuilder.DropColumn(
                name: "ProductItemId",
                table: "Faults");

            migrationBuilder.AlterColumn<int>(
                name: "InvoiceLineId",
                table: "Faults",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductItems_ActivityId",
                table: "ProductItems",
                column: "ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Faults_InvoiceLines_InvoiceLineId",
                table: "Faults",
                column: "InvoiceLineId",
                principalTable: "InvoiceLines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
