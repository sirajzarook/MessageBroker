using AllSaints.MessageBroker.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;
using static AllSaints.MessageBroker.Delegates.MessengerDelegates;
using AllSaints.MessageBroker.Configurations.Abstracts;

namespace Dropship.IntegrationTests
{
    public class FakeAnnouncer : IAnnouncer
    {
        FakeMessageBroker broker;
        public FakeAnnouncer(FakeMessageBroker broker)
        {
            this.broker = broker;
        }
        public event OnError OnError;

        public bool Announce(IMessage message, IMessageSettingsOut messageSettings)
        {
            broker.AnnouncerReceiveMessage(message, messageSettings);
            return true;
        }

        public void Dispose()
        {
        }
               
    }
}
