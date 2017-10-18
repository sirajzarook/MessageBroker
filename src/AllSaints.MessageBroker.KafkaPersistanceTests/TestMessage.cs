using AllSaints.MessageBroker.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllSaints.MessageBroker.KafkaPersistanceTests
{
    public class TestMessage : IMessage
    {
        public Guid Id { get; set; }
        public string TestValue { get; set; }
        public DateTime DateStampUtc { get; set; }
        public bool IsTest { get; set; }
    }
}
