using AllSaints.MessageBroker.Abstracts;
using AllSaints.MessageBroker.Configurations.Abstracts;
using AllSaints.MessageBroker.Kafka;
using AllSaints.MessageBroker.KafkaPersistanceTests.Moq;
using AllSaints.MessageBroker.Models;
using AllsaintsMessageLogger;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllSaints.MessageBroker.KafkaPersistanceTests
{

    /// <summary>
    /// Warning !! long running tests.
    /// Message Broker integrations and messages persistance tests
    /// </summary>
    public class MessageBrokerShould
    {
        private readonly string kafkaHost = "10.0.75.1:9094";

        IHostSettings hostSettings;
        IProcessor processor;

        private string listenerError = null;
        private IMessageLogger messageLogger;
        private CancellationTokenSource listenerTimeoutCancelationToken = new CancellationTokenSource();


        private List<TestMessage> testMessages;
        private List<TestMessage> receivedMessages;
        private int maxMessagesToReadInSingleListener;
        private int consumedMessages =0;

        public MessageBrokerShould()
        {
            hostSettings = new ProviderHostSettings(kafkaHost);
            processor = new Processor();

            var loggerMock = new Mock<IMessageLogger>();
            loggerMock.Setup(x => x.LogMessage(It.IsAny<EventId>(), It.IsAny<IProcessor>(), It.IsAny<ILogMessage>()));
            this.messageLogger = loggerMock.Object;

            testMessages = TestMessages.GenerateMessages(4);
            receivedMessages = new List<TestMessage>();
            maxMessagesToReadInSingleListener = testMessages.Count;
        }


        [Fact]
        public void ReceiveAllMessages_1()
        {
            generateTestMessages(6);
            var queueName = "queueone";
            var messageSettings = new MessageSettingsIn(queueName);
            using (var listener = new Listener<TestMessage>(hostSettings, processor, messageLogger))
            {
                listener.OnError += OnError;
                listener.OnMessage += Listener_OnMessage;

                //Start listener
                foreach (var message in testMessages)
                {
                    SendTestMessageToQueue(message, queueName);
                }

                var task = Task.Run(() => listener.StartListening(messageSettings, listenerTimeoutCancelationToken));

                var dealy = new Task(() => { while (!listenerTimeoutCancelationToken.IsCancellationRequested) Thread.Sleep(100); });
                dealy.Start();
                dealy.Wait(30 * 1000); //Wait 3 minutes on messages 
                listenerTimeoutCancelationToken.Cancel();
                Thread.Sleep(1000);
            }

            foreach (var sent in testMessages)
            {
                var received = receivedMessages.SingleOrDefault(x => x.Id == sent.Id);

                Assert.NotNull(received);
                Assert.Equal(sent.Id, received.Id);
                Assert.Equal(sent.IsTest, received.IsTest);
                Assert.Equal(sent.TestValue, received.TestValue);
            }

        }

  
        [Fact]
        public void ReceiveAllMessages_2()
        {
            generateTestMessages(6);
            var randint = new Random().Next();
            var queueName = "queuetwo";
            var messageSettings = new MessageSettingsIn(queueName);
            using (var listener = new Listener<TestMessage>(hostSettings, processor, messageLogger))
            {
                listener.OnError += OnError;
                listener.OnMessage += Listener_OnMessage;

                //Start listener
                foreach (var message in testMessages)
                {
                    SendTestMessageToQueue(message, queueName);
                    Thread.Sleep(1000);
                }

                Thread.Sleep(1 * 1000);

                var task = Task.Run(() => listener.StartListening(messageSettings, listenerTimeoutCancelationToken));

                var dealy = new Task(() => { while (!listenerTimeoutCancelationToken.IsCancellationRequested) Thread.Sleep(200); });
                dealy.Start();
                dealy.Wait(180 * 1000); //Wait 3 minutes on messages 
                listenerTimeoutCancelationToken.Cancel(false);
            }

            foreach (var sent in testMessages)
            {
                var received = receivedMessages.SingleOrDefault(x => x.Id == sent.Id);

                Assert.NotNull(received);
                Assert.Equal(sent.Id, received.Id);
                Assert.Equal(sent.IsTest, received.IsTest);
                Assert.Equal(sent.TestValue, received.TestValue);
            }

        }

        //Warning!! - long running test
        //Wait a little bit longer after messages send. 
        [Fact]
        public void ReceiveAllMessages_3()
        {
            generateTestMessages(6);
            var randint = new Random().Next();
            var queueName = "queuethree";
            var messageSettings = new MessageSettingsIn(queueName);
            using (var listener = new Listener<TestMessage>(hostSettings, processor, messageLogger))
            {
                listener.OnError += OnError;
                listener.OnMessage += Listener_OnMessage;

                //Start listener
                foreach (var message in testMessages)
                {
                    SendTestMessageToQueue(message, queueName);
                    Thread.Sleep(1000);
                }

                Thread.Sleep(1 * 60 * 1000); //One minute

                var task = Task.Run(() => listener.StartListening(messageSettings, listenerTimeoutCancelationToken));

                var dealy = new Task(() => { while (!listenerTimeoutCancelationToken.IsCancellationRequested) Thread.Sleep(200); });
                dealy.Start();
                dealy.Wait(180 * 1000); //Wait 3 minutes on messages 
                listenerTimeoutCancelationToken.Cancel(false);
            }

            foreach (var sent in testMessages)
            {
                var received = receivedMessages.SingleOrDefault(x => x.Id == sent.Id);

                Assert.NotNull(received);
                Assert.Equal(sent.Id, received.Id);
                Assert.Equal(sent.IsTest, received.IsTest);
                Assert.Equal(sent.TestValue, received.TestValue);
            }

        }


        //Warning!! - long running test
        //Wait a little bit longer after messages send. 
        [Fact]
        public void ReceiveAllMessages_4()
        {
            generateTestMessages(6);
            var randint = new Random().Next();
            var queueName = "queuethree";
            var messageSettings = new MessageSettingsIn(queueName);
            using (var listener = new Listener<TestMessage>(hostSettings, processor, messageLogger))
            {
                listener.OnError += OnError;
                listener.OnMessage += Listener_OnMessage;

                //Start listener
                foreach (var message in testMessages)
                {
                    SendTestMessageToQueue(message, queueName);
                    Thread.Sleep(10 * 1000); //Wait 10 seconds between messages announce
                }

                Thread.Sleep(10 * 60 * 1000); //10 minute's wait

                var task = Task.Run(() => listener.StartListening(messageSettings, listenerTimeoutCancelationToken));

                var dealy = new Task(() => { while (!listenerTimeoutCancelationToken.IsCancellationRequested) Thread.Sleep(200); });
                dealy.Start();
                dealy.Wait(180 * 1000); //Wait 3 minutes on messages 
                listenerTimeoutCancelationToken.Cancel(false);
            }

            foreach (var sent in testMessages)
            {
                var received = receivedMessages.SingleOrDefault(x => x.Id == sent.Id);

                Assert.NotNull(received);
                Assert.Equal(sent.Id, received.Id);
                Assert.Equal(sent.IsTest, received.IsTest);
                Assert.Equal(sent.TestValue, received.TestValue);
            }

        }

        //Warning!! - long running test
        //Wait a little bit longer after messages send. 
        [Fact]
        public void ReceiveAllMessages_5()
        {
            generateTestMessages(6);
            var randint = new Random().Next();
            var queueName = "queuethree";
            var messageSettings = new MessageSettingsIn(queueName);
            using (var listener = new Listener<TestMessage>(hostSettings, processor, messageLogger))
            {
                listener.OnError += OnError;
                listener.OnMessage += Listener_OnMessage;

                //Start listener
                foreach (var message in testMessages)
                {
                    SendTestMessageToQueue(message, queueName);
                    Thread.Sleep(5 * 1000); //Wait 5 seconds between messages announce
                }

                Thread.Sleep(20 * 60 * 1000); //20 minute's wait

                var task = Task.Run(() => listener.StartListening(messageSettings, listenerTimeoutCancelationToken));

                var dealy = new Task(() => { while (!listenerTimeoutCancelationToken.IsCancellationRequested) Thread.Sleep(200); });
                dealy.Start();
                dealy.Wait(180 * 1000); //Wait 3 minutes on messages 
                listenerTimeoutCancelationToken.Cancel(false);
            }

            foreach (var sent in testMessages)
            {
                var received = receivedMessages.SingleOrDefault(x => x.Id == sent.Id);

                Assert.NotNull(received);
                Assert.Equal(sent.Id, received.Id);
                Assert.Equal(sent.IsTest, received.IsTest);
                Assert.Equal(sent.TestValue, received.TestValue);
            }

        }


        //Warning!! - long running test
        //Wait a little bit longer after messages send. 
        [Fact]
        public void ReceiveAllMessages_6()
        {
            generateTestMessages(200);
            var randint = new Random().Next();
            var queueName = "queuethree";
            var messageSettings = new MessageSettingsIn(queueName);
            using (var listener = new Listener<TestMessage>(hostSettings, processor, messageLogger))
            {
                listener.OnError += OnError;
                listener.OnMessage += Listener_OnMessage;

                //Start listener

                SendTestMessageToQueue(testMessages, queueName);

                Thread.Sleep(1 * 60 * 1000); //1 minute's wait

                var task = Task.Run(() => listener.StartListening(messageSettings, listenerTimeoutCancelationToken));

                var dealy = new Task(() => { while (!listenerTimeoutCancelationToken.IsCancellationRequested) Thread.Sleep(200); });
                dealy.Start();
                dealy.Wait(180 * 1000); //Wait 3 minutes on messages 
                listenerTimeoutCancelationToken.Cancel(false);
            }

            foreach (var sent in testMessages)
            {
                var received = receivedMessages.SingleOrDefault(x => x.Id == sent.Id);

                Assert.NotNull(received);
                Assert.Equal(sent.Id, received.Id);
                Assert.Equal(sent.IsTest, received.IsTest);
                Assert.Equal(sent.TestValue, received.TestValue);
            }

        }


        //Warning!! - long running test
        //Wait a little bit longer after messages send. 
        [Fact]
        public void ReceiveAllMessages_7()
        {
            generateTestMessages(1000);
            var randint = new Random().Next();
            var queueName = "queuethree";
            var messageSettings = new MessageSettingsIn(queueName);
            using (var listener = new Listener<TestMessage>(hostSettings, processor, messageLogger))
            {
                listener.OnError += OnError;
                listener.OnMessage += Listener_OnMessage;

                //Start listener            
                var task = Task.Run(() => listener.StartListening(messageSettings, listenerTimeoutCancelationToken));

                SendTestMessageToQueue(testMessages, queueName);

                var dealy = new Task(() => { while (!listenerTimeoutCancelationToken.IsCancellationRequested) Thread.Sleep(200); });
                dealy.Start();
                dealy.Wait(180 * 1000); //Wait 3 minutes on messages 
                listenerTimeoutCancelationToken.Cancel(false);
            }

            foreach (var sent in testMessages)
            {
                var received = receivedMessages.SingleOrDefault(x => x.Id == sent.Id);

                Assert.NotNull(received);
                Assert.Equal(sent.Id, received.Id);
                Assert.Equal(sent.IsTest, received.IsTest);
                Assert.Equal(sent.TestValue, received.TestValue);
            }

        }



        //Warning!! - long running test
        //Wait a little bit longer after messages send. 
        [Fact]
        public void ReceiveMessagesOnlyOnce()
        {
            generateTestMessages(10);
            maxMessagesToReadInSingleListener = 5;
            
            var randint = new Random().Next();
            var queueName = "queuethree";
            var messageSettings = new MessageSettingsIn(queueName);
            
            using (var listener = new Listener<TestMessage>(hostSettings, processor, messageLogger))
            {
                listener.OnError += OnError;
                listener.OnMessage += Listener_OnMessage;
                           
                SendTestMessageToQueue(testMessages, queueName);

                var task = Task.Run(() => listener.StartListening(messageSettings, listenerTimeoutCancelationToken));

                var dealy = new Task(() => { while (!listenerTimeoutCancelationToken.IsCancellationRequested) Thread.Sleep(200); });
                dealy.Start();
                dealy.Wait(180 * 1000); //Wait 3 minutes on messages 
                listenerTimeoutCancelationToken.Cancel(false);
            }

            consumedMessages = 0;
            listenerTimeoutCancelationToken = new CancellationTokenSource();

            //Start listener once again. To check are we read messages only once by listener. 
            using (var listener = new Listener<TestMessage>(hostSettings, processor, messageLogger))
            {
                listener.OnError += OnError;
                listener.OnMessage += Listener_OnMessage;
                             
                var task = Task.Run(() => listener.StartListening(messageSettings, listenerTimeoutCancelationToken));

                var dealy = new Task(() => { while (!listenerTimeoutCancelationToken.IsCancellationRequested) Thread.Sleep(200); });
                dealy.Start();
                dealy.Wait(60 * 1000); //Wait 60 seconds on messages 
                listenerTimeoutCancelationToken.Cancel(false);
            }

            Assert.Equal(testMessages.Count, receivedMessages.Count);

            foreach (var sent in testMessages)
            {
                var received = receivedMessages.SingleOrDefault(x => x.Id == sent.Id);

                Assert.NotNull(received);
                Assert.Equal(sent.Id, received.Id);
                Assert.Equal(sent.IsTest, received.IsTest);
                Assert.Equal(sent.TestValue, received.TestValue);
            }

        }

        private void Listener_OnMessage(IMessage message)
        {
            var received = (TestMessage)message;
            var receivedIsInSent = testMessages.Where(x => x.Id == received.Id).Any();
            if (receivedIsInSent)
            {
                consumedMessages++;
                receivedMessages.Add(received);
                if (consumedMessages >= maxMessagesToReadInSingleListener)
                    listenerTimeoutCancelationToken.Cancel(false);
            }


            if (receivedMessages.Count == testMessages.Count)
            {
                listenerTimeoutCancelationToken.Cancel(false);
            }
        }

        private void OnError(string errorMessage)
        {
            Assert.False(true);
        }


        private void SendTestMessageToQueue(TestMessage testMessage, string queue)
        {
            var processor = new Processor();
            var messageSettings = new MessageSettingsOut(queue);

            var loggerMock = new Mock<IMessageLogger>();
            loggerMock.Setup(x => x.LogMessage(It.IsAny<EventId>(), It.IsAny<IProcessor>(), It.IsAny<ILogMessage>()));

            using (var announcer = new Announcer(hostSettings, processor, loggerMock.Object))
            {
                announcer.OnError += OnError;
                var isSuccess = announcer.Announce(testMessage, messageSettings);
                if (!isSuccess)
                    OnError("not sended");
            }
        }
        private void generateTestMessages(int number)
        {
            testMessages = TestMessages.GenerateMessages(number);
            maxMessagesToReadInSingleListener = number;
        }

        private void SendTestMessageToQueue(List<TestMessage> testMessages, string queue)
        {
            var processor = new Processor();
            var messageSettings = new MessageSettingsOut(queue);

            var loggerMock = new Mock<IMessageLogger>();
            loggerMock.Setup(x => x.LogMessage(It.IsAny<EventId>(), It.IsAny<IProcessor>(), It.IsAny<ILogMessage>()));

            using (var announcer = new Announcer(hostSettings, processor, loggerMock.Object))
            {
                announcer.OnError += OnError;
                foreach (var message in testMessages)
                {
                    var isSuccess = announcer.Announce(message, messageSettings);
                    if (!isSuccess)
                        OnError("not sended");
                }
            }
        }
    }
}
