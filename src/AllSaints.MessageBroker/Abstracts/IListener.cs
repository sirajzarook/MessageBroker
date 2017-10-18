using AllSaints.MessageBroker.Configurations.Abstracts;
using AllSaints.MessageBroker.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static AllSaints.MessageBroker.Delegates.MessengerDelegates;

namespace AllSaints.MessageBroker.Abstracts
{
	public interface IListener : IDisposable
	{
		//InMessage inMessage { get; set; }
		event OnError OnError;
		event OnMessageReceive OnMessage;
		void StartListening(IMessageSettings inMessageSettings, CancellationTokenSource cancellationTokenSource = null);
    }
}
