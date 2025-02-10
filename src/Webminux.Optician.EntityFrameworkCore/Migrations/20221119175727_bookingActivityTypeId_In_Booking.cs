using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class bookingActivityTypeId_In_Booking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookingActivityTypeId",
                table: "Bookings",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_BookingActivityTypeId",
                table: "Bookings",
                column: "BookingActivityTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_bookingActivityTypes_BookingActivityTypeId",
                table: "Bookings",
                column: "BookingActivityTypeId",
                principalTable: "bookingActivityTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_bookingActivityTypes_BookingActivityTypeId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_BookingActivityTypeId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "BookingActivityTypeId",
                table: "Bookings");
        }
    }
}
