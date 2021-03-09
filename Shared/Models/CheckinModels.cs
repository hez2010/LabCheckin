namespace LabCenter.Shared.Models
{
    public record CheckinModel(int WorkPlanId, bool Overtime, int OvertimeMinutes, string? Note);
    public record CheckinRecordModel(int Id, WorkPlanModel? WorkPlan, bool OverTime, int OvertimeMinutes, string? Note);
}
