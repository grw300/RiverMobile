using System;
using MobileCore.ViewModels;
using RiverMobile.Services;

namespace RiverMobile.ViewModels
{
    public class ReportViewModel : ViewModelBase
    {
        readonly IRiverAPIService riverAPIService;

        public ReportViewModel(IRiverAPIService riverAPIService)
        {
            this.riverAPIService = riverAPIService;

            Title = "Report";
        }
    }
}
