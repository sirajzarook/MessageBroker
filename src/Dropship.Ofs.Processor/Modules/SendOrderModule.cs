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
    public class SendOrderModule : IModule
    {
        private readonly ApplicationMode applicationMode = ApplicationMode.SEND_ORDER;
        private readonly IListener listener;
        private readonly IAnnouncer announcer;
        private readonly ILogger<SendOrderModule> logger;
        private readonly ModuleSettings settings;

        public SendOrderModule( ModuleSettings settings,
                                ILogger<SendOrderModule> logger,
                                IListener listener, 
                                IAnnouncer announcer)
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
            Console.WriteLine($"Finished processing in:  {settings.ListenOn}");
        }
        public void Dispose()
        {
            listener.Dispose();
            announcer.Dispose();
        }
    }
}
