using AllsaintsMessageLogger;
using System;

namespace AllsaintsMessageLogger
{
    public class LogMessage : ILogMessage
    {
        public Guid Id { get; set; }
        public DateTime DateStampUtc { get; set; }
        public bool IsTest { get; set; }
        public object Message { get; set; }
    }
}
