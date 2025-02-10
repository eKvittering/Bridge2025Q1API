using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class changeincustomeranduser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ResponsibleEmployeeId",
                table: "Customers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CanAnswerPhoneCalls",
                table: "AbpUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanOpenCloseShop",
                table: "AbpUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReceptionAllowed",
                table: "AbpUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsResponsibleForStocks",
                table: "AbpUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ResponsibleEmployeeId",
                table: "Customers",
                column: "ResponsibleEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_AbpUsers_ResponsibleEmployeeId",
                table: "Customers",
                column: "ResponsibleEmployeeId",
                principalTable: "AbpUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_AbpUsers_ResponsibleEmployeeId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_ResponsibleEmployeeId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ResponsibleEmployeeId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CanAnswerPhoneCalls",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "CanOpenCloseShop",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "IsReceptionAllowed",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "IsResponsibleForStocks",
                table: "AbpUsers");
        }
    }
}
