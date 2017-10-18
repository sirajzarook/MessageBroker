using AllSaints.MessageBroker.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllSaints.MessageBroker.Delegates
{
	public class MessengerDelegates
	{
		public delegate void OnError(string errorMessage);
		public delegate void OnMessageReceive(IMessage message);
	}
}
