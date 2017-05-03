using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NPascalCoin.DTO {


	/// <summary>
	/// A "Raw operations object" is a JSON object with information about a signed operation made by "signsendto" or "signchangekey"
	/// </summary>
	public class RawOperationDTO {

		/// <summary>
		/// Count how many operations has rawoperations param
		/// </summary>
		[JsonProperty("operations")]
		public uint NumOperations { get; set; }

		/// <summary>
		/// Total amount
		/// </summary>
		[JsonProperty("amount")]
		public decimal TotalAmount { get; set; }

		/// <summary>
		/// Total fee
		/// </summary>
		[JsonProperty("fee")]
		public decimal TotalFee { get; set; }

		/// <summary>
		/// This is the operations in raw format.
		/// </summary>
		[JsonProperty("rawoperations")]
		public string RawOperations { get; set; }
	}

}
