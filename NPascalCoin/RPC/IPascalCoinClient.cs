using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPascalCoin.DTO;

namespace NPascalCoin {
	public interface IPascalCoinClient {

		/// <summary>
		/// Adds a node to connect
		/// </summary>
		/// <param name="nodes">String containing 1 or multiple IP:port separated by ";"</param>
		/// <returns>Returns an integer with nodes added</returns>
		int AddNode(string nodes);

		/// <summary>
		/// Returns a JSON Object with account information including pending operations not included in blockchain yet, but affecting this account.		
		/// </summary>
		/// <remarks>
		/// To know if there are pending operations, must look at updated_b param.It tells last block that modified this account.If this number is equal to blockchain blocks then this account is affected by pending operations (send/receive or change key)
		/// </remarks>
		/// <param name="account">Cardinal containing account number</param>
		AccountDTO GetAccount(uint account);

		/// <summary>
		/// Find accounts by name/type and returns them as an array of Account objects
		/// </summary>
		/// <param name="name">If has value, will return the account that match name</param>
		/// <param name="type">If has value, will return accounts with same type</param>
		/// <param name="status">If has value, will filter account with status as follows: 0 = all accounts, 1 = accounts for public or private sale only, 2 = accounts for private sale only, 3 = accounts for public sale only</param>
		/// <param name="start">Start account (by default, 0)</param>
		/// <param name="max">Max of accounts returned in array (by default, 100)</param>
		/// <returns></returns>
		AccountDTO[] FindAccounts(string name, uint? type = null, uint? status = null, uint? start = null, uint? max = null);

		/// <summary>
		/// Returns a JSON array with all wallet accounts.
		/// </summary>
		/// <param name="enc_pubkey">HEXASTRING (optional). If provided, return only accounts of this public key</param>
		/// <param name="b58_pubkey">String (optional). If provided, return only accounts of this public key. Note: If use enc_pubkey and b58_pubkey together and is not the same public key, will return an error</param>
		/// <param name="start">Integer (optional, default = 0). If provided, will return wallet accounts starting at this position (index starts at position 0)</param>
		/// <param name="max">Integer (optional, default = 100). If provided, will return max accounts. If not provided, max=100 by default</param>
		/// <returns>Each JSON array item contains an Account Object</returns>
		AccountDTO[] GetWalletAccounts(string enc_pubkey = null, string b58_pubkey = null, int? start = null, int? max = null);

		/// <summary>
		/// Get number of available wallet accounts (total or filtered by public key)
		/// </summary>
		/// <param name="enc_pubkey">HEXASTRING (optional). If provided, return only accounts of this public key</param>
		/// <param name="b58_pubkey">String (optional). If provided, return only accounts of this public key. Note: If use enc_pubkey and b58_pubkey together and is not the same public key, will return an error</param>
		/// <param name="start">Integer (optional, default = 0). If provided, will return wallet accounts starting at this position (index starts at position 0)</param>
		/// <param name="max">Integer (optional, default = 100). If provided, will return max accounts. If not provided, max=100 by default</param>
		/// <returns>Returns an integer with total</returns>		
		uint GetWalletAccountsCount(string enc_pubkey = null, string b58_pubkey = null, int? start = null, int? max = null);

		/// <summary>
		/// Returns a JSON Object with a public key if found in the Wallet
		/// </summary>
		/// <param name="enc_pubkey">HEXASTRING</param>
		/// <param name="b58_pubkey">String</param>
		/// <remarks>Note: If use enc_pubkey and b58_pubkey together and is not the same public key, will return an error</remarks>
		/// <returns>Returns a JSON Object with a "Public Key Object"</returns>
		PublicKeyDTO GetWalletPubKey(string enc_pubkey = null, string b58_pubkey = null);

		/// <summary>
		/// Returns a JSON Array with all pubkeys of the Wallet (address)
		/// </summary>
		/// <param name="start">Integer (optional, default = 0). If provided, will return wallet public keys starting at this position (index starts at position 0)</param>
		/// <param name="max">Integer (optional, default = 100). If provided, will return max public keys. If not provided, max=100 by default</param>
		/// <returns>Returns a JSON Array with "Public Key Object"</returns>
		PublicKeyDTO[] GetWalletPubKeys(int? start = null, int? max = null);

		/// <summary>
		/// Returns coins balance.
		/// </summary>
		/// <param name="enc_pubkey">HEXASTRING (optional). If provided, return only this public key balance</param>
		/// <param name="b58_pubkey">String (optional). If provided, return only this public key balance  <remarks>If use enc_pubkey and b58_pubkey together and is not the same public key, will throw an error</remarks></param>
		/// <returns>Returns a PASCURRENCY value with maximum 4 decimals</returns>
		decimal GetWalletCoins(string enc_pubkey = null, string b58_pubkey = null);

		/// <summary>
		/// Returns a JSON Object with a block information
		/// </summary>
		/// <param name="block">Block number (0..blocks count-1)</param>
		/// <returns>Returns a JSON Object with a "Block Object"</returns>
		BlockDTO GetBlock(uint block);

		/// <summary>
		/// Returns a JSON Array with blocks information from "start" to "end" (or "last" n blocks) Blocks are returned in DESCENDING order. See <see cref="GetBlock"/>
		/// </summary>
		/// <param name="last">Last n blocks in the blockchain (n>0 and n&lt;=1000)</param>
		/// <param name="start">From this block</param>
		/// <param name="end">To this block</param>
		/// <remarks>Must use last exclusively, or start and end, or error</remarks>
		/// <returns></returns>
		BlockDTO[] GetBlocks(uint? last = null, int? start = null, int? end = null);

		/// <summary>
		/// Returns an Integer with blockcount of node
		/// </summary>
		/// <returns>Total blocks</returns>
		uint GetBlockCount();

		/// <summary>
		/// Returns a JSON Object with an operation inside a block
		/// </summary>
		/// <param name="block">Block number</param>
		/// <param name="opblock">Operation (0..operations-1) of this block</param>
		/// <returns>JSON Object with a "Operation Object"</returns>
		OperationDTO GetBlockOperation(uint block, uint opblock);

		/// <summary>
		/// Returns a JSON Array with all operations of specified block Operations are returned in DESCENDING order
		/// </summary>
		/// <param name="block">Block number</param>
		/// <param name="start">Integer (optional, default = 0). If provided, will start at this position (index starts at position 0)</param>
		/// <param name="max">Integer (optional, default = 100). If provided, will return max registers. If not provided, max=100 by default</param>
		/// <returns>Returns a JSON Array with "Operation Object" items</returns>
		OperationDTO[] GetBlockOperations(uint block, int? start = null, int? max = null);

		/// <summary>
		/// Return a JSON Array with "Operation Object" items. Operations made over an account Operations are returned in DESCENDING order
		/// </summary>
		/// <param name="account">Account number (0..accounts count-1)</param>
		/// <param name="depth">Integer - (Optional, default value 100) Depth to search on blocks where this account has been affected. Allowed to use deep as a param name too.</param>
		/// <param name="start">Integer (optional, default = 0). If provided, will start at this position (index starts at position 0). If start is -1, then will include pending operations, otherwise only operations included on the blockchain</param>
		/// <param name="max">Integer (optional, default = 100). If provided, will return max registers. If not provided, max=100 by default</param>
		/// <returns>Returns an array holding operations made over account in "Operation Object" format</returns>
		OperationDTO[] GetAccountOperations(uint account, int? depth = null, int? start = null, int? max = null);

		/// <summary>
		/// Return a JSON Array with "Operation Object" items with operations pending to be included at the Blockchain.
		/// </summary>
		/// <returns>Returns an array holding pending operations in "Operation Object" format</returns>
		OperationDTO[] GetPendings();

		/// <summary>
		/// Return a JSON Object in "Operation Object" format.
		/// </summary>
		/// <param name="ophash">HEXASTRING - Value ophash received on an operation</param>
		/// <returns>Returns "Operation Object" format JSON object</returns>
		OperationDTO FindOperation(string ophash);

		/// <summary>
		/// Changes an account Public key, or name, or type value (at least 1 on 3)
		/// </summary>
		/// <param name="account_target">Account being changed</param>
		/// <param name="account_signer">Account paying the fee (must have same public key as account_target)</param>
		/// <param name="new_enc_pubkey">New account public key encoded in hexadecimal format</param>
		/// <param name="new_b58_pubkey">New account public key encoded in base58 format</param>
		/// <param name="new_name">New account name encoded in PascalCoin64 format (null means keep current name)</param>
		/// <param name="new_type">New account type (null means keep current type)</param>
		/// <param name="fee">PASCURRENCY - Fee of the operation</param>
		/// <param name="payload">Payload "item" that will be included in this operation</param>
		/// <param name="payloadMethod">Encode type of the item payload</param>
		/// <param name="pwd">Used to encrypt payload with aes as a payload_method. If none equals to empty password</param>
		/// <remarks>Only one or none of new_b58_pubkey, new_enc_pubkey should be used. Populating both will result in an error.</remarks>
		/// <returns>If operation is successfull will return a JSON Object in "Operation Object" format.</returns>
		OperationDTO ChangeAccountInfo(uint account_target, uint account_signer, string new_enc_pubkey, string new_b58_pubkey, string new_name, ushort? new_type, decimal fee, byte[] payload = null, PayloadEncryptionMethod? payloadMethod = null, string pwd = null);

		/// <summary>
		/// Executes a transaction operation from "sender" to "target"
		/// </summary>
		/// <param name="sender">Sender account</param>
		/// <param name="target">Destination account</param>
		/// <param name="amount">Coins to be transferred</param>
		/// <param name="fee">Fee of the operation</param>
		/// <param name="payload">Payload "item" that will be included in this operation</param>
		/// <param name="payloadMethod">Payload "item" that will be included in this operation</param>
		/// <param name="pwd">Used to encrypt payload with aes as a payload_method. If none equals to empty password</param>
		/// <returns>If transaction is successfull will return a JSON Object in "Operation Object" format. Otherwise, will return a JSON-RPC error code with description</returns>
		OperationDTO SendTo(uint sender, uint target, decimal amount, decimal fee, byte[] payload = null, PayloadEncryptionMethod? payloadMethod = null, string pwd = null);

		/// <summary>
		/// Executes a change key operation, changing "account" public key for a new one.
		/// </summary>
		/// <remarks>Note that new one public key can be another Wallet public key, or none.When none, it's like a transaction, tranferring account owner to an external owner</remarks>
		/// <param name="account">Account number to change key</param>
		/// <param name="account_signer">Account that signs and pays the fee (must have same public key that delisted account, or be the same)</param>
		/// <param name="new_enc_pubkey">HEXASTRING - New public key in encoded format</param>
		/// <param name="new_b58_pubkey">New public key in Base 58 format (the same that Application Wallet exports)</param>
		/// <param name="fee">PASCURRENCY - Fee of the operation</param>
		/// <param name="payload">Payload "item" that will be included in this operation</param>
		/// <param name="payloadMethod">Encode type of the item payload</param>
		/// <param name="pwd">Used to encrypt payload with aes as a payload_method. If none equals to empty password</param>
		/// <remarks>Only one or none of new_b58_pubkey, new_enc_pubkey should be used. Populating both will result in an error.</remarks>
		/// <returns>If operation is successfull will return a JSON Object in "Operation Object" format.</returns>
		OperationDTO ChangeKey(uint account, uint account_signer, string new_enc_pubkey, string new_b58_pubkey, decimal fee, byte[] payload = null, PayloadEncryptionMethod? payloadMethod = null, string pwd = null);

		/// <summary>
		/// Executes a change key operation, changing "account" public key for a new one, in multiple accounts Works like changekey
		/// </summary>
		/// <param name="accounts">List of accounts separated by a comma</param>
		/// <param name="new_enc_pubkey">HEXASTRING - New public key in encoded format</param>
		/// <param name="new_b58_pubkey">New public key in Base 58 format (the same that Application Wallet exports)</param>
		/// <param name="fee">PASCURRENCY - Fee of the operation</param>
		/// <param name="payload">Payload "item" that will be included in this operation</param>
		/// <param name="payloadMethod">Encode type of the item payload</param>
		/// <param name="pwd">Used to encrypt payload with aes as a payload_method. If none equals to empty password</param>
		/// <returns>If operation is successfull will return a JSON Array with Operation object items for each key If operation cannot be made, a JSON-RPC error message is returned</returns>
		OperationDTO[] ChangeKeys(string accounts, string new_enc_pubkey, string new_b58_pubkey, decimal fee, byte[] payload = null, PayloadEncryptionMethod? payloadMethod = null, string pwd = null);

		/// <summary>
		/// Lists an account for sale (public or private)
		/// </summary>
		/// <param name="account_target">Account to be listed</param>
		/// <param name="account_signer">Account that signs and pays the fee (must have same public key that listed account, or be the same)</param>
		/// <param name="price">price account can be purchased for</param>
		/// <param name="seller_account">Account that will receive "price" amount on sell</param>
		/// <param name="new_b58_pubkey">Base58 encoded public key (for private sale only)</param>
		/// <param name="new_enc_pubkey">Hex-encoded public key (for private sale only)</param>
		/// <param name="locked_until_block">Block number until this account will be locked (a locked account cannot execute operations while locked)</param>
		/// <param name="fee">PASCURRENCY - Fee of the operation</param>
		/// <param name="payload">Payload "item" that will be included in this operation</param>
		/// <param name="payloadMethod">Encode type of the item payload</param>
		/// <param name="pwd">Used to encrypt payload with aes as a payload_method. If none equals to empty password</param>
		/// <remarks>Only one or none of new_b58_pubkey, new_enc_pubkey should be used. Populating both will result in an error.</remarks>
		/// <returns>If operation is successfull will return a JSON Object in "Operation Object" format.</returns>
		OperationDTO ListAccountForSale(uint account_target, uint account_signer, decimal price, uint seller_account, string new_b58_pubkey, string new_enc_pubkey, uint locked_until_block, decimal fee, byte[] payload = null, PayloadEncryptionMethod? payloadMethod = null, string pwd = null);

		/// <summary>
		///  Delist an account from sale.
		/// </summary>
		/// <param name="account_target">Account to be delisted</param>
		/// <param name="account_signer">Account that signs and pays the fee (must have same public key that delisted account, or be the same)</param>
		/// <param name="fee">PASCURRENCY - Fee of the operation</param>
		/// <param name="payload">Payload "item" that will be included in this operation</param>
		/// <param name="payloadMethod">Encode type of the item payload</param>
		/// <param name="pwd">Used to encrypt payload with aes as a payload_method. If none equals to empty password</param>
		/// <returns>If operation is successfull will return a JSON Object in "Operation Object" format.</returns>
		OperationDTO DelistAccountForSale(uint account_target, uint account_signer, decimal fee, byte[] payload = null, PayloadEncryptionMethod? payloadMethod = null, string pwd = null);

		/// <summary>
		/// Buy an account currently listed for sale (public or private)
		/// </summary>
		/// <param name="buyer_account">Account number of buyer who is purchasing the account</param>
		/// <param name="account_to_purchase">Account number being purchased</param>
		/// <param name="price">Settlement price of account being purchased</param>
		/// <param name="seller_account">Account of seller, receiving payment</param>
		/// <param name="new_b58_pubkey">Post-settlement public key in base58 encoded format.</param>
		/// <param name="new_enc_pubkey">Post-settlement public key in hexadecimal encoded format.</param>
		/// <param name="amount">Amount being transferred from buyer_account to seller_account (the settlement). This is a PASCURRENCY value.</param>
		/// <param name="fee">Fee of the operation. This is a PASCURRENCY value.</param>
		/// <param name="payload">Payload "item" that will be included in this operation</param>
		/// <param name="payloadMethod">Encode type of the item payload</param>
		/// <param name="pwd">Used to encrypt payload with aes as a payload_method. If none equals to empty password</param>
		/// <returns>If operation is successfull will return a JSON Object in "Operation Object" format.</returns>
		OperationDTO BuyAccount(uint buyer_account, uint account_to_purchase, decimal price, uint seller_account, string new_b58_pubkey, string new_enc_pubkey, decimal amount, decimal fee, byte[] payload = null, PayloadEncryptionMethod? payloadMethod = null, string pwd = null);

		/// <summary>
		/// Signs a "Change Account Info" operation, suitable for cold wallet usage.
		/// </summary>
		/// <param name="account_target">Account being changed</param>
		/// <param name="account_signer">Account paying the fee (must have same public key as account_target)</param>
		/// <param name="new_enc_pubkey">New account public key encoded in hexadecimal format</param>
		/// <param name="new_b58_pubkey">New account public key encoded in base58 format</param>
		/// <param name="new_name">New account name encoded in PascalCoin64 format (null means keep current name)</param>
		/// <param name="new_type">New account type (null means keep current type)</param>
		/// <param name="last_n_operation">Last value of n_operation obtained with an Account object, for example when called to getaccount</param>
		/// <param name="fee">PASCURRENCY - Fee of the operation</param>
		/// <param name="payload">Payload "item" that will be included in this operation</param>
		/// <param name="payloadMethod">Encode type of the item payload</param>
		/// <param name="pwd">Used to encrypt payload with aes as a payload_method. If none equals to empty password</param>
		/// <param name="signer_b58_pubkey">The current public key of "account_signer" in base58 encoding</param> 
		/// <param name="signer_enc_pubkey">The current public key of "account_signer" in hexadecimal encoding</param> 
		/// <param name="rawoperations">HEXASTRING (optional) - If we want to add a sign operation with other previous operations, here we must put previous rawoperations result</param>
		/// <remarks>Only one or none of new_b58_pubkey, new_enc_pubkey should be used. Populating both will result in an error.</remarks>
		/// <returns>Returns a Raw Operations Object</returns>
		OperationDTO SignChangeAccountInfo(uint account_target, uint account_signer, string new_enc_pubkey, string new_b58_pubkey, string new_name, ushort? new_type, uint last_n_operation, decimal fee, byte[] payload = null, PayloadEncryptionMethod? payloadMethod = null, string pwd = null, string signer_b58_pubkey = null, string signer_enc_pubkey = null, string rawoperations = null);

		/// <summary>
		/// Creates and signs a "Send to" operation without checking information and without transfering to the network. It's useful for "cold wallets" that are off-line (not synchronized with the network) and only holds private keys
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
		/// <param name="payload">Payload "item" that will be included in this operation</param>
		/// <param name="payloadMethod">Encode type of the item payload</param>
		/// <param name="pwd">Used to encrypt payload with aes as a payload_method. If none equals to empty password</param>
		/// <param name="rawoperations">HEXASTRING (optional) - If we want to add a sign operation with other previous operations, here we must put previous rawoperations result</param>
		/// <remarks>Wallet must be unlocked and sender private key (searched with provided public key) must be in wallet. No other checks are made (no checks for valid target, valid n_operation, valid amount or fee ...)</remarks>
		/// <remarks>Only one of sender_enc_pubkey, sender_b58_pubkey needs be provided</remarks>
		/// <remarks>Only one of target_enc_pubkey, target_b58_pubkey needs be provided</remarks>
		/// <returns>Returns a Raw Operations Object</returns>
		RawOperationDTO SignSendTo(uint sender, uint target, string sender_enc_pubkey, string sender_b58_pubkey, string target_enc_pubkey, string target_b58_pubkey, uint last_n_operation, decimal amount, decimal fee, byte[] payload = null, PayloadEncryptionMethod? payloadMethod = null, string pwd = null, string rawoperations = null);

		/// <summary>
		/// Creates and signs a "Change key" operation without checking information and without transfering to the network. It's useful for "cold wallets" that are off-line (not synchronized with the network) and only holds private keys
		/// </summary>
		/// <param name="account">Account number to change key</param>
		/// <param name="account_signer">Account that signs and pays the fee (must have same public key that delisted account, or be the same)</param>
		/// <param name="old_enc_pubkey">HEXASTRING - Public key of the account in encoded format</param>
		/// <param name="old_b58_pubkey">HEXASTRING - Public key of the account in base58 format</param>
		/// <param name="new_enc_pubkey">HEXASTRING - Public key of the new key for the account in encoded format</param>
		/// <param name="new_b58_pubkey">HEXASTRING - Public key of the new key for the account in base58 format</param>
		/// <param name="last_n_operation">Last value of n_operation obtained with an Account object, for example when called to getaccount</param>
		/// <param name="fee">Fee of the operation</param>
		/// <param name="payload">Payload "item" that will be included in this operation</param>
		/// <param name="payloadMethod">Encode type of the item payload</param>
		/// <param name="pwd">Used to encrypt payload with aes as a payload_method. If none equals to empty password</param>
		/// <param name="rawoperations">HEXASTRING (optional) - If we want to add a sign operation with other previous operations, here we must put previous rawoperations result</param>
		/// <remarks>Wallet must be unlocked and private key (searched with provided public key) must be in wallet. No other checks are made (no checks for valid n_operation, valid fee ...) </remarks>
		/// <remarks>Only one of old_enc_pubkey, old_b58_pubkey needs be provided</remarks>
		/// <remarks>Only one of new_enc_pubkey, new_b58_pubkey needs be provided</remarks>
		/// <returns>Returns a Raw Operations Object</returns>
		RawOperationDTO SignChangeKey(uint account, uint account_signer, string old_enc_pubkey, string old_b58_pubkey, string new_enc_pubkey, string new_b58_pubkey, uint last_n_operation, decimal fee, byte[] payload = null, PayloadEncryptionMethod ? payloadMethod = null, string pwd = null, string rawoperations = null);

		/// <summary>
		/// Signs a "List Account For Sale" operation. 
		/// </summary>
		/// <param name="account_target">Account to be listed</param>
		/// <param name="account_signer">Account that signs and pays the fee (must have same public key that listed account, or be the same)</param>
		/// <param name="price">price account can be purchased for</param>
		/// <param name="seller_account">Account that will receive "price" amount on sell</param>
		/// <param name="new_b58_pubkey">Base58 encoded public key (for private sale only)</param>
		/// <param name="new_enc_pubkey">Hex-encoded public key (for private sale only)</param>
		/// <param name="locked_until_block">Block number until this account will be locked (a locked account cannot execute operations while locked)</param>
		/// <param name="last_n_operation">Last value of n_operation obtained with an Account object, for example when called to getaccount</param>
		/// <param name="fee">PASCURRENCY - Fee of the operation</param>
		/// <param name="payload">Payload "item" that will be included in this operation</param>
		/// <param name="payloadMethod">Encode type of the item payload</param>
		/// <param name="pwd">Used to encrypt payload with aes as a payload_method. If none equals to empty password</param>
		/// <param name="signer_b58_pubkey">The current public key of "account_signer" in base58 encoding</param> 
		/// <param name="signer_enc_pubkey">The current public key of "account_signer" in hexadecimal encoding</param> 
		/// <param name="rawoperations">HEXASTRING (optional) - If we want to add a sign operation with other previous operations, here we must put previous rawoperations result</param>
		/// <remarks>Only one or none of new_b58_pubkey, new_enc_pubkey should be used. Populating both will result in an error.</remarks>
		/// <remarks>Only one or none of signer_b58_pubkey, signer_b58_pubkey should be used. Populating both will result in an error.</remarks>
		/// <returns>Returns a Raw Operations Object</returns>
		RawOperationDTO SignListAccountForSale(uint account_target, uint account_signer, decimal price, uint seller_account, string new_b58_pubkey, string new_enc_pubkey, uint locked_until_block, uint last_n_operation, decimal fee, byte[] payload = null, PayloadEncryptionMethod? payloadMethod = null, string pwd = null, string signer_b58_pubkey = null, string signer_enc_pubkey = null, string rawoperations = null);

		/// <summary>
		/// Signs a "Delist Account For Sale" operation, suitable for cold wallet usage.
		/// </summary>
		/// <param name="account_target">Account to be delisted</param>
		/// <param name="account_signer">Account that signs and pays the fee (must have same public key that delisted account, or be the same)</param>
		/// <param name="last_n_operation">Last value of n_operation obtained with an Account object, for example when called to getaccount</param>
		/// <param name="fee">PASCURRENCY - Fee of the operation</param>
		/// <param name="payload">Payload "item" that will be included in this operation</param>
		/// <param name="payloadMethod">Encode type of the item payload</param>
		/// <param name="pwd">Used to encrypt payload with aes as a payload_method. If none equals to empty password</param>
		/// <param name="signer_b58_pubkey">The current public key of "account_signer" in base58 encoding</param> 
		/// <param name="signer_enc_pubkey">The current public key of "account_signer" in hexadecimal encoding</param> 
		/// <param name="rawoperations">HEXASTRING (optional) - If we want to add a sign operation with other previous operations, here we must put previous rawoperations result</param>
		/// <remarks>Only one or none of signer_b58_pubkey, signer_b58_pubkey should be used. Populating both will result in an error.</remarks>
		/// <returns>Returns a Raw Operations Object</returns>
		RawOperationDTO SignDelistAccountForSale(uint account_target, uint account_signer, uint last_n_operation, decimal fee, byte[] payload = null, PayloadEncryptionMethod? payloadMethod = null, string pwd = null, string signer_b58_pubkey = null, string signer_enc_pubkey = null, string rawoperations = null);

		/// <summary>
		/// Signs a "Buy Account" operation, suitable for cold wallet usage.
		/// </summary>
		/// <param name="buyer_account">Account number of buyer who is purchasing the account</param>
		/// <param name="account_to_purchase">Account number being purchased</param>
		/// <param name="price">Settlement price of account being purchased</param>
		/// <param name="seller_account">Account of seller, receiving payment</param>
		/// <param name="new_b58_pubkey">Post-settlement public key in base58 encoded format.</param>
		/// <param name="new_enc_pubkey">Post-settlement public key in hexadecimal encoded format.</param>
		/// <param name="amount">Amount being transferred from buyer_account to seller_account (the settlement). This is a PASCURRENCY value.</param>
		/// <param name="last_n_operation">Last value of n_operation obtained with an Account object, for example when called to getaccount</param>
		/// <param name="fee">Fee of the operation. This is a PASCURRENCY value.</param>
		/// <param name="payload">Payload "item" that will be included in this operation</param>
		/// <param name="payloadMethod">Encode type of the item payload</param>
		/// <param name="pwd">Used to encrypt payload with aes as a payload_method. If none equals to empty password</param>
		/// <param name="signer_b58_pubkey">The current public key of "account_signer" in base58 encoding</param> 
		/// <param name="signer_enc_pubkey">The current public key of "account_signer" in hexadecimal encoding</param> 
		/// <param name="rawoperations">HEXASTRING (optional) - If we want to add a sign operation with other previous operations, here we must put previous rawoperations result</param>
		/// <remarks>Only one or none of new_b58_pubkey, new_enc_pubkey should be used. Populating both will result in an error.</remarks>
		/// <remarks>Only one or none of signer_b58_pubkey, signer_b58_pubkey should be used. Populating both will result in an error.</remarks>
		/// <returns>Returns a Raw Operations Object</returns>
		RawOperationDTO SignBuyAccount(uint buyer_account, uint account_to_purchase, decimal price, uint seller_account, string new_b58_pubkey, string new_enc_pubkey, decimal amount, uint last_n_operation, decimal fee, byte[] payload = null, PayloadEncryptionMethod? payloadMethod = null, string pwd = null, string signer_b58_pubkey = null, string signer_enc_pubkey = null, string rawoperations = null);

		/// <summary>
		/// Returns information stored in a rawoperations param (obtained calling signchangekey or signsendto)
		/// </summary>
		/// <param name="rawoperations">HEXASTRING (obtained calling signchangekey or signsendto)</param>
		/// <returns>Returns a JSON Array with Operation Object items, one for each operation in rawoperations param. NOTE: Remember that rawoperations are operations that maybe are not correct</returns>
		OperationDTO[] OperationsInfo(string rawoperations);

		/// <summary>
		/// Executes operations included in rawopertions param and transfers to the network. Raw operations can include "Send to" oprations or "Change key" operations.
		/// </summary>
		/// <param name="rawoperations">Executes operations included in rawopertions param and transfers to the network. Raw operations can include "Send to" oprations or "Change key" operations.</param>
		/// <remarks>For each Operation Object item, if there is an error, param valid will be false and param errors will show error description.Otherwise, operation is correct and will contain ophash param</remarks>
		/// <returns>Returns a JSON Array with Operation Object items, one for each operation in rawoperations param.</returns>
		OperationDTO[] ExecuteOperations(string rawoperations);

		/// <summary>
		/// Returns information of the Node in a JSON Object
		/// </summary>
		/// <returns>JSON Object with information</returns>
		NodeStatusDTO NodeStatus();

		/// <summary>
		/// Encodes a public key based on params information
		/// </summary>
		/// <param name="ec_nid">key type</param>
		/// <param name="x">HEXASTRING with x value of public key</param>
		/// <param name="y">HEXASTRING with y value of public key</param>
		/// <returns>Returns a HEXASTRING with encoded public key</returns>
		string EncodePubKey(KeyType ec_nid, string x, string y);

		/// <summary>
		/// Decodes an encoded public key
		/// </summary>
		/// <param name="enc_pubkey">HEXASTRING with encoded public key</param>
		/// <param name="b58_pubkey">String. b58_pubkey is the same value that Application Wallet exports as a public key</param>
		/// <remarks>Note: If use enc_pubkey and b58_pubkey together and is not the same public key, will return an error</remarks>
		/// <returns>Returns a JSON Object with a "Public Key Object"</returns>
		PublicKeyDTO DecodePubKey(string enc_pubkey, string b58_pubkey);

		/// <summary>
		/// Encrypt a text "paylad" using "payload_method"
		/// </summary>
		/// <param name="payload">HEXASTRING - Text to encrypt in hexadecimal format</param>
		/// <param name="payload_method">Payload method</param>
		/// <param name="pwd">Using a Password. Must provide pwd param</param>
		/// <returns>Returns a HEXASTRING with encrypted payload</returns>
		string PayloadEncrypt(string payload, PayloadEncryptionMethod payload_method, string pwd);

		/// <summary>
		/// Returns a HEXASTRING with decrypted text (a payload) using private keys in the wallet or a list of Passwords (used in "aes" encryption)
		/// </summary>
		/// <param name="payload">HEXASTRING - Encrypted data</param>
		/// <param name="pwds">List of passwords to use</param>
		/// <remarks>If using one of private keys is able to decrypt payload then returns value "key" in payload_method and enc_pubkey contains encoded public key in HEXASTRING</remarks>
		/// <remarks>If using one of passwords to decrypt payload then returns value "pwd" in payload_method and pwd contains password used</remarks>
		/// <returns>Decryped payload</returns>
		DecryptedPayloadDTO PayloadDecrypt(string payload, string[] pwds = null);

		/// <summary>
		/// Returns all the current connections
		/// </summary>
		/// <returns>JSON Array with Connection Objects</returns>
		ConnectionDTO[] GetConnections();

		/// <summary>
		/// Creates a new Private key and sotres it on the wallet, returning an enc_pubkey value
		/// </summary>
		/// <param name="ec_nid">Type of key encryption</param>
		/// <param name="name">Name to alias this new private key</param>
		/// <returns></returns>
		PublicKeyDTO AddNewKey(KeyType ec_nid, string name);

		/// <summary>
		/// Locks the Wallet if it has a password, otherwise wallet cannot be locked
		/// </summary>
		/// <returns>Returns a Boolean indicating if Wallet is locked. If false that means that Wallet has an empty password and cannot be locked</returns>
		bool Lock();

		/// <summary>
		/// Unlocks a locked Wallet using "pwd" param
		/// </summary>
		/// <param name="pwd"></param>
		/// <returns>Returns a Boolean indicating if Wallet is unlocked after using pwd password</returns>
		bool Unlock(string pwd);

		/// <summary>
		/// Changes the password of the Wallet. (Must be previously unlocked) Note: If pwd param is empty string, then wallet will be not protected by password
		/// </summary>
		/// <param name="pwd">New password</param>
		/// <returns>Returns a Boolean if Wallet password changed with new pwd password</returns>
		bool SetWalletPassword(string pwd);

		/// <summary>
		/// Stops the node and the server. Closes all connections
		/// </summary>
		/// <returns></returns>
		bool StopNode();

		/// <summary>
		/// Starts the node and the server. Starts connection process
		/// </summary>
		/// <returns>Boolean "true"</returns>
		bool StartNode();

	}
}
