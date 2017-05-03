using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPascalCoin.DTO;

namespace NPascalCoin {
	internal enum ApiMethodName {
		addnode,
		getaccount,
		getwalletaccounts,
		getwalletaccountscount,
		getwalletpubkeys,
		getwalletpubkey,
		getwalletcoins,
		getblock,
		getblocks,
		getblockcount,
		getblockoperation,
		getblockoperations,
		getaccountoperations,
		getpendings,
		findoperation,
		sendto,
		changekey,
		changekeys,
		signsendto,
		signchangekey,
		operationsinfo,
		executeoperations,
		nodestatus,
		encodepubkey,
		decodepubkey,
		payloadencrypt,
		payloaddecrypt,
		getconnections,
		addnewkey,
		@lock,
		unlock,
		setwalletpassword,
		stopnode,
		startnode,
	}
}
