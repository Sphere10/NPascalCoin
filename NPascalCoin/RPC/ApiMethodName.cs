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
		findaccounts,
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
		changeaccountinfo,
		sendto,
		changekey,
		changekeys,
		listaccountforsale,
		delistaccountforsale,
		buyaccount,
		signchangeaccountinfo,
		signsendto,
		signchangekey,
		signlistaccountforsale,
		signdelistaccountforsale,
		signbuyaccount,
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
