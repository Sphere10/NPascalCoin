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
	public class NetProtocolDTO {

		/// <summary>
		/// Net protocol version
		/// </summary>
		[JsonProperty("ver")]
		public uint Version { get; set; }

		/// <summary>
		/// Net protocol available
		/// </summary>
		[JsonProperty("ver_a")]
		public uint AvailableVersion { get; set; }
	}

}
