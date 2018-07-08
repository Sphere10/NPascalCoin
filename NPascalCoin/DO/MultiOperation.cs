using System.Collections.Generic;

namespace NPascalCoin {
	public class MultiOperation : Operation {
		public MultiOperation() {
			Senders = new List<MultiOperationSender>();
			Receivers = new List<MultiOperationReceiver>();
			Changers = new List<MultiOperationChangers>();
		}
		public virtual ICollection<MultiOperationSender> Senders { get; set; }
		public virtual ICollection<MultiOperationReceiver> Receivers { get; set; }
		public virtual ICollection<MultiOperationChangers> Changers { get; set; }
	}
}