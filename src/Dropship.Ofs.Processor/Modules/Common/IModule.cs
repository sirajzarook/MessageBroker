using System;
using System.Collections.Generic;
using System.Text;

namespace Dropship.Ofs.Processor.Modules
{
    public interface IModule : IDisposable
    {
        void Start();
    }
}
