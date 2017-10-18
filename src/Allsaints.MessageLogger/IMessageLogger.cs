
using Microsoft.Extensions.Logging;
using System;

namespace AllsaintsMessageLogger
{
    public interface IMessageLogger
    {
        void LogMessage(EventId eventId, IProcessor processor, ILogMessage message);
    }
}