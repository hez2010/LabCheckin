using LabCheckin.Shared.Models;
using LabCheckin.Shared.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace LabCheckin.Client.Services
{
    public class CheckinService : ICheckinService
    {
        private readonly HttpClient httpClient;

        public CheckinService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<bool> CheckInAsync(DateTime date, WorkType type, Room room, DateTime? startTime, DateTime? endTime, List<int>? classes, bool bonus, bool overtime, int? overtimeMinutes, string? overtimeReason, string? note)
        {
            var result = await httpClient.PostAsJsonAsync("/api/checkin", new CheckinModel(date, type, room, startTime, endTime, classes, bonus, overtime, overtimeMinutes, overtimeReason, note));
            return result.IsSuccessStatusCode;
        }
    }
}
