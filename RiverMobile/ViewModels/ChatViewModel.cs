using System;
using MobileCore.ViewModels;
using RiverMobile.Services;

namespace RiverMobile.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        readonly IRiverApiService riverApiService;

        public ChatViewModel(IRiverApiService riverApiService)
        {
            this.riverApiService = riverApiService;

            Title = "Chat";
        }
    }
}
