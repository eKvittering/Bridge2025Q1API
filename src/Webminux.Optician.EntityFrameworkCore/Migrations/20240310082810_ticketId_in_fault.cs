using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class ticketId_in_fault : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TicketId",
                table: "Faults",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Faults_TicketId",
                table: "Faults",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Faults_tickets_TicketId",
                table: "Faults",
                column: "TicketId",
                principalTable: "tickets",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faults_tickets_TicketId",
                table: "Faults");

            migrationBuilder.DropIndex(
                name: "IX_Faults_TicketId",
                table: "Faults");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "Faults");
        }
    }
}
