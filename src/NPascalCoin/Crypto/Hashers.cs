using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto.Digests;
using Sphere10.Framework;


namespace NPascalCoin.Crypto {

	/// <summary>
	/// Fast, thread-safe static hash methods.
	/// </summary>
	public static class Hashers {
		private static readonly ConcurrentStack<SHA256Managed> SHA2_256Hashers;
		private static readonly ConcurrentStack<SHA384Managed> SHA2_384Hashers;
		private static readonly ConcurrentStack<SHA512Managed> SHA2_512Hashers;
		private static readonly ConcurrentStack<Sha3Digest> SHA3_256Hashers;
		private static readonly ConcurrentStack<Sha3Digest> SHA3_384Hashers;
		private static readonly ConcurrentStack<Sha3Digest> SHA3_512Hashers;
		private static readonly ConcurrentStack<RipeMD160Digest> RIPEMD160Hashers;
		private static readonly ConcurrentStack<RipeMD256Digest> RIPEMD256Hashers;
		private static readonly ConcurrentStack<RipeMD320Digest> RIPEMD320Hashers;
		private static readonly ConcurrentStack<Blake2bDigest> Blake2BHashers;
		private static readonly ConcurrentStack<Blake2sDigest> Blake2SHashers;
		private static readonly ConcurrentStack<TigerDigest> Tiger2_5_192Hashers;
		//private static readonly ConcurrentStack<Snefru_8_256Digest> Snefru_8_256Hashers;
		//private static readonly ConcurrentStack<Grindahl_512_Digest> Grindahl_512Hashers;
		//private static readonly ConcurrentStack<Haval_5_256Digest> Haval_5_256Hashers;
		private static readonly ConcurrentStack<MD5Digest> MD5Hashers;
		//private static readonly ConcurrentStack<RadioGatun32Digest> RadioGatun32Hashers;
		private static readonly ConcurrentStack<WhirlpoolDigest> WhirlpoolHashers;

		// Snefru_8_256
		// Grindahl512
		// RadioGatun32

		static Hashers() {
			SHA2_256Hashers = new ConcurrentStack<SHA256Managed>();
			SHA2_384Hashers = new ConcurrentStack<SHA384Managed>();
			SHA2_512Hashers = new ConcurrentStack<SHA512Managed>();
			SHA3_256Hashers = new ConcurrentStack<Sha3Digest>();
			SHA3_384Hashers = new ConcurrentStack<Sha3Digest>();
			SHA3_512Hashers = new ConcurrentStack<Sha3Digest>();
			RIPEMD160Hashers = new ConcurrentStack<RipeMD160Digest>();
			RIPEMD256Hashers = new ConcurrentStack<RipeMD256Digest>();
			RIPEMD320Hashers = new ConcurrentStack<RipeMD320Digest>();
			Blake2BHashers = new ConcurrentStack<Blake2bDigest>();
			Blake2SHashers = new ConcurrentStack<Blake2sDigest>();
			Tiger2_5_192Hashers = new ConcurrentStack<TigerDigest>();
			//Snefru_8_256Hashers = new ConcurrentStack<Snefru_8_256Digest>();
			//Grindahl_512Hashers = new ConcurrentStack<Grindahl_512_Digest>();
			//Haval_5_256Digest = new ConcurrentStack<Haval_5_256Digest>();
			MD5Hashers = new ConcurrentStack<MD5Digest>();
			//RadioGatun32Digest = new ConcurrentStack<RadioGatun32Digest>();
			WhirlpoolHashers = new ConcurrentStack<WhirlpoolDigest>();
		}

		public static uint MURMUR3_32(byte[] digest, uint seed) {
			return (uint)digest.GetMurMurHash3((int)seed);
		}

		public static byte[] SHA2_256(byte[] bytes) {
			if (!SHA2_256Hashers.TryPop(out var hasher)) {
				hasher = new SHA256Managed();
			}
			try {
				return hasher.ComputeHash(bytes);
			} finally {
				SHA2_256Hashers.Push(hasher);
			}
		}

		public static byte[] SHA2_384(byte[] bytes) {
			if (!SHA2_384Hashers.TryPop(out var hasher)) {
				hasher = new SHA384Managed();
			}
			try {
				return hasher.ComputeHash(bytes);
			} finally {
				SHA2_384Hashers.Push(hasher);
			}
		}

		public static byte[] SHA2_512(byte[] bytes) {
			if (!SHA2_512Hashers.TryPop(out var hasher)) {
				hasher = new SHA512Managed();
			}
			try {
				return hasher.ComputeHash(bytes);
			} finally {
				SHA2_512Hashers.Push(hasher);
			}
		}

		public static byte[] SHA3_256(byte[] bytes) {
			if (!SHA3_256Hashers.TryPop(out var hasher)) {
				hasher = new Sha3Digest(256);
			}
			try {
				var result = new byte[hasher.GetDigestSize()];
				hasher.BlockUpdate(bytes, 0, bytes.Length);				
				hasher.DoFinal(result, 0);
				hasher.Reset();
				return result;
			} finally {
				SHA3_256Hashers.Push(hasher);
			}
		}

		public static byte[] SHA3_384(byte[] bytes) {
			if (!SHA3_384Hashers.TryPop(out var hasher)) {
				hasher = new Sha3Digest(384);
			}
			try {
				var result = new byte[hasher.GetDigestSize()];
				hasher.BlockUpdate(bytes, 0, bytes.Length);
				hasher.DoFinal(result, 0);
				hasher.Reset();
				return result;
			} finally {
				SHA3_384Hashers.Push(hasher);
			}
		}

		public static byte[] SHA3_512(byte[] bytes) {
			if (!SHA3_512Hashers.TryPop(out var hasher)) {
				hasher = new Sha3Digest(512);
			}
			try {
				var result = new byte[hasher.GetDigestSize()];
				hasher.BlockUpdate(bytes, 0, bytes.Length);
				hasher.DoFinal(result, 0);
				hasher.Reset();
				return result;
			} finally {
				SHA3_512Hashers.Push(hasher);
			}
		}

		public static byte[] RIPEMD160(byte[] bytes) {
			if (!RIPEMD160Hashers.TryPop(out var hasher)) {
				hasher = new RipeMD160Digest();
			}
			try {
				var result = new byte[hasher.GetDigestSize()];
				hasher.BlockUpdate(bytes, 0, bytes.Length);
				hasher.DoFinal(result, 0);
				hasher.Reset();
				return result;
			} finally {
				RIPEMD160Hashers.Push(hasher);
			}
		}

		public static byte[] RIPEMD256(byte[] bytes) {
			if (!RIPEMD256Hashers.TryPop(out var hasher)) {
				hasher = new RipeMD256Digest();
			}
			try {
				var result = new byte[hasher.GetDigestSize()];
				hasher.BlockUpdate(bytes, 0, bytes.Length);
				hasher.DoFinal(result, 0);
				hasher.Reset();
				return result;
			} finally {
				RIPEMD256Hashers.Push(hasher);
			}
		}

		public static byte[] RIPEMD320(byte[] bytes) {
			if (!RIPEMD320Hashers.TryPop(out var hasher)) {
				hasher = new RipeMD320Digest();
			}
			try {
				var result = new byte[hasher.GetDigestSize()];
				hasher.BlockUpdate(bytes, 0, bytes.Length);
				hasher.DoFinal(result, 0);
				hasher.Reset();
				return result;
			} finally {
				RIPEMD320Hashers.Push(hasher);
			}
		}

		public static byte[] BLAKE2B(byte[] bytes) {
			if (!Blake2BHashers.TryPop(out var hasher)) {
				hasher = new Blake2bDigest();
			}
			try {
				var result = new byte[hasher.GetDigestSize()];
				hasher.BlockUpdate(bytes, 0, bytes.Length);
				hasher.DoFinal(result, 0);
				hasher.Reset();
				return result;
			} finally {
				Blake2BHashers.Push(hasher);
			}
		}

		public static byte[] BLAKE2S(byte[] bytes) {
			if (!Blake2SHashers.TryPop(out var hasher)) {
				hasher = new Blake2sDigest();
			}
			try {
				var result = new byte[hasher.GetDigestSize()];
				hasher.BlockUpdate(bytes, 0, bytes.Length);
				hasher.DoFinal(result, 0);
				hasher.Reset();
				return result;
			} finally {
				Blake2SHashers.Push(hasher);
			}
		}

		public static byte[] Tiger2_5_192(byte[] bytes) {
			if (!Tiger2_5_192Hashers.TryPop(out var hasher)) {
				hasher = new TigerDigest();
			}
			try {
				var result = new byte[hasher.GetDigestSize()];
				hasher.BlockUpdate(bytes, 0, bytes.Length);
				hasher.DoFinal(result, 0);
				hasher.Reset();
				return result;
			} finally {
				Tiger2_5_192Hashers.Push(hasher);
			}
		}

		public static byte[] MD5(byte[] bytes) {
			if (!MD5Hashers.TryPop(out var hasher)) {
				hasher = new MD5Digest();
			}
			try {
				var result = new byte[hasher.GetDigestSize()];
				hasher.BlockUpdate(bytes, 0, bytes.Length);
				hasher.DoFinal(result, 0);
				hasher.Reset();
				return result;
			} finally {
				MD5Hashers.Push(hasher);
			}
		}

		public static byte[] Whirlpool(byte[] bytes) {
			if (!WhirlpoolHashers.TryPop(out var hasher)) {
				hasher = new WhirlpoolDigest();
			}
			try {
				var result = new byte[hasher.GetDigestSize()];
				hasher.BlockUpdate(bytes, 0, bytes.Length);
				hasher.DoFinal(result, 0);
				hasher.Reset();
				return result;
			} finally {
				WhirlpoolHashers.Push(hasher);
			}
		}
	}
}
