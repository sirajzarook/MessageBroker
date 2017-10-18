using System;
using System.Collections.Generic;
using System.Text;

namespace AllSaints.MessageBroker.KafkaPersistanceTests.Moq
{
    public static class TestMessages
    {
        public static TestMessage Simple
        {
            get
            {
                return new TestMessage()
                {
                    DateStampUtc = DateTime.Now.ToUniversalTime(),
                    Id = Guid.NewGuid(),
                    IsTest = true,
                    TestValue = "Hello from test",
                };
            }
        }

        public static List<TestMessage> GenerateMessages(int count)
        {
            var result = new List<TestMessage>(count);
            for (int i = 0; i < count; i++)
            {
                result.Add(Simple);
            }
            return result;
        }
    }
}
