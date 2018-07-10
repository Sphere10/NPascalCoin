using System;
using System.Collections.Generic;
// ReSharper disable InconsistentNaming

namespace NPascalCoin {
	public class Block : IEquatable<Block> {
		public Block() {
			Operations = new List<Operation>();
		}
		public virtual Branch Branch { get; set; }
		public virtual uint Number { get; set; }		
		public virtual string EncPubKey { get; set; }
		public virtual ulong Reward { get; set; }
		public virtual ulong Fee { get; set; }
		public virtual uint Version { get; set; }
		public virtual uint AvailableVersion { get; set; }
		public virtual uint TimeUnix { get; set; }
		public virtual DateTime TimeUTC => DateTimeOffset.FromUnixTimeSeconds(TimeUnix).UtcDateTime;
		public virtual uint CompactTarget { get; set; }
		public virtual uint Nonce { get; set; }
		public virtual string MinerPayload { get; set; }
		public virtual string SafeBoxHash { get; set; }
		public virtual string OperationsHash { get; set; }
		public virtual string ProofOfWork { get; set; }
		public virtual uint OperationCount { get; set; }
		public virtual ulong Last50HashRateKhs { get; set; }
		public virtual ICollection<Operation> Operations { get; set; }
	
		public virtual bool Equals(Block other) {
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Equals(Branch, other.Branch) && Number == other.Number && string.Equals(EncPubKey, other.EncPubKey) && Reward == other.Reward && Fee == other.Fee && Version == other.Version && AvailableVersion == other.AvailableVersion && TimeUnix == other.TimeUnix && CompactTarget == other.CompactTarget && Nonce == other.Nonce && string.Equals(MinerPayload, other.MinerPayload) && string.Equals(SafeBoxHash, other.SafeBoxHash) && string.Equals(OperationsHash, other.OperationsHash) && string.Equals(ProofOfWork, other.ProofOfWork) && OperationCount == other.OperationCount && Last50HashRateKhs == other.Last50HashRateKhs && Equals(Operations, other.Operations);
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Block) obj);
		}

		public override int GetHashCode() {
			unchecked {
				var hashCode = (Branch != null ? Branch.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (int) Number;
				hashCode = (hashCode * 397) ^ (EncPubKey != null ? EncPubKey.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ Reward.GetHashCode();
				hashCode = (hashCode * 397) ^ Fee.GetHashCode();
				hashCode = (hashCode * 397) ^ (int) Version;
				hashCode = (hashCode * 397) ^ (int) AvailableVersion;
				hashCode = (hashCode * 397) ^ (int) TimeUnix;
				hashCode = (hashCode * 397) ^ (int) CompactTarget;
				hashCode = (hashCode * 397) ^ (int) Nonce;
				hashCode = (hashCode * 397) ^ (MinerPayload != null ? MinerPayload.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (SafeBoxHash != null ? SafeBoxHash.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (OperationsHash != null ? OperationsHash.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (ProofOfWork != null ? ProofOfWork.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (int) OperationCount;
				hashCode = (hashCode * 397) ^ Last50HashRateKhs.GetHashCode();
				hashCode = (hashCode * 397) ^ (Operations != null ? Operations.GetHashCode() : 0);
				return hashCode;
			}
		}

		public static bool operator ==(Block left, Block right) {
			return Equals(left, right);
		}

		public static bool operator !=(Block left, Block right) {
			return !Equals(left, right);
		}
	}

}