using CroHoliCityAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace CroHoliCityAPI.Data
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<Lokacija> Lokacije { get; set; }

        public DbSet<Dan> Kalendar { get; set; }
        // Konstruktor koji prima DbContextOptions objekt i proslijeđuje ga baznom konstruktoru
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }
    }
}
