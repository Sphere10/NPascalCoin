using System;

namespace NPascalCoin {
	public class TransactionOperation : Operation, IEquatable<TransactionOperation> {
		public override OperationType Type => OperationType.Transaction;
		public virtual ulong Sender { get; set; } 
		public virtual ulong NOperation { get; set; }
		public virtual ulong Target { get; set; }
		public virtual ulong Amount { get; set; }
		public virtual ulong Fee { get; set; }
		public virtual byte[] Payload { get; set; }
		public virtual PublicKey PublicKey { get; set; }
		public virtual Signature Signature { get; set; }
		// Protocol 2
		// Next values will only be filled after this operation is executed
		public virtual TransactionStyle Style { get; set; }
		public virtual ulong AccountPrice { get; set; }
		public virtual ulong SellerAccount { get; set; }
		public virtual PublicKey NewAccountKey { get; set; }

		public virtual bool Equals(TransactionOperation other) {
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return base.Equals(other) && Sender == other.Sender && NOperation == other.NOperation && Target == other.Target && Amount == other.Amount && Fee == other.Fee && Equals(Payload, other.Payload) && Equals(PublicKey, other.PublicKey) && Equals(Signature, other.Signature) && Style == other.Style && AccountPrice == other.AccountPrice && SellerAccount == other.SellerAccount && Equals(NewAccountKey, other.NewAccountKey);
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((TransactionOperation) obj);
		}

		public override int GetHashCode() {
			unchecked {
				int hashCode = base.GetHashCode();
				hashCode = (hashCode * 397) ^ Sender.GetHashCode();
				hashCode = (hashCode * 397) ^ NOperation.GetHashCode();
				hashCode = (hashCode * 397) ^ Target.GetHashCode();
				hashCode = (hashCode * 397) ^ Amount.GetHashCode();
				hashCode = (hashCode * 397) ^ Fee.GetHashCode();
				hashCode = (hashCode * 397) ^ (Payload != null ? Payload.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (PublicKey != null ? PublicKey.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Signature != null ? Signature.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (int) Style;
				hashCode = (hashCode * 397) ^ AccountPrice.GetHashCode();
				hashCode = (hashCode * 397) ^ SellerAccount.GetHashCode();
				hashCode = (hashCode * 397) ^ (NewAccountKey != null ? NewAccountKey.GetHashCode() : 0);
				return hashCode;
			}
		}

		public static bool operator ==(TransactionOperation left, TransactionOperation right) {
			return Equals(left, right);
		}

		public static bool operator !=(TransactionOperation left, TransactionOperation right) {
			return !Equals(left, right);
		}
	}
}