using AllSaints.MessageBroker.Abstracts;
using AllSaints.MessageBroker.Configurations.Abstracts;
using AllSaints.MessageBroker.Kafka;
using AllSaints.MessageBroker.KafkaTests.Moq;
using AllSaints.MessageBroker.Models;
using AllsaintsMessageLogger;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using Xunit;

namespace AllSaints.MessageBroker.KafkaTests
{
    public class AnnouncerShould
    {
        private readonly string kafkaHost = "10.0.75.1:9094";
        private readonly string produceOnTopic = "test_produce";

        IHostSettings hostSettings;
        IProcessor processor;
        IMessageSettingsOut messageSettings;

        public AnnouncerShould()
        {
            hostSettings = new ProviderHostSettings(kafkaHost);
            messageSettings = new MessageSettingsOut(produceOnTopic);
            processor = new Processor();
        }

        private string announcerError = null;

        [Fact]
        public void SuccessfullyProduceMessage()
        {
            var loggerMock = new Mock<IMessageLogger>();
            loggerMock.Setup(x => x.LogMessage(It.IsAny<EventId>(), It.IsAny<IProcessor>(), It.IsAny<ILogMessage>()));
            
            var announcer = new Announcer(hostSettings, processor, loggerMock.Object);
            announcer.OnError += Announcer_OnError;

            var isSuccess = announcer.Announce(TestMessages.Simple, messageSettings);
            Assert.True(isSuccess,"Announcer should return success on anounce.");
            Assert.Null(announcerError); //Annpuncer should return error.
        }

        [Fact]
        public void LogMessageWithWasSend()
        {
            var loggerMock = new Mock<IMessageLogger>();
            loggerMock.Setup(x => x.LogMessage(It.IsAny<EventId>(), It.IsAny<IProcessor>(), It.IsAny<ILogMessage>())).Verifiable("Announcer should log message with was send.");
            
            var announcer = new Announcer(hostSettings, processor, loggerMock.Object);

            var isSuccess = announcer.Announce(TestMessages.Simple, messageSettings);
            Assert.True(isSuccess, "Announcer should return success on anounce.");
           
            //Wait some time to allow background method add log.
            Thread.Sleep(500); 
            //Check are message log was called
            loggerMock.VerifyAll();
        }

        [Fact]
        public void RaiseOnErrorEventWhenProduceMessageOnNotAvailableMessageQueue()
        {
            var hostSettings = new ProviderHostSettings("notworkinghost:1234");
          
            var loggerMock = new Mock<IMessageLogger>();
            loggerMock.Setup(x => x.LogMessage(It.IsAny<EventId>(), It.IsAny<IProcessor>(), It.IsAny<ILogMessage>()));
                  
            var announcer = new Announcer(hostSettings, processor, loggerMock.Object);
            announcer.OnError += Announcer_OnError;

            var isSuccess = announcer.Announce(TestMessages.Simple, messageSettings);
            Assert.False(isSuccess, "Announcer should return not success on anounce.");
            Assert.NotNull(announcerError); //Annpuncer should return error.
        }

        private void Announcer_OnError(string errorMessage)
        {
            announcerError = errorMessage;
        }
    }
}
