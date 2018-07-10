using System;

namespace NPascalCoin {
	public class MultiOperationChanger : IEquatable<MultiOperationChanger> {
		public virtual MultiOperation Owner { get; set; }
		public virtual ulong Index { get; set; }
		public virtual ulong Account { get; set; }
		public virtual ulong NOperation { get; set; }
		public virtual ChangeInfoType ChangeType { get; set; }
		public virtual PublicKey NewAccountKey { get; set; }  // If (changes_mask and $0001)=$0001 then change account key
		public virtual byte[] NewName { get; set; }          // If (changes_mask and $0002)=$0002 then change name
		public virtual ushort NewType { get; set; }               // If (changes_mask and $0004)=$0004 then change type
		public virtual ulong SellerAccount { get; set; }
		public virtual ulong AccountPrice { get; set; }
		public virtual ulong LockedUntilBlock { get; set; }
		public virtual ulong Fee { get; set; }
		public virtual Signature Signature { get; set; }

		public virtual bool Equals(MultiOperationChanger other) {
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Equals(Owner, other.Owner) && Index == other.Index && Account == other.Account && NOperation == other.NOperation && ChangeType == other.ChangeType && Equals(NewAccountKey, other.NewAccountKey) && Equals(NewName, other.NewName) && NewType == other.NewType && SellerAccount == other.SellerAccount && AccountPrice == other.AccountPrice && LockedUntilBlock == other.LockedUntilBlock && Fee == other.Fee && Equals(Signature, other.Signature);
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((MultiOperationChanger) obj);
		}

		public override int GetHashCode() {
			unchecked {
				var hashCode = (Owner != null ? Owner.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ Index.GetHashCode();
				hashCode = (hashCode * 397) ^ Account.GetHashCode();
				hashCode = (hashCode * 397) ^ NOperation.GetHashCode();
				hashCode = (hashCode * 397) ^ (int) ChangeType;
				hashCode = (hashCode * 397) ^ (NewAccountKey != null ? NewAccountKey.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (NewName != null ? NewName.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ NewType.GetHashCode();
				hashCode = (hashCode * 397) ^ SellerAccount.GetHashCode();
				hashCode = (hashCode * 397) ^ AccountPrice.GetHashCode();
				hashCode = (hashCode * 397) ^ LockedUntilBlock.GetHashCode();
				hashCode = (hashCode * 397) ^ Fee.GetHashCode();
				hashCode = (hashCode * 397) ^ (Signature != null ? Signature.GetHashCode() : 0);
				return hashCode;
			}
		}

		public static bool operator ==(MultiOperationChanger left, MultiOperationChanger right) {
			return Equals(left, right);
		}

		public static bool operator !=(MultiOperationChanger left, MultiOperationChanger right) {
			return !Equals(left, right);
		}
	}
}