using AllSaints.MessageBroker.Abstracts;
using Dropship.Ofs.Processor.Models;
using System;

namespace Dropship.Ofs.Processor.Common
{
    public interface IMessageLogger
    {
        void LogFlow(string appID, int processId, IMessage message, string eventType, EntityTypeEnum entityType);
        void LogFlow(Guid appID, ApplicationMode applicationMode, IMessage message, string eventType, EntityTypeEnum entityType);
        void LogFlowOrderReceived(Guid appID, ApplicationMode applicationMode, IMessage message);
        void LogFlowOrderProcessed(Guid appID, ApplicationMode applicationMode, IMessage message);
        void LogFlowOrderItemReceived(Guid appID, ApplicationMode applicationMode, IMessage message);
        void LogFlowOrderItemProcessed(Guid appID, ApplicationMode applicationMode, IMessage message);
    }
}