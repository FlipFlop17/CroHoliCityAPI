using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CroHoliCityAPI.Migrations
{
    /// <inheritdoc />
    public partial class updatedatumi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "VrijediDo",
                table: "Lokacije",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "VrijediOd",
                table: "Lokacije",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "VrijediDo",
                table: "Kalendar",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "VrijediOd",
                table: "Kalendar",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VrijediDo",
                table: "Lokacije");

            migrationBuilder.DropColumn(
                name: "VrijediOd",
                table: "Lokacije");

            migrationBuilder.DropColumn(
                name: "VrijediDo",
                table: "Kalendar");

            migrationBuilder.DropColumn(
                name: "VrijediOd",
                table: "Kalendar");
        }
    }
}
