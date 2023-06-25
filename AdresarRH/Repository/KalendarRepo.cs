using CroHoliCityAPI.Data;
using CroHoliCityAPI.Model;
using CroHoliCityAPI.Repository.IRepository;

namespace CroHoliCityAPI.Repository
{
    public class KalendarRepo:RepositoryBase<Dan>,IKalendarRepo
    {
        private readonly ApplicationDbContext _db;

        public KalendarRepo(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

    }
}
