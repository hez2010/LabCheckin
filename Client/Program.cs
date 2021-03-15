using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using BlazorFluentUI;
using LabCenter.Shared.Services;
using LabCenter.Client.Services;
using System;
using System.Net.Http;

namespace LabCenter.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services.AddBlazorFluentUI();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ICheckinService, CheckinService>();
            builder.Services.AddScoped<IDeviceProvider, DeviceProvider>();
            builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<GlobalStates>();

            await builder.Build().RunAsync();
        }
    }
}
