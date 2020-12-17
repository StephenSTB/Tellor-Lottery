using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts;
using System.Threading;
using TellorTimeContracts.Contracts.TellorPlayground.ContractDefinition;

namespace TellorTimeContracts.Contracts.TellorPlayground
{
    public partial class TellorPlaygroundService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, TellorPlaygroundDeployment tellorPlaygroundDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<TellorPlaygroundDeployment>().SendRequestAndWaitForReceiptAsync(tellorPlaygroundDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, TellorPlaygroundDeployment tellorPlaygroundDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<TellorPlaygroundDeployment>().SendRequestAsync(tellorPlaygroundDeployment);
        }

        public static async Task<TellorPlaygroundService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, TellorPlaygroundDeployment tellorPlaygroundDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, tellorPlaygroundDeployment, cancellationTokenSource);
            return new TellorPlaygroundService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public TellorPlaygroundService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> AddTipRequestAsync(AddTipFunction addTipFunction)
        {
             return ContractHandler.SendRequestAsync(addTipFunction);
        }

        public Task<TransactionReceipt> AddTipRequestAndWaitForReceiptAsync(AddTipFunction addTipFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addTipFunction, cancellationToken);
        }

        public Task<string> AddTipRequestAsync(BigInteger requestId, BigInteger amount)
        {
            var addTipFunction = new AddTipFunction();
                addTipFunction.RequestId = requestId;
                addTipFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(addTipFunction);
        }

        public Task<TransactionReceipt> AddTipRequestAndWaitForReceiptAsync(BigInteger requestId, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var addTipFunction = new AddTipFunction();
                addTipFunction.RequestId = requestId;
                addTipFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addTipFunction, cancellationToken);
        }

        public Task<BigInteger> AllowanceQueryAsync(AllowanceFunction allowanceFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<AllowanceFunction, BigInteger>(allowanceFunction, blockParameter);
        }

        
        public Task<BigInteger> AllowanceQueryAsync(string owner, string spender, BlockParameter blockParameter = null)
        {
            var allowanceFunction = new AllowanceFunction();
                allowanceFunction.Owner = owner;
                allowanceFunction.Spender = spender;
            
            return ContractHandler.QueryAsync<AllowanceFunction, BigInteger>(allowanceFunction, blockParameter);
        }

        public Task<string> ApproveRequestAsync(ApproveFunction approveFunction)
        {
             return ContractHandler.SendRequestAsync(approveFunction);
        }

        public Task<TransactionReceipt> ApproveRequestAndWaitForReceiptAsync(ApproveFunction approveFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(approveFunction, cancellationToken);
        }

        public Task<string> ApproveRequestAsync(string spender, BigInteger amount)
        {
            var approveFunction = new ApproveFunction();
                approveFunction.Spender = spender;
                approveFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(approveFunction);
        }

        public Task<TransactionReceipt> ApproveRequestAndWaitForReceiptAsync(string spender, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var approveFunction = new ApproveFunction();
                approveFunction.Spender = spender;
                approveFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(approveFunction, cancellationToken);
        }

        public Task<BigInteger> BalanceOfQueryAsync(BalanceOfFunction balanceOfFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction, blockParameter);
        }

        
        public Task<BigInteger> BalanceOfQueryAsync(string account, BlockParameter blockParameter = null)
        {
            var balanceOfFunction = new BalanceOfFunction();
                balanceOfFunction.Account = account;
            
            return ContractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction, blockParameter);
        }

        public Task<BigInteger> BalancesQueryAsync(BalancesFunction balancesFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BalancesFunction, BigInteger>(balancesFunction, blockParameter);
        }

        
        public Task<BigInteger> BalancesQueryAsync(string returnValue1, BlockParameter blockParameter = null)
        {
            var balancesFunction = new BalancesFunction();
                balancesFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryAsync<BalancesFunction, BigInteger>(balancesFunction, blockParameter);
        }

        public Task<byte> DecimalsQueryAsync(DecimalsFunction decimalsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DecimalsFunction, byte>(decimalsFunction, blockParameter);
        }

        
        public Task<byte> DecimalsQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DecimalsFunction, byte>(null, blockParameter);
        }

        public Task<string> DecreaseAllowanceRequestAsync(DecreaseAllowanceFunction decreaseAllowanceFunction)
        {
             return ContractHandler.SendRequestAsync(decreaseAllowanceFunction);
        }

        public Task<TransactionReceipt> DecreaseAllowanceRequestAndWaitForReceiptAsync(DecreaseAllowanceFunction decreaseAllowanceFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(decreaseAllowanceFunction, cancellationToken);
        }

        public Task<string> DecreaseAllowanceRequestAsync(string spender, BigInteger subtractedValue)
        {
            var decreaseAllowanceFunction = new DecreaseAllowanceFunction();
                decreaseAllowanceFunction.Spender = spender;
                decreaseAllowanceFunction.SubtractedValue = subtractedValue;
            
             return ContractHandler.SendRequestAsync(decreaseAllowanceFunction);
        }

        public Task<TransactionReceipt> DecreaseAllowanceRequestAndWaitForReceiptAsync(string spender, BigInteger subtractedValue, CancellationTokenSource cancellationToken = null)
        {
            var decreaseAllowanceFunction = new DecreaseAllowanceFunction();
                decreaseAllowanceFunction.Spender = spender;
                decreaseAllowanceFunction.SubtractedValue = subtractedValue;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(decreaseAllowanceFunction, cancellationToken);
        }

        public Task<string> DisputeValueRequestAsync(DisputeValueFunction disputeValueFunction)
        {
             return ContractHandler.SendRequestAsync(disputeValueFunction);
        }

        public Task<TransactionReceipt> DisputeValueRequestAndWaitForReceiptAsync(DisputeValueFunction disputeValueFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(disputeValueFunction, cancellationToken);
        }

        public Task<string> DisputeValueRequestAsync(BigInteger requestId, BigInteger timestamp)
        {
            var disputeValueFunction = new DisputeValueFunction();
                disputeValueFunction.RequestId = requestId;
                disputeValueFunction.Timestamp = timestamp;
            
             return ContractHandler.SendRequestAsync(disputeValueFunction);
        }

        public Task<TransactionReceipt> DisputeValueRequestAndWaitForReceiptAsync(BigInteger requestId, BigInteger timestamp, CancellationTokenSource cancellationToken = null)
        {
            var disputeValueFunction = new DisputeValueFunction();
                disputeValueFunction.RequestId = requestId;
                disputeValueFunction.Timestamp = timestamp;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(disputeValueFunction, cancellationToken);
        }

        public Task<string> FaucetRequestAsync(FaucetFunction faucetFunction)
        {
             return ContractHandler.SendRequestAsync(faucetFunction);
        }

        public Task<TransactionReceipt> FaucetRequestAndWaitForReceiptAsync(FaucetFunction faucetFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(faucetFunction, cancellationToken);
        }

        public Task<string> FaucetRequestAsync(string user)
        {
            var faucetFunction = new FaucetFunction();
                faucetFunction.User = user;
            
             return ContractHandler.SendRequestAsync(faucetFunction);
        }

        public Task<TransactionReceipt> FaucetRequestAndWaitForReceiptAsync(string user, CancellationTokenSource cancellationToken = null)
        {
            var faucetFunction = new FaucetFunction();
                faucetFunction.User = user;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(faucetFunction, cancellationToken);
        }

        public Task<BigInteger> GetNewValueCountbyRequestIdQueryAsync(GetNewValueCountbyRequestIdFunction getNewValueCountbyRequestIdFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetNewValueCountbyRequestIdFunction, BigInteger>(getNewValueCountbyRequestIdFunction, blockParameter);
        }

        
        public Task<BigInteger> GetNewValueCountbyRequestIdQueryAsync(BigInteger requestId, BlockParameter blockParameter = null)
        {
            var getNewValueCountbyRequestIdFunction = new GetNewValueCountbyRequestIdFunction();
                getNewValueCountbyRequestIdFunction.RequestId = requestId;
            
            return ContractHandler.QueryAsync<GetNewValueCountbyRequestIdFunction, BigInteger>(getNewValueCountbyRequestIdFunction, blockParameter);
        }

        public Task<BigInteger> GetTimestampbyRequestIDandIndexQueryAsync(GetTimestampbyRequestIDandIndexFunction getTimestampbyRequestIDandIndexFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetTimestampbyRequestIDandIndexFunction, BigInteger>(getTimestampbyRequestIDandIndexFunction, blockParameter);
        }

        
        public Task<BigInteger> GetTimestampbyRequestIDandIndexQueryAsync(BigInteger requestId, BigInteger index, BlockParameter blockParameter = null)
        {
            var getTimestampbyRequestIDandIndexFunction = new GetTimestampbyRequestIDandIndexFunction();
                getTimestampbyRequestIDandIndexFunction.RequestId = requestId;
                getTimestampbyRequestIDandIndexFunction.Index = index;
            
            return ContractHandler.QueryAsync<GetTimestampbyRequestIDandIndexFunction, BigInteger>(getTimestampbyRequestIDandIndexFunction, blockParameter);
        }

        public Task<string> IncreaseAllowanceRequestAsync(IncreaseAllowanceFunction increaseAllowanceFunction)
        {
             return ContractHandler.SendRequestAsync(increaseAllowanceFunction);
        }

        public Task<TransactionReceipt> IncreaseAllowanceRequestAndWaitForReceiptAsync(IncreaseAllowanceFunction increaseAllowanceFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(increaseAllowanceFunction, cancellationToken);
        }

        public Task<string> IncreaseAllowanceRequestAsync(string spender, BigInteger addedValue)
        {
            var increaseAllowanceFunction = new IncreaseAllowanceFunction();
                increaseAllowanceFunction.Spender = spender;
                increaseAllowanceFunction.AddedValue = addedValue;
            
             return ContractHandler.SendRequestAsync(increaseAllowanceFunction);
        }

        public Task<TransactionReceipt> IncreaseAllowanceRequestAndWaitForReceiptAsync(string spender, BigInteger addedValue, CancellationTokenSource cancellationToken = null)
        {
            var increaseAllowanceFunction = new IncreaseAllowanceFunction();
                increaseAllowanceFunction.Spender = spender;
                increaseAllowanceFunction.AddedValue = addedValue;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(increaseAllowanceFunction, cancellationToken);
        }

        public Task<bool> IsDisputedQueryAsync(IsDisputedFunction isDisputedFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsDisputedFunction, bool>(isDisputedFunction, blockParameter);
        }

        
        public Task<bool> IsDisputedQueryAsync(BigInteger returnValue1, BigInteger returnValue2, BlockParameter blockParameter = null)
        {
            var isDisputedFunction = new IsDisputedFunction();
                isDisputedFunction.ReturnValue1 = returnValue1;
                isDisputedFunction.ReturnValue2 = returnValue2;
            
            return ContractHandler.QueryAsync<IsDisputedFunction, bool>(isDisputedFunction, blockParameter);
        }

        public Task<bool> IsInDisputeQueryAsync(IsInDisputeFunction isInDisputeFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsInDisputeFunction, bool>(isInDisputeFunction, blockParameter);
        }

        
        public Task<bool> IsInDisputeQueryAsync(BigInteger requestId, BigInteger timestamp, BlockParameter blockParameter = null)
        {
            var isInDisputeFunction = new IsInDisputeFunction();
                isInDisputeFunction.RequestId = requestId;
                isInDisputeFunction.Timestamp = timestamp;
            
            return ContractHandler.QueryAsync<IsInDisputeFunction, bool>(isInDisputeFunction, blockParameter);
        }

        public Task<string> NameQueryAsync(NameFunction nameFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<NameFunction, string>(nameFunction, blockParameter);
        }

        
        public Task<string> NameQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<NameFunction, string>(null, blockParameter);
        }

        public Task<BigInteger> RetrieveDataQueryAsync(RetrieveDataFunction retrieveDataFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<RetrieveDataFunction, BigInteger>(retrieveDataFunction, blockParameter);
        }

        
        public Task<BigInteger> RetrieveDataQueryAsync(BigInteger requestId, BigInteger timestamp, BlockParameter blockParameter = null)
        {
            var retrieveDataFunction = new RetrieveDataFunction();
                retrieveDataFunction.RequestId = requestId;
                retrieveDataFunction.Timestamp = timestamp;
            
            return ContractHandler.QueryAsync<RetrieveDataFunction, BigInteger>(retrieveDataFunction, blockParameter);
        }

        public Task<string> SubmitValueRequestAsync(SubmitValueFunction submitValueFunction)
        {
             return ContractHandler.SendRequestAsync(submitValueFunction);
        }

        public Task<TransactionReceipt> SubmitValueRequestAndWaitForReceiptAsync(SubmitValueFunction submitValueFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(submitValueFunction, cancellationToken);
        }

        public Task<string> SubmitValueRequestAsync(BigInteger requestId, BigInteger value)
        {
            var submitValueFunction = new SubmitValueFunction();
                submitValueFunction.RequestId = requestId;
                submitValueFunction.Value = value;
            
             return ContractHandler.SendRequestAsync(submitValueFunction);
        }

        public Task<TransactionReceipt> SubmitValueRequestAndWaitForReceiptAsync(BigInteger requestId, BigInteger value, CancellationTokenSource cancellationToken = null)
        {
            var submitValueFunction = new SubmitValueFunction();
                submitValueFunction.RequestId = requestId;
                submitValueFunction.Value = value;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(submitValueFunction, cancellationToken);
        }

        public Task<string> SymbolQueryAsync(SymbolFunction symbolFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SymbolFunction, string>(symbolFunction, blockParameter);
        }

        
        public Task<string> SymbolQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SymbolFunction, string>(null, blockParameter);
        }

        public Task<BigInteger> TimestampsQueryAsync(TimestampsFunction timestampsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TimestampsFunction, BigInteger>(timestampsFunction, blockParameter);
        }

        
        public Task<BigInteger> TimestampsQueryAsync(BigInteger returnValue1, BigInteger returnValue2, BlockParameter blockParameter = null)
        {
            var timestampsFunction = new TimestampsFunction();
                timestampsFunction.ReturnValue1 = returnValue1;
                timestampsFunction.ReturnValue2 = returnValue2;
            
            return ContractHandler.QueryAsync<TimestampsFunction, BigInteger>(timestampsFunction, blockParameter);
        }

        public Task<BigInteger> TotalSupplyQueryAsync(TotalSupplyFunction totalSupplyFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalSupplyFunction, BigInteger>(totalSupplyFunction, blockParameter);
        }

        
        public Task<BigInteger> TotalSupplyQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalSupplyFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> TransferRequestAsync(TransferFunction transferFunction)
        {
             return ContractHandler.SendRequestAsync(transferFunction);
        }

        public Task<TransactionReceipt> TransferRequestAndWaitForReceiptAsync(TransferFunction transferFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFunction, cancellationToken);
        }

        public Task<string> TransferRequestAsync(string recipient, BigInteger amount)
        {
            var transferFunction = new TransferFunction();
                transferFunction.Recipient = recipient;
                transferFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(transferFunction);
        }

        public Task<TransactionReceipt> TransferRequestAndWaitForReceiptAsync(string recipient, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var transferFunction = new TransferFunction();
                transferFunction.Recipient = recipient;
                transferFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFunction, cancellationToken);
        }

        public Task<string> TransferFromRequestAsync(TransferFromFunction transferFromFunction)
        {
             return ContractHandler.SendRequestAsync(transferFromFunction);
        }

        public Task<TransactionReceipt> TransferFromRequestAndWaitForReceiptAsync(TransferFromFunction transferFromFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFromFunction, cancellationToken);
        }

        public Task<string> TransferFromRequestAsync(string sender, string recipient, BigInteger amount)
        {
            var transferFromFunction = new TransferFromFunction();
                transferFromFunction.Sender = sender;
                transferFromFunction.Recipient = recipient;
                transferFromFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(transferFromFunction);
        }

        public Task<TransactionReceipt> TransferFromRequestAndWaitForReceiptAsync(string sender, string recipient, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var transferFromFunction = new TransferFromFunction();
                transferFromFunction.Sender = sender;
                transferFromFunction.Recipient = recipient;
                transferFromFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFromFunction, cancellationToken);
        }

        public Task<BigInteger> ValuesQueryAsync(ValuesFunction valuesFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ValuesFunction, BigInteger>(valuesFunction, blockParameter);
        }

        
        public Task<BigInteger> ValuesQueryAsync(BigInteger returnValue1, BigInteger returnValue2, BlockParameter blockParameter = null)
        {
            var valuesFunction = new ValuesFunction();
                valuesFunction.ReturnValue1 = returnValue1;
                valuesFunction.ReturnValue2 = returnValue2;
            
            return ContractHandler.QueryAsync<ValuesFunction, BigInteger>(valuesFunction, blockParameter);
        }
    }
}
