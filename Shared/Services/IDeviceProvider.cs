using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace LabCenter.Shared.Services
{
    public interface IDeviceProvider : INotifyPropertyChanged, IDisposable
    {
        ValueTask InitAysnc();
        bool IsDesktop { get; }
        bool IsMobile { get; }
        bool IsServerSide { get; }
    }
}
