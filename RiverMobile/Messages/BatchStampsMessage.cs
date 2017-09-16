using System;
using System.Collections.Generic;
using RiverMobile.Models;

namespace RiverMobile.Messages
{
    public class BatchStampsMessage : IMessage
    {
        public readonly IEnumerable<Stamp> Stamps;

        public BatchStampsMessage(IEnumerable<Stamp> stamps)
        {
            Stamps = stamps;
        }
    }
}
