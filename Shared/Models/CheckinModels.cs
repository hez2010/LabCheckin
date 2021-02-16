using System;
using System.Collections.Generic;

namespace LabCheckin.Shared.Models
{
    public record CheckinModel(DateTime Date, WorkType Type, Room Room, DateTime? StartTime, DateTime? EndTime, List<int>? Classes, bool Bonus, bool Overtime, int? OvertimeMinutes, string? OvertimeReason, string? Note);
}
