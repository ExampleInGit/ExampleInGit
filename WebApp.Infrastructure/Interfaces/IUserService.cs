using WebApp.Shared.Dto;
using WebApp.Shared.Enums;
using WebApp.Shared.Models;

namespace WebApp.Infrastructure.Interfaces
{
    public interface IUserService
    {
        Task<UserModel> AddAsync(CreateUserModel user, RoleType role);
        Task<IEnumerable<UserModel>> GetAllAsync();
        Task<UserModel?> GetByIdAsync(int id);
        Task UpdateAsync(UserModel userModel);
        Task DeleteAsync(int id);
        Task<TokenDto> GetTokenAsync(LoginModel loginModel);
    }
}
