using System;
using System.Collections.Generic;
using System.Text;

namespace AllSaints.MessageBroker.Configurations.Abstracts
{
	public interface IMessageSettings
	{
		Dictionary<string, string> MessageSettings { get; set; }

	}
}
