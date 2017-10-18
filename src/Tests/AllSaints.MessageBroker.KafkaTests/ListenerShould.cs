using AllSaints.MessageBroker.Abstracts;
using AllSaints.MessageBroker.Configurations.Abstracts;
using AllSaints.MessageBroker.Kafka;
using AllSaints.MessageBroker.KafkaTests.Moq;
using AllSaints.MessageBroker.Models;
using AllsaintsMessageLogger;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllSaints.MessageBroker.KafkaTests
{
    public class ListenerShould
    {
        private readonly string kafkaHost = "10.0.75.1:9094";
        private readonly string onTopic = "test_topic_receive";

        IHostSettings hostSettings;
        IProcessor processor;
        IMessageSettingsIn messageSettings;

        private string listenerError = null;
        private TestMessage receivedMessage = null;
        private CancellationTokenSource listenerTimeoutCancelationToken = new CancellationTokenSource();

        public ListenerShould()
        {
            hostSettings = new ProviderHostSettings(kafkaHost);
            messageSettings = new MessageSettingsIn(onTopic);
            processor = new Processor();
        }


        [Fact]
        public void SuccessfullyReceiveMessageFromProducer()
        {
            var testMessage = TestMessages.Simple;

            var loggerMock = new Mock<IMessageLogger>();
            loggerMock.Setup(x => x.LogMessage(It.IsAny<EventId>(), It.IsAny<IProcessor>(), It.IsAny<ILogMessage>()));

            var listener = new Listener<TestMessage>(hostSettings, processor, loggerMock.Object);
            listener.OnError += Listener_OnError;
            listener.OnMessage += Listener_OnMessage;

            //Start listener
            var task = Task.Run(() => listener.StartListening(messageSettings, listenerTimeoutCancelationToken));

            SendTestMessageToQueue(testMessage, onTopic);

            var dealy = new Task(() => { while (!listenerTimeoutCancelationToken.IsCancellationRequested) Thread.Sleep(100); });
            dealy.Start();
            dealy.Wait(60 * 1000); //Wait 1 minutes on message 

            Assert.Null(listenerError);
            Assert.NotNull(receivedMessage);
            Assert.Equal(testMessage.Id, receivedMessage.Id);
            Assert.Equal(testMessage.IsTest, receivedMessage.IsTest);
            Assert.Equal(testMessage.TestValue, receivedMessage.TestValue);
        }

        [Fact]
        public void LogMessageWithWasReceivedFromProducer()
        {
            var testMessage = TestMessages.Simple;

            var loggerMock = new Mock<IMessageLogger>();
            loggerMock.Setup(x => x.LogMessage(It.IsAny<EventId>(), It.IsAny<IProcessor>(), It.IsAny<ILogMessage>())).Verifiable("Listener should log message with was received.");

            var listener = new Listener<TestMessage>(hostSettings, processor, loggerMock.Object);
            listener.OnError += Listener_OnError;
            listener.OnMessage += Listener_OnMessage;

            //Start listener
            var task = Task.Run(() => listener.StartListening(messageSettings, listenerTimeoutCancelationToken));

            SendTestMessageToQueue(testMessage, onTopic);

            var dealy = new Task(() => { while (!listenerTimeoutCancelationToken.IsCancellationRequested) Thread.Sleep(100); });
            dealy.Start();
            dealy.Wait(60 * 1000); //Wait 1 minutes on message 

            Thread.Sleep(500); //Wait some time to allow background method add log.
            loggerMock.VerifyAll();
        }

        [Fact]
        public void ErrorOnUnavailableMessageQueue()
        {
            var testMessage = TestMessages.Simple;

            var loggerMock = new Mock<IMessageLogger>();
            loggerMock.Setup(x => x.LogMessage(It.IsAny<EventId>(), It.IsAny<IProcessor>(), It.IsAny<ILogMessage>()));

            var badHostSettings = new ProviderHostSettings("testwrong:1234");
            var listener = new Listener<TestMessage>(badHostSettings, processor, loggerMock.Object);
            listener.OnError += Listener_OnError;
            listener.OnMessage += Listener_OnMessage;

            var task = Task.Run(() => listener.StartListening(messageSettings, listenerTimeoutCancelationToken));

            Thread.Sleep(5 * 1000);

            Assert.NotNull(listenerError);
        }

        private void Listener_OnMessage(IMessage message)
        {
            receivedMessage = (TestMessage)message;
            listenerTimeoutCancelationToken.Cancel(false);
        }

        private void Listener_OnError(string errorMessage)
        {
            this.listenerError = errorMessage;
            listenerTimeoutCancelationToken.Cancel(false);
        }

        private void SendTestMessageToQueue(TestMessage testMessage, string queue)
        {
            var processor = new Processor();
            var messageSettings = new MessageSettingsOut(queue);

            var loggerMock = new Mock<IMessageLogger>();
            loggerMock.Setup(x => x.LogMessage(It.IsAny<EventId>(), It.IsAny<IProcessor>(), It.IsAny<ILogMessage>()));

            var announcer = new Announcer(hostSettings, processor, loggerMock.Object);
            announcer.Announce(testMessage, messageSettings);
        }
    }
}
