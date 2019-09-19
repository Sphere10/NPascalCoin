using System;
using System.Collections.Generic;
using System.Text;
using NPascalCoin.DataObjects;

namespace NPascalCoin.Blockchain {

    public struct Orphan {
        private string ID;
    }

    public interface IBlockchainStorage {

        Int64 FirstBlockNumber { get; set; }
        Int64 LastBlockNumber { get; set; }

        void SetOrphan(Orphan value);
        
        void Erase();
        
        bool ReadOnly { get; set; }
    }


    public interface ISafeBoxStorage {

    }
}


/*
 * 
 * 
 *   TStorage = Class(TComponent)
  private
    FOrphan: TOrphan;
    FBank : TPCBank;
    FReadOnly: Boolean;
    procedure SetBank(const Value: TPCBank);
  protected
    FIsMovingBlockchain : Boolean;
    procedure SetOrphan(const Value: TOrphan); virtual;
    procedure SetReadOnly(const Value: Boolean); virtual;
    Function DoLoadBlockChain(Operations : TPCOperationsComp; Block : Cardinal) : Boolean; virtual; abstract;
    Function DoSaveBlockChain(Operations : TPCOperationsComp) : Boolean; virtual; abstract;
    Function DoMoveBlockChain(StartBlock : Cardinal; Const DestOrphan : TOrphan; DestStorage : TStorage) : Boolean; virtual; abstract;
    Function DoSaveBank : Boolean; virtual; abstract;
    Function DoRestoreBank(max_block : Int64; restoreProgressNotify : TProgressNotify) : Boolean; virtual; abstract;
    Procedure DoDeleteBlockChainBlocks(StartingDeleteBlock : Cardinal); virtual; abstract;
    Function DoBlockExists(Block : Cardinal) : Boolean; virtual; abstract;
    function GetFirstBlockNumber: Int64; virtual; abstract;
    function GetLastBlockNumber: Int64; virtual; abstract;
    function DoInitialize:Boolean; virtual; abstract;
    Function DoCreateSafeBoxStream(blockCount : Cardinal) : TStream; virtual; abstract;
    Procedure DoEraseStorage; virtual; abstract;
    Procedure DoSavePendingBufferOperations(OperationsHashTree : TOperationsHashTree); virtual; abstract;
    Procedure DoLoadPendingBufferOperations(OperationsHashTree : TOperationsHashTree); virtual; abstract;
  public
    Function LoadBlockChainBlock(Operations : TPCOperationsComp; Block : Cardinal) : Boolean;
    Function SaveBlockChainBlock(Operations : TPCOperationsComp) : Boolean;
    Function MoveBlockChainBlocks(StartBlock : Cardinal; Const DestOrphan : TOrphan; DestStorage : TStorage) : Boolean;
    Procedure DeleteBlockChainBlocks(StartingDeleteBlock : Cardinal);
    Function SaveBank(forceSave : Boolean) : Boolean;
    Function RestoreBank(max_block : Int64; restoreProgressNotify : TProgressNotify = Nil) : Boolean;
    Constructor Create(AOwner : TComponent); Override;
    Property Orphan : TOrphan read FOrphan write SetOrphan;
    Property ReadOnly : Boolean read FReadOnly write SetReadOnly;
    Property Bank : TPCBank read FBank write SetBank;
    Procedure CopyConfiguration(Const CopyFrom : TStorage); virtual;
    Property FirstBlock : Int64 read GetFirstBlockNumber;
    Property LastBlock : Int64 read GetLastBlockNumber;
    Function Initialize : Boolean;
    Function CreateSafeBoxStream(blockCount : Cardinal) : TStream;
    Function HasUpgradedToVersion2 : Boolean; virtual; abstract;
    Procedure CleanupVersion1Data; virtual; abstract;
    Procedure EraseStorage;
    Procedure SavePendingBufferOperations(OperationsHashTree : TOperationsHashTree);
    Procedure LoadPendingBufferOperations(OperationsHashTree : TOperationsHashTree);
    Function BlockExists(Block : Cardinal) : Boolean;
  End;


    */