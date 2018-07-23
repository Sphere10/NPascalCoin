using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NPascalCoin.Processing {
	public class NodeEndpoint {
		private NodeEndpoint(IPAddress ip, int port) {
			IP = ip;
			Port = port;
		}

		public static NodeEndpoint For(string ipAddress, int? port = null) {
			IPAddress ip;
			if (!IPAddress.TryParse(ipAddress, out ip)) {
				throw new ArgumentException("Malformed IP address", "ipAddress");
			}

			return new NodeEndpoint(ip, port ?? Constants.DefaultMainNetPort);
		}

		public static NodeEndpoint For(IPAddress ipAddress, int? port = null) {
			return new NodeEndpoint(ipAddress, port ?? Constants.DefaultMainNetPort);
		}

		public static bool TryParse(string ipAddress, int? port, out NodeEndpoint nodeEndpoint) {
			nodeEndpoint = null;
			if (string.IsNullOrWhiteSpace(ipAddress))
				return false;
			IPAddress ip;
			if (IPAddress.TryParse(ipAddress, out ip)) {
				nodeEndpoint = NodeEndpoint.For(ip, port);
				return true;
			}

			return false;
		}

		public IPAddress IP { get; set; }

		public int Port { get; set; }
	}
}
