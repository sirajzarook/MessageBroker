using System;
using System.Collections.Generic;
using System.Text;

namespace Dropship.Ofs.Processor.Modules
{
    public interface IModulesFactory
    {
        IModule Get(ApplicationMode applicationMode);
    }
}
