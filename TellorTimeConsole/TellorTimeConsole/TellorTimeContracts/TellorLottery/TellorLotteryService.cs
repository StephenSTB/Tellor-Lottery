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
using TellorTimeContracts.Contracts.TellorLottery.ContractDefinition;

namespace TellorTimeContracts.Contracts.TellorLottery
{
    public partial class TellorLotteryService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, TellorLotteryDeployment tellorLotteryDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<TellorLotteryDeployment>().SendRequestAndWaitForReceiptAsync(tellorLotteryDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, TellorLotteryDeployment tellorLotteryDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<TellorLotteryDeployment>().SendRequestAsync(tellorLotteryDeployment);
        }

        public static async Task<TellorLotteryService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, TellorLotteryDeployment tellorLotteryDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, tellorLotteryDeployment, cancellationTokenSource);
            return new TellorLotteryService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public TellorLotteryService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> BuyTicketRequestAsync(BuyTicketFunction buyTicketFunction)
        {
             return ContractHandler.SendRequestAsync(buyTicketFunction);
        }

        public Task<TransactionReceipt> BuyTicketRequestAndWaitForReceiptAsync(BuyTicketFunction buyTicketFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(buyTicketFunction, cancellationToken);
        }

        public Task<string> BuyTicketRequestAsync(List<ulong> numbers)
        {
            var buyTicketFunction = new BuyTicketFunction();
                buyTicketFunction.Numbers = numbers;
            
             return ContractHandler.SendRequestAsync(buyTicketFunction);
        }

        public Task<TransactionReceipt> BuyTicketRequestAndWaitForReceiptAsync(List<ulong> numbers, CancellationTokenSource cancellationToken = null)
        {
            var buyTicketFunction = new BuyTicketFunction();
                buyTicketFunction.Numbers = numbers;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(buyTicketFunction, cancellationToken);
        }

        public Task<BigInteger> CompetitionMaskQueryAsync(CompetitionMaskFunction competitionMaskFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<CompetitionMaskFunction, BigInteger>(competitionMaskFunction, blockParameter);
        }

        
        public Task<BigInteger> CompetitionMaskQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<CompetitionMaskFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> CompetitionNumberQueryAsync(CompetitionNumberFunction competitionNumberFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<CompetitionNumberFunction, BigInteger>(competitionNumberFunction, blockParameter);
        }

        
        public Task<BigInteger> CompetitionNumberQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<CompetitionNumberFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> DisputePeriodQueryAsync(DisputePeriodFunction disputePeriodFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DisputePeriodFunction, BigInteger>(disputePeriodFunction, blockParameter);
        }

        
        public Task<BigInteger> DisputePeriodQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DisputePeriodFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> GenerateTicketIDsRequestAsync(GenerateTicketIDsFunction generateTicketIDsFunction)
        {
             return ContractHandler.SendRequestAsync(generateTicketIDsFunction);
        }

        public Task<TransactionReceipt> GenerateTicketIDsRequestAndWaitForReceiptAsync(GenerateTicketIDsFunction generateTicketIDsFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(generateTicketIDsFunction, cancellationToken);
        }

        public Task<string> GenerateTicketIDsRequestAsync(List<ulong> numbers)
        {
            var generateTicketIDsFunction = new GenerateTicketIDsFunction();
                generateTicketIDsFunction.Numbers = numbers;
            
             return ContractHandler.SendRequestAsync(generateTicketIDsFunction);
        }

        public Task<TransactionReceipt> GenerateTicketIDsRequestAndWaitForReceiptAsync(List<ulong> numbers, CancellationTokenSource cancellationToken = null)
        {
            var generateTicketIDsFunction = new GenerateTicketIDsFunction();
                generateTicketIDsFunction.Numbers = numbers;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(generateTicketIDsFunction, cancellationToken);
        }

        public Task<GetCurrentValueOutputDTO> GetCurrentValueQueryAsync(GetCurrentValueFunction getCurrentValueFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetCurrentValueFunction, GetCurrentValueOutputDTO>(getCurrentValueFunction, blockParameter);
        }

        public Task<GetCurrentValueOutputDTO> GetCurrentValueQueryAsync(BigInteger requestId, BlockParameter blockParameter = null)
        {
            var getCurrentValueFunction = new GetCurrentValueFunction();
                getCurrentValueFunction.RequestId = requestId;
            
            return ContractHandler.QueryDeserializingToObjectAsync<GetCurrentValueFunction, GetCurrentValueOutputDTO>(getCurrentValueFunction, blockParameter);
        }

        public Task<string> GetDataBeforeRequestAsync(GetDataBeforeFunction getDataBeforeFunction)
        {
             return ContractHandler.SendRequestAsync(getDataBeforeFunction);
        }

        public Task<TransactionReceipt> GetDataBeforeRequestAndWaitForReceiptAsync(GetDataBeforeFunction getDataBeforeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(getDataBeforeFunction, cancellationToken);
        }

        public Task<string> GetDataBeforeRequestAsync(BigInteger requestId, BigInteger timestamp)
        {
            var getDataBeforeFunction = new GetDataBeforeFunction();
                getDataBeforeFunction.RequestId = requestId;
                getDataBeforeFunction.Timestamp = timestamp;
            
             return ContractHandler.SendRequestAsync(getDataBeforeFunction);
        }

        public Task<TransactionReceipt> GetDataBeforeRequestAndWaitForReceiptAsync(BigInteger requestId, BigInteger timestamp, CancellationTokenSource cancellationToken = null)
        {
            var getDataBeforeFunction = new GetDataBeforeFunction();
                getDataBeforeFunction.RequestId = requestId;
                getDataBeforeFunction.Timestamp = timestamp;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(getDataBeforeFunction, cancellationToken);
        }

        public Task<GetIndexForDataBeforeOutputDTO> GetIndexForDataBeforeQueryAsync(GetIndexForDataBeforeFunction getIndexForDataBeforeFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetIndexForDataBeforeFunction, GetIndexForDataBeforeOutputDTO>(getIndexForDataBeforeFunction, blockParameter);
        }

        public Task<GetIndexForDataBeforeOutputDTO> GetIndexForDataBeforeQueryAsync(BigInteger requestId, BigInteger timestamp, BlockParameter blockParameter = null)
        {
            var getIndexForDataBeforeFunction = new GetIndexForDataBeforeFunction();
                getIndexForDataBeforeFunction.RequestId = requestId;
                getIndexForDataBeforeFunction.Timestamp = timestamp;
            
            return ContractHandler.QueryDeserializingToObjectAsync<GetIndexForDataBeforeFunction, GetIndexForDataBeforeOutputDTO>(getIndexForDataBeforeFunction, blockParameter);
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

        public Task<string> LotteryPoolQueryAsync(LotteryPoolFunction lotteryPoolFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<LotteryPoolFunction, string>(lotteryPoolFunction, blockParameter);
        }

        
        public Task<string> LotteryPoolQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<LotteryPoolFunction, string>(null, blockParameter);
        }

        public Task<byte> MaskQueryAsync(MaskFunction maskFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MaskFunction, byte>(maskFunction, blockParameter);
        }

        
        public Task<byte> MaskQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MaskFunction, byte>(null, blockParameter);
        }

        public Task<NumberOfWinnersOutputDTO> NumberOfWinnersQueryAsync(NumberOfWinnersFunction numberOfWinnersFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<NumberOfWinnersFunction, NumberOfWinnersOutputDTO>(numberOfWinnersFunction, blockParameter);
        }

        public Task<NumberOfWinnersOutputDTO> NumberOfWinnersQueryAsync(BigInteger competition, BlockParameter blockParameter = null)
        {
            var numberOfWinnersFunction = new NumberOfWinnersFunction();
                numberOfWinnersFunction.Competition = competition;
            
            return ContractHandler.QueryDeserializingToObjectAsync<NumberOfWinnersFunction, NumberOfWinnersOutputDTO>(numberOfWinnersFunction, blockParameter);
        }

        public Task<BigInteger> PrizePoolQueryAsync(PrizePoolFunction prizePoolFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PrizePoolFunction, BigInteger>(prizePoolFunction, blockParameter);
        }

        
        public Task<BigInteger> PrizePoolQueryAsync(BigInteger competition, BlockParameter blockParameter = null)
        {
            var prizePoolFunction = new PrizePoolFunction();
                prizePoolFunction.Competition = competition;
            
            return ContractHandler.QueryAsync<PrizePoolFunction, BigInteger>(prizePoolFunction, blockParameter);
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

        public Task<BigInteger> SenderAllownaceQueryAsync(SenderAllownaceFunction senderAllownaceFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SenderAllownaceFunction, BigInteger>(senderAllownaceFunction, blockParameter);
        }

        
        public Task<BigInteger> SenderAllownaceQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SenderAllownaceFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> TicketCostQueryAsync(TicketCostFunction ticketCostFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TicketCostFunction, BigInteger>(ticketCostFunction, blockParameter);
        }

        
        public Task<BigInteger> TicketCostQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TicketCostFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> TicketNumberQueryAsync(TicketNumberFunction ticketNumberFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TicketNumberFunction, BigInteger>(ticketNumberFunction, blockParameter);
        }

        
        public Task<BigInteger> TicketNumberQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TicketNumberFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> UpdateLottoRequestAsync(UpdateLottoFunction updateLottoFunction)
        {
             return ContractHandler.SendRequestAsync(updateLottoFunction);
        }

        public Task<string> UpdateLottoRequestAsync()
        {
             return ContractHandler.SendRequestAsync<UpdateLottoFunction>();
        }

        public Task<TransactionReceipt> UpdateLottoRequestAndWaitForReceiptAsync(UpdateLottoFunction updateLottoFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(updateLottoFunction, cancellationToken);
        }

        public Task<TransactionReceipt> UpdateLottoRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<UpdateLottoFunction>(null, cancellationToken);
        }

        public Task<ViewTicketsOutputDTO> ViewTicketsQueryAsync(ViewTicketsFunction viewTicketsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<ViewTicketsFunction, ViewTicketsOutputDTO>(viewTicketsFunction, blockParameter);
        }

        public Task<ViewTicketsOutputDTO> ViewTicketsQueryAsync(BigInteger competition, BlockParameter blockParameter = null)
        {
            var viewTicketsFunction = new ViewTicketsFunction();
                viewTicketsFunction.Competition = competition;
            
            return ContractHandler.QueryDeserializingToObjectAsync<ViewTicketsFunction, ViewTicketsOutputDTO>(viewTicketsFunction, blockParameter);
        }

        public Task<List<ulong>> WinningNumbersQueryAsync(WinningNumbersFunction winningNumbersFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<WinningNumbersFunction, List<ulong>>(winningNumbersFunction, blockParameter);
        }

        
        public Task<List<ulong>> WinningNumbersQueryAsync(BigInteger competition, BlockParameter blockParameter = null)
        {
            var winningNumbersFunction = new WinningNumbersFunction();
                winningNumbersFunction.Competition = competition;
            
            return ContractHandler.QueryAsync<WinningNumbersFunction, List<ulong>>(winningNumbersFunction, blockParameter);
        }
    }
}
