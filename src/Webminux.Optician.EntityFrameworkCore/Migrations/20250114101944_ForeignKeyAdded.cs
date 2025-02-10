using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class ForeignKeyAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EconomicSyncHistoryId",
                table: "SyncHistoryDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SyncHistoryDetails_EconomicSyncHistoryId",
                table: "SyncHistoryDetails",
                column: "EconomicSyncHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_SyncHistoryDetails_EconomicSyncHistories_EconomicSyncHistoryId",
                table: "SyncHistoryDetails",
                column: "EconomicSyncHistoryId",
                principalTable: "EconomicSyncHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SyncHistoryDetails_EconomicSyncHistories_EconomicSyncHistoryId",
                table: "SyncHistoryDetails");

            migrationBuilder.DropIndex(
                name: "IX_SyncHistoryDetails_EconomicSyncHistoryId",
                table: "SyncHistoryDetails");

            migrationBuilder.DropColumn(
                name: "EconomicSyncHistoryId",
                table: "SyncHistoryDetails");
        }
    }
}
