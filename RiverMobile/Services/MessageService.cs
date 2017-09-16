using System;
using RiverMobile.Messages;
using Xamarin.Forms;

namespace RiverMobile.Services
{
    public class MessageService : IMessageService
    {
        public void Send<TMessage>(TMessage message, object sender = null)
            where TMessage : IMessage
        {
            if (sender == null)
                sender = new object();

            MessagingCenter.Send(sender, typeof(TMessage).FullName, message);
        }

        public void Subscribe<TMessage>(object subscriber, Action<object, TMessage> callback)
            where TMessage : IMessage
        {
            MessagingCenter.Subscribe(subscriber, typeof(TMessage).FullName, callback, null);
        }

        public void Unsubscribe<TMessage>(object subscriber)
            where TMessage : IMessage
        {
            MessagingCenter.Unsubscribe<object, TMessage>(subscriber, typeof(TMessage).FullName);
        }
    }
}
