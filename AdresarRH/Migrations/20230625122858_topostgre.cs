using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CroHoliCityAPI.Migrations
{
    /// <inheritdoc />
    public partial class topostgre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kalendar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Datum = table.Column<string>(type: "text", nullable: false),
                    NeradniDan = table.Column<bool>(type: "boolean", nullable: false),
                    NazivDan = table.Column<string>(type: "text", nullable: false),
                    Opis = table.Column<string>(type: "text", nullable: false),
                    VrijediOd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VrijediDo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kalendar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lokacije",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PostanskiBroj = table.Column<int>(type: "integer", nullable: false),
                    Naziv = table.Column<string>(type: "text", nullable: false),
                    Naselje = table.Column<string>(type: "text", nullable: true),
                    Zupanija = table.Column<string>(type: "text", nullable: false),
                    VrijediOd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VrijediDo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lokacije", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kalendar");

            migrationBuilder.DropTable(
                name: "Lokacije");
        }
    }
}
