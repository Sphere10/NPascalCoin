using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NPascalCoin.DTO {

	/// <summary>
	/// Embedded JSON object describing node protocol support
	/// </summary>
	public class NetStatsDTO {

		[JsonProperty("active")]
		public uint Active { get; set; }

		[JsonProperty("clients")]
		public uint Clients { get; set; }

		[JsonProperty("servers")]
		public uint Servers { get; set; }

		[JsonProperty("servers_t")]
		public uint Servers_t { get; set; }

		[JsonProperty("total")]
		public uint Total { get; set; }

		[JsonProperty("tclients")]
		public uint TotalClients { get; set; }

		[JsonProperty("tservers")]
		public uint TotalServers { get; set; }

		[JsonProperty("breceived")]
		public uint BytesReceived { get; set; }

		[JsonProperty("bsend")]
		public uint BytesSent { get; set; }
	}

}
