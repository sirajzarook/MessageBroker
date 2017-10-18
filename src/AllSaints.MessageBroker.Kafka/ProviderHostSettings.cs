using AllSaints.MessageBroker.Configurations.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllSaints.MessageBroker.Kafka
{
    public class ProviderHostSettings : IHostSettings
    {
        public Dictionary<string, string> HostSettings { get; set; }

        public ProviderHostSettings()
        {
            HostSettings = new Dictionary<string, string>();
            HostSettings.Add("acks", "all");
            HostSettings.Add("group.id", "nordstrom.dropship.fulfillment");
            HostSettings.Add("enable.auto.commit", "false");
            HostSettings.Add("auto.offset.reset", "earliest");            
        }

        public ProviderHostSettings(string server) : this()
        {
            HostSettings.Add("server", server);
        }

        public void SetServer(string server)
        {
            if (HostSettings.ContainsKey("server"))
                HostSettings["server"] = server;
            else
                HostSettings.Add("server", server);
        }
    }
}
