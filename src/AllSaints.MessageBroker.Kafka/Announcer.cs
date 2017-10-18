using AllSaints.MessageBroker.Abstracts;
using AllSaints.MessageBroker.Configurations.Abstracts;
using AllSaints.MessageBroker.Models;
using AllSaints.NetStandard.JsonHelper;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using AllSaints.MessageBroker.Delegates;
using static AllSaints.MessageBroker.Delegates.MessengerDelegates;
using AllsaintsMessageLogger;
using System.Threading.Tasks;

namespace AllSaints.MessageBroker.Kafka
{

    public class Announcer : IAnnouncer
    {
        private Producer<string, string> producer;
        public event OnError OnError;

        private readonly IHostSettings _hostSettings;
        private readonly IProcessor _processor;
        private readonly IMessageLogger _messageLogger;

        public Announcer(IHostSettings hostSettings, IProcessor processor, IMessageLogger messageLogger)
        {
            _hostSettings = hostSettings;
            _processor = processor;
            _messageLogger = messageLogger;

            var config = new Dictionary<string, object> {
                //No't expect many brokers, so config contains only one.
                { "bootstrap.servers", _hostSettings.HostSettings["server"] },
                { "acks", _hostSettings.HostSettings["acks"] },
            };
            producer = new Producer<string, string>(config, new StringSerializer(Encoding.UTF8), new StringSerializer(Encoding.UTF8));
            producer.OnError += Producer_OnError;
        }

        public bool Announce(IMessage message, IMessageSettingsOut messageSettings)
        {
            _processor.Type = messageSettings.MessageSettings["produce.ontopic"];
            var valueToSend = JsonHelperSerialiser.ToJson(message, JsonHelperSerialiserSettings.CamelCaseSerialiseSettings);

            var deliveryReport = producer.ProduceAsync(messageSettings.MessageSettings["produce.ontopic"], null, valueToSend);

            var result = deliveryReport.ConfigureAwait(false).GetAwaiter().GetResult(); // synchronously waits for message to be produced. 

            if (result.IsSuccess())
            {
                producer.Flush(TimeSpan.FromSeconds(10));

                var messageToLog = new AllsaintsMessageLogger.LogMessage()
                {
                    Id = message.Id,
                    DateStampUtc = message.DateStampUtc,
                    IsTest = message.IsTest,
                    Message = message,
                };
                _messageLogger.LogMessage(EventTypes.Sent, _processor, messageToLog);
                return true;
            }
            else
            {
                OnError(result.Error.ToString());
                return false;
            }

        }

        private void Producer_OnError(object sender, Error e)
        {
            OnError(e.ToString());
        }

        public void Dispose()
        {
            producer.Dispose();
        }
    }
}
