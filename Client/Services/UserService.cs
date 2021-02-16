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

        public async Task<UserInfo?> GetProfileAsync()
        {
            var response = await httpClient.GetAsync("/api/users/profile");
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsyncWithExceptionHandled<UserInfo>();
        }

        public async Task<UserInfo?> SignInAsync(string userName, string password)
        {
            var response = await httpClient.PostAsJsonAsync("/api/users/signin", new { UserName = userName, Password = password });
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsyncWithExceptionHandled<UserInfo>();
        }

        public async Task SignOutAsync()
        {
            await httpClient.PostAsJsonAsync("/api/users/signout", new { });
        }
    }
}
