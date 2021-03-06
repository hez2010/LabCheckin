﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabCenter.Server.Data
{
    public class MonthlySalary
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserId { get; set; } = default!;
        public int Minutes { get; set; }
        public int ValidHours { get; set; }
        public int RemainingHours { get; set; }

        public ApplicationUser User { get; set; } = default!;
    }
}
