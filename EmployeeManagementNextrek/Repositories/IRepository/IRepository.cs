using EmployeeManagementNextrek.Models;
using System.Linq.Expressions;

namespace EmployeeManagementNextrek.Repositories.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task Create(T entity);
        Task<List<T>> GetAll(Expression<Func<T, bool>>? filtro = null);

        Task<T> GetById(Expression<Func<T, bool>> filtro = null, bool Tracked = true);

        Task Delete(T entidad);
        Task Save();
    }
}
