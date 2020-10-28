using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace NPascalCoin.API.Objects {

	/// <summary>
	/// State of an account
	/// </summary>
	public enum AccountState {
		[Description("normal")]
		Normal,

		[Description("listed")]
		Listed
	}
}
