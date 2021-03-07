using LabCenter.Server.Data;
using LabCenter.Shared.Models;
using LabCenter.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace LabCenter.Server.Services
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

        public Task<bool> ChangePasswordAsync(string oldPassword, string newPassword) => throw new NotImplementedException();

        public async Task<UserInfo?> GetProfileAsync()
        {
            var claim = httpContextAccessor.HttpContext?.User;
            if (claim is null) return null;
            var user = await userManager.GetUserAsync(claim);
            if (user is null) return null;
            return new(user.Id, user.Name, user.UserName, user.Admin);
        }

        public Task<UserInfo?> SignInAsync(string userName, string password) => throw new NotImplementedException();

        public Task SignOutAsync() => throw new NotImplementedException();
    }
}
