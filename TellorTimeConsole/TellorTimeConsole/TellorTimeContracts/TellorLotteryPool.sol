pragma solidity >=0.4.21 <0.7.0;

import "./node_modules/@openzeppelin/contracts/token/ERC20/ERC20.sol";

import "./node_modules/usingtellor/Interface/ITellor.sol";

contract TellorLotteryPool is ERC20{
    // Tellor interface variable to recieve TRBP from the TellorLottery contract.
    ITellor tellor;

    // Address to hold the lotterycontract. uitilized in update pool external function.
    address lotteryContract;

    // Struct to hold variables relevant to pools created after lotto rounds.
    struct Pool{
        uint256 totalStaked;
        uint totalValue;
    }

    // Struct to hold variable relavent to a participants stake.
    struct Stake{
        uint256 amount;
        uint32 poolNumber;
    }

    // Mapping to hold/retrieve a participants Stake info.
    mapping (address => Stake) participantStake;

    // Mapping to maintain pools via there pool number.
    mapping (uint32 => Pool) lottoPools;

    // The current pool that will be viable for new stakers.
    uint32 currentPoolNumber;

    // current staked amount lotteryPool tokens.
    uint public currentStaked;

    // amount to mint when a ticket is bought.
    uint mintAmount = 1 * 10 ** 18;

    event staked(uint _amount);

    event poolUpdate(uint32 poolNumber, uint staked, uint value);

    // Contructor initializes lotter contract and creates Tellor Lottery Token.
    constructor(address _lotteryContract, address _tellor, uint _poolNumber, string memory _name, string memory _symbol) public ERC20(_name, _symbol){
        lotteryContract = _lotteryContract;
        tellor = ITellor(_tellor);
        currentPoolNumber = uint32(_poolNumber);
    }

    // function to update the stake pool after a lotto round.
    function updatePool() external {
        require(msg.sender == lotteryContract, "Invalid lottery contract address."); 
        
        uint poolTotalValue = tellor.allowance(msg.sender, address(this));

        // Transfer TRBP into the lotto pool.
        tellor.transferFrom(msg.sender, address(this), poolTotalValue);

        lottoPools[currentPoolNumber] = Pool(currentStaked, poolTotalValue);

        emit poolUpdate(currentPoolNumber, lottoPools[currentPoolNumber].totalStaked, lottoPools[currentPoolNumber].totalValue);

        currentPoolNumber++;
    }

    function mint(address receiver) external{
        require(msg.sender == lotteryContract, "Invalid lottery contract address.");
        _mint(receiver, mintAmount);
    }

    // function for a participant to stake Tellor Lottery tokens
    function stake(uint _amount) public{        
        require(this.allowance(msg.sender, address(this)) >= _amount && this.allowance(msg.sender, address(this)) > 0, "Invalid allowance for stake.");

        if(!harvest()){
            participantStake[msg.sender].poolNumber = currentPoolNumber;
        }

        this.transferFrom(msg.sender, address(this), _amount);

        participantStake[msg.sender].amount += _amount;

        currentStaked += _amount;
    }

    // function for a participant to unstake Tellor Lottery tokens
    function unstake(uint _amount) public{
        require(participantStake[msg.sender].amount >= _amount, "Invalid unstake amount");
        
        harvest();

        currentStaked -= _amount;

        if(participantStake[msg.sender].amount == _amount){
            delete(participantStake[msg.sender]);
            return;
        }

        participantStake[msg.sender].amount -= _amount;
    }

    // function for a participant to havest rewards from staking Tellor Lottery tokens
    function harvest() public returns(bool _success){
        if(participantStake[msg.sender].poolNumber > 0 && participantStake[msg.sender].poolNumber != currentPoolNumber){
            uint32 i;
            for(i = participantStake[msg.sender].poolNumber; i < currentPoolNumber; i++){
                tellor.transfer(msg.sender, 
                ((participantStake[msg.sender].amount * (10 ** 18)).div(lottoPools[i].totalStaked) * lottoPools[i].totalValue) / 10 ** 18);
            }
            participantStake[msg.sender].poolNumber = currentPoolNumber;
            return true;
        }
        return false;
    }

    // function to view a participants stake and the pool yet to be harvested.
    function viewStake() public view returns(uint _poolNumber, uint _amount){
        return (participantStake[msg.sender].poolNumber, participantStake[msg.sender].amount);
    }

    function viewHarvest() public view returns(uint[25] memory _poolNumbers, uint[25] memory _amount){
        uint32 i;
        uint index = 0;
        for(i = participantStake[msg.sender].poolNumber; i < currentPoolNumber && index < 25; i++){
            _poolNumbers[index] = i;
            _amount[index] = ((participantStake[msg.sender].amount * (10 ** 18)).div(lottoPools[i].totalStaked) * lottoPools[i].totalValue) / 10 ** 18;
        }
        return(_poolNumbers, _amount);
    }
}