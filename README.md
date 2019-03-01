# NPascalCoin

NPascalCoin is a cross-platform (.NET Standard 2) library for PascalCoin. 

Currently only a client for the Classic JSON-RPC is provided.

**NOTE**: This library is under heavy development, please use [Releases/1.0.0](https://github.com/Sphere10/NPascalCoin/tree/Releases/1.0.0) branch for prior version.

## Usage

Copy and include the project in your solution and add `using NPascalCoin`. Use the `IPascalCoinClient` interface to interact with JSON-RPC API.

## Example:  Listing Wallet Accounts

```csharp
var client = new PascalCoinClient("127.0.0.1");
var accounts = client.GetWalletAccounts();
```

Asyncronously

```csharp
var client = new PascalCoinClient("127.0.0.1");
var accounts = await client.GetWalletAccountsAsync();
```

## Example: Sending funds

Unlocking wallet and sending funds

```csharp 
var client = new PascalCoinClient("127.0.0.1");
using (client.EnterUnlockWalletScope("password")) {
    client.SendTo(senderAccount, targetAcc, qty, fee, "Payload text".ToByteArray(), PayloadEncryptionMethod.None)
} 
```

Asyncronously

```csharp 
var client = new PascalCoinClient("127.0.0.1");
using (await client.EnterUnlockWalletScopeAsync("password")) {
    await client.SendToAsync(senderAccount, targetAcc, qty, fee, "Payload text".ToByteArray(), PayloadEncryptionMethod.None)
} 
```

**NOTE**: WalletScopes unlock the wallet inside the scope and then lock as the scope exists. They are only needed when authoring operations. You can also manually lock and unlock the wallet using `Lock` and `Unlock` methods.

# Roadmap
  - Full Layer-1 protocol implementation for all versions (version 1 to current)
    - Networking Layer
    - Consensus Layer
  - Stand-alone node implementation (can run as daemon/service without classic)
    - Classic datafile persistance support
    - SQL persistance support
    - NoSQL persisance support
  - Full Layer-2 implementation
    - E-PASA
    - General-purpose networking support for Layer-2 Dapps.
    - General-purpose consensus support for Layer-2 Dapps.
  