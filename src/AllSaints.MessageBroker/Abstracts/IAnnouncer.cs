using System;
using AllSaints.MessageBroker.Configurations.Abstracts;
using static AllSaints.MessageBroker.Delegates.MessengerDelegates;

namespace AllSaints.MessageBroker.Abstracts
{
	public interface IAnnouncer : IDisposable
	{
		bool Announce(IMessage message, IMessageSettingsOut messageSettings);

		event OnError OnError;
	}
}
