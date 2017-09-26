using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MobileCore.Interfaces;
using MobileCore.ViewModels;
using RiverMobile.Helpers;
using RiverMobile.Messages;
using RiverMobile.Services;
using Xamarin.Forms;
using RiverMobile.Models;
using Newtonsoft.Json;
using System.Linq;

namespace RiverMobile.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        readonly ILoginService loginService;
        readonly IRiverApiService riverApiService;
        readonly IDialogProvider dialogProvider;


        public LoginViewModel(
            IDialogProvider dialogProvider,
            ILoginService loginService,
            IRiverApiService riverApiService)
        {
            this.dialogProvider = dialogProvider;
            this.loginService = loginService;
            this.riverApiService = riverApiService;

            LoginCommand = new Command(async () =>
                await LoginAsync()
            );

            Title = "Login";
        }

        public string UserName
        {
            get => Settings.UserName;
            set
            {
                if (Settings.UserName == value)
                    return;

                string userName = string.Empty;
                SetProperty(ref userName, value);
                Settings.UserName = userName;
            }
        }

        public ICommand LoginCommand { get; private set; }

        async Task LoginAsync()
        {
            IsBusy = true;

            try
            {
                await loginService.LoginAsync(UserName);
            }
            catch (Exception e)
            {
                var result = await dialogProvider.DisplayActionSheet(e.Message, null, null, "Retry");

                if (result == "Retry")
                    return;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public override void OnAppearing(object obj, EventArgs e)
        {
            base.OnAppearing(obj, e);
        }
    }
}
