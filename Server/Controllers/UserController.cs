using LabCheckin.Server.Data;
using LabCheckin.Server.Models;
using LabCheckin.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LabCheckin.Server.Controllers
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

        [Route("signin"), HttpPost]
        public async Task<ResponseModel<UserInfo>> SignInAsync([FromBody] SignInModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                var user = await userManager.FindByNameAsync(model.UserName);
                return new JsonResultModel<UserInfo>(new(user.Id, user.UserName, user.Name));
            }

            return new UnauthorizedModel<UserInfo>("用户名或密码不正确");
        }

        [Route("signout"), HttpPost]
        public Task SignOutAsync() => signInManager.SignOutAsync();

        [Route("profile"), HttpGet]
        public async Task<ResponseModel<UserInfo>> GetProfileAsync()
        {
            if (!signInManager.IsSignedIn(User))
            {
                return new UnauthorizedModel<UserInfo>("未登录");
            }

            var user = await userManager.GetUserAsync(User);
            return new JsonResultModel<UserInfo>(new(user.Id, user.UserName, user.Name));
        }

        [Route("password"), HttpPost]
        public async Task<ResponseModel<bool>> ChangePasswordAsync([FromBody] ChangePasswordModel model)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return new UnauthorizedModel<bool>("未登录");
            }

            var user = await userManager.GetUserAsync(User);
            var result = await userManager.ChangePasswordAsync(user, model.OldPaassword, model.NewPassword);

            if (result.Succeeded)
            {
                return new JsonResultModel<bool>(true);
            }

            return new BadRequestModel<bool>("密码不正确");
        }
    }
}
