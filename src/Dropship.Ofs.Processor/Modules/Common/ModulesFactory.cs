using AllSaints.MessageBroker.Abstracts;
using AllsaintsMessageLogger;
using Dropship.Ofs.Processor.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dropship.Ofs.Processor.Modules
{
    public class ModulesFactory : IModulesFactory
    {
        private readonly IListener listener;
        private readonly IAnnouncer announcer;
        private readonly FlowSettings settings;
        private readonly ILoggerFactory loggerFactory;
        

        public ModulesFactory(IOptions<FlowSettings> settings, ILoggerFactory loggerFactory, IListener listener, IAnnouncer announcer)
        {
            this.listener = listener;
            this.announcer = announcer;
            this.settings = settings.Value;
            this.loggerFactory = loggerFactory;
        }

        //TODO: Think aboud use AutoFac IoC. To avouid manual factory creating.
        public IModule Get(ApplicationMode applicationMode)
        {
            switch (applicationMode)
            {
                case ApplicationMode.CHECK_IN:
                    {
                        return new CheckInModule(settings.CheckIn, loggerFactory.CreateLogger<CheckInModule>(), 
                                                  listener, announcer);
                    }
                case ApplicationMode.UPDATE_ADDRESS:
                    {
                        return new UpdateAddressModule(settings.UpdateAddress, loggerFactory.CreateLogger<UpdateAddressModule>(),                                            listener, announcer);
                    }
                case ApplicationMode.SEND_ORDER:
                    {
                        return new SendOrderModule(settings.SendOrder, loggerFactory.CreateLogger<SendOrderModule>(), 
                                                   listener, announcer);
                    }
                default:
                    {
                        //TODO: think about better solution
                        throw new NotImplementedException("Not implemented applicatiom module");
                    }
            }
        }
    }
}
