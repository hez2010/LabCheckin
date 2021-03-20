using LabCenter.Server.Data;
using LabCenter.Server.Models;
using LabCenter.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabCenter.Server.Controllers
{
    [ApiController]
    [Route("/api/workplan")]
    public class WorkPlanController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public WorkPlanController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        [Route(""), HttpGet]
        public async Task<Response<PagingModel<WorkPlanModel>>> GetWorkPlanListAsync(DateTime startDate, DateTime endDate)
        {
            var user = await userManager.GetUserAsync(User);
            if (user is null)
            {
                return new Response<PagingModel<WorkPlanModel>>.Error.Unauthorized("未登录账户");
            }

            var plans = await dbContext.Plans.Where(i => i.Users.Any(j => j.Id == user.Id) && i.DayIndex >= startDate.Date.Ticks && i.DayIndex <= endDate.Date.Ticks)
                .Select(i =>
                    new WorkPlanModel(i!.Id, i.Type, i.StartTime, i.EndTime, i.ClassRoom, i.SalaryBonus, i.Note))
                .ToListAsync();

            return new PagingModel<WorkPlanModel>
            {
                Count = plans.Count,
                Data = plans
            };
        }

        [Route("assign"), HttpPost]
        public async Task<Response<bool>> AssignWorkPlanAsync([FromBody] AssignModel model)
        {
            var user = await userManager.GetUserAsync(User);
            if (user is not { Admin: true })
            {
                return new Response<bool>.Error.Unauthorized("没有权限访问");
            }

            var plans = await dbContext.Plans.Where(i => model.WorkPlanIds.Contains(i.Id)).ToListAsync();

            foreach (var i in plans)
            {
                user.Plans.Add(i);
            }

            await dbContext.SaveChangesAsync();

            return true;
        }

        [Route(""), HttpPost]
        public async Task<Response<List<int>>> AddWorkPlanAsync([FromBody] WorkPlanCreationModel model)
        {
            var user = await userManager.GetUserAsync(User);
            if (user is not { Admin: true })
            {
                return new Response<List<int>>.Error.Unauthorized("没有权限访问");
            }

            var plans = new List<WorkPlan>();
            var users = model.UserIds.Select(u => new ApplicationUser { Id = u }).ToList();

            for (var i = 0; i < model.RepeatWeeks; i++)
            {
                var offset = TimeSpan.FromDays(i * 7);
                plans.Add(new WorkPlan
                {
                    ClassRoom = model.ClassRoom,
                    StartTime = model.StartTime + offset,
                    EndTime = model.EndTime + offset,
                    MaxUsers = model.MaxUsers,
                    Note = model.Note,
                    SalaryBonus = model.SalaryBonus,
                    Type = model.Type,
                    Minutes = model.Minutes,
                    Users = users
                });
            }

            await dbContext.Plans.AddRangeAsync(plans);
            await dbContext.SaveChangesAsync();

            return plans.Select(i => i.Id).ToList();
        }
    }
}
