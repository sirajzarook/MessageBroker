using System;
using System.Collections.Generic;
using System.Text;

namespace AllsaintsMessageLogger
{
	public interface IProcessor
	{
		Guid Id { get; set; }
		string HostName { get; set; }
		string IpAddress { get; set; }
        string Type { get; set; }

    }
}
