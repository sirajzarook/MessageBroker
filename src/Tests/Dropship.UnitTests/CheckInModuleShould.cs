using AllSaints.MessageBroker.Abstracts;
using AllSaints.MessageBroker.Kafka;
using Dropship.Ofs.Processor;
using Dropship.Ofs.Processor.Models;
using Dropship.Ofs.Processor.Modules;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Dropship.UnitTests
{
    public class CheckInModuleShould
    {
        [Fact]
        public void ReceiveMessageFromListenerAndCallAnnouncer ()
        {
            ModuleSettings setting = new ModuleSettings()
            {
                AppId = Guid.Empty,
                ListenOn = "test",
                SendTo = "test",
            };

            var fakeMessageFormListener = new CreateOrderMessage
            {
                Id = Guid.Empty,
                IsTest = true,
                OrderId = 53462,
                OrderName = "Test order",
            };

            var messageSendSettings = new MessageSettingsOut(setting.SendTo);
            var messageListenSettings = new MessageSettingsIn(setting.ListenOn);

            var loggerMock = new Mock<ILogger<CheckInModule>>();

            var queueListenerMok = new Mock<IListener>();
            queueListenerMok.Setup(x => x.StartListening(messageListenSettings,null));


            var queueAnnouncerMok = new Mock<IAnnouncer>();
            queueAnnouncerMok.Setup(x => x.Announce(fakeMessageFormListener, It.IsAny<IMessageSettingsOut>())).Verifiable("Listener should call anouncer!");
        

            var module = new CheckInModule(setting, loggerMock.Object, queueListenerMok.Object, queueAnnouncerMok.Object);
            module.Start();
            queueListenerMok.Raise(x => x.OnMessage += (test) => { }, fakeMessageFormListener);
            Task.Delay(2000);
            queueAnnouncerMok.VerifyAll();
            //If we are in here it's mean success
            Assert.True(true);

        }

        
    }
}
