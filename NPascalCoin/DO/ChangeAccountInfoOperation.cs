using System;

namespace NPascalCoin {
	public class ChangeAccountInfoOperation : Operation, IEquatable<ChangeAccountInfoOperation> {
		public override OperationType Type => OperationType.ChangeAccountInfo;
		public virtual ulong AccountSigner { get; set; }
		public virtual ulong AccountTarget { get; set; }
		public virtual ulong NOperation { get; set; }
		public virtual ulong Fee { get; set; }
		public virtual byte[] Payload { get; set; }
		public virtual PublicKey PublicKey { get; set; }
		public virtual ChangeInfoType ChangesType { get; set; }
		public virtual PublicKey NewAccountkey { get; set; }
		public virtual byte[] NewName { get; set; }
		public virtual ushort NewType { get; set; }
		public virtual Signature Signature { get; set; }

		public virtual bool Equals(ChangeAccountInfoOperation other) {
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return base.Equals(other) && AccountSigner == other.AccountSigner && AccountTarget == other.AccountTarget && NOperation == other.NOperation && Fee == other.Fee && Equals(Payload, other.Payload) && Equals(PublicKey, other.PublicKey) && ChangesType == other.ChangesType && Equals(NewAccountkey, other.NewAccountkey) && Equals(NewName, other.NewName) && NewType == other.NewType && Equals(Signature, other.Signature);
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((ChangeAccountInfoOperation) obj);
		}

		public override int GetHashCode() {
			unchecked {
				int hashCode = base.GetHashCode();
				hashCode = (hashCode * 397) ^ AccountSigner.GetHashCode();
				hashCode = (hashCode * 397) ^ AccountTarget.GetHashCode();
				hashCode = (hashCode * 397) ^ NOperation.GetHashCode();
				hashCode = (hashCode * 397) ^ Fee.GetHashCode();
				hashCode = (hashCode * 397) ^ (Payload != null ? Payload.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (PublicKey != null ? PublicKey.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (int) ChangesType;
				hashCode = (hashCode * 397) ^ (NewAccountkey != null ? NewAccountkey.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (NewName != null ? NewName.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ NewType.GetHashCode();
				hashCode = (hashCode * 397) ^ (Signature != null ? Signature.GetHashCode() : 0);
				return hashCode;
			}
		}

		public static bool operator ==(ChangeAccountInfoOperation left, ChangeAccountInfoOperation right) {
			return Equals(left, right);
		}

		public static bool operator !=(ChangeAccountInfoOperation left, ChangeAccountInfoOperation right) {
			return !Equals(left, right);
		}
	}
}