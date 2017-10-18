using System;
using System.Collections.Generic;
using System.Text;

namespace AllSaints.MessageBroker.Configurations.Abstracts
{
	public interface IHostSettings
	{
		Dictionary<string, string> HostSettings { get; set; }

	}
}
