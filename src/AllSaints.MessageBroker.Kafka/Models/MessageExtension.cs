using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllSaints.MessageBroker.Kafka
{
    public static class MessageExtension
    {
        public static bool IsSuccess(this Message<string, string> message)
        {
            return message.Error == null ||
                    (!message.Error.HasError && !message.Error.IsBrokerError && !message.Error.IsLocalError && message.Error.Reason == "Success");
        }
    }
}
