using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace LabCenter.Server.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = default!;
        public bool Valid { get; set; }
        public int RemainingHours { get; set; }
        public bool Admin { get; set; }

        public IList<CheckinRecord> Records { get; set; } = default!;
        public IList<MonthlySalary> Salaries { get; set; } = default!;
        public IList<WorkPlan> Plans { get; set; } = default!;
    }
}
