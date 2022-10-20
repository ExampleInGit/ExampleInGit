using Microsoft.EntityFrameworkCore;
using WebApp.Data.DbContexts;
using WebApp.Data.Entities.Classes;
using WebApp.Data.Repositories.Interfaces;

namespace WebApp.Data.Repositories.Classes
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _databaseContext;
        public UserRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _databaseContext.Users.Include(x => x.Role).Where(x => x.Email == email).FirstOrDefaultAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _databaseContext.Users.Include(x => x.Role).SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
