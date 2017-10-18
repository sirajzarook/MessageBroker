using System;

namespace AllsaintsMessageLogger
{
    public interface ILogMessage
    {
        Guid Id { get; set; }
        DateTime DateStampUtc { get; set; }
        bool IsTest { get; set; }
        object Message { get; set; }
    }
}
