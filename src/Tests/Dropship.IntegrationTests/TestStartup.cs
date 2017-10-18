using AllSaints.MessageBroker.Abstracts;
using AllSaints.MessageBroker.Configurations.Abstracts;
using AllSaints.MessageLogger.NLog;
using AllsaintsMessageLogger;
using Dropship.Ofs.Processor;
using Dropship.Ofs.Processor.Models;
using Dropship.Ofs.Processor.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dropship.IntegrationTests
{
    public class TestStartup : Startup
    {
        public IMessage Message { get; set; }
        public string Topic { get; set; }

        event OnMessageReceived TestSendMessage;
        public TestStartup(ApplicationMode applicationMode) : base(applicationMode)
        {
        }

        protected override void ConfigureDependencies()
        {
            serviceProvider = new ServiceCollection()
               .AddLogging(config =>
               {
                   config.AddConfiguration(Configuration.GetSection("Logging"));
               })
               .AddOptions()
               .Configure<MessagingProviderSettings>(x => Configuration.GetSection("MessagingProviderSettings").Bind(x))
               .Configure<FlowSettings>(x => Configuration.GetSection("FlowSettings").Bind(x))
               .AddScoped<IHostSettings, OrderMessagingSettingsHostSettings>()
               .AddScoped<IAnnouncer, FakeAnnouncer>()
               .AddScoped<IListener, FakeListener>()
               .AddScoped<IModulesFactory, ModulesFactory>()
               .AddScoped<IMessageLogger, MessageLogger>()
               .AddScoped<FakeMessageBroker, FakeMessageBroker>()
               .BuildServiceProvider();

            var broker = serviceProvider.GetService<FakeMessageBroker>();
            broker.OnMessageSending += Broker_TestSendMessage;
        }

        private void Broker_TestSendMessage(IMessage message, IMessageSettings messageSettings)
        {
            Message = message;
            Topic = messageSettings.MessageSettings["produce.ontopic"];
            Console.WriteLine("Announcer sending fake message");
        }

        public void SendFakeMessage(IMessage message)
        {
            var announcer =(FakeListener)serviceProvider.GetService<IListener>();
            announcer.GenerateFakeEvent(message);
        }
    }
}
