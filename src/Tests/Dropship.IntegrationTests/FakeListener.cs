using AllSaints.MessageBroker.Abstracts;
using Dropship.Ofs.Processor.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static AllSaints.MessageBroker.Delegates.MessengerDelegates;
using AllSaints.MessageBroker.Configurations.Abstracts;
using AllSaints.MessageBroker.Delegates;
using System.Threading;

namespace Dropship.IntegrationTests
{
    public class FakeListener : IListener
    {
        public FakeListener()
        {
        }
        
        public event OnError OnError;
        public event OnMessageReceive OnMessage;

        public void Dispose()
        {
        }
        

        public void GenerateFakeEvent(IMessage message)
        {

            OnMessage(message);
        }

        public void StartListening(IMessageSettings inMessageSettings, CancellationTokenSource cancellationTokenSource = null)
        {
        }
        
    }
}
