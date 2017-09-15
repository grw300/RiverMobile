using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MobileCore.ViewModels;
using RiverMobile.Helpers;
using RiverMobile.Services;
using Xamarin.Forms;

namespace RiverMobile.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        readonly ILoginService loginService;

        public string UserName
        {
            get => Settings.UserName;
            private set
            {
                if (Settings.UserName == value)
                    return;

                string userName = string.Empty;

                SetProperty(ref userName, value);
                Settings.UserName = userName;
            }
        }

        public ICommand LogoutCommand { get; private set; }

        public SettingsViewModel(
            ILoginService loginService)
        {
            this.loginService = loginService;

            LogoutCommand = new Command(LogoutAsync);

            Title = "Personnel Settings";
        }

        void LogoutAsync()
        {
            loginService.LogoutAsync();
        }
    }
}