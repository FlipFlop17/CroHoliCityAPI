using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CroHoliCityAPI.Migrations
{
    /// <inheritdoc />
    public partial class DTkalendarupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NeradniDan",
                table: "Kalendar",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NeradniDan",
                table: "Kalendar");
        }
    }
}
