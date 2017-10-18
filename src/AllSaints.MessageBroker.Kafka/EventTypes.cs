using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllSaints.MessageBroker.Kafka
{
    public static class EventTypes
    {
        public static EventId Reveived = new EventId(1, "Received");
        public static EventId Sent = new EventId(2, "Sent");
    }
}
