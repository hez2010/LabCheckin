using LabCenter.Shared.Services;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace LabCenter.Client.Services
{
    public sealed class DeviceProvider : IDeviceProvider
    {
        private const int WIDTH_THRESHOLD = 798;
        private static readonly List<WeakReference<DeviceProvider>> references = new();
        private readonly IJSRuntime jsRuntime;
        private static int width;
        private static int height;

        private static int Height { get => height; set => height = value; }
        private static int Width
        {
            get => width;
            set
            {
                var oldWidth = width;
                width = value;
                if ((oldWidth > WIDTH_THRESHOLD && width <= WIDTH_THRESHOLD) || oldWidth <= WIDTH_THRESHOLD && width > 798)
                {
                    foreach (var i in references)
                    {
                        if (i.TryGetTarget(out var target) && target is not null)
                        {
                            target.PropertyChanged?.Invoke(target, new PropertyChangedEventArgs(nameof(IsDesktop)));
                            target.PropertyChanged?.Invoke(target, new PropertyChangedEventArgs(nameof(IsMobile)));
                        }
                    }
                }
            }
        }

        public bool IsDesktop => Width > WIDTH_THRESHOLD;

        public bool IsMobile => !IsDesktop;

        public bool IsServerSide => false;

        public event PropertyChangedEventHandler? PropertyChanged;

        [JSInvokable]
        public static void WindowSizeChanged(int[] size)
        {
            Width = size[0];
            Height = size[1];
        }

        public DeviceProvider(IJSRuntime jsRuntime)
        {
            references.Add(new(this));
            this.jsRuntime = jsRuntime;
        }

        public async ValueTask InitAysnc()
        {
            var size = await jsRuntime.InvokeAsync<int[]>("utils.getWindowSize");
            width = size[0];
            height = size[1];
        }

        public void Dispose()
        {
            var item = references.FirstOrDefault(i => i.TryGetTarget(out var target) && target == this);
            if (item is not null)
            {
                references.Remove(item);
            }
        }
    }
}
