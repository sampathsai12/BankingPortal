using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingPortal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCustomerKycTimestamps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "CustomerKycs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "CustomerKycs",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "CustomerKycs");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "CustomerKycs");
        }
    }
}
