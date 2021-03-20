using LabCenter.Server.Data;
using LabCenter.Server.Models;
using LabCenter.Shared.Models;
using LabCenter.Shared.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LabCenter.Server.Controllers
{
    [ApiController]
    [Route("/api/checkin")]
    public class CheckinController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICheckinService checkinService;

        public CheckinController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, ICheckinService checkinService)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.checkinService = checkinService;
        }

        [Route(""), HttpGet]
        public async Task<Response<PagingModel<CheckinRecordModel>>> GetCheckinRecordHistoryAsync(int skip = 0, int count = 10, string? userId = default)
        {
            var user = await userManager.GetUserAsync(User);
            if (user is null)
            {
                return new Response<PagingModel<CheckinRecordModel>>.Error.Unauthorized("未登录账户");
            }

            if (!user.Admin && !string.IsNullOrEmpty(userId) && userId != user.Id)
            {
                return new Response<PagingModel<CheckinRecordModel>>.Error.Forbidden("没有权限访问");
            }

            var query = (userId, user.Admin) switch
            {
                ("" or null, true) => dbContext.Records,
                _ => dbContext.Records.Where(i => i.UserId == userId)
            };

            var records = await query.OrderByDescending(i => i.Id).Skip(skip).Take(count)
                .Select(record =>
                    new CheckinRecordModel(record.Id,
                        new(record.WorkPlan.Id, record.WorkPlan.Type, record.WorkPlan.StartTime,
                            record.WorkPlan.EndTime, record.WorkPlan.ClassRoom,
                            record.WorkPlan.SalaryBonus, record.WorkPlan.Note),
                    record.Overtime, record.OvertimeMinutes, record.Note)).ToListAsync();

            return new PagingModel<CheckinRecordModel>
            {
                Count = await query.CountAsync(),
                Data = records
            };
        }

        //[Route("{id}"), HttpGet]
        //public async Task<ResponseModel<CheckinRecordModel>> GetCheckinRecordAsync(int id)
        //{
        //    var user = await userManager.GetUserAsync(User);
        //    if (user is null)
        //    {
        //        return new UnauthorizedModel<CheckinRecordModel>("未登录账户");
        //    }

        //    var query = dbContext.Records.Where(i => i.Id == id);
        //    if (!user.Admin)
        //    {
        //        query = query.Where(i => i.UserId == user.Id);
        //    }

        //    var record = await query.FirstOrDefaultAsync();

        //    if (record is null)
        //    {
        //        return new NotFoundModel<CheckinRecordModel>("不存在此项记录");
        //    }

        //    return new CheckinRecordModel(record.Id,
        //        new(record.WorkPlan!.Id, record.WorkPlan.Type, record.WorkPlan.StartTime, record.WorkPlan.EndTime,
        //            record.WorkPlan.Classes, record.WorkPlan.ClassRoom, record.WorkPlan.SalaryBonus, record.WorkPlan.Note),
        //        record.ExtraTaskDuration, record.Note);
        //}

        [Route(""), HttpPost]
        public async Task<Response<int>> CheckinAsync([FromBody] CheckinModel model)
        {
            var user = await userManager.GetUserAsync(User);
            if (user is null)
            {
                return new Response<int>.Error.Unauthorized("未登录账户");
            }

            var workPlan = await dbContext.Plans.FirstOrDefaultAsync(i => i.Id == model.WorkPlanId);

            if (workPlan is null)
            {
                return new Response<int>.Error.NotFound("不存在此项工作");
            }

            if (await dbContext.Records.AnyAsync(i => i.WorkPlanId == model.WorkPlanId && i.UserId == user.Id))
            {
                return new Response<int>.Error.BadRequest("已存在签到记录");
            }

            if (workPlan.MaxUsers is > 0 && await dbContext.Records.CountAsync(i => i.WorkPlanId == model.WorkPlanId) > workPlan.MaxUsers)
            {
                return new Response<int>.Error.BadRequest("超过最大允许的签到人数");
            }

            var record = new CheckinRecord
            {
                Note = model.Note,
                UserId = user.Id,
                WorkPlanId = model.WorkPlanId,
                Overtime = model.Overtime,
                OvertimeMinutes = model.OvertimeMinutes
            };

            await dbContext.Records.AddAsync(record);

            await dbContext.SaveChangesAsync();

            return record.Id;
        }
    }
}
