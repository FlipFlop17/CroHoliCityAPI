using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CroHoliCityAPI.Migrations
{
    /// <inheritdoc />
    public partial class updatekalendar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Kalendar");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
