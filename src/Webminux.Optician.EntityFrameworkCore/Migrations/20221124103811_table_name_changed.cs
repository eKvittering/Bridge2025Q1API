using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class table_name_changed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_bookingActivityTypes_BookingActivityTypeId",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bookingActivityTypes",
                table: "bookingActivityTypes");

            migrationBuilder.RenameTable(
                name: "bookingActivityTypes",
                newName: "BookingActivityTypes");

            migrationBuilder.AlterColumn<int>(
                name: "BookingActivityTypeId",
                table: "Bookings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingActivityTypes",
                table: "BookingActivityTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_BookingActivityTypes_BookingActivityTypeId",
                table: "Bookings",
                column: "BookingActivityTypeId",
                principalTable: "BookingActivityTypes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_BookingActivityTypes_BookingActivityTypeId",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingActivityTypes",
                table: "BookingActivityTypes");

            migrationBuilder.RenameTable(
                name: "BookingActivityTypes",
                newName: "bookingActivityTypes");

            migrationBuilder.AlterColumn<int>(
                name: "BookingActivityTypeId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_bookingActivityTypes",
                table: "bookingActivityTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_bookingActivityTypes_BookingActivityTypeId",
                table: "Bookings",
                column: "BookingActivityTypeId",
                principalTable: "bookingActivityTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
