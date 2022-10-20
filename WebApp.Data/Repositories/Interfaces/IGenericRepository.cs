using WebApp.Data.Entities.Classes;

namespace WebApp.Data.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity?> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task AddAsync(TEntity entity);
        Task<int> UpdateAsync(TEntity entity);
    }
}
