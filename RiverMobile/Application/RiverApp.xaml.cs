using RiverMobile.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobileCore.Factories;
using RiverMobile.ViewModels;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace RiverMobile
{
    public partial class RiverApp : Application
    {
        internal static Lazy<MasterDetailPage> mainView;
        public static MasterDetailPage MainView
        {
            get
            {
                return mainView.Value;
            }
        }

        public RiverApp()
        {
            InitializeComponent();
        }
    }
}
