using Auth.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Auth.Core.Repositories
{
    public interface IAuthRepository
    {
        Task<ApplicationUser?> GetUserByEmailAsync(string email);
        Task<ApplicationUser?> GetUserByIdAsync(string userId);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task<IdentityResult> UpdateUserAsync(ApplicationUser user);
        Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
        Task<bool> RoleExistsAsync(string roleName);
        Task<IdentityResult> CreateRoleAsync(ApplicationRole role);
    }
}
