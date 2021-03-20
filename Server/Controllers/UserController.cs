using LabCenter.Server.Data;
using LabCenter.Server.Models;
using LabCenter.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using static LabCenter.Server.Middlewares.CredentialsFilter;

namespace LabCenter.Server.Controllers
{
    [ApiController]
    [Route("/api/users")]
    public class UserController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public UserController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        [Route("signup"), HttpPost]
        public async Task<Response<UserInfo>> SignUpAsync([FromBody] SignUpModel model)
        {
            var userInfo = new ApplicationUser
            {
                Admin = false,
                Email = model.Email,
                Name = model.Name,
                UserName = model.UserName
            };

            var result = await userManager.CreateAsync(userInfo, model.Password);

            if (result.Succeeded)
            {
                return new UserInfo(userInfo.Id, userInfo.UserName, userInfo.Name, userInfo.Admin);
            }

            return new Response<UserInfo>.Error.BadRequest(result.Errors.Select(i => i.Description).Aggregate((accu, next) => $"{accu}, {next}"));
        }

        [Route("signin"), HttpPost]
        public async Task<Response<UserInfo>> SignInAsync([FromBody] SignInModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                var user = await userManager.FindByNameAsync(model.UserName);
                return new UserInfo(user.Id, user.UserName, user.Name, user.Admin);
            }

            return new Response<UserInfo>.Error.Unauthorized("用户名或密码不正确");
        }

        [Route("signout"), RequireSignIn, HttpPost]
        public Task SignOutAsync() => signInManager.SignOutAsync();

        [Route("profile"), RequireSignIn, HttpGet]
        public async Task<Response<UserInfo>> GetProfileAsync()
        {
            var user = await userManager.GetUserAsync(User);
            return new UserInfo(user.Id, user.UserName, user.Name, user.Admin);
        }

        [Route("password"), RequireSignIn, HttpPost]
        public async Task<Response<bool>> ChangePasswordAsync([FromBody] ChangePasswordModel model)
        {
            var user = await userManager.GetUserAsync(User);
            var result = await userManager.ChangePasswordAsync(user, model.OldPaassword, model.NewPassword);

            if (result.Succeeded)
            {
                return true;
            }

            return new Response<bool>.Error.BadRequest("密码不正确");
        }
    }
}
