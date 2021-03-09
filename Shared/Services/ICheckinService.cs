using LabCenter.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LabCenter.Shared.Services
{
    public interface ICheckinService
    {
        Task<bool> CheckinAsync(int workPlanId, bool overtime, int overtimeMinutes, string? note);
        Task<List<CheckinRecordModel>> QueryCheckinRecordsAsync(int? workPlanId, DateTimeOffset? date, string? userId, Room? classroom, int beforeId = -1);
    }
}
