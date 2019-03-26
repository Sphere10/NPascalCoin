using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NPascalCoin.Crypto;
using NUnit.Framework;
using Sphere10.Framework;

namespace NPascalCoin.UnitTests.Crypto {
	public class HashTests {

		// General purpose byte array for testing		
		const string DATA_BYTES = "0x4f550200ca022000bb718b4b00d6f74478c332f5fb310507e55a9ef9b38551f63858e3f7c86dbd00200006f69afae8a6b0735b6acfcc58b7865fc8418897c530211f19140c9f95f24532102700000000000003000300a297fd17506f6c796d696e65722e506f6c796d696e65722e506f6c796d6939303030303030302184d63666eb166619e925cef2a306549bbc4d6f4da3bdf28b4393d5c1856f0ee3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855000000006d68295b00000000";

		public TestItem<int, uint>[] DATA_MURMUR3_32 = {
			// NOTE: Input denotes the number of bytes to take from DATA_BYTES when executing test
			new TestItem<int, uint> {Input = 17,  Expected = 3935905087},
			new TestItem<int, uint> {Input = 31,  Expected = 357806018},
			new TestItem<int, uint> {Input = 32,  Expected = 2437342702},
			new TestItem<int, uint> {Input = 33,  Expected = 2644441622},
			new TestItem<int, uint> {Input = 34,  Expected = 3741302463},
			new TestItem<int, uint> {Input = 63,  Expected = 1446059036},
			new TestItem<int, uint> {Input = 64,  Expected = 1305844766},
			new TestItem<int, uint> {Input = 65,  Expected = 2842590698},
			new TestItem<int, uint> {Input = 100, Expected = 1638914994},
			new TestItem<int, uint> {Input = 117, Expected = 81024260},
			new TestItem<int, uint> {Input = 127, Expected = 586511266},
			new TestItem<int, uint> {Input = 128, Expected = 1415230935},
			new TestItem<int, uint> {Input = 129, Expected = 1171678054},
			new TestItem<int, uint> {Input = 178, Expected = 3944120518},
			new TestItem<int, uint> {Input = 199, Expected = 866217581},
			new TestItem<int, uint> {Input = 200, Expected = 1143690750}
		};

		public TestItem<int, string>[] DATA_MURMUR3_32_HEX =  {
			// NOTE: Input denotes the number of bytes to take from DATA_BYTES when executing test
			new TestItem<int, string> { Input = 17,  Expected = "0xea99253f" },
			new TestItem<int, string> { Input= 31,  Expected= "0x1553afc2"},
			new TestItem<int, string> { Input= 32,  Expected= "0x9146e5ee"},
			new TestItem<int, string> { Input= 33,  Expected= "0x9d9efa16"},
			new TestItem<int, string> { Input= 34,  Expected= "0xdeffbebf"},
			new TestItem<int, string> { Input= 63,  Expected= "0x56311c1c"},
			new TestItem<int, string> { Input= 64,  Expected= "0x4dd59c1e"},
			new TestItem<int, string> { Input= 65,  Expected= "0xa96e7dea"},
			new TestItem<int, string> { Input= 100, Expected= "0x61afdbb2"},
			new TestItem<int, string> { Input= 117, Expected= "0x04d45504"},
			new TestItem<int, string> { Input= 127, Expected= "0x22f573a2"},
			new TestItem<int, string> { Input= 128, Expected= "0x545ab5d7"},
			new TestItem<int, string> { Input= 129, Expected= "0x45d66366"},
			new TestItem<int, string> { Input= 178, Expected= "0xeb1680c6"},
			new TestItem<int, string> { Input= 199, Expected= "0x33a16e6d"},
			new TestItem<int, string> { Input= 200, Expected= "0x442b55fe"}
		};

		public TestItem<int, string>[] DATA_SHA2_256 =  {
		// NOTE: Input denotes the number of bytes to take from DATA_BYTES when executing test
			new TestItem<int, string> {Input= 17,  Expected= "0x0fd3f87ae8963c1ac8aabc0706d2ad5a66c2d88b50f57821b864b093263a7a05"},
			new TestItem<int, string> {Input= 31,  Expected= "0x209ef563d4ac7d51968cced180be0145dbd4d4c9688bdbdd8fcdb171029bff35"},
			new TestItem<int, string> {Input= 32,  Expected= "0xa910d364190b6aed1c0a4198688a1a5ac4b37205c542d665be0f5aa558ad483e"},
			new TestItem<int, string> {Input= 33,  Expected= "0x8f2d5d44ca1a2f534253a600c4e95f315133f775127a11bcb22db928efbd638d"},
			new TestItem<int, string> {Input= 34,  Expected= "0xda8f41e9f2ac0effa4815a50f599b0791f210cb85f056672404639c960f56fe8"},
			new TestItem<int, string> {Input= 63,  Expected= "0xb06a88f708c40510cc132a5108c6f26a9a3f7f6d42e0143baaacaf96aec16952"},
			new TestItem<int, string> {Input= 64,  Expected= "0x3725408cbe6e81f8a05bd2f1b4618a356235b7262eb809608bc4e3dc38e4fa1f"},
			new TestItem<int, string> {Input= 65,  Expected= "0xaf29a07c4c9ca57aa087a3c6134573615ec8b54706c75361cfd23fba38d8a5d0"},
			new TestItem<int, string> {Input= 100, Expected= "0x30cb592bdaf02c26fcba00c055059d9c3cf74f10a7eb49e2fcd4926c86c85e00"},
			new TestItem<int, string> {Input= 117, Expected= "0x1e34859b3591e50f8522d707a554725591603b95725d8d16f9dc728f901091d4"},
			new TestItem<int, string> {Input= 127, Expected= "0x6b3e56f2349c09aa0a814a0c5a9dfb72e13b79c57d3dd5bf802ab00c5040164b"},
			new TestItem<int, string> {Input= 128, Expected= "0x75b01600de565f4138151f345028a91a8471385509dfe27e2d07096b4c82136b"},
			new TestItem<int, string> {Input= 129, Expected= "0x5536bf5cdf0739e4ff259eb79a4276a009717e371057a3b8afe4ba79a03a884a"},
			new TestItem<int, string> {Input= 178, Expected= "0xad69c11f5d88dc4b047174218e843fdb29dbfb8dd2697f017bc8cd98a6a7b7fd"},
			new TestItem<int, string> {Input= 199, Expected= "0xcafebf56cdeaec6505b97a0f52369a79fa441d4d2e5a034d16ab0df00172b907"},
			new TestItem<int, string> {Input= 200, Expected= "0xd20e764994f9a21ca01a3e9247bc70618f39663773c3a7a839d8a2e1072f182d"}
		}; 

		[Test]
		public void TestMurMur3() {
			TestHash(Hashers.MURMUR3_32, DATA_MURMUR3_32);
		}

		[Test]
		public void TestMurMur3_AsHex() {
			TestHash((bytes) => EndianBitConverter.Big.GetBytes(Hashers.MURMUR3_32(bytes)).ToHexString(), DATA_MURMUR3_32_HEX);
		}

		[Test]
		public void TestSHA2_256() {
			TestHash((x) => Hashers.SHA2_256(x).ToHexString(), DATA_SHA2_256);
		}

		protected void TestHash<TResult>(Func<byte[], TResult> hasher, IEnumerable<TestItem<int, TResult>> testCases) {
			foreach (var testCase in testCases) {
				var input = ToHexByteArray2(DATA_BYTES).Take(testCase.Input).ToArray();
				var result = hasher(input);
				Assert.AreEqual(testCase.Expected, result);
			}
		}

		public static byte[] ToHexByteArray2(string hex) {
			if (string.IsNullOrEmpty(hex))
				return new byte[0];

			var offset = 0;
			if (hex.StartsWith("0x"))
				offset = 2;

			var numberChars = (hex.Length - offset) / 2;

			var bytes = new byte[numberChars];
			using (var stringReader = new StringReader(hex)) {
				for (var i = 0; i < offset; i++)
					stringReader.Read();
				var nibbles = new char[2];
				for (var i = offset; i < numberChars; i++) {
					nibbles[0] = (char) stringReader.Read();
					nibbles[1] = (char) stringReader.Read();
					bytes[i - offset] = Convert.ToByte(new string(nibbles), 16);
				}
			}
			return bytes;
		}
	}
}
