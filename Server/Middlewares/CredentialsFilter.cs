using LabCenter.Server.Data;
using LabCenter.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LabCenter.Server.Middlewares
{
    public class CredentialsFilter
    {
        [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
        public sealed class RequireSignInAttribute : Attribute, IAsyncActionFilter
        {
            private SignInManager<ApplicationUser>? signInManager;


            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                signInManager = context.HttpContext.RequestServices.GetRequiredService<SignInManager<ApplicationUser>>();
                
                if (!signInManager.IsSignedIn(context.HttpContext.User))
                {
                    context.Result = new Response<object>.Error.Unauthorized("未登录");
                    return;
                }

                await next();
            }
        }

        [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
        public sealed class RequireAdminAttribute : Attribute, IAsyncActionFilter
        {
            private SignInManager<ApplicationUser>? signInManager;
            private UserManager<ApplicationUser>? userManager;

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                signInManager = context.HttpContext.RequestServices.GetRequiredService<SignInManager<ApplicationUser>>();
                userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();

                if (!signInManager.IsSignedIn(context.HttpContext.User))
                {
                    context.Result = new Response<object>.Error.Unauthorized("未登录");
                    return;
                }

                var userId = userManager.GetUserId(context.HttpContext.User);
                var userInfo = await userManager.Users.Select(i => new { i.Id, i.Admin }).FirstOrDefaultAsync(i => i.Id == userId);

                if (userInfo is null || !userInfo.Admin)
                {
                    context.Result = new Response<object>.Error.Forbidden("无权访问");
                    return;
                }

                await next();
            }
        }
    }
}
