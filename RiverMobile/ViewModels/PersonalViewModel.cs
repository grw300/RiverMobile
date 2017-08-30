using MobileCore.ViewModels;
using RiverMobile.Models;

namespace RiverMobile.ViewModels
{
    public class PersonalViewModel : ViewModelBase
    {
        Personal personal;

        public Personal Personal
        {
            get { return personal; }
            set { SetProperty(ref personal, value); }
        }
    }
}