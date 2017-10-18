using AllSaints.MessageBroker.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;
using AllSaints.MessageBroker.Configurations.Abstracts;

namespace AllSaints.MessageBroker.Models
{
	public class Message : IMessage
	{
		public Guid Id { get; set; }
		public DateTime DateStampUtc { get; set; }
        public bool IsTest { get; set; }
    }
}
