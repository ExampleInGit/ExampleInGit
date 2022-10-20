using Microsoft.EntityFrameworkCore;
using WebApp.Data.DbContexts;
using WebApp.Data.Entities.Classes;
using WebApp.Data.Repositories.Interfaces;

namespace WebApp.Data.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DatabaseContext _databaseContext;
        private readonly DbSet<TEntity> _dbSet;
        public GenericRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _dbSet = _databaseContext.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            _databaseContext.Entry(entity).State = EntityState.Added;
            await _dbSet.AddAsync(entity);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _dbSet.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> UpdateAsync(TEntity entity)
        {
            _databaseContext.Entry(entity).State = EntityState.Modified;
            return await _databaseContext.SaveChangesAsync();
        }
    }
}
