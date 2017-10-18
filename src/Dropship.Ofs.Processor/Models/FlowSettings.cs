using System;
using System.Collections.Generic;
using System.Text;

namespace Dropship.Ofs.Processor.Models
{
    public class FlowSettings
    {
        public ModuleSettings CheckIn { get; set; }
        public ModuleSettings SendOrder { get; set; }
        public ModuleSettings UpdateAddress { get; set; }
    }
}
