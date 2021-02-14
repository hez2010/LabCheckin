using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace LabCheckin.Server.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = default!;
        public bool Valid { get; set; }
        public int RemainingHours { get; set; }

        public virtual ICollection<CheckinRecord> Records { get; set; } = new HashSet<CheckinRecord>();
        public virtual ICollection<MonthlySalary> Salaries { get; set; } = new HashSet<MonthlySalary>();
    }
}
