using AllSaints.MessageBroker.Configurations.Abstracts;
using AllSaints.MessageBroker.Models;
using AllsaintsMessageLogger;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllSaints.MessageBroker.Abstracts
{
    public interface IMessage 
    {		
		Guid Id { get; set; }
		DateTime DateStampUtc { get; set; }
        bool IsTest { get; set; }
    }
}
