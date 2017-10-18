using AllSaints.MessageBroker.Configurations.Abstracts;
using AllSaints.MessageBroker.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllSaints.MessageBroker.Kafka
{
    public class MessageSettingsIn : IMessageSettingsIn
    {
        public Dictionary<string, string> MessageSettings { get; set; }

        public MessageSettingsIn()
        {
            MessageSettings = new Dictionary<string, string>();
            
            //NOT USED
            MessageSettings.Add("group.id", "nordstrom.dropship.fulfillment");
            MessageSettings.Add("enable.auto.commit", "false");
            //----
        }

        public MessageSettingsIn(string topic) : this()
        {
            MessageSettings.Add("listening.ontopic", topic);
        }
    }
}
