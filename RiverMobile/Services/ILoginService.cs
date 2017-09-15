using System;
using System.Threading.Tasks;
using RiverMobile.Models;
using Xamarin.Forms;

namespace RiverMobile.Services
{
    public interface ILoginService
    {
        Task LoginAsync(string UserName);
        void LogoutAsync();
        Personal Register();
        MasterDetailPage MainPage { get; }
    }
}
