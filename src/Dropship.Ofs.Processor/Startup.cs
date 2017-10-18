using Dropship.Ofs.Processor.Models;
using Dropship.Ofs.Processor.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NLog;
using NLog.Extensions.Logging;
using AllSaints.MessageBroker.Kafka;
using AllSaints.MessageBroker.Abstracts;
using AllSaints.MessageBroker.Configurations.Abstracts;
using AllsaintsMessageLogger;
using AllSaints.MessageLogger.NLog;

namespace Dropship.Ofs.Processor
{
    public class Startup
    {
        protected IConfigurationRoot Configuration { get; }
        protected readonly ApplicationMode applicationMode;
        protected IServiceProvider serviceProvider;

        public Startup(ApplicationMode applicationMode)
        {
            this.applicationMode = applicationMode;

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();

            ConfigureDependencies();

        }

        protected virtual void ConfigureDependencies()
        {
            serviceProvider = new ServiceCollection()
                .AddLogging(config =>
                {
                    config.AddConfiguration(Configuration.GetSection("Logging"));
                })
                .AddOptions()
                .Configure<MessagingProviderSettings>(x => Configuration.GetSection("MessagingProviderSettings").Bind(x))
                .Configure<FlowSettings>(x => Configuration.GetSection("FlowSettings").Bind(x))
                .AddTransient<IHostSettings, OrderMessagingSettingsHostSettings>()
                .AddTransient<IAnnouncer, Announcer>()
                .AddTransient<IListener, Listener<CreateOrderMessage>>()
                .AddTransient<IModulesFactory, ModulesFactory>()
                .AddTransient<IMessageLogger, MessageLogger>()
                .AddTransient<IMessageSettingsIn, MessageSettingsIn>()
                .AddTransient<IMessageSettingsOut, MessageSettingsOut>()
                .AddTransient<IProcessor, AllSaints.MessageBroker.Models.Processor>()                
                .BuildServiceProvider();
            

            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            loggerFactory.AddNLog();
            loggerFactory.ConfigureNLog("nlog.config");
        }

        public void Start()
        {
            var modulesFacrory = serviceProvider.GetService<IModulesFactory>();

            using (var module = modulesFacrory.Get(applicationMode))
            {
                module.Start();
            }

        }

        //Only for tests! Remove me
        public void ProduceFakeTestMessage()
        {
            using (var anouncer = serviceProvider.GetService<IAnnouncer>())
            {
                while (true)
                {
                    var text = Console.ReadLine();
                    var message = new CreateOrderMessage()
                    {
                        Id = Guid.NewGuid(),
                        OrderId = 5425212,
                        OrderName = text,
                        IsTest = true,
                    };
                    // for (int i = 0; i < 5; i++)
                    {
                        var messageSendSettings = new MessageSettingsOut(ApplicationMode.CHECK_IN.ToName());
                        anouncer.Announce(message, messageSendSettings);
                    }
                }
            }
        }
    }
}
