using System;
using System.Collections.Generic;

namespace NPascalCoin {
	public class MultiOperation : Operation, IEquatable<MultiOperation> {
		public MultiOperation() {
			Senders = new List<MultiOperationSender>();
			Receivers = new List<MultiOperationReceiver>();
			Changers = new List<MultiOperationChanger>();
		}
		public override OperationType Type => OperationType.MultiOperation;
		public virtual ICollection<MultiOperationSender> Senders { get; set; }
		public virtual ICollection<MultiOperationReceiver> Receivers { get; set; }
		public virtual ICollection<MultiOperationChanger> Changers { get; set; }

		public virtual bool Equals(MultiOperation other) {
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return base.Equals(other) && Equals(Senders, other.Senders) && Equals(Receivers, other.Receivers) && Equals(Changers, other.Changers);
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((MultiOperation) obj);
		}

		public override int GetHashCode() {
			unchecked {
				int hashCode = base.GetHashCode();
				hashCode = (hashCode * 397) ^ (Senders != null ? Senders.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Receivers != null ? Receivers.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Changers != null ? Changers.GetHashCode() : 0);
				return hashCode;
			}
		}

		public static bool operator ==(MultiOperation left, MultiOperation right) {
			return Equals(left, right);
		}

		public static bool operator !=(MultiOperation left, MultiOperation right) {
			return !Equals(left, right);
		}
	}
}