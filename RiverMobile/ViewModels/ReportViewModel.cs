using System;
using MobileCore.ViewModels;
using RiverMobile.Services;

namespace RiverMobile.ViewModels
{
    public class ReportViewModel : ViewModelBase
    {
        readonly IRiverApiService riverApiService;

        public ReportViewModel(IRiverApiService riverApiService)
        {
            this.riverApiService = riverApiService;

            Title = "Report";
        }
    }
}
