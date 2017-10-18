using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AllSaints.NetStandard.JsonHelper;
using Microsoft.Extensions.Options;
using Dropship.Ofs.Processor.Models;
using Microsoft.Extensions.Logging;
using AllSaints.MessageBroker.Abstracts;
using AllSaints.MessageBroker.Kafka;

namespace Dropship.Ofs.Processor.Modules
{
    public class CheckInModule : IModule
    {
        private readonly ApplicationMode applicationMode = ApplicationMode.CHECK_IN;
        private readonly IListener listener;
        private readonly IAnnouncer announcer;
        private readonly ILogger<CheckInModule> logger;
        private readonly ModuleSettings settings;
        
        public CheckInModule(ModuleSettings settings, ILogger<CheckInModule> logger, IListener listener, IAnnouncer announcer)
        {
            this.listener = listener;
            this.announcer = announcer;
            this.logger = logger;
            this.settings = settings;
        }

        public void Start()
        {
            var messageListeningSettings = new MessageSettingsIn(settings.ListenOn);
            listener.OnMessage += Listener_OnMessageReceive;
            listener.StartListening(messageListeningSettings);
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
