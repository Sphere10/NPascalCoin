using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NPascalCoin.DTO {

	/// <summary>
	/// A "Connection object" is a JSON object with a connection to other node information.
	/// </summary>
	public class ConnectionDTO : PascalCoinDTO {

		/// <summary>
		/// True if this connection is to a server node.False if this connection is a client node
		/// </summary>
		[JsonProperty("server")]
		public bool IsServer { get; set; }

		/// <summary>
		/// IP
		/// </summary>
		[JsonProperty("ip")]
		public string IP { get; set; }

		/// <summary>
		/// Port
		/// </summary>
		[JsonProperty("port")]
		public int Port { get; set; }

		/// <summary>
		/// seconds of live of this connection
		/// </summary>
		[JsonProperty("secs")]
		public uint ConnectedDurationSec { get; set; }

		/// <summary>
		/// Bytes sent
		/// </summary>
		[JsonProperty("sent")]
		public uint BytesSent { get; set; }

		/// <summary>
		/// Bytes received
		/// </summary>
		[JsonProperty("recv")]
		public int BytesReceived { get; set; }

		/// <summary>
		/// Other node App version
		/// </summary>
		[JsonProperty("appver")]
		public string AppVersion { get; set; }

		/// <summary>
		/// Net protocol of other node
		/// </summary>
		[JsonProperty("netvar")]
		public uint RemoteVersion { get; set; }

		/// <summary>
		/// Net protocol available of other node
		/// </summary>
		[JsonProperty("netvar_a")]
		public uint RemoveAvailableVersion { get; set; }

		/// <summary>
		/// Net timediff of other node (vs wallet)
		/// </summary>
		[JsonProperty("timediff")]
		public int TimeDiff { get; set; }
	}

}
