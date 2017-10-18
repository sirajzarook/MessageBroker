using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AllSaints.NetStandard.JsonHelper;
using NLog;
using AllsaintsMessageLogger;

namespace AllSaints.MessageLogger.NLog
{
    public class MessageLogger : IMessageLogger
    {
        private readonly Microsoft.Extensions.Logging.ILogger logger;

        public MessageLogger(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger("FlowLogger");
        }

        public void LogMessage(EventId eventId, IProcessor processor, ILogMessage message)
        {
            var messageType = message.Message.GetType().ToString();
            var objectAsString = JsonHelperSerialiser.ToJson(message.Message, JsonHelperSerialiserSettings.CamelCaseSerialiseSettings);
            GlobalDiagnosticsContext.Set("entityId", message.Id.ToString());
            GlobalDiagnosticsContext.Set("entityType", messageType);
            GlobalDiagnosticsContext.Set("event", eventId);
            GlobalDiagnosticsContext.Set("processId", processor.Type);
            GlobalDiagnosticsContext.Set("processorId", processor.Id);
            GlobalDiagnosticsContext.Set("processor", String.Format("ConsoleAPP {0}  IP: {1}", processor.HostName, processor.IpAddress));
            GlobalDiagnosticsContext.Set("message", objectAsString);
            GlobalDiagnosticsContext.Set("isTest", message.IsTest ? 1 : 0);
            logger.LogDebug(eventId, $"Flow processing log");
        }
        
    }
}
