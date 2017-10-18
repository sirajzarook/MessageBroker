
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AllSaints.NetStandard.JsonHelper;
using Microsoft.Extensions.Logging;
using Dropship.Ofs.Processor.Models;
using AllSaints.MessageBroker.Abstracts;
using AllSaints.MessageBroker.Kafka;

namespace Dropship.Ofs.Processor.Modules
{
    public class UpdateAddressModule : IModule
    {
        private readonly ApplicationMode applicationMode = ApplicationMode.UPDATE_ADDRESS;
        private readonly string ListeningOn = ApplicationMode.UPDATE_ADDRESS.ToName();
        private readonly IListener listener;
        private readonly IAnnouncer announcer;
        private readonly ILogger<UpdateAddressModule> logger;
        private readonly ModuleSettings settings;


        public UpdateAddressModule(ModuleSettings settings,ILogger<UpdateAddressModule> logger, IListener listener, IAnnouncer announcer)
        {
            this.listener = listener;
            this.announcer = announcer;
            this.logger = logger;
            this.settings = settings;
        }
        

        public void Start()
        {
            var messageSendSettings = new MessageSettingsOut(settings.SendTo);
            listener.OnMessage += Listener_OnMessageReceive;
            listener.StartListening(messageSendSettings);
        }

        private void Listener_OnMessageReceive(IMessage message)
        {           
            var messageSendSettings = new MessageSettingsOut(settings.SendTo);
            announcer.Announce(message, messageSendSettings);            
        }

        public void Dispose()
        {
            listener.Dispose();
            announcer.Dispose();
        }
    }
}
