using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ADRESSE1",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ADRESSE2",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AENDRET",
                table: "Customers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "ALTERNATIV_ADRESSE",
                table: "Customers",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ARBEJDSTELEFON",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "DB_FAMILIEN_FAAR_BLADET",
                table: "Customers",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "DB_OENSKERIKKE",
                table: "Customers",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "DB_SENDALTID",
                table: "Customers",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "DB_UGYLDIG_ADRESSE",
                table: "Customers",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "DOED",
                table: "Customers",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EFTERNAVN",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EMAIL",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FKLANDEKODE",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FKPOSTNR",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FOEDSELSDAG",
                table: "Customers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FORNAVN",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "K1AAR",
                table: "Customers",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MELLEMNAVN",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MOBILTELEFON",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MPTITEL",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MPTITEL_AENDRET",
                table: "Customers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "NAAL_OENSKES",
                table: "Customers",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "NOTER",
                table: "Customers",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NUMMER",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "NUVHAC",
                table: "Customers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "OPDATERWINFINANS",
                table: "Customers",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OPRETTET",
                table: "Customers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PRIVATTELEFON",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TRIAL675",
                table: "Customers",
                type: "nvarchar(1)",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "VEDLIGEHOLDER_EGNE_STAMDATA",
                table: "Customers",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WEB_ADGANGSKODE",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ADRESSE1",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ADRESSE2",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "AENDRET",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ALTERNATIV_ADRESSE",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ARBEJDSTELEFON",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DB_FAMILIEN_FAAR_BLADET",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DB_OENSKERIKKE",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DB_SENDALTID",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DB_UGYLDIG_ADRESSE",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DOED",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "EFTERNAVN",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "EMAIL",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "FKLANDEKODE",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "FKPOSTNR",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "FOEDSELSDAG",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "FORNAVN",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "K1AAR",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "MELLEMNAVN",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "MOBILTELEFON",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "MPTITEL",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "MPTITEL_AENDRET",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "NAAL_OENSKES",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "NOTER",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "NUMMER",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "NUVHAC",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "OPDATERWINFINANS",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "OPRETTET",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "PRIVATTELEFON",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "TRIAL675",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "VEDLIGEHOLDER_EGNE_STAMDATA",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "WEB_ADGANGSKODE",
                table: "Customers");
        }
    }
}
