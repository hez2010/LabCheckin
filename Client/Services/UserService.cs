using LabCheckin.Shared.Extensions;
using LabCheckin.Shared.Models;
using LabCheckin.Shared.Services;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace LabCheckin.Client.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient httpClient;

        public UserService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<bool> ChangePasswordAsync(string oldPassword, string newPassword)
        {
            var response = await httpClient.PostAsJsonAsync("/api/users/password", new ChangePasswordModel(oldPassword, newPassword));
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public async Task<UserInfo?> GetProfileAsync()
        {
            var response = await httpClient.GetAsync("/api/users/profile");
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsyncWithExceptionHandled<UserInfo>();
        }

        public async Task<UserInfo?> SignInAsync(string userName, string password)
        {
            var response = await httpClient.PostAsJsonAsync("/api/users/signin", new SignInModel(userName, password, true));
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsyncWithExceptionHandled<UserInfo>();
        }

        public async Task SignOutAsync()
        {
            await httpClient.PostAsJsonAsync("/api/users/signout", new { });
        }
    }
}
