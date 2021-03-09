using LabCenter.Shared.Models;
using LabCenter.Shared.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace LabCenter.Client.Services
{
    public class CheckinService : ICheckinService
    {
        private readonly HttpClient httpClient;

        public CheckinService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<bool> CheckinAsync(int workPlanId, bool overtime, int overtimeMinutes, string? note)
        {
            var result = await httpClient.PostAsJsonAsync("/api/checkin", new CheckinModel(workPlanId, overtime, overtimeMinutes, note));
            return result.IsSuccessStatusCode;
        }

        public Task<List<CheckinRecordModel>> QueryCheckinRecordsAsync(int? workPlanId, DateTimeOffset? date, string? userId, Room? classroom, int beforeId = -1) => throw new NotImplementedException();
    }
}
