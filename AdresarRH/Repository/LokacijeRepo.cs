using CroHoliCityAPI.Data;
using CroHoliCityAPI.Model;
using CroHoliCityAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace CroHoliCityAPI.Repository
{
    public class LokacijeRepo : RepositoryBase<Lokacija>, IlokacijeRepo
    {
        private readonly ApplicationDbContext _db;

        public LokacijeRepo(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
