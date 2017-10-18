using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AllSaints.NetStandard.JsonHelper;
using NLog;
using Dropship.Ofs.Processor.Models;
using AllSaints.MessageBroker.Abstracts;

namespace Dropship.Ofs.Processor.Common
{
    public class MessageLogger : IMessageLogger
    {
        private readonly Microsoft.Extensions.Logging.ILogger logger;
        public MessageLogger(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger("FlowLogger");
        }
      
        public void LogFlow(string appID, int processId, IMessage message, string eventType, EntityTypeEnum messageType)
        {
            var messageTypeTest = message.GetType();
            var objectAsString = JsonHelperSerialiser.ToJson(message, JsonHelperSerialiserSettings.CamelCaseSerialiseSettings);
            GlobalDiagnosticsContext.Set("entityId", message.Id.ToString());
            GlobalDiagnosticsContext.Set("entityType", (int)messageType);
            GlobalDiagnosticsContext.Set("event", eventType);
            GlobalDiagnosticsContext.Set("processId", processId);
            GlobalDiagnosticsContext.Set("processorId", "ConsoleAPP " + appID);
            GlobalDiagnosticsContext.Set("message", objectAsString);
            GlobalDiagnosticsContext.Set("isTest", message.IsTest ? 1 : 0);
            logger.LogDebug($"Flow processing log");
        }

        public void LogFlow(Guid appID, ApplicationMode applicationMode, IMessage message, string eventType, EntityTypeEnum messageType)
        {
            this.LogFlow(appID.ToString(),(int)applicationMode, message, eventType, messageType);
        }
            
        public void LogFlowOrderReceived(Guid appID, ApplicationMode applicationMode, IMessage message)
        {
            this.LogFlow(appID.ToString(), (int)applicationMode, message, EventTypeEnum.RECEIVED.ToName(),EntityTypeEnum.ORDER);
        }

        public void LogFlowOrderProcessed(Guid appID, ApplicationMode applicationMode, IMessage message)
        {
            this.LogFlow(appID.ToString(), (int)applicationMode, message, EventTypeEnum.SENT.ToName(), EntityTypeEnum.ORDER);
        }

        public void LogFlowOrderItemReceived(Guid appID, ApplicationMode applicationMode, IMessage message)
        {
            this.LogFlow(appID.ToString(), (int)applicationMode, message, EventTypeEnum.RECEIVED.ToName(), EntityTypeEnum.ORDER_ITEM);
        }

        public void LogFlowOrderItemProcessed(Guid appID, ApplicationMode applicationMode, IMessage message)
        {
            this.LogFlow(appID.ToString(), (int)applicationMode, message, EventTypeEnum.SENT.ToName(), EntityTypeEnum.ORDER_ITEM);
        }
    }
}
