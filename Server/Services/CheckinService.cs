using LabCheckin.Shared.Models;
using LabCheckin.Shared.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LabCheckin.Server.Services
{
    public class CheckinService : ICheckinService
    {
        public Task<bool> CheckInAsync(DateTime date, WorkType type, Room room, DateTime? startTime, DateTime? endTime, List<int>? classes, bool bonus, bool overtime, int? overtimeMinutes, string? overtimeReason, string? note) => throw new NotImplementedException();
    }
}
