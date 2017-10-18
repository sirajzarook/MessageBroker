using AllSaints.MessageBroker.Abstracts;
using AllSaints.MessageBroker.Configurations.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;
using AllSaints.MessageBroker.Models;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using static AllSaints.MessageBroker.Delegates.MessengerDelegates;
using AllSaints.MessageBroker.Delegates;
using AllSaints.NetStandard.JsonHelper;
using AllsaintsMessageLogger;
using System.Threading;

namespace AllSaints.MessageBroker.Kafka
{
	public class Listener<T> : IListener where T: IMessage 
	{
		public event OnError OnError;
		public event OnMessageReceive OnMessage;

		private readonly IHostSettings _hostSettings;
		private readonly IProcessor _processor;
        private readonly IMessageLogger _messageLogger;

        private readonly Consumer<string, string> consumer;


		public IMessage InMessage { get; set; }

		public Listener(IHostSettings hostSettings, IProcessor processor, IMessageLogger messageLogger)
		{
			_hostSettings = hostSettings;
			_processor = processor;
            _messageLogger = messageLogger;


			var config = new Dictionary<string, object> {
                //No't expect many brokers, so config contains only one.
                { "bootstrap.servers",  _hostSettings.HostSettings["server"] },
				{ "group.id", _hostSettings.HostSettings["group.id"] },
				{ "enable.auto.commit", _hostSettings.HostSettings["enable.auto.commit"] == "false" ? false: true },
                //Protect loosing messages in kafka queue.   
                //What to do when there is no initial offset in Kafka or if the current offset does not exist any more on the server (e.g. because that data has been deleted):
                //earliest: automatically reset the offset to the earliest offset
                //https://stackoverflow.com/questions/41875288/kafka-lost-messages 
                { "auto.offset.reset", _hostSettings.HostSettings["auto.offset.reset"] },
			};
			this.consumer = new Consumer<string, string>(config, new StringDeserializer(Encoding.UTF8), new StringDeserializer(Encoding.UTF8));
			this.consumer.OnMessage += Consumer_OnMessage;
            this.consumer.OnError += Consumer_OnError;
            this.consumer.OnConsumeError += Consumer_OnConsumeError;
        }

        private void Consumer_OnConsumeError(object sender, Confluent.Kafka.Message e)
        {
            OnError(e.Error.ToString());
        }

        private void Consumer_OnError(object sender, Error e)
        {
            OnError(e.ToString());
        }


        private void Consumer_OnMessage(object sender, Message<string, string> msg)
		{
			var messageObject = JsonHelperSerialiser.FromJson<T>(msg.Value, JsonHelperSerialiserSettings.CamelCaseSerialiseSettings);
			OnMessage(messageObject);
			var committedOffsets = consumer.CommitAsync(msg).ConfigureAwait(false).GetAwaiter().GetResult(); // synchronously waits for message to be produced. 

            var messageToLog = new AllsaintsMessageLogger.LogMessage()
            {
                Id = messageObject.Id,
                DateStampUtc = messageObject.DateStampUtc,
                IsTest = messageObject.IsTest,
                Message = messageObject,
            };
            _messageLogger.LogMessage(EventTypes.Reveived, _processor, messageToLog);
        }

		public void StartListening(IMessageSettings inMessageSettings, CancellationTokenSource cancellationTokenSource = null)
		{
			consumer.Subscribe(inMessageSettings.MessageSettings["listening.ontopic"]);
            _processor.Type = inMessageSettings.MessageSettings["listening.ontopic"];
            while (!cancellationTokenSource?.IsCancellationRequested ?? true)
			{
				//Default time was 100 milisecond. I change it to 200 to reduce server load
				consumer.Poll(TimeSpan.FromMilliseconds(200));
			}

		}

        public void Dispose()
        {
            consumer.Dispose();
        }
        
    }
}
