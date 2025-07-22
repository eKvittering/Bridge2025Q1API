using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class tenants_updates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Adresse1",
                table: "AbpTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Adresse2",
                table: "AbpTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bankkonto",
                table: "AbpTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "BrugerBC",
                table: "AbpTenants",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Dansk_bridge_antal",
                table: "AbpTenants",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Dansk_bridge_procent",
                table: "AbpTenants",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Eksportbruger",
                table: "AbpTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Eksportkode",
                table: "AbpTenants",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "AbpTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FKDistriktsId",
                table: "AbpTenants",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FKLandekode",
                table: "AbpTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FKPostnr",
                table: "AbpTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FTPKode",
                table: "AbpTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Foerste_medlemsaAr",
                table: "AbpTenants",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "FravaelgHAC",
                table: "AbpTenants",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Halvt_KM_Kontingent",
                table: "AbpTenants",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Ikkeryger",
                table: "AbpTenants",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Installationskode",
                table: "AbpTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Instlokalt",
                table: "AbpTenants",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Klubnummer",
                table: "AbpTenants",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Klubtype",
                table: "AbpTenants",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Modtager_DB",
                table: "AbpTenants",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Mpordning",
                table: "AbpTenants",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Noter",
                table: "AbpTenants",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "OpdaterWinFinans",
                table: "AbpTenants",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Overfoeres",
                table: "AbpTenants",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Saesonstart",
                table: "AbpTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sikkerhedskode",
                table: "AbpTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Spillested",
                table: "AbpTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Startdato",
                table: "AbpTenants",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefax",
                table: "AbpTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefon",
                table: "AbpTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Tilbyder_undervisning",
                table: "AbpTenants",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Udskriv_klubLabels",
                table: "AbpTenants",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Www",
                table: "AbpTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserTenants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTenants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTenants_AbpTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "AbpTenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTenants_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTenants_TenantId",
                table: "UserTenants",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTenants_UserId",
                table: "UserTenants",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTenants");

            migrationBuilder.DropColumn(
                name: "Adresse1",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Adresse2",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Bankkonto",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "BrugerBC",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Dansk_bridge_antal",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Dansk_bridge_procent",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Eksportbruger",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Eksportkode",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "FKDistriktsId",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "FKLandekode",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "FKPostnr",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "FTPKode",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Foerste_medlemsaAr",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "FravaelgHAC",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Halvt_KM_Kontingent",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Ikkeryger",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Installationskode",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Instlokalt",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Klubnummer",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Klubtype",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Modtager_DB",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Mpordning",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Noter",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "OpdaterWinFinans",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Overfoeres",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Saesonstart",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Sikkerhedskode",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Spillested",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Startdato",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Telefax",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Telefon",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Tilbyder_undervisning",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Udskriv_klubLabels",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "Www",
                table: "AbpTenants");
        }
    }
}
