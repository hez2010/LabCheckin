using System;
using System.Collections.Generic;

namespace LabCenter.Shared.Models
{
    public record WorkPlanModel(int Id, WorkType Type, DateTimeOffset StartTime, DateTimeOffset EndTime, Room ClassRoom, bool SalaryBonus, string? Note);
    public record AddWorkPlanModel(WorkType Type, DateTime StartTime, DateTime EndTime, int MaxUsers, int Minutes, Room ClassRoom, bool SalaryBonus, string? Note, List<string> UserIds, int RepeatWeeks = 1);
    public record AssignModel(List<int> WorkPlanIds, string UserId);
}
