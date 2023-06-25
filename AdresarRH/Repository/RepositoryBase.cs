using CroHoliCityAPI.Data;
using CroHoliCityAPI.Model;
using CroHoliCityAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CroHoliCityAPI.Repository
{
    //djeluje kao wraper za sve repozitorije i omogucuje da se koriste u kontrolerima
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        private DbSet<T> _dbSet;

        public RepositoryBase(ApplicationDbContext db)
        {
            _db = db;
            this._dbSet= _db.Set<T>();  
        }
        /// <summary>
        /// Dohvaća sve zapise iz baze o lokacijama
        /// </summary>
        /// <returns></returns>
        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter=null,bool asNoTracking=false)
        {
            //IQueryable<T> query = _dbSet;

            if(filter!=null) {
                if(asNoTracking) {
                    return await _dbSet.Where(filter).AsNoTracking().ToListAsync();
                }
                return await _dbSet.Where(filter).ToListAsync();
            }  
            if(asNoTracking) {
                return await _dbSet.AsNoTracking().ToListAsync();
            }
            return await _dbSet.ToListAsync();

        }
        /// <summary>
        /// Dohvaća zapis iz baze prema idu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetAsync(Expression<Func<T,bool>> filter, bool asNoTracking=false)
        {
            if(asNoTracking) {
                return await _dbSet.Where(filter).AsNoTracking().FirstOrDefaultAsync();
            }
            return await _dbSet.Where(filter).FirstOrDefaultAsync();
        }
    }
}
