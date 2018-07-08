using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPascalCoin {

	/// <summary>
	/// An "Operation object" is a JSON object with information about an operation.
	/// </summary>
	public class Operation {
		public virtual int DBID { get; set; }
		public virtual string OPHASH { get; set; }
		public virtual long? Block { get; set; }
		public virtual long Time { get; set; }
		public virtual string TypeDescriptor { get; set; }
	}

}