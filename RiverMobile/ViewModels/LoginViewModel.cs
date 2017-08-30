using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MobileCore.Interfaces;
using MobileCore.ViewModels;
using RiverMobile.Services;
using Xamarin.Forms;

namespace RiverMobile.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        readonly INavigator navigator;
        readonly IRiverAPIService riverAPIService;

        public LoginViewModel(
            INavigator navigator,
            IRiverAPIService riverAPIService)
        {
            this.navigator = navigator;
            this.riverAPIService = riverAPIService;

            LoginCommand = new Command(async () =>
                await LoginAsync()
            );
        }

        public ICommand LoginCommand { get; private set; }

        Task LoginAsync()
        {
            return navigator.PopModalAsync();
        }
    }
}
