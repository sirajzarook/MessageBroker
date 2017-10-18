using System;
using System.Collections.Generic;
using System.Text;

namespace AllSaints.MessageBroker.KafkaTests.Moq
{
    public static class TestMessages
    {
        public static TestMessage Simple
        {
            get
            {
                return new TestMessage()
                {
                    DateStampUtc = DateTime.Now,
                    Id = Guid.NewGuid(),
                    IsTest = true,
                    TestValue = "Hello from test",
                };
            }
        }
    }
}
