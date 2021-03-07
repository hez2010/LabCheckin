using System.Threading.Tasks;

namespace LabCenter.Shared.Services
{
    public interface ICheckinService
    {
        Task<bool> CheckInAsync(int workPlanId, bool overtime, int overtimeMinutes, string? note);
    }
}
