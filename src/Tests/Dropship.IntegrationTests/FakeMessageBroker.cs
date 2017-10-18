using AllSaints.MessageBroker.Abstracts;
using AllSaints.MessageBroker.Configurations.Abstracts;
using Dropship.Ofs.Processor.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dropship.IntegrationTests
{
    public class FakeMessageBroker
    {
        public event OnMessageReceived OnMessageSending;

        public void AnnouncerReceiveMessage(IMessage message, IMessageSettings settings)
        {
            OnMessageSending(message, settings);
        }
    }
}
