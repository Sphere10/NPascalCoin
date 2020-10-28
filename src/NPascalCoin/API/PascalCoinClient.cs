using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sphere10.Framework;
using NPascalCoin.API.Objects;
using NPascalCoin.Common;
using NPascalCoin.Network;

// ReSharper disable InconsistentNaming

namespace NPascalCoin.API {
	public class PascalCoinClient : IPascalCoinClient {
		private readonly Uri _url;
		private int _callID;

		public PascalCoinClient() : this(PascalCoinClientConfiguration.DefaultMainNet) {			
		}

		public PascalCoinClient(PascalCoinClientConfiguration configuration) : this(configuration.Server, configuration.Port) {			
		}

		public PascalCoinClient(NodeEndpoint endpoint) : this(endpoint.IP.ToString(), endpoint.Port) {
		}

		public PascalCoinClient(string server, int port = Constants.DefaultMainNetClassicRpcPort) {
			_url = new Uri($"http://{server}:{port}");			
			_callID = 0;
		}

		public virtual int AddNode(string nodes) {
			return Invoke<int>(ApiMethodName.addnode.ToString(), new Dictionary<string, object>() { 
				["nodes"] = nodes
			});
		}

		public virtual Account GetAccount(uint account) {
			return Invoke<Account>(ApiMethodName.getaccount.ToString(), new Dictionary<string, object>() {
				["account"] = account
			});
		}

		public virtual Account[] FindAccounts(string name, uint? type = null, uint? status = null, uint? start = null, uint? max = null) {
			return Invoke<Account[]>(ApiMethodName.findaccounts.ToString(), new Dictionary<string, object>() {
				["name"] = name,
				["type"] = type,
				["status"] = status,
				["start"] = start,
				["max"] = max,
			});
		}

		public virtual Account[] GetWalletAccounts(string enc_pubkey = null, string b58_pubkey = null, int? start = null, int? max = null) {
			return Invoke<Account[]>(ApiMethodName.getwalletaccounts.ToString(), new Dictionary<string, object>() {
				["enc_pubkey"] = enc_pubkey,
				["b58_pubkey"] = b58_pubkey,
				["start"] = start,
				["max"] = max,
			});
		}

		public virtual uint GetWalletAccountsCount(string enc_pubkey = null, string b58_pubkey = null, int? start = null, int? max = null) {
			return Invoke<uint>(ApiMethodName.getwalletaccountscount.ToString(), new Dictionary<string, object>() {
				["enc_pubkey"] = enc_pubkey,
				["b58_pubkey"] = b58_pubkey,
				["start"] = start,
				["max"] = max,
			});
		}

		public virtual PublicKey GetWalletPubKey(string enc_pubkey = null, string b58_pubkey = null) {
			return Invoke<PublicKey>(ApiMethodName.getwalletpubkey.ToString(), new Dictionary<string, object>() {
				["enc_pubkey"] = enc_pubkey,
				["b58_pubkey"] = b58_pubkey
			});
		}

		public virtual PublicKey[] GetWalletPubKeys(int? start = null, int? max = null) {
			return Invoke<PublicKey[]>(ApiMethodName.getwalletpubkeys.ToString(), new Dictionary<string, object>() {
				["start"] = start,
				["max"] = max,
			});
		}

		public virtual decimal GetWalletCoins(string enc_pubkey = null, string b58_pubkey = null) {
			return Invoke<decimal>(ApiMethodName.getwalletcoins.ToString(), new Dictionary<string, object>() {
				["enc_pubkey"] = enc_pubkey,
				["b58_pubkey"] = b58_pubkey,
			});
		}

		public virtual Block GetBlock(uint block) {
			return Invoke<Block>(ApiMethodName.getblock.ToString(), new Dictionary<string, object> {
				["block"] = block
			});
		}

		public virtual Block[] GetBlocks(uint? last = null, int? start = null, int? end = null) {
			return Invoke<Block[]>(ApiMethodName.getblocks.ToString(), new Dictionary<string, object> {
				["last"] = last,
				["start"] = start,
				["end"] = end,
			});
		}

		public virtual uint GetBlockCount() {
			return Invoke<uint>(ApiMethodName.getblockcount.ToString());
		}

		public virtual Operation GetBlockOperation(uint block, uint opblock) {
			return Invoke<Operation>(ApiMethodName.getblockoperation.ToString(), new Dictionary<string, object>() {
				["block"] = block,
				["opblock"] = opblock,
			});
		}

		public virtual Operation[] GetBlockOperations(uint block, int? start = null, int? max = null) {
			return Invoke<Operation[]>(ApiMethodName.getblockoperations.ToString(), new Dictionary<string, object>() {
				["block"] = block,
				["start"] = start,
				["max"] = max,
			});
		}

		public virtual Operation[] GetAccountOperations(uint account, int? depth = null, int? start = null, int? max = null) {
			return Invoke<Operation[]>(ApiMethodName.getaccountoperations.ToString(), new Dictionary<string, object>() {
				["account"] = account,
				["depth"] = depth,
				["start"] = start,
				["max"] = max,
			});
		}


		public virtual Operation[] GetPendings(int? start = null, int? max = null) { 
			return Invoke<Operation[]>(ApiMethodName.getpendings.ToString(), new Dictionary<string, object>() {
				["start"] = start,
				["max"] = max
			});
		}

		public virtual Operation FindOperation(string ophash) {
			return Invoke<Operation>(ApiMethodName.findoperation.ToString(), new Dictionary<string, object>() {
				["ophash"] = ophash
			});
		}

		public virtual Operation ChangeAccountInfo(uint account_target, uint account_signer, string new_enc_pubkey, string new_b58_pubkey, string new_name, ushort? new_type, decimal fee, byte[] payload = null, PayloadEncryptionMethod? payloadMethod = null, string pwd = null) {
			return Invoke<Operation>(ApiMethodName.changeaccountinfo.ToString(), new Dictionary<string, object>() {
				["account_target"] = account_target,
				["account_signer"] = account_signer,
				["new_enc_pubkey"] = new_enc_pubkey,
				["new_enc_pubkey"] = new_enc_pubkey,
				["new_name"] = new_name,
				["new_type"] = new_type,
				["fee"] = fee,
				["payload"] = payload?.ToHexString(),
				["payloadMethod"] = ToPayloadMethodString(payloadMethod),
				["pwd"] = pwd
			});
		}

		public virtual Operation SendTo(uint sender, uint target, decimal amount, decimal fee, byte[] payload = null, PayloadEncryptionMethod? payloadMethod = null, string pwd = null) {
			return Invoke<Operation>(ApiMethodName.sendto.ToString(), new Dictionary<string, object>() {
				["sender"] = sender,
				["target"] = target,
				["amount"] = amount,
				["fee"] = fee,
				["payload"] = payload?.ToHexString(),
				["payloadMethod"] = ToPayloadMethodString(payloadMethod),
				["pwd"] = pwd
			});
		}

		public virtual Operation ChangeKey(uint account, uint account_signer, string new_enc_pubkey, string new_b58_pubkey, decimal fee, byte[] payload = null, PayloadEncryptionMethod? payloadMethod = null, string pwd = null) {
			return Invoke<Operation>(ApiMethodName.changekey.ToString(), new Dictionary<string, object>() {
				["account"] = account,
				["account_signer"] = account_signer,
				["new_enc_pubkey"] = new_enc_pubkey,
				["new_b58_pubkey"] = new_b58_pubkey,
				["fee"] = fee,
				["payload"] = payload.ToHexString(),
				["payloadMethod"] = ToPayloadMethodString(payloadMethod),
				["pwd"] = pwd
			});
		}

		public virtual Operation[] ChangeKeys(string accounts, string new_enc_pubkey, string new_b58_pubkey, decimal fee, byte[] payload = null, PayloadEncryptionMethod? payloadMethod = null, string pwd = null) {
			return Invoke<Operation[]>(ApiMethodName.changekeys.ToString(), new Dictionary<string, object>() {
				["accounts"] = accounts,
				["new_enc_pubkey"] = new_enc_pubkey,
				["new_b58_pubkey"] = new_b58_pubkey,
				["fee"] = fee,
				["payload"] = payload.ToHexString(),
				["payloadMethod"] = ToPayloadMethodString(payloadMethod),
				["pwd"] = pwd,
			});
		}

		public virtual Operation ListAccountForSale(uint account_target, uint account_signer, decimal price, uint seller_account, string new_b58_pubkey, string new_enc_pubkey, uint locked_until_block, decimal fee, byte[] payload = null, PayloadEncryptionMethod? payloadMethod = null, string pwd = null) {
			return Invoke<Operation>(ApiMethodName.listaccountforsale.ToString(), new Dictionary<string, object>() {
				["account_target"] = account_target,
				["account_signer"] = account_signer,
				["price"] = price,
				["seller_account"] = seller_account,
				["new_b58_pubkey"] = new_b58_pubkey,
				["new_enc_pubkey"] = new_enc_pubkey,
				["locked_until_block"] = locked_until_block,
				["fee"] = fee,
				["payload"] = payload.ToHexString(),
				["payloadMethod"] = ToPayloadMethodString(payloadMethod),
				["pwd"] = pwd,
			});
		}

		public virtual Operation DelistAccountForSale(uint account_target, uint account_signer, decimal fee, byte[] payload = null, PayloadEncryptionMethod? payloadMethod = null, string pwd = null) {
			return Invoke<Operation>(ApiMethodName.delistaccountforsale.ToString(), new Dictionary<string, object>() {
				["account_target"] = account_target,
				["account_signer"] = account_signer,
				["fee"] = fee,
				["payload"] = payload.ToHexString(),
				["payloadMethod"] = ToPayloadMethodString(payloadMethod),
				["pwd"] = pwd,
			});
		}

		public virtual Operation BuyAccount(uint buyer_account, uint account_to_purchase, decimal price, uint seller_account, string new_b58_pubkey, string new_enc_pubkey, decimal amount, decimal fee, byte[] payload = null, PayloadEncryptionMethod? payloadMethod = null, string pwd = null) {
			return Invoke<Operation>(ApiMethodName.buyaccount.ToString(), new Dictionary<string, object>() {
				["buyer_account"] = buyer_account,
				["account_to_purchase"] = account_to_purchase,
				["price"] = price,
				["seller_account"] = seller_account,
				["new_b58_pubkey"] = new_b58_pubkey,
				["new_enc_pubkey"] = new_enc_pubkey,
				["amount"] = amount,
				["fee"] = fee,
				["payload"] = payload.ToHexString(),
				["payloadMethod"] = ToPayloadMethodString(payloadMethod),
				["pwd"] = pwd,
			});
		}

		public virtual Operation SignChangeAccountInfo(uint account_target, uint account_signer, string new_enc_pubkey, string new_b58_pubkey, string new_name, ushort? new_type, uint last_n_operation, decimal fee, byte[] payload = null, PayloadEncryptionMethod? payloadMethod = null, string pwd = null, string signer_b58_pubkey = null, string signer_enc_pubkey = null, string rawoperations = null) {
			return Invoke<Operation>(ApiMethodName.signchangeaccountinfo.ToString(), new Dictionary<string, object>() {
				["account_target"] = account_target,
				["account_signer"] = account_signer,
				["new_enc_pubkey"] = new_enc_pubkey,
				["new_enc_pubkey"] = new_enc_pubkey,
				["new_name"] = new_name,
				["new_type"] = new_type,
				["last_n_operation"] = last_n_operation,
				["fee"] = fee,
				["payload"] = payload?.ToHexString(),
				["payloadMethod"] = ToPayloadMethodString(payloadMethod),
				["pwd"] = pwd,
				["signer_b58_pubkey"] = signer_b58_pubkey,
				["signer_enc_pubkey"] = signer_enc_pubkey,
				["rawoperations"] = rawoperations,
			});
		}

		public virtual RawOperation SignSendTo(uint sender, uint target, string sender_enc_pubkey, string sender_b58_pubkey, string target_enc_pubkey, string target_b58_pubkey, uint last_n_operation, decimal amount, decimal fee, byte[] payload = null, PayloadEncryptionMethod? payloadMethod = null, string pwd = null, string rawoperations = null) {
			return Invoke<RawOperation>(ApiMethodName.signsendto.ToString(), new Dictionary<string, object>() {				
				["sender"] = sender,
				["target"] = target,
				["sender_enc_pubkey"] = sender_enc_pubkey,
				["sender_b58_pubkey"] = sender_b58_pubkey,
				["target_enc_pubkey"] = target_enc_pubkey,
				["target_b58_pubkey"] = target_b58_pubkey,
				["last_n_operation"] = last_n_operation,
				["amount"] = amount,
				["fee"] = fee,
				["payload"] = payload.ToHexString(),
				["payloadMethod"] = ToPayloadMethodString(payloadMethod),
				["pwd"] = pwd,
				["rawoperations"] = rawoperations,
			});
		}

		public virtual RawOperation SignChangeKey(uint account, uint account_signer, string old_enc_pubkey, string old_b58_pubkey, string new_enc_pubkey, string new_b58_pubkey, uint last_n_operation, decimal fee, byte[] payload = null, PayloadEncryptionMethod? payloadMethod = null, string pwd = null, string rawoperations = null) {
			return Invoke<RawOperation>(ApiMethodName.signchangekey.ToString(), new Dictionary<string, object>() {				
				["account"] = account,
				["account_signer"] = account_signer,
				["old_enc_pubkey"] = old_enc_pubkey,
				["old_b58_pubkey"] = old_b58_pubkey,
				["new_enc_pubkey"] = new_enc_pubkey,
				["new_b58_pubkey"] = new_b58_pubkey,
				["last_n_operation"] = last_n_operation,
				["fee"] = fee,
				["payload"] = payload.ToHexString(),
				["payloadMethod"] = ToPayloadMethodString(payloadMethod),
				["pwd"] = pwd,
				["rawoperations"] = rawoperations,
			});
		}

		public virtual RawOperation SignListAccountForSale(uint account_target, uint account_signer, decimal price, uint seller_account, string new_b58_pubkey, string new_enc_pubkey, uint locked_until_block, uint last_n_operation, decimal fee, byte[] payload = null, PayloadEncryptionMethod? payloadMethod = null, string pwd = null, string signer_b58_pubkey = null, string signer_enc_pubkey = null, string rawoperations = null) {
			return Invoke<RawOperation>(ApiMethodName.signlistaccountforsale.ToString(), new Dictionary<string, object>() {
				["account_target"] = account_target,
				["account_signer"] = account_signer,
				["price"] = price,
				["seller_account"] = seller_account,
				["new_b58_pubkey"] = new_b58_pubkey,
				["new_enc_pubkey"] = new_enc_pubkey,
				["locked_until_block"] = locked_until_block,
				["last_n_operation"] = last_n_operation,
				["fee"] = fee,
				["payload"] = payload.ToHexString(),
				["payloadMethod"] = ToPayloadMethodString(payloadMethod),
				["pwd"] = pwd,
				["signer_b58_pubkey"] = signer_b58_pubkey,
				["signer_enc_pubkey"] = signer_enc_pubkey,
				["rawoperations"] = rawoperations,
			});
		}

		public virtual RawOperation SignDelistAccountForSale(uint account_target, uint account_signer, uint last_n_operation, decimal fee, byte[] payload = null, PayloadEncryptionMethod? payloadMethod = null, string pwd = null, string signer_b58_pubkey = null, string signer_enc_pubkey = null, string rawoperations = null) {
			return Invoke<RawOperation>(ApiMethodName.signdelistaccountforsale.ToString(), new Dictionary<string, object>() {
				["account_target"] = account_target,
				["account_signer"] = account_signer,
				["last_n_operation"] = last_n_operation,
				["fee"] = fee,
				["payload"] = payload.ToHexString(),
				["payloadMethod"] = ToPayloadMethodString(payloadMethod),
				["pwd"] = pwd,
				["signer_b58_pubkey"] = signer_b58_pubkey,
				["signer_enc_pubkey"] = signer_enc_pubkey,
				["rawoperations"] = rawoperations,
			});
		}

		public virtual RawOperation SignBuyAccount(uint buyer_account, uint account_to_purchase, decimal price, uint seller_account, string new_b58_pubkey, string new_enc_pubkey, decimal amount, uint last_n_operation, decimal fee, byte[] payload = null, PayloadEncryptionMethod? payloadMethod = null, string pwd = null, string signer_b58_pubkey = null, string signer_enc_pubkey = null, string rawoperations = null) {
			return Invoke<RawOperation>(ApiMethodName.signbuyaccount.ToString(), new Dictionary<string, object>() {
				["buyer_account"] = buyer_account,
				["account_to_purchase"] = account_to_purchase,
				["price"] = price,
				["seller_account"] = seller_account,
				["new_b58_pubkey"] = new_b58_pubkey,
				["new_enc_pubkey"] = new_enc_pubkey,
				["amount"] = amount,
				["last_n_operation"] = last_n_operation,
				["fee"] = fee,
				["payload"] = payload.ToHexString(),
				["payloadMethod"] = ToPayloadMethodString(payloadMethod),
				["pwd"] = pwd,
				["signer_b58_pubkey"] = signer_b58_pubkey,
				["signer_enc_pubkey"] = signer_enc_pubkey,
				["rawoperations"] = rawoperations,
			});
		}

		public virtual Operation[] OperationsInfo(string rawoperations) {
			return Invoke<Operation[]>(ApiMethodName.operationsinfo.ToString(), new Dictionary<string, object>() {
				["rawoperations"] = rawoperations
			});
		}

		public virtual Operation[] ExecuteOperations(string rawoperations) {
			return Invoke<Operation[]>(ApiMethodName.executeoperations.ToString(), new Dictionary<string, object>() {
				["rawoperations"] = rawoperations
			});
		}

		public virtual NodeStatus NodeStatus() {
			return Invoke<NodeStatus>(ApiMethodName.nodestatus.ToString());
		}

		public virtual string EncodePubKey(KeyType ec_nid, string x, string y) {
			return Invoke<string>(ApiMethodName.encodepubkey.ToString(), new Dictionary<string, object>() {
				["ec_nid"] = ec_nid,
				["x"] = x,
				["y"] = y,
			});
		}

		public virtual PublicKey DecodePubKey(string enc_pubkey, string b58_pubkey) {
			return Invoke<PublicKey>(ApiMethodName.decodepubkey.ToString(), new Dictionary<string, object>() {
				["enc_pubkey"] = enc_pubkey,
				["b58_pubkey"] = b58_pubkey,
			});
		}

		public string PayloadEncrypt(string payload, PayloadEncryptionMethod payload_method, string pwd) {
			return Invoke<string>(ApiMethodName.payloadencrypt.ToString(), new Dictionary<string, object>() {
				["payload"] = payload,
				["payload_method"] = ToPayloadMethodString(payload_method),
				["pwd"] = pwd
			});
		}

		public virtual DecryptedPayload PayloadDecrypt(string payload, string[] pwds = null) {
			return Invoke<DecryptedPayload>(ApiMethodName.payloaddecrypt.ToString(), new Dictionary<string, object>() {
				["payload"] = payload,
				["pwds"] = pwds,
			});
		}

		public virtual Connection[] GetConnections() {
			return Invoke<Connection[]>(ApiMethodName.getconnections.ToString());
		}

		public virtual PublicKey AddNewKey(KeyType ec_nid, string name) {
			return Invoke<PublicKey>(ApiMethodName.addnewkey.ToString(), new Dictionary<string, object>() {
				["ec_nid"] = ec_nid,
				["name"] = name,
			});
		}

		public virtual bool Lock() {
			return Invoke<bool>(ApiMethodName.@lock.ToString());
		}

		public virtual bool Unlock(string pwd) {
			return Invoke<bool>(ApiMethodName.unlock.ToString(), new Dictionary<string, object>() {
				["pwd"] = pwd
			});
		}

		public virtual bool SetWalletPassword(string pwd) {
			return Invoke<bool>(ApiMethodName.setwalletpassword.ToString(), new Dictionary<string, object>() {
				["pwd"] = pwd
			});
		}

		public virtual bool StopNode() {
			return Invoke<bool>(ApiMethodName.stopnode.ToString());
		}

		public virtual bool StartNode() {
			return Invoke<bool>(ApiMethodName.startnode.ToString());
		}

		public SignResult SignMessage(string digest, string b58_pubkey, string enc_pubkey) {
			return Invoke<SignResult>(ApiMethodName.signmessage.ToString(), new Dictionary<string, object>() {
				["digest"] = digest,
				["b58_pubkey"] = b58_pubkey,
				["enc_pubkey"] = enc_pubkey
			});
		}

		public SignResult VerifySign(string digest, string enc_pubkey, string signature) {
			return Invoke<SignResult>(ApiMethodName.verifysign.ToString(), new Dictionary<string, object>() {
				["digest"] = digest,
				["enc_pubkey"] = enc_pubkey,
				["signature"] = signature
			});
		}

		public RawMultiOperation MultiOperationAdd(string rawoperations, bool auto_n_operation, Sender[] senders, Receiver[] receivers, Changer[] changesinfo) {
			return Invoke<RawMultiOperation>(ApiMethodName.multioperationaddoperation.ToString(), new Dictionary<string, object>() {
				["rawoperations"] = rawoperations,
				["auto_n_operation"] = auto_n_operation,
				["senders"] = senders,
				["receivers"] = receivers,
				["changesinfo"] = changesinfo,
			});
		}

		public RawMultiOperation MultiOperationSignOffline(string rawoperations, AccountKey[] signers) {
			return Invoke<RawMultiOperation>(ApiMethodName.multioperationsignoffline.ToString(), new Dictionary<string, object>() {
				["rawoperations"] = rawoperations,
				["signers"] = signers
			});
		}

		public RawMultiOperation MultiOperationSignOnline(string rawoperations) {
			return Invoke<RawMultiOperation>(ApiMethodName.multioperationsignonline.ToString(), new Dictionary<string, object>() {
				["rawoperations"] = rawoperations
			});
		}

		public RawMultiOperation MultiOperationDeleteOperation(string rawoperations, int index) {
			return Invoke<RawMultiOperation>(ApiMethodName.multioperationdeleteoperation.ToString(), new Dictionary<string, object>() {
				["rawoperations"] = rawoperations,
				["index"] = index,
			});
		}


		protected virtual T Invoke<T>(string method, IDictionary<string, object> arguments = null) {
			using (var callScope = new CallScope(this)) {
				var webRequest = (HttpWebRequest)WebRequest.Create(_url);
				webRequest.ProtocolVersion = System.Net.HttpVersion.Version11;
				webRequest.UserAgent = $"NPascalCoin {Constants.NPascalCoinVersion}";
				webRequest.ContentType = "application/json-rpc";
				webRequest.Method = "POST";
				
				var request = new JObject {
					["jsonrpc"] = "2.0",
					["id"] = callScope.CallID,
					["method"] = method
				};

				if (arguments?.Count > 0) {
					var @params = new JObject();
					foreach (var param in arguments.Where(x => x.Value != null)) {
						var val = param.Value; //param.Value.GetType().IsEnum ? Convert.ChangeType(param.Value, Enum.GetUnderlyingType(param.Value.GetType())) : param.Value;
						@params.Add( new  JProperty(param.Key, val));
					}
					request.Add(new JProperty("params", @params));
				}

				var requestJson = JsonConvert.SerializeObject(request);
				Trace.WriteLine($"RPC Request:{Environment.NewLine}{requestJson}", TraceCategories.RPC);


				// serialize json for the request
				var requestBytes = System.Text.Encoding.UTF8.GetBytes(requestJson);
				webRequest.ContentLength = requestBytes.Length;
				using (var requestStream = webRequest.GetRequestStream()) {
					requestStream.Write(requestBytes, 0, requestBytes.Length);
				}
				try {
					using (var webResponse = webRequest.GetResponse()) {
						using (var responseStream = webResponse.GetResponseStream()) {
							using (var sr = new StreamReader(responseStream)) {
								var responseJson = sr.ReadToEnd();
								Trace.WriteLine($"RPC Response:{Environment.NewLine}{responseJson}", TraceCategories.RPC);
								var jsonResponse = JsonConvert.DeserializeObject<JsonRpcResponse>(responseJson);
								if (jsonResponse.Error != null) {
									throw new PascalCoinRPCException(JsonConvert.DeserializeObject<ErrorResult>(jsonResponse.Error.ToString()));
								}
								if (jsonResponse.RPCVersion != "2.0")
									throw new ApplicationException($"JSON-RPC response was an invalid version '{jsonResponse.RPCVersion ?? string.Empty}'. Expected '2.0'");
								if (jsonResponse.ID != callScope.CallID)
									throw new ApplicationException($"JSON-RPC response ID had invalid value '{jsonResponse.ID}'. Expected '{callScope.CallID}'");
								if (jsonResponse.Result == null)
									return default(T);
								if (jsonResponse.Result.GetType().IsPrimitive)
									return (T)Convert.ChangeType(jsonResponse.Result, typeof(T));
								if (jsonResponse.Result is JToken) {
									return JsonConvert.DeserializeObject<T>(((JToken)jsonResponse.Result).ToString());
								}
								if (jsonResponse.Result is ICollection<JToken>) { 
									if (!typeof(T).IsArray)
										throw new ArgumentException("Result returned an array but requesting type is not an array", nameof(T));

									var objArr = ((ICollection<JToken>) jsonResponse.Result).Select(x => JsonConvert.DeserializeObject(x.ToString(), typeof(T).GetElementType())).ToArray();
									var retArr = Array.CreateInstance(typeof(T).GetElementType(), objArr.Length);
									Array.Copy(objArr, retArr, objArr.Length);
									return (T) (object)retArr;


								}

								//if (jsonResponse.Result is JObject)
								//return ((JObject) jsonResponse.Result).ToString();

								return JsonConvert.DeserializeObject<T>(jsonResponse.Result.ToString());
							}
						}
					}
				} catch (WebException error) {
					if (error.Response == null)
						throw;
					var resp = new StreamReader(error.Response.GetResponseStream()).ReadToEnd();
					var errorDTO = JsonConvert.DeserializeObject<ErrorResult>(resp);
					throw new PascalCoinRPCException(errorDTO);
				}
			}
		}

		protected string ToPayloadMethodString(PayloadEncryptionMethod? payload) {
			return payload != null ? ToPayloadMethodString(payload.Value) : null;
		}

		protected string ToPayloadMethodString(PayloadEncryptionMethod payload) {
			switch (payload) {
				case PayloadEncryptionMethod.None:
					return "none";
				case PayloadEncryptionMethod.Dest:
					return "dest";
				case PayloadEncryptionMethod.Sender:
					return "sender";
				case PayloadEncryptionMethod.Aes:
					return "aes";
				default:
					throw new ArgumentOutOfRangeException(nameof(payload), payload, null);
			}
		}

		protected class CallScope : IDisposable {
			private readonly PascalCoinClient _client;
			public readonly int CallID;
			public CallScope(PascalCoinClient client) {
				_client = client;
				CallID = Interlocked.Increment(ref _client._callID);
			}

			public void Dispose() {
				Interlocked.Decrement(ref _client._callID);
			}
		}

		private class JsonRpcResponse {
			[JsonProperty("jsonrpc")]
			public string RPCVersion { get; set; }

			[JsonProperty("id")]
			public int ID { get; set; }

			[JsonProperty("result")]
			public object Result { get; set; }

			[JsonProperty("error")]
			public object Error { get; set; }
		}
	}
}
