using LabCheckin.Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabCheckin.Server.Data
{
    public class CheckinRecord
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserId { get; set; } = default!;
        public WorkType Type { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int? StartClass { get; set; }
        public int? EndClass { get; set; }
        public Room ClassRoom { get; set; }
        public bool ExtraTask { get; set; }
        public TimeSpan ExtraTaskDuration { get; set; }
        public string? Note { get; set; }

        public virtual ApplicationUser User { get; set; } = default!;
    }
}
