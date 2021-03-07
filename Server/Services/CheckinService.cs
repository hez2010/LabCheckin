using LabCenter.Shared.Services;
using System;
using System.Threading.Tasks;

namespace LabCenter.Server.Services
{
    public class CheckinService : ICheckinService
    {
        public Task<bool> CheckInAsync(int workPlanId, bool overtime, int overtimeMinutes, string? note) => throw new NotImplementedException();
    }
}
