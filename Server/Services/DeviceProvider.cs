using LabCenter.Shared.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace LabCenter.Server.Services
{
    public sealed class DeviceProvider : IDeviceProvider
    {
        public DeviceProvider(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor.HttpContext is not null)
            {
                IsMobile = httpContextAccessor.HttpContext.Request.Headers
                    .FirstOrDefault(i => string.Equals(i.Key, "User-Agent", StringComparison.OrdinalIgnoreCase))
                    .Value.ToString()?.ToLowerInvariant().Contains("mobi") ?? false;
            }
        }

        public bool IsDesktop => !IsMobile;

        public bool IsMobile { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void Dispose() { }
        public ValueTask InitAysnc() => ValueTask.CompletedTask;
        public bool IsServerSide => true;
    }
}
