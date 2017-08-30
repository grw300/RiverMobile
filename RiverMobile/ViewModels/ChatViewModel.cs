using System;
using MobileCore.ViewModels;
using RiverMobile.Services;

namespace RiverMobile.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        readonly IRiverAPIService riverAPIService;

        public ChatViewModel(IRiverAPIService riverAPIService)
        {
            this.riverAPIService = riverAPIService;

            Title = "Chat";
        }
    }
}
