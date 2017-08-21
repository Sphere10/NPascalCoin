# NPascalCoin

NPascalCoin is a C# library for PascalCoin. Currently only the JSON-RPC API is supported with plans to add full-protocol support in future.

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

# Credits
Herman Schoenfeld <herman@sphere10.com>