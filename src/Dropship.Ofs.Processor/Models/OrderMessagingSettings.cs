using AllSaints.MessageBroker.Kafka;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dropship.Ofs.Processor.Models
{
    public class OrderMessagingSettingsHostSettings : ProviderHostSettings
    {
        public OrderMessagingSettingsHostSettings(IOptions<MessagingProviderSettings> settings)
        {
            this.SetServer(settings.Value.Server);
        }        
    }
}
