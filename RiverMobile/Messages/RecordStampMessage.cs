using System;
using RiverMobile.Models;

namespace RiverMobile.Messages
{
    public class RecordStampMessage : IMessage
    {
        public readonly Stamp Stamp;

        public RecordStampMessage(Stamp stamp)
        {
            Stamp = stamp;
        }
    }
}
