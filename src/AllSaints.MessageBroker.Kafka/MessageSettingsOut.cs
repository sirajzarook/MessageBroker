using AllSaints.MessageBroker.Abstracts;
using AllSaints.MessageBroker.Configurations.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllSaints.MessageBroker.Kafka
{
	public class MessageSettingsOut : IMessageSettingsOut
	{     
		public Dictionary<string, string> MessageSettings { get; set; }
		public MessageSettingsOut()
		{
            MessageSettings = new Dictionary<string, string>();
            //NOT USED
            MessageSettings.Add("group.id", "nordstrom.dropship.fulfillment");
        }

        public MessageSettingsOut(string topic) : this()
        {
            MessageSettings.Add("produce.ontopic", topic);
        }


    }
}
