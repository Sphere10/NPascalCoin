using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using Sphere10.Framework;

namespace NPascalCoin.Consensus.V1 {
	public class PascalCoinProtocolV1 : IPascalCoinProtocol {

		public uint ActivationBlock => 0;

		public virtual void SerializeKey(Stream stream, Key key) {
			var writer = new EndianBinaryWriter(EndianBitConverter.Little, stream);
			writer.Write(key.CurveID);
			writer.Write(key.X);
			writer.Write(key.Y);
		}

		public virtual bool TryDeserializeKey(Stream stream, out Key key) {
			key = new Key();
			var reader = new EndianBinaryReader(EndianBitConverter.Little, stream);
			key.CurveID = reader.ReadUInt16();
			var length = reader.ReadUInt16();
			key.X = reader.ReadBytes(length);
			length = reader.ReadUInt16();
			key.Y = reader.ReadBytes(length);
			return true;
		}
	}
}
