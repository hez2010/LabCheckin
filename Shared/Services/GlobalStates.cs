using LabCenter.Shared.Models;
using System.ComponentModel;

namespace LabCenter.Shared.Services
{
    public class GlobalStates : INotifyPropertyChanged
    {
        private UserInfo? userInfo;

        public UserInfo? UserInfo
        {
            get => userInfo;
            set
            {
                userInfo = value;
                PropertyChanged?.Invoke(this, new(nameof(UserInfo)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
