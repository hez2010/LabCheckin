using LabCenter.Server.Data;
using LabCenter.Shared.Models;
using LabCenter.Shared.Services;
using LabCenter.Shared.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabCenter.Server.Services
{
    public class CheckinService : ICheckinService
    {
        private readonly ApplicationDbContext dbContext;

        public CheckinService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<bool> CheckinAsync(int workPlanId, bool overtime, int overtimeMinutes, string? note) => throw new NotImplementedException();
        public Task<List<CheckinRecordModel>> QueryCheckinRecordsAsync(int? workPlanId, DateTimeOffset? date, string? userId, Room? classroom, int beforeId = -1)
        {
            IQueryable<CheckinRecord> records = dbContext.Records;

            if (workPlanId is not null) records = records.Where(i => i.WorkPlanId == workPlanId);
            if (date is not null) records = records.Include(i => i.WorkPlan).Where(i => i.WorkPlan.DayIndex == date.Value.GetDayIndex());
            if (userId is not null) records = records.Where(i => i.UserId == userId);
            if (classroom is not null) records = records.Include(i => i.WorkPlan).Where(i => i.WorkPlan.ClassRoom == classroom);

            if (beforeId != -1) records = records.Where(i => i.Id < beforeId);

            return records.OrderByDescending(i => i.Id).Take(20).Select(i => new CheckinRecordModel(i.Id, null, i.Overtime, i.OvertimeMinutes, i.Note)).ToListAsync();
        }
    }
}
