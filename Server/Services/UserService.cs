using LabCheckin.Server.Data;
using LabCheckin.Shared;
using LabCheckin.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace LabCheckin.Server.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserService(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserInfo?> GetProfileAsync()
        {
            var claim = httpContextAccessor.HttpContext?.User;
            if (claim is null) return null;
            var user = await userManager.GetUserAsync(claim);
            if (user is null) return null;
            return new()
            {
                Id = user.Id,
                Name = user.Name,
                UserName = user.UserName
            };
        }

        public Task<UserInfo?> SignInAsync(string userName, string password) => throw new NotImplementedException();

        public Task SignOutAsync() => throw new NotImplementedException();
    }
}
