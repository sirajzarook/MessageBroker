using System;
using System.Collections.Generic;
using System.Text;

namespace Dropship.Ofs.Processor.Models
{
    public class ModuleSettings
    {
        public Guid AppId { get; set; }
        public string ListenOn { get; set; }
        public string SendTo { get; set; }
    }
}
