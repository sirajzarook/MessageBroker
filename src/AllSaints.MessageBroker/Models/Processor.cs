using AllSaints.MessageBroker.Abstracts;
using AllsaintsMessageLogger;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AllSaints.MessageBroker.Models
{
	public class Processor : IProcessor
	{
		public Guid Id { get; set; }
		public string HostName { get; set; }
		public string IpAddress { get; set; }
		public DateTime StartedUtc { get; set; }
		public DateTime? StoppedUtc { get; set; }
        public string Type { get; set; }

        public Processor()
		{
			Id = Guid.NewGuid();
			HostName = Environment.MachineName;
			IpAddress = GetIpAddress();
			StartedUtc = DateTime.UtcNow;
		}

		private string GetIpAddress()
		{
			var host = Dns.GetHostEntryAsync(Dns.GetHostName());
			host.Wait();
			foreach (var ip in host.Result.AddressList)
			{
				if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
				{
					return ip.ToString();
				}
			}
			return null;
		}

	}
}
