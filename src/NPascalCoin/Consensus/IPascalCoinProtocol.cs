using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NPascalCoin.Consensus {
	public interface IPascalCoinProtocol {
		uint ActivationBlock { get; }
		void SerializeKey(Stream stream, Key key);
		bool TryDeserializeKey(Stream stream, out Key key);
	}
}
