using HiringTask.DTO;
using HiringTask.Models;
namespace HiringTask.Services
{
    public interface IAuthService
    {
        Task<Auth> RegisterAsync(RegisterDTO model);
        Task<Auth> GetTokenAsync(TokenRequestDT model);
        Task<Auth> LoginAsync(LoginDTO model);
        Task<bool> LogoutAsync();
    }
}
