using LabCheckin.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LabCheckin.Shared.Services
{
    public interface ICheckinService
    {
        Task<bool> CheckInAsync(
            DateTime date,
            WorkType type,
            Room room,
            DateTime? startTime,
            DateTime? endTime,
            List<int>? classes,
            bool bonus,
            bool overtime,
            int? overtimeMinutes,
            string? overtimeReason,
            string? note);
    }
}
