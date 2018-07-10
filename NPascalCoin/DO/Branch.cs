using System;
using System.Collections.Generic;
// ReSharper disable InconsistentNaming

namespace NPascalCoin {
	public class Branch : IEquatable<Branch> {
		public Branch() {
			Blocks = new List<Block>();
		}
		public virtual int ID { get; set; }
		public virtual string Name { get; set; }
		public virtual uint TimeUnix { get; set; }
		public virtual DateTime TimeUTC => DateTimeOffset.FromUnixTimeSeconds(TimeUnix).UtcDateTime;
		public virtual ICollection<Block> Blocks { get; set; }

		public virtual bool Equals(Branch other) {
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return ID == other.ID && string.Equals(Name, other.Name) && TimeUnix == other.TimeUnix && Equals(Blocks, other.Blocks);
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Branch) obj);
		}

		public override int GetHashCode() {
			unchecked {
				var hashCode = ID;
				hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (int) TimeUnix;
				hashCode = (hashCode * 397) ^ (Blocks != null ? Blocks.GetHashCode() : 0);
				return hashCode;
			}
		}

		public static bool operator ==(Branch left, Branch right) {
			return Equals(left, right);
		}

		public static bool operator !=(Branch left, Branch right) {
			return !Equals(left, right);
		}
	}
}