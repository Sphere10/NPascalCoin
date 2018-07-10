using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPascalCoin {

	public abstract class Operation : IEquatable<Operation> {
		public virtual Block Block { get; set; }
		public virtual ulong Index { get; set; }
		public abstract OperationType Type { get; }
		public virtual string OPHASH { get; set; }
		public virtual long TimeUnix { get; set; }
		public virtual DateTime TimeUTC => DateTimeOffset.FromUnixTimeSeconds(TimeUnix).UtcDateTime;
		public virtual string Description { get; set; }
		public virtual ulong TotalFee { get; set; }


		public virtual bool Equals(Operation other) {
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Equals(Block, other.Block) && Index == other.Index && string.Equals(OPHASH, other.OPHASH) && TimeUnix == other.TimeUnix && string.Equals(Description, other.Description) && TotalFee == other.TotalFee;
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Operation) obj);
		}

		public override int GetHashCode() {
			unchecked {
				var hashCode = (Block != null ? Block.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ Index.GetHashCode();
				hashCode = (hashCode * 397) ^ (OPHASH != null ? OPHASH.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ TimeUnix.GetHashCode();
				hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ TotalFee.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(Operation left, Operation right) {
			return Equals(left, right);
		}

		public static bool operator !=(Operation left, Operation right) {
			return !Equals(left, right);
		}
	}

}