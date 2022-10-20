using WebApp.Data.Entities.Classes;

namespace WebApp.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
    }
}
