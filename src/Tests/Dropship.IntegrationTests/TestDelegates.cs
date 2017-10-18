
using AllSaints.MessageBroker.Abstracts;
using AllSaints.MessageBroker.Configurations.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dropship.IntegrationTests
{
    public delegate void OnMessageReceived(IMessage message, IMessageSettings topic);
}
