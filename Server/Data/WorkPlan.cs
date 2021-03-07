using LabCenter.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabCenter.Server.Data
{
    public class WorkPlan
    {
        private DateTimeOffset startTime;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public ICollection<ApplicationUser> Users { get; set; } = default!;
        public WorkType Type { get; set; }
        public long DayIndex { get; set; }
        public DateTimeOffset StartTime
        {
            get => startTime;
            set
            {
                startTime = value;
                DayIndex = value.Date.Ticks;
            }
        }
        public DateTimeOffset EndTime { get; set; }
        public int Minutes { get; set; }
        public Room ClassRoom { get; set; }
        public bool SalaryBonus { get; set; }
        public string? Note { get; set; }
        public int MaxUsers { get; set; }

        public IList<CheckinRecord> Records { get; set; } = default!;
    }
}
