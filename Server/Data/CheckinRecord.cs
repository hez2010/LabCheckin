using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabCenter.Server.Data
{
    public class CheckinRecord
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserId { get; set; } = default!;
        public int WorkPlanId { get; set; }
        public bool Overtime { get; set; }
        public int OvertimeMinutes { get; set; }
        public string? Note { get; set; }

        public ApplicationUser User { get; set; } = default!;
        public WorkPlan WorkPlan { get; set; } = default!;
    }
}
