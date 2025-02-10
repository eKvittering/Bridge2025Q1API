using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webminux.Optician.Migrations
{
    public partial class updateIndexing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "Invoices",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductNumber",
                table: "InvoiceLines",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "CustomerTypes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_AdminId",
                table: "Invoices",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_InvoiceNo",
                table: "Invoices",
                column: "InvoiceNo");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_IsDraft",
                table: "Invoices",
                column: "IsDraft");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_PaidAmount",
                table: "Invoices",
                column: "PaidAmount");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_SerialNumber",
                table: "Invoices",
                column: "SerialNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_TenantId",
                table: "Invoices",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLines_ProductNumber",
                table: "InvoiceLines",
                column: "ProductNumber");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLines_TenantId",
                table: "InvoiceLines",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTypes_TenantId",
                table: "CustomerTypes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTypes_Type",
                table: "CustomerTypes",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Address",
                table: "Customers",
                column: "Address");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CustomerNo",
                table: "Customers",
                column: "CustomerNo");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_IsSubCustomer",
                table: "Customers",
                column: "IsSubCustomer");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Postcode",
                table: "Customers",
                column: "Postcode");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_TelephoneFax",
                table: "Customers",
                column: "TelephoneFax");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_TenantId",
                table: "Customers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_EmailAddress",
                table: "AbpUsers",
                column: "EmailAddress");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_Name",
                table: "AbpUsers",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_Surname",
                table: "AbpUsers",
                column: "Surname");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_TenantId",
                table: "AbpUsers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_UserName",
                table: "AbpUsers",
                column: "UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Invoices_AdminId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_InvoiceNo",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_IsDraft",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_PaidAmount",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_SerialNumber",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_TenantId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceLines_ProductNumber",
                table: "InvoiceLines");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceLines_TenantId",
                table: "InvoiceLines");

            migrationBuilder.DropIndex(
                name: "IX_CustomerTypes_TenantId",
                table: "CustomerTypes");

            migrationBuilder.DropIndex(
                name: "IX_CustomerTypes_Type",
                table: "CustomerTypes");

            migrationBuilder.DropIndex(
                name: "IX_Customers_Address",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CustomerNo",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_IsSubCustomer",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_Postcode",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_TelephoneFax",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_TenantId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_AbpUsers_EmailAddress",
                table: "AbpUsers");

            migrationBuilder.DropIndex(
                name: "IX_AbpUsers_Name",
                table: "AbpUsers");

            migrationBuilder.DropIndex(
                name: "IX_AbpUsers_Surname",
                table: "AbpUsers");

            migrationBuilder.DropIndex(
                name: "IX_AbpUsers_TenantId",
                table: "AbpUsers");

            migrationBuilder.DropIndex(
                name: "IX_AbpUsers_UserName",
                table: "AbpUsers");

            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductNumber",
                table: "InvoiceLines",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "CustomerTypes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
