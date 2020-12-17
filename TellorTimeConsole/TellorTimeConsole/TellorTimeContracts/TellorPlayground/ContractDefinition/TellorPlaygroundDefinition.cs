using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts;
using System.Threading;

namespace TellorTimeContracts.Contracts.TellorPlayground.ContractDefinition
{


    public partial class TellorPlaygroundDeployment : TellorPlaygroundDeploymentBase
    {
        public TellorPlaygroundDeployment() : base(BYTECODE) { }
        public TellorPlaygroundDeployment(string byteCode) : base(byteCode) { }
    }

    public class TellorPlaygroundDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "608060405234801561001057600080fd5b506040805180820190915260108082526f15195b1b1bdc941b185e59dc9bdd5b9960821b602090920191825261004891600791610087565b50604080518082019091526004808252630545242560e41b602090920191825261007491600891610087565b506009805460ff1916601217905561011a565b828054600181600116156101000203166002900490600052602060002090601f016020900481019282601f106100c857805160ff19168380011785556100f5565b828001600101855582156100f5579182015b828111156100f55782518255916020019190600101906100da565b50610101929150610105565b5090565b5b808211156101015760008155600101610106565b610ee9806101296000396000f3fe608060405234801561001057600080fd5b506004361061014d5760003560e01c8063752d49a1116100c3578063a9059cbb1161007c578063a9059cbb1461041a578063acebfc5414610446578063b041d69614610469578063b86d1d631461048c578063dd62ed3e146104b2578063fb0ceb04146104e05761014d565b8063752d49a11461035a57806377fbb6631461037d57806393fa4915146103a057806395d89b41146103c3578063a3183701146103cb578063a457c2d7146103ee5761014d565b8063313ce56711610115578063313ce5671461028557806339509351146102a35780633df0777b146102cf57806346eee1c4146102f257806362f551121461030f57806370a08231146103345761014d565b806306fdde0314610152578063095ea7b3146101cf57806318160ddd1461020f57806323b872dd1461022957806327e235e31461025f575b600080fd5b61015a610503565b6040805160208082528351818301528351919283929083019185019080838360005b8381101561019457818101518382015260200161017c565b50505050905090810190601f1680156101c15780820380516001836020036101000a031916815260200191505b509250505060405180910390f35b6101fb600480360360408110156101e557600080fd5b506001600160a01b038135169060200135610599565b604080519115158252519081900360200190f35b6102176105b0565b60408051918252519081900360200190f35b6101fb6004803603606081101561023f57600080fd5b506001600160a01b038135811691602081013590911690604001356105b6565b6102176004803603602081101561027557600080fd5b50356001600160a01b031661061f565b61028d610631565b6040805160ff9092168252519081900360200190f35b6101fb600480360360408110156102b957600080fd5b506001600160a01b03813516906020013561063a565b6101fb600480360360408110156102e557600080fd5b5080359060200135610670565b6102176004803603602081101561030857600080fd5b5035610690565b6103326004803603604081101561032557600080fd5b50803590602001356106a2565b005b6102176004803603602081101561034a57600080fd5b50356001600160a01b031661071f565b6103326004803603604081101561037057600080fd5b508035906020013561073a565b6102176004803603604081101561039357600080fd5b5080359060200135610780565b610217600480360360408110156103b657600080fd5b50803590602001356107d9565b61015a6107f4565b610217600480360360408110156103e157600080fd5b5080359060200135610855565b6101fb6004803603604081101561040457600080fd5b506001600160a01b03813516906020013561086f565b6101fb6004803603604081101561043057600080fd5b506001600160a01b0381351690602001356108be565b6103326004803603604081101561045c57600080fd5b50803590602001356108cb565b6101fb6004803603604081101561047f57600080fd5b5080359060200135610907565b610332600480360360208110156104a257600080fd5b50356001600160a01b0316610927565b610217600480360360408110156104c857600080fd5b506001600160a01b038135811691602001351661093d565b610217600480360360408110156104f657600080fd5b5080359060200135610968565b60078054604080516020601f600260001961010060018816150201909516949094049384018190048102820181019092528281526060939092909183018282801561058f5780601f106105645761010080835404028352916020019161058f565b820191906000526020600020905b81548152906001019060200180831161057257829003601f168201915b5050505050905090565b60006105a6338484610996565b5060015b92915050565b60065490565b60006105c3848484610a82565b610615843361061085604051806060016040528060288152602001610e1e602891396001600160a01b038a1660009081526005602090815260408083203384529091529020549190610bd4565b610996565b5060019392505050565b60036020526000908152604090205481565b60095460ff1690565b3360008181526005602090815260408083206001600160a01b038716845290915281205490916105a69185906106109086610c6b565b600091825260016020908152604080842092845291905290205460ff1690565b60009081526002602052604090205490565b600082815260208181526040808320428085529083528184208590558584526002835281842080546001810182559085529383902090930183905580518581529182019290925280820183905290517fba11e319aee26e7bbac889432515ba301ec8f6d27bf6b94829c21a65c5f6ff259181900360600190a15050565b6001600160a01b031660009081526004602052604090205490565b610745333083610a82565b604080518281529051839133917f9e771e1220a6c2e407f3601f70a769ca9fff75a110d1687e0b582824673a1f5c9181900360200190a35050565b60008281526002602052604081205480158061079c5750828111155b156107ab5760009150506105aa565b60008481526002602052604090208054849081106107c557fe5b906000526020600020015491505092915050565b60009182526020828152604080842092845291905290205490565b60088054604080516020601f600260001961010060018816150201909516949094049384018190048102820181019092528281526060939092909183018282801561058f5780601f106105645761010080835404028352916020019161058f565b600060208181529281526040808220909352908152205481565b60006105a6338461061085604051806060016040528060258152602001610e8f602591393360009081526005602090815260408083206001600160a01b038d1684529091529020549190610bd4565b60006105a6338484610a82565b6000828152602081815260408083208484528252808320839055938252600180825284832093835292905291909120805460ff19169091179055565b600160209081526000928352604080842090915290825290205460ff1681565b61093a81683635c9adc5dea00000610ccc565b50565b6001600160a01b03918216600090815260056020908152604080832093909416825291909152205490565b6002602052816000526040600020818154811061098157fe5b90600052602060002001600091509150505481565b6001600160a01b0383166109db5760405162461bcd60e51b8152600401808060200182810382526024815260200180610e6b6024913960400191505060405180910390fd5b6001600160a01b038216610a205760405162461bcd60e51b8152600401808060200182810382526022815260200180610dd66022913960400191505060405180910390fd5b6001600160a01b03808416600081815260056020908152604080832094871680845294825291829020859055815185815291517f8c5be1e5ebec7d5bd14f71427d1e84f3dd0314c0f7b2291e5b200ac8c7c3b9259281900390910190a3505050565b6001600160a01b038316610ac75760405162461bcd60e51b8152600401808060200182810382526025815260200180610e466025913960400191505060405180910390fd5b6001600160a01b038216610b0c5760405162461bcd60e51b8152600401808060200182810382526023815260200180610db36023913960400191505060405180910390fd5b610b4981604051806060016040528060268152602001610df8602691396001600160a01b0386166000908152600460205260409020549190610bd4565b6001600160a01b038085166000908152600460205260408082209390935590841681522054610b789082610c6b565b6001600160a01b0380841660008181526004602090815260409182902094909455805185815290519193928716927fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef92918290030190a3505050565b60008184841115610c635760405162461bcd60e51b81526004018080602001828103825283818151815260200191508051906020019080838360005b83811015610c28578181015183820152602001610c10565b50505050905090810190601f168015610c555780820380516001836020036101000a031916815260200191505b509250505060405180910390fd5b505050900390565b600082820183811015610cc5576040805162461bcd60e51b815260206004820152601b60248201527f536166654d6174683a206164646974696f6e206f766572666c6f770000000000604482015290519081900360640190fd5b9392505050565b6001600160a01b038216610d27576040805162461bcd60e51b815260206004820152601f60248201527f45524332303a206d696e7420746f20746865207a65726f206164647265737300604482015290519081900360640190fd5b600654610d349082610c6b565b6006556001600160a01b038216600090815260046020526040902054610d5a9082610c6b565b6001600160a01b03831660008181526004602090815260408083209490945583518581529351929391927fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef9281900390910190a3505056fe45524332303a207472616e7366657220746f20746865207a65726f206164647265737345524332303a20617070726f766520746f20746865207a65726f206164647265737345524332303a207472616e7366657220616d6f756e7420657863656564732062616c616e636545524332303a207472616e7366657220616d6f756e74206578636565647320616c6c6f77616e636545524332303a207472616e736665722066726f6d20746865207a65726f206164647265737345524332303a20617070726f76652066726f6d20746865207a65726f206164647265737345524332303a2064656372656173656420616c6c6f77616e63652062656c6f77207a65726fa26469706673582212200eda70bc72051b5b09592ba59611435841c32b196586c0e9b5117e80533e891b64736f6c63430007000033";
        public TellorPlaygroundDeploymentBase() : base(BYTECODE) { }
        public TellorPlaygroundDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class AddTipFunction : AddTipFunctionBase { }

    [Function("addTip")]
    public class AddTipFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_requestId", 1)]
        public virtual BigInteger RequestId { get; set; }
        [Parameter("uint256", "_amount", 2)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class AllowanceFunction : AllowanceFunctionBase { }

    [Function("allowance", "uint256")]
    public class AllowanceFunctionBase : FunctionMessage
    {
        [Parameter("address", "owner", 1)]
        public virtual string Owner { get; set; }
        [Parameter("address", "spender", 2)]
        public virtual string Spender { get; set; }
    }

    public partial class ApproveFunction : ApproveFunctionBase { }

    [Function("approve", "bool")]
    public class ApproveFunctionBase : FunctionMessage
    {
        [Parameter("address", "spender", 1)]
        public virtual string Spender { get; set; }
        [Parameter("uint256", "amount", 2)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class BalanceOfFunction : BalanceOfFunctionBase { }

    [Function("balanceOf", "uint256")]
    public class BalanceOfFunctionBase : FunctionMessage
    {
        [Parameter("address", "account", 1)]
        public virtual string Account { get; set; }
    }

    public partial class BalancesFunction : BalancesFunctionBase { }

    [Function("balances", "uint256")]
    public class BalancesFunctionBase : FunctionMessage
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class DecimalsFunction : DecimalsFunctionBase { }

    [Function("decimals", "uint8")]
    public class DecimalsFunctionBase : FunctionMessage
    {

    }

    public partial class DecreaseAllowanceFunction : DecreaseAllowanceFunctionBase { }

    [Function("decreaseAllowance", "bool")]
    public class DecreaseAllowanceFunctionBase : FunctionMessage
    {
        [Parameter("address", "spender", 1)]
        public virtual string Spender { get; set; }
        [Parameter("uint256", "subtractedValue", 2)]
        public virtual BigInteger SubtractedValue { get; set; }
    }

    public partial class DisputeValueFunction : DisputeValueFunctionBase { }

    [Function("disputeValue")]
    public class DisputeValueFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_requestId", 1)]
        public virtual BigInteger RequestId { get; set; }
        [Parameter("uint256", "_timestamp", 2)]
        public virtual BigInteger Timestamp { get; set; }
    }

    public partial class FaucetFunction : FaucetFunctionBase { }

    [Function("faucet")]
    public class FaucetFunctionBase : FunctionMessage
    {
        [Parameter("address", "user", 1)]
        public virtual string User { get; set; }
    }

    public partial class GetNewValueCountbyRequestIdFunction : GetNewValueCountbyRequestIdFunctionBase { }

    [Function("getNewValueCountbyRequestId", "uint256")]
    public class GetNewValueCountbyRequestIdFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_requestId", 1)]
        public virtual BigInteger RequestId { get; set; }
    }

    public partial class GetTimestampbyRequestIDandIndexFunction : GetTimestampbyRequestIDandIndexFunctionBase { }

    [Function("getTimestampbyRequestIDandIndex", "uint256")]
    public class GetTimestampbyRequestIDandIndexFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_requestId", 1)]
        public virtual BigInteger RequestId { get; set; }
        [Parameter("uint256", "index", 2)]
        public virtual BigInteger Index { get; set; }
    }

    public partial class IncreaseAllowanceFunction : IncreaseAllowanceFunctionBase { }

    [Function("increaseAllowance", "bool")]
    public class IncreaseAllowanceFunctionBase : FunctionMessage
    {
        [Parameter("address", "spender", 1)]
        public virtual string Spender { get; set; }
        [Parameter("uint256", "addedValue", 2)]
        public virtual BigInteger AddedValue { get; set; }
    }

    public partial class IsDisputedFunction : IsDisputedFunctionBase { }

    [Function("isDisputed", "bool")]
    public class IsDisputedFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
        [Parameter("uint256", "", 2)]
        public virtual BigInteger ReturnValue2 { get; set; }
    }

    public partial class IsInDisputeFunction : IsInDisputeFunctionBase { }

    [Function("isInDispute", "bool")]
    public class IsInDisputeFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_requestId", 1)]
        public virtual BigInteger RequestId { get; set; }
        [Parameter("uint256", "_timestamp", 2)]
        public virtual BigInteger Timestamp { get; set; }
    }

    public partial class NameFunction : NameFunctionBase { }

    [Function("name", "string")]
    public class NameFunctionBase : FunctionMessage
    {

    }

    public partial class RetrieveDataFunction : RetrieveDataFunctionBase { }

    [Function("retrieveData", "uint256")]
    public class RetrieveDataFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_requestId", 1)]
        public virtual BigInteger RequestId { get; set; }
        [Parameter("uint256", "_timestamp", 2)]
        public virtual BigInteger Timestamp { get; set; }
    }

    public partial class SubmitValueFunction : SubmitValueFunctionBase { }

    [Function("submitValue")]
    public class SubmitValueFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_requestId", 1)]
        public virtual BigInteger RequestId { get; set; }
        [Parameter("uint256", "_value", 2)]
        public virtual BigInteger Value { get; set; }
    }

    public partial class SymbolFunction : SymbolFunctionBase { }

    [Function("symbol", "string")]
    public class SymbolFunctionBase : FunctionMessage
    {

    }

    public partial class TimestampsFunction : TimestampsFunctionBase { }

    [Function("timestamps", "uint256")]
    public class TimestampsFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
        [Parameter("uint256", "", 2)]
        public virtual BigInteger ReturnValue2 { get; set; }
    }

    public partial class TotalSupplyFunction : TotalSupplyFunctionBase { }

    [Function("totalSupply", "uint256")]
    public class TotalSupplyFunctionBase : FunctionMessage
    {

    }

    public partial class TransferFunction : TransferFunctionBase { }

    [Function("transfer", "bool")]
    public class TransferFunctionBase : FunctionMessage
    {
        [Parameter("address", "recipient", 1)]
        public virtual string Recipient { get; set; }
        [Parameter("uint256", "amount", 2)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class TransferFromFunction : TransferFromFunctionBase { }

    [Function("transferFrom", "bool")]
    public class TransferFromFunctionBase : FunctionMessage
    {
        [Parameter("address", "sender", 1)]
        public virtual string Sender { get; set; }
        [Parameter("address", "recipient", 2)]
        public virtual string Recipient { get; set; }
        [Parameter("uint256", "amount", 3)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class ValuesFunction : ValuesFunctionBase { }

    [Function("values", "uint256")]
    public class ValuesFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
        [Parameter("uint256", "", 2)]
        public virtual BigInteger ReturnValue2 { get; set; }
    }

    public partial class ApprovalEventDTO : ApprovalEventDTOBase { }

    [Event("Approval")]
    public class ApprovalEventDTOBase : IEventDTO
    {
        [Parameter("address", "owner", 1, true )]
        public virtual string Owner { get; set; }
        [Parameter("address", "spender", 2, true )]
        public virtual string Spender { get; set; }
        [Parameter("uint256", "value", 3, false )]
        public virtual BigInteger Value { get; set; }
    }

    public partial class NewValueEventDTO : NewValueEventDTOBase { }

    [Event("NewValue")]
    public class NewValueEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "_requestId", 1, false )]
        public virtual BigInteger RequestId { get; set; }
        [Parameter("uint256", "_time", 2, false )]
        public virtual BigInteger Time { get; set; }
        [Parameter("uint256", "_value", 3, false )]
        public virtual BigInteger Value { get; set; }
    }

    public partial class TipAddedEventDTO : TipAddedEventDTOBase { }

    [Event("TipAdded")]
    public class TipAddedEventDTOBase : IEventDTO
    {
        [Parameter("address", "_sender", 1, true )]
        public virtual string Sender { get; set; }
        [Parameter("uint256", "_requestId", 2, true )]
        public virtual BigInteger RequestId { get; set; }
        [Parameter("uint256", "_tip", 3, false )]
        public virtual BigInteger Tip { get; set; }
    }

    public partial class TransferEventDTO : TransferEventDTOBase { }

    [Event("Transfer")]
    public class TransferEventDTOBase : IEventDTO
    {
        [Parameter("address", "from", 1, true )]
        public virtual string From { get; set; }
        [Parameter("address", "to", 2, true )]
        public virtual string To { get; set; }
        [Parameter("uint256", "value", 3, false )]
        public virtual BigInteger Value { get; set; }
    }



    public partial class AllowanceOutputDTO : AllowanceOutputDTOBase { }

    [FunctionOutput]
    public class AllowanceOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }



    public partial class BalanceOfOutputDTO : BalanceOfOutputDTOBase { }

    [FunctionOutput]
    public class BalanceOfOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class BalancesOutputDTO : BalancesOutputDTOBase { }

    [FunctionOutput]
    public class BalancesOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class DecimalsOutputDTO : DecimalsOutputDTOBase { }

    [FunctionOutput]
    public class DecimalsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint8", "", 1)]
        public virtual byte ReturnValue1 { get; set; }
    }







    public partial class GetNewValueCountbyRequestIdOutputDTO : GetNewValueCountbyRequestIdOutputDTOBase { }

    [FunctionOutput]
    public class GetNewValueCountbyRequestIdOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class GetTimestampbyRequestIDandIndexOutputDTO : GetTimestampbyRequestIDandIndexOutputDTOBase { }

    [FunctionOutput]
    public class GetTimestampbyRequestIDandIndexOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }



    public partial class IsDisputedOutputDTO : IsDisputedOutputDTOBase { }

    [FunctionOutput]
    public class IsDisputedOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }

    public partial class IsInDisputeOutputDTO : IsInDisputeOutputDTOBase { }

    [FunctionOutput]
    public class IsInDisputeOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }

    public partial class NameOutputDTO : NameOutputDTOBase { }

    [FunctionOutput]
    public class NameOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class RetrieveDataOutputDTO : RetrieveDataOutputDTOBase { }

    [FunctionOutput]
    public class RetrieveDataOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }



    public partial class SymbolOutputDTO : SymbolOutputDTOBase { }

    [FunctionOutput]
    public class SymbolOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class TimestampsOutputDTO : TimestampsOutputDTOBase { }

    [FunctionOutput]
    public class TimestampsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class TotalSupplyOutputDTO : TotalSupplyOutputDTOBase { }

    [FunctionOutput]
    public class TotalSupplyOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }





    public partial class ValuesOutputDTO : ValuesOutputDTOBase { }

    [FunctionOutput]
    public class ValuesOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }
}
