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
using TellorTimeContracts.Contracts.TellorLotteryPool.ContractDefinition;

namespace TellorTimeContracts.Contracts.TellorLotteryPool
{
    public partial class TellorLotteryPoolService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, TellorLotteryPoolDeployment tellorLotteryPoolDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<TellorLotteryPoolDeployment>().SendRequestAndWaitForReceiptAsync(tellorLotteryPoolDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, TellorLotteryPoolDeployment tellorLotteryPoolDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<TellorLotteryPoolDeployment>().SendRequestAsync(tellorLotteryPoolDeployment);
        }

        public static async Task<TellorLotteryPoolService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, TellorLotteryPoolDeployment tellorLotteryPoolDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, tellorLotteryPoolDeployment, cancellationTokenSource);
            return new TellorLotteryPoolService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public TellorLotteryPoolService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
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

        public Task<BigInteger> CurrentStakedQueryAsync(CurrentStakedFunction currentStakedFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<CurrentStakedFunction, BigInteger>(currentStakedFunction, blockParameter);
        }

        
        public Task<BigInteger> CurrentStakedQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<CurrentStakedFunction, BigInteger>(null, blockParameter);
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

        public Task<string> HarvestRequestAsync(HarvestFunction harvestFunction)
        {
             return ContractHandler.SendRequestAsync(harvestFunction);
        }

        public Task<string> HarvestRequestAsync()
        {
             return ContractHandler.SendRequestAsync<HarvestFunction>();
        }

        public Task<TransactionReceipt> HarvestRequestAndWaitForReceiptAsync(HarvestFunction harvestFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(harvestFunction, cancellationToken);
        }

        public Task<TransactionReceipt> HarvestRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<HarvestFunction>(null, cancellationToken);
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

        public Task<string> MintRequestAsync(MintFunction mintFunction)
        {
             return ContractHandler.SendRequestAsync(mintFunction);
        }

        public Task<TransactionReceipt> MintRequestAndWaitForReceiptAsync(MintFunction mintFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(mintFunction, cancellationToken);
        }

        public Task<string> MintRequestAsync(string receiver)
        {
            var mintFunction = new MintFunction();
                mintFunction.Receiver = receiver;
            
             return ContractHandler.SendRequestAsync(mintFunction);
        }

        public Task<TransactionReceipt> MintRequestAndWaitForReceiptAsync(string receiver, CancellationTokenSource cancellationToken = null)
        {
            var mintFunction = new MintFunction();
                mintFunction.Receiver = receiver;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(mintFunction, cancellationToken);
        }

        public Task<string> NameQueryAsync(NameFunction nameFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<NameFunction, string>(nameFunction, blockParameter);
        }

        
        public Task<string> NameQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<NameFunction, string>(null, blockParameter);
        }

        public Task<string> StakeRequestAsync(StakeFunction stakeFunction)
        {
             return ContractHandler.SendRequestAsync(stakeFunction);
        }

        public Task<TransactionReceipt> StakeRequestAndWaitForReceiptAsync(StakeFunction stakeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(stakeFunction, cancellationToken);
        }

        public Task<string> StakeRequestAsync(BigInteger amount)
        {
            var stakeFunction = new StakeFunction();
                stakeFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(stakeFunction);
        }

        public Task<TransactionReceipt> StakeRequestAndWaitForReceiptAsync(BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var stakeFunction = new StakeFunction();
                stakeFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(stakeFunction, cancellationToken);
        }

        public Task<string> SymbolQueryAsync(SymbolFunction symbolFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SymbolFunction, string>(symbolFunction, blockParameter);
        }

        
        public Task<string> SymbolQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SymbolFunction, string>(null, blockParameter);
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

        public Task<string> UnstakeRequestAsync(UnstakeFunction unstakeFunction)
        {
             return ContractHandler.SendRequestAsync(unstakeFunction);
        }

        public Task<TransactionReceipt> UnstakeRequestAndWaitForReceiptAsync(UnstakeFunction unstakeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(unstakeFunction, cancellationToken);
        }

        public Task<string> UnstakeRequestAsync(BigInteger amount)
        {
            var unstakeFunction = new UnstakeFunction();
                unstakeFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(unstakeFunction);
        }

        public Task<TransactionReceipt> UnstakeRequestAndWaitForReceiptAsync(BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var unstakeFunction = new UnstakeFunction();
                unstakeFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(unstakeFunction, cancellationToken);
        }

        public Task<string> UpdatePoolRequestAsync(UpdatePoolFunction updatePoolFunction)
        {
             return ContractHandler.SendRequestAsync(updatePoolFunction);
        }

        public Task<string> UpdatePoolRequestAsync()
        {
             return ContractHandler.SendRequestAsync<UpdatePoolFunction>();
        }

        public Task<TransactionReceipt> UpdatePoolRequestAndWaitForReceiptAsync(UpdatePoolFunction updatePoolFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(updatePoolFunction, cancellationToken);
        }

        public Task<TransactionReceipt> UpdatePoolRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<UpdatePoolFunction>(null, cancellationToken);
        }

        public Task<ViewHarvestOutputDTO> ViewHarvestQueryAsync(ViewHarvestFunction viewHarvestFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<ViewHarvestFunction, ViewHarvestOutputDTO>(viewHarvestFunction, blockParameter);
        }

        public Task<ViewHarvestOutputDTO> ViewHarvestQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<ViewHarvestFunction, ViewHarvestOutputDTO>(null, blockParameter);
        }

        public Task<ViewStakeOutputDTO> ViewStakeQueryAsync(ViewStakeFunction viewStakeFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<ViewStakeFunction, ViewStakeOutputDTO>(viewStakeFunction, blockParameter);
        }

        public Task<ViewStakeOutputDTO> ViewStakeQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<ViewStakeFunction, ViewStakeOutputDTO>(null, blockParameter);
        }
    }
}
