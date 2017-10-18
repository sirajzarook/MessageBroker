using AllSaints.MessageBroker.Abstracts;
using AllsaintsMessageLogger;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dropship.Ofs.Processor.Models
{
    public class CreateOrderMessage : IMessage
    {
        public int OrderId { get; set; }
        public string OrderName { get; set; }
        public Guid Id { get; set; }
        public bool IsTest { get; set; }
        public IMessageSettingsIn InMessageSettings { get; set; }
        public DateTime DateStampUtc { get; set; }
    }
}
