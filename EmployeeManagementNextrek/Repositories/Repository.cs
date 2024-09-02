using EmployeeManagementNextrek.Data;
using EmployeeManagementNextrek.Models;
using EmployeeManagementNextrek.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;

namespace EmployeeManagementNextrek.Repositories
{
    public class Repository<T>: IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public async Task Create(T entity)
        {
            await dbSet.AddAsync(entity);
            await Save();
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<T> GetById(Expression<Func<T, bool>> filtro = null, bool Tracked = true)
        {
            IQueryable<T> query = dbSet;
            if (!Tracked)
            {
                query = query.AsNoTracking();
            }
            if (filtro != null)
            {
                query = query.Where(filtro);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>>? filtro = null)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro);
            }
            return await query.ToListAsync();
        }

        public async Task Delete(T entidad)
        {
            dbSet.Remove(entidad);
            await Save();
        }
    }

    
}
