using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPascalCoin.DTO;

namespace NPascalCoin {
	public static class PascalCoinClientExtensions {

		/// <summary>
		/// Adds a node to connect
		/// </summary>
		/// <param name="nodes">String containing 1 or multiple IP:port separated by "{ }"</param>
		/// <returns>Returns an integer with nodes added</returns>
		public static Task<int> AddNodeAsync(this IPascalCoinClient client, string nodes) {
			return Task.Run(() => client.AddNode(nodes));
		}

		/// <summary>
		/// Returns a JSON Object with account information including pending operations not included in blockchain yet, but affecting this account.		
		/// </summary>
		/// <remarks>
		/// To know if there are pending operations, must look at updated_b param.It tells last block that modified this account.If this number is equal to blockchain blocks then this account is affected by pending operations (send/receive or change key)
		/// </remarks>
		/// <param name="account">Cardinal containing account number</param>
		public static Task<AccountDTO> GetAccountAsync(this IPascalCoinClient client, uint account) {
			return Task.Run(() => client.GetAccount(account));
		}

		/// <summary>
		/// Returns a JSON array with all wallet accounts.
		/// </summary>
		/// <param name="enc_pubkey">HEXASTRING (optional). If provided, return only accounts of this public key</param>
		/// <param name="b58_pubkey">String (optional). If provided, return only accounts of this public key. Note: If use enc_pubkey and b58_pubkey together and is not the same public key, will return an error</param>
		/// <param name="start">Integer (optional, default = 0). If provided, will return wallet accounts starting at this position (index starts at position 0)</param>
		/// <param name="max">Integer (optional, default = 100). If provided, will return max accounts. If not provided, max=100 by default</param>
		/// <returns>Each JSON array item contains an Account Object</returns>
		public static Task<AccountDTO[]> GetWalletAccountsAsync(this IPascalCoinClient client, string enc_pubkey = null, string b58_pubkey = null, uint? start = null, uint? max = null) {
			return Task.Run(() => client.GetWalletAccounts(enc_pubkey, b58_pubkey, start, max));
		}

		/// <summary>
		/// Get number of available wallet accounts (total or filtered by public key)
		/// </summary>
		/// <param name="enc_pubkey">HEXASTRING (optional). If provided, return only accounts of this public key</param>
		/// <param name="b58_pubkey">String (optional). If provided, return only accounts of this public key. Note: If use enc_pubkey and b58_pubkey together and is not the same public key, will return an error</param>
		/// <param name="start">Integer (optional, default = 0). If provided, will return wallet accounts starting at this position (index starts at position 0)</param>
		/// <param name="max">Integer (optional, default = 100). If provided, will return max accounts. If not provided, max=100 by default</param>
		/// <returns>Returns an integer with total</returns>		
		public static Task<uint> GetWalletAccountsCountAsync(this IPascalCoinClient client, string enc_pubkey = null, string b58_pubkey = null, uint? start = null, uint? max = null) {
			return Task.Run(() => client.GetWalletAccountsCount());
		}

		/// <summary>
		/// Returns a JSON Object with a public key if found in the Wallet
		/// </summary>
		/// <param name="enc_pubkey">HEXASTRING</param>
		/// <param name="b58_pubkey">String</param>
		/// <remarks>Note: If use enc_pubkey and b58_pubkey together and is not the same public key, will return an error</remarks>
		/// <returns>Returns a JSON Object with a "Public Key Object"</returns>
		public static Task<PublicKeyDTO> GetWalletPubKeyAsync(this IPascalCoinClient client, string enc_pubkey = null, string b58_pubkey = null) {
			return Task.Run(() => client.GetWalletPubKey(enc_pubkey, b58_pubkey));
		}


		/// <summary>
		/// Returns a JSON Array with all pubkeys of the Wallet (address)
		/// </summary>
		/// <param name="start">Integer (optional, default = 0). If provided, will return wallet public keys starting at this position (index starts at position 0)</param>
		/// <param name="max">Integer (optional, default = 100). If provided, will return max public keys. If not provided, max=100 by default</param>
		/// <returns>Returns a JSON Array with "Public Key Object"</returns>
		public static Task<PublicKeyDTO[]> GetWalletPubKeysAsync(this IPascalCoinClient client, uint? start = null, uint? max = null) {
			return Task.Run(() => client.GetWalletPubKeys(start, max));
		}

		/// <summary>
		/// Returns coins balance.
		/// </summary>
		/// <param name="enc_pubkey">HEXASTRING (optional). If provided, return only this public key balance</param>
		/// <param name="b58_pubkey">String (optional). If provided, return only this public key balance  <remarks>If use enc_pubkey and b58_pubkey together and is not the same public key, will throw an error</remarks></param>
		/// <returns>Returns a PASCURRENCY value with maximum 4 decimals</returns>
		public static Task<decimal> GetWalletCoinsAsync(this IPascalCoinClient client, string enc_pubkey = null, string b58_pubkey = null) {
			return Task.Run(() => client.GetWalletCoins(enc_pubkey, b58_pubkey));
		}

		/// <summary>
		/// Returns a JSON Object with a block information
		/// </summary>
		/// <param name="block">Block number (0..blocks count-1)</param>
		/// <returns>Returns a JSON Object with a "Block Object"</returns>
		public static Task<BlockDTO> GetBlockAsync(this IPascalCoinClient client, uint block) {
			return Task.Run(() => client.GetBlock(block));
		}

		/// <summary>
		/// Returns a JSON Array with blocks information from "start" to "end" (or "last" n blocks) Blocks are returned in DESCENDING order. See <see cref="GetBlock"/>
		/// </summary>
		/// <param name="last">Last n blocks in the blockchain (n>0 and n&lt{ }=1000)</param>
		/// <param name="start">From this block</param>
		/// <param name="end">To this block</param>
		/// <remarks>Must use last exclusively, or start and end, or error</remarks>
		/// <returns></returns>
		public static Task<BlockDTO[]> GetBlocksAsync(this IPascalCoinClient client, uint? last = null, uint? start = null, uint? end = null) {
			return Task.Run(() => client.GetBlocks(last, start, end));
		}

		/// <summary>
		/// Returns an Integer with blockcount of node
		/// </summary>
		/// <returns>Total blocks</returns>
		public static Task<uint> GetBlockCountAsync(this IPascalCoinClient client) {
			return Task.Run(() => client.GetBlockCount());
		}

		/// <summary>
		/// Returns a JSON Object with an operation inside a block
		/// </summary>
		/// <param name="block">Block number</param>
		/// <param name="opblock">Operation (0..operations-1) of this block</param>
		/// <returns>JSON Object with a "Operation Object"</returns>
		public static Task<OperationDTO> GetBlockOperationAsync(this IPascalCoinClient client, uint block, uint opblock) {
			return Task.Run(() => client.GetBlockOperation(block, opblock));
		}

		/// <summary>
		/// Returns a JSON Array with all operations of specified block Operations are returned in DESCENDING order
		/// </summary>
		/// <param name="block">Block number</param>
		/// <param name="start">Integer (optional, default = 0). If provided, will start at this position (index starts at position 0)</param>
		/// <param name="max">Integer (optional, default = 100). If provided, will return max registers. If not provided, max=100 by default</param>
		/// <returns>Returns a JSON Array with "Operation Object" items</returns>
		public static Task<OperationDTO[]> GetBlockOperationsAsync(this IPascalCoinClient client, uint block, uint? start = null, uint? max = null) {
			return Task.Run(() => client.GetBlockOperations(block, start, max));
		}

		/// <summary>
		/// Return a JSON Array with "Operation Object" items. Operations made over an account Operations are returned in DESCENDING order
		/// </summary>
		/// <param name="account">Account number (0..accounts count-1)</param>
		/// <param name="depth">Integer - (Optional, default value 100) Depth to search on blocks where this account has been affected. Allowed to use deep as a param name too.</param>
		/// <param name="start">Integer (optional, default = 0). If provided, will start at this position (index starts at position 0). If start is -1, then will include pending operations, otherwise only operations included on the blockchain</param>
		/// <param name="max">Integer (optional, default = 100). If provided, will return max registers. If not provided, max=100 by default</param>
		/// <returns>Returns an array holding operations made over account in "Operation Object" format</returns>
		public static Task<OperationDTO[]> GetAccountOperationsAsync(this IPascalCoinClient client, uint account, uint? depth = null, uint? start = null, uint? max = null) {
			return Task.Run(() => client.GetAccountOperations(account, depth, start, max));
		}

		/// <summary>
		/// Return a JSON Array with "Operation Object" items with operations pending to be included at the Blockchain.
		/// </summary>
		/// <returns>Returns an array holding pending operations in "Operation Object" format</returns>
		public static Task<OperationDTO[]> GetPendingsAsync(this IPascalCoinClient client) {
			return Task.Run(() => client.GetPendings());
		}

		/// <summary>
		/// Return a JSON Object in "Operation Object" format.
		/// </summary>
		/// <param name="ophash">HEXASTRING - Value ophash received on an operation</param>
		/// <returns>Returns "Operation Object" format JSON object</returns>
		public static Task<OperationDTO> FindOperationAsync(this IPascalCoinClient client, string ophash) {
			return Task.Run(() => client.FindOperation(ophash));
		}


		/// <summary>
		/// Executes a transaction operation from "sender" to "target"
		/// </summary>
		/// <param name="sender">Sender account</param>
		/// <param name="target">Destination account</param>
		/// <param name="amount">Coins to be transferred</param>
		/// <param name="fee">Fee of the operation</param>
		/// <param name="payloadMethod">Payload "item" that will be included in this operation</param>
		/// <param name="pwd">Used to encrypt payload with aes as a payload_method. If none equals to empty password</param>
		/// <returns>If transaction is successfull will return a JSON Object in "Operation Object" format. Otherwise, will return a JSON-RPC error code with description</returns>
		public static Task<OperationDTO> SendToAsync(this IPascalCoinClient client, uint sender, uint target, decimal amount, decimal fee, PayloadMethod? payloadMethod = null, string pwd = null) {
			return Task.Run(() => client.SendTo(sender, target, amount, fee, payloadMethod, pwd));
		}


		/// <summary>
		/// Executes a change key operation, changing "account" public key for a new one.
		/// </summary>
		/// <remarks>Note that new one public key can be another Wallet public key, or none.When none, it's like a transaction, tranferring account owner to an external owner</remarks>
		/// <param name="account">Account number to change key</param>
		/// <param name="new_enc_pubkey">HEXASTRING - New public key in encoded format</param>
		/// <param name="new_b58_pubkey">New public key in Base 58 format (the same that Application Wallet exports)</param>
		/// <param name="fee">PASCURRENCY - Fee of the operation</param>
		/// <param name="payload">HEXASTRING - Payload "item" that will be included in this operation</param>
		/// <param name="payloadMethod">Encode type of the item payload</param>
		/// <param name="pwd">Used to encrypt payload with aes as a payload_method. If none equals to empty password</param>
		/// <returns>If operation is successfull will return a JSON Object in "Operation Object" format. Otherwise, will return a JSON-RPC error code with description</returns>
		public static Task<OperationDTO> ChangeKeyAsync(this IPascalCoinClient client, uint account, string new_enc_pubkey, string new_b58_pubkey, decimal fee, string payload, PayloadMethod? payloadMethod = null, string pwd = null) {
			return Task.Run(() => client.ChangeKey(account, new_enc_pubkey, new_b58_pubkey, fee, payload, payloadMethod, pwd));
		}


		/// <summary>
		/// Executes a change key operation, changing "account" public key for a new one, in multiple accounts Works like changekey
		/// </summary>
		/// <param name="accounts">List of accounts separated by a comma</param>
		/// <param name="new_enc_pubkey">HEXASTRING - New public key in encoded format</param>
		/// <param name="new_b58_pubkey">New public key in Base 58 format (the same that Application Wallet exports)</param>
		/// <param name="fee">PASCURRENCY - Fee of the operation</param>
		/// <param name="payload">HEXASTRING - Payload "item" that will be included in this operation</param>
		/// <param name="payloadMethod">Encode type of the item payload</param>
		/// <param name="pwd">Used to encrypt payload with aes as a payload_method. If none equals to empty password</param>
		/// <returns>If operation is successfull will return a JSON Array with Operation object items for each key If operation cannot be made, a JSON-RPC error message is returned</returns>
		public static Task<OperationDTO[]> ChangeKeysAsync(this IPascalCoinClient client, string accounts, string new_enc_pubkey, string new_b58_pubkey, decimal fee, string payload, PayloadMethod? payloadMethod = null, string pwd = null) {
			return Task.Run(() => client.ChangeKeys(accounts, new_enc_pubkey, new_b58_pubkey, fee, payload, payloadMethod, pwd));
		}


		/// <summary>
		/// Creates and signs a "Send to" operation without checking information and without transfering to the network. It's usefull for "cold wallets" that are off-line (not synchronized with the network) and only holds private keys
		/// </summary>		
		/// <param name="sender">Sender account</param>
		/// <param name="target">Target account</param>
		/// <param name="sender_enc_pubkey">HEXASTRING - Public key of the sender account in encoded format</param>
		/// <param name="sender_b58_pubkey">HEXASTRING - Public key of the sender account in base58 format</param>
		/// <param name="target_enc_pubkey">HEXASTRING - Public key of the target account in encoded format</param>
		/// <param name="target_b58_pubkey">HEXASTRING - Public key of the target account in base58 format</param>
		/// <param name="last_n_operation">Last value of n_operation obtained with an Account object, for example when called to getaccount</param>
		/// <param name="amount">Coins to be transferred</param>
		/// <param name="fee">Fee of the operation</param>
		/// <param name="payloadMethod">Payload "item" that will be included in this operation</param>
		/// <param name="pwd">Used to encrypt payload with aes as a payload_method. If none equals to empty password</param>
		/// <param name="rawoperations">HEXASTRING (optional) - If we want to add a sign operation with other previous operations, here we must put previous rawoperations result</param>
		/// <remarks>Wallet must be unlocked and sender private key (searched with provided public key) must be in wallet. No other checks are made (no checks for valid target, valid n_operation, valid amount or fee ...)</remarks>
		/// <remarks>Only one of sender_enc_pubkey, sender_b58_pubkey needs be provided</remarks>
		/// <remarks>Only one of target_enc_pubkey, target_b58_pubkey needs be provided</remarks>
		/// <returns>Returns a Raw Operations Object</returns>
		public static Task<RawOperationDTO> SignSendToAsync(this IPascalCoinClient client, uint sender, uint target, string sender_enc_pubkey, string sender_b58_pubkey, string target_enc_pubkey, string target_b58_pubkey, uint last_n_operation, decimal amount, decimal fee, PayloadMethod? payloadMethod = null, string pwd = null, string rawoperations = null) {
			return Task.Run(() => client.SignSendTo(sender, target, sender_enc_pubkey, sender_b58_pubkey, target_enc_pubkey, target_b58_pubkey, last_n_operation, amount, fee, payloadMethod, pwd, rawoperations));
		}


		/// <summary>
		/// Creates and signs a "Change key" operation without checking information and without transfering to the network. It's usefull for "cold wallets" that are off-line (not synchronized with the network) and only holds private keys
		/// </summary>
		/// <param name="account">Account number to change key</param>
		/// <param name="old_enc_pubkey">HEXASTRING - Public key of the account in encoded format</param>
		/// <param name="old_b58_pubkey">HEXASTRING - Public key of the account in base58 format</param>
		/// <param name="new_enc_pubkey">HEXASTRING - Public key of the new key for the account in encoded format</param>
		/// <param name="new_b58_pubkey">HEXASTRING - Public key of the new key for the account in base58 format</param>
		/// <param name="last_n_operation">Last value of n_operation obtained with an Account object, for example when called to getaccount</param>
		/// <param name="fee">Fee of the operation</param>
		/// <param name="payloadMethod">Payload "item" that will be included in this operation</param>
		/// <param name="pwd">Used to encrypt payload with aes as a payload_method. If none equals to empty password</param>
		/// <param name="rawoperations">HEXASTRING (optional) - If we want to add a sign operation with other previous operations, here we must put previous rawoperations result</param>
		/// <remarks>Wallet must be unlocked and private key (searched with provided public key) must be in wallet. No other checks are made (no checks for valid n_operation, valid fee ...) </remarks>
		/// <remarks>Only one of old_enc_pubkey, old_b58_pubkey needs be provided</remarks>
		/// <remarks>Only one of new_enc_pubkey, new_b58_pubkey needs be provided</remarks>
		/// <returns>Wallet must be unlocked and private key (searched with provided public key) must be in wallet. No other checks are made (no checks for valid n_operation, valid fee ...) Returns a Raw Operations Object</returns>
		public static Task<RawOperationDTO> SignChangeKeyAsync(this IPascalCoinClient client, uint account, string old_enc_pubkey, string old_b58_pubkey, string new_enc_pubkey, string new_b58_pubkey, uint last_n_operation, decimal fee, PayloadMethod? payloadMethod = null, string pwd = null, string rawoperations = null) {
			return Task.Run(() => client.SignChangeKey(account, old_enc_pubkey, old_b58_pubkey, new_enc_pubkey, new_b58_pubkey, last_n_operation, fee, payloadMethod, pwd, rawoperations));
		}

		/// <summary>
		/// Returns information stored in a rawoperations param (obtained calling signchangekey or signsendto)
		/// </summary>
		/// <param name="rawoperations">HEXASTRING (obtained calling signchangekey or signsendto)</param>
		/// <returns>Returns a JSON Array with Operation Object items, one for each operation in rawoperations param. NOTE: Remember that rawoperations are operations that maybe are not correct</returns>
		public static Task<OperationDTO[]> OperationsInfoAsync(this IPascalCoinClient client, string rawoperations) {
			return Task.Run(() => client.OperationsInfo(rawoperations));
		}

		/// <summary>
		/// Executes operations included in rawopertions param and transfers to the network. Raw operations can include "Send to" oprations or "Change key" operations.
		/// </summary>
		/// <param name="rawoperations">Executes operations included in rawopertions param and transfers to the network. Raw operations can include "Send to" oprations or "Change key" operations.</param>
		/// <remarks>For each Operation Object item, if there is an error, param valid will be false and param errors will show error description.Otherwise, operation is correct and will contain ophash param</remarks>
		/// <returns>Returns a JSON Array with Operation Object items, one for each operation in rawoperations param.</returns>
		public static Task<OperationDTO[]> ExecuteOperationsAsync(this IPascalCoinClient client, string rawoperations) {
			return Task.Run(() => client.ExecuteOperations(rawoperations));
		}


		/// <summary>
		/// Returns information of the Node in a JSON Object
		/// </summary>
		/// <returns>JSON Object with information</returns>
		public static Task<NodeStatusDTO> NodeStatusAsync(this IPascalCoinClient client) {
			return Task.Run(() => client.NodeStatus());
		}


		/// <summary>
		/// Encodes a public key based on params information
		/// </summary>
		/// <param name="ec_nid">key type</param>
		/// <param name="x">HEXASTRING with x value of public key</param>
		/// <param name="y">HEXASTRING with y value of public key</param>
		/// <returns>Returns a HEXASTRING with encoded public key</returns>
		public static Task<string> EncodePubKeyAsync(this IPascalCoinClient client, KeyType ec_nid, string x, string y) {
			return Task.Run(() => client.EncodePubKey(ec_nid, x, y));
		}


		/// <summary>
		/// Decodes an encoded public key
		/// </summary>
		/// <param name="enc_pubkey">HEXASTRING with encoded public key</param>
		/// <param name="b58_pubkey">String. b58_pubkey is the same value that Application Wallet exports as a public key</param>
		/// <remarks>Note: If use enc_pubkey and b58_pubkey together and is not the same public key, will return an error</remarks>
		/// <returns>Returns a JSON Object with a "Public Key Object"</returns>
		public static Task<PublicKeyDTO> DecodePubKeyAsync(this IPascalCoinClient client, string enc_pubkey, string b58_pubkey) {
			return Task.Run(() => client.DecodePubKey(enc_pubkey, b58_pubkey));
		}


		/// <summary>
		/// Encrypt a text "paylad" using "payload_method"
		/// </summary>
		/// <param name="payload">HEXASTRING - Text to encrypt in hexadecimal format</param>
		/// <param name="payload_method">Payload method</param>
		/// <param name="pwd">Using a Password. Must provide pwd param</param>
		/// <returns>Returns a HEXASTRING with encrypted payload</returns>
		public static Task<string> PayloadEncryptAsync(this IPascalCoinClient client, string payload, PayloadMethod payload_method, string pwd) {
			return Task.Run(() => client.PayloadEncrypt(payload, payload_method, pwd));
		}


		/// <summary>
		/// Returns a HEXASTRING with decrypted text (a payload) using private keys in the wallet or a list of Passwords (used in "aes" encryption)
		/// </summary>
		/// <param name="payload">HEXASTRING - Encrypted data</param>
		/// <param name="pwds">List of passwords to use</param>
		/// <remarks>If using one of private keys is able to decrypt payload then returns value "key" in payload_method and enc_pubkey contains encoded public key in HEXASTRING</remarks>
		/// <remarks>If using one of passwords to decrypt payload then returns value "pwd" in payload_method and pwd contains password used</remarks>
		/// <returns>Decryped payload</returns>
		public static Task<DecryptedPayloadDTO> PayloadDecryptAsync(this IPascalCoinClient client, string payload, string[] pwds = null) {
			return Task.Run(() => client.PayloadDecrypt(payload, pwds));
		}

		/// <summary>
		/// Returns all the current connections
		/// </summary>
		/// <returns>JSON Array with Connection Objects</returns>
		public static Task<ConnectionDTO[]> GetConnectionsAsync(this IPascalCoinClient client) {
			return Task.Run(() => client.GetConnections());
		}


		/// <summary>
		/// Creates a new Private key and sotres it on the wallet, returning an enc_pubkey value
		/// </summary>
		/// <param name="ec_nid">Type of key encryption</param>
		/// <param name="name">Name to alias this new private key</param>
		/// <returns></returns>
		public static Task<PublicKeyDTO> AddNewKeyAsync(this IPascalCoinClient client, KeyType ec_nid, string name) {
			return Task.Run(() => client.AddNewKey(ec_nid, name));
		}

		/// <summary>
		/// Locks the Wallet if it has a password, otherwise wallet cannot be locked
		/// </summary>
		/// <returns>Returns a Boolean indicating if Wallet is locked. If false that means that Wallet has an empty password and cannot be locked</returns>
		public static Task<bool> LockAsync(this IPascalCoinClient client) {
			return Task.Run(() => client.Lock());
		}

		/// <summary>
		/// Unlocks a locked Wallet using "pwd" param
		/// </summary>
		/// <param name="pwd"></param>
		/// <returns>Returns a Boolean indicating if Wallet is unlocked after using pwd password</returns>
		public static Task<bool> UnlockAsync(this IPascalCoinClient client, string pwd) {
			return Task.Run(() => client.Unlock(pwd));
		}


		/// <summary>
		/// Changes the password of the Wallet. (Must be previously unlocked) Note: If pwd param is empty string, then wallet will be not protected by password
		/// </summary>
		/// <param name="pwd">New password</param>
		/// <returns>Returns a Boolean if Wallet password changed with new pwd password</returns>
		public static Task<bool> SetWalletPasswordAsync(this IPascalCoinClient client, string pwd) {
			return Task.Run(() => client.SetWalletPassword(pwd));
		}

		/// <summary>
		/// Stops the node and the server. Closes all connections
		/// </summary>
		/// <returns></returns>
		public static Task<bool> StopNodeAsync(this IPascalCoinClient client) {
			return Task.Run(() => client.StopNode());
		}

		/// <summary>
		/// Starts the node and the server. Starts connection process
		/// </summary>
		/// <returns>Boolean "true"</returns>
		public static Task<bool> StartNodeAsync(this IPascalCoinClient client) {
			return Task.Run(() => client.StartNode());
		}

	}
}
