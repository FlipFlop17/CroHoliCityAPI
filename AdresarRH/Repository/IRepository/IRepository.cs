using CroHoliCityAPI.Model;
using System.Linq.Expressions;

namespace CroHoliCityAPI.Repository.IRepository
{
    public interface IRepository<T>  where T : class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null,bool asNoTracking = false);
        Task<T> GetAsync(Expression<Func<T, bool>> filter,bool asNoTracking =false );
    }
}
