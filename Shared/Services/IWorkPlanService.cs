using LabCenter.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LabCenter.Shared.Services
{
    public interface IWorkPlanService
    {
        Task<bool> CreateWorkPlanAsync(WorkPlanCreationModel model);
        Task<List<WorkPlanModel>> QueryWorkPlanAsync(int? id, string? userId, DateTimeOffset? date, Room? classroom, int beforeId = -1);
    }
}
