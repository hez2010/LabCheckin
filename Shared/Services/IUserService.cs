using LabCheckin.Shared.Models;
using System.Threading.Tasks;

namespace LabCheckin.Shared.Services
{
    public interface IUserService
    {
        Task<UserInfo?> SignInAsync(string userName, string password);
        Task SignOutAsync();
        Task<UserInfo?> GetProfileAsync();
        Task<bool> ChangePasswordAsync(string oldPassword, string newPassword);
    }
}
