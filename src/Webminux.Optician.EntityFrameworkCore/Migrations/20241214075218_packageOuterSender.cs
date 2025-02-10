using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class packageOuterSender : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SenderPhoneNumber",
                table: "Packages",
                newName: "OuterSenderPhoneNumber");

            migrationBuilder.RenameColumn(
                name: "SenderLastName",
                table: "Packages",
                newName: "OuterSenderLastName");

            migrationBuilder.RenameColumn(
                name: "SenderFirstName",
                table: "Packages",
                newName: "OuterSenderFirstName");

            migrationBuilder.RenameColumn(
                name: "SenderEmail",
                table: "Packages",
                newName: "OuterSenderEmail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OuterSenderPhoneNumber",
                table: "Packages",
                newName: "SenderPhoneNumber");

            migrationBuilder.RenameColumn(
                name: "OuterSenderLastName",
                table: "Packages",
                newName: "SenderLastName");

            migrationBuilder.RenameColumn(
                name: "OuterSenderFirstName",
                table: "Packages",
                newName: "SenderFirstName");

            migrationBuilder.RenameColumn(
                name: "OuterSenderEmail",
                table: "Packages",
                newName: "SenderEmail");
        }
    }
}
