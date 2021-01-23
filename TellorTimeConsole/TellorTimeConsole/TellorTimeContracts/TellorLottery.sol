pragma solidity >=0.4.21 <0.7.0;

import "./node_modules/usingtellor/contracts/UsingTellor.sol";

import "./node_modules/@openzeppelin/contracts/math/SafeMath.sol";

import "./TellorLotteryPool.sol";

contract TellorLottery is UsingTellor{  
    // SafeMath for div
    using SafeMath for uint256; 

    // Tellor Lottery Pool to distribute dividends to lottery participants.
    TellorLotteryPool public lotteryPool;

    // Variable to hold the requestId.
    uint requestId = 77;

    uint public disputePeriod = 1 seconds; // hours;

    // Structure to hold owner of a ticket number
    struct Ticket{
      address owner;
      uint ticketNumber;
      uint64[6] numbers;
    }

    // Structure to hold variables pertaining to a lotto round.
    struct LottoRound{
      // Winning numbers of the LottoRound.
      uint64[6] winningNumbers;
      // Winning ID
      uint64[22] winningIDs;
      // Prize Pool of the LottoRound.
      uint prizePool;
      // Variable to determine if the winnings have been distributed.
      bool distributed;
      // Mapping to track ticketIDs to ticket.
      mapping(uint64 => Ticket[]) tickets;
      // Mapping to hold jackpot winners from ticket numbers;
      mapping(uint => bool) jackpotWinners;
      // Mapping to hold tier two winners from ticket numbers;
      mapping(uint => bool) tierTwoWinners;
      // Mapping to hold the ticket numbers of an address.
      mapping(address => Ticket[]) participantTickets;
    }

    // Array of lottoRounds;
    mapping(uint => LottoRound) lottoRounds;

    // Variable to hold the latest competition number.
    uint public competitionNumber;

    // Variable to hold the current ticket number of the latest competition.
    uint public ticketNumber;

    // Cost of a ticket;
    uint public ticketCost = 1 * 10 ** 18;

    // Lotto number mask.
    uint8 public mask = 63;

    // Mask to get the competition number fromt the lotto query. 
    uint public competitionMask = 65535;

    // Lotto update event.
    event lottoUpdate(uint competitionNumber ,uint64[22] winningIDs, uint64[6] numbers);

    // Event to be emitted when there is an error buying a ticket.
    event buyTicketError(string error, uint n);

    // Event emitted upon a successful ticket buy.
    event buyTicketSuccess(uint competitionNumber, address sender, uint ticketNumber, uint64[6] numbers, uint prizePool);

    // Event emitted when winnings are distributed.
    event winningsDistributed(uint competitionNumber, address winner, uint prize);

    // Event emitted when distribute winnings function is called.
    event distributeVariables(uint jackpotPrize, uint tierTwoPrize, uint tierThreePrize, 
                              uint jackpotWinnerNum, uint tierTwoWinnerNum, uint tierthreeWinnerNum);

    // Event emitted when ticket ids are generated
    event generatedIDs(uint64[22] ticketIDs);

    // Event emitted to view ticket numbers;
    event viewParticipantTickets(address owner, uint ticketNumber, uint64[6] numbers);

    // Constructor.
    constructor(address payable _tellorAddress, uint _competitionStart) UsingTellor(_tellorAddress) public {
      competitionNumber = _competitionStart;
      lotteryPool = new TellorLotteryPool(address(this), _tellorAddress, _competitionStart, "Tellor Lottery Token", "TLT");
    }

    // Function for a participant to buy a ticket for the next lotto.
    function buyTicket(uint64[6] memory _numbers) public{
      if(tellor.allowance(msg.sender, address(this)) >= ticketCost){
        uint8 i; uint64 last = 0;
        for(i = 0; i < 6; i++){
          if(!( _numbers[i] > last && (_numbers[i] < 61))){
            emit buyTicketError("Invalid ticket numbers.", _numbers[i]);
            return;
          }
          last = _numbers[i];
        }
        if(tellor.transferFrom(msg.sender, address(this), ticketCost)){
          lotteryPool.mint(msg.sender); // mint new Tellor Lottery Token.
          lottoRounds[competitionNumber].prizePool += ticketCost;
          uint64[22] memory ticketIDs = generateTicketIDs(_numbers);
          Ticket memory ticket = Ticket(msg.sender, ++ticketNumber, _numbers);
          lottoRounds[competitionNumber].participantTickets[msg.sender].push(ticket);
          for(i = 0; i < ticketIDs.length; i++){
            lottoRounds[competitionNumber].tickets[ticketIDs[i]].push(ticket);
          }
          emit buyTicketSuccess(competitionNumber, 
          lottoRounds[competitionNumber].tickets[ticketIDs[0]][lottoRounds[competitionNumber].tickets[ticketIDs[0]].length-1].owner, 
          lottoRounds[competitionNumber].tickets[ticketIDs[0]][lottoRounds[competitionNumber].tickets[ticketIDs[0]].length-1].ticketNumber, 
          _numbers,
          lottoRounds[competitionNumber].prizePool);
        }
        return;
      }
      emit buyTicketError("Invalid contract allowance", tellor.allowance(msg.sender, address(this)));
    }

    function senderAllownace() public view returns(uint){
      return tellor.allowance(msg.sender, address(this));
    }

    // Function to generate ticket ids for easy distribution.
    function generateTicketIDs(uint64[6] memory _numbers) public returns(uint64[22] memory ticketIDs){
      uint64[22] memory ids;

      // Full ticket id; Jackpot
      ids[0] = (_numbers[0] << 30) + (_numbers[1] << 24) + (_numbers[2] << 18) + (_numbers[3] << 12) + (_numbers[4] << 6) + _numbers[5];

      // 5 of 6 ticket ids. Tier 2
      ids[1] = (_numbers[0] << 24) + (_numbers[1] << 18) + (_numbers[2] << 12) + (_numbers[3] << 6) + _numbers[4]; // First 5 numbers
      ids[2] = (_numbers[0] << 24) + (_numbers[1] << 18) + (_numbers[2] << 12) + (_numbers[3] << 6) + _numbers[5]; // First 4 and last numbers
      ids[3] = (_numbers[0] << 24) + (_numbers[1] << 18) + (_numbers[2] << 12) + (_numbers[4] << 6) + _numbers[5]; // First 3 and  last 2 numbers
      ids[4] = (_numbers[0] << 24) + (_numbers[1] << 18) + (_numbers[3] << 12) + (_numbers[4] << 6) + _numbers[5]; // First 2 and last 3 numbers
      ids[5] = (_numbers[0] << 24) + (_numbers[2] << 18) + (_numbers[3] << 12) + (_numbers[4] << 6) + _numbers[5]; // First and last 4 numbers
      ids[6] = (_numbers[1] << 24) + (_numbers[2] << 18) + (_numbers[3] << 12) + (_numbers[4] << 6) + _numbers[5]; // Last 5 numbers

      // 4 of 6 ticket ids. Tier3
      ids[7]  = (_numbers[0] << 24) + (_numbers[1] << 18) + (_numbers[2] << 12) + _numbers[3]; // positions 0,1,2,3
      ids[8]  = (_numbers[0] << 24) + (_numbers[1] << 18) + (_numbers[2] << 12) + _numbers[4]; // positions 0,1,2,4
      ids[9]  = (_numbers[0] << 24) + (_numbers[1] << 18) + (_numbers[2] << 12) + _numbers[5]; // positions 0,1,2,5
      ids[10] = (_numbers[0] << 24) + (_numbers[1] << 18) + (_numbers[3] << 12) + _numbers[4]; // positions 0,1,3,4
      ids[11] = (_numbers[0] << 24) + (_numbers[1] << 18) + (_numbers[3] << 12) + _numbers[5]; // positions 0,1,3,5
      ids[12] = (_numbers[0] << 24) + (_numbers[1] << 18) + (_numbers[4] << 12) + _numbers[5]; // positions 0,1,4,5
      ids[13] = (_numbers[0] << 24) + (_numbers[2] << 18) + (_numbers[3] << 12) + _numbers[4]; // positions 0,2,3,4
      ids[14] = (_numbers[0] << 24) + (_numbers[2] << 18) + (_numbers[3] << 12) + _numbers[5]; // positions 0,2,3,5
      ids[15] = (_numbers[0] << 24) + (_numbers[2] << 18) + (_numbers[4] << 12) + _numbers[5]; // positions 0,2,4,5
      ids[16] = (_numbers[0] << 24) + (_numbers[3] << 18) + (_numbers[4] << 12) + _numbers[5]; // positions 0,3,4,5
      ids[17] = (_numbers[1] << 24) + (_numbers[2] << 18) + (_numbers[3] << 12) + _numbers[4]; // positions 1,2,3,4
      ids[18] = (_numbers[1] << 24) + (_numbers[2] << 18) + (_numbers[3] << 12) + _numbers[5]; // positions 1,2,3,5
      ids[19] = (_numbers[1] << 24) + (_numbers[2] << 18) + (_numbers[4] << 12) + _numbers[5]; // positions 1,2,4,5
      ids[20] = (_numbers[1] << 24) + (_numbers[3] << 18) + (_numbers[4] << 12) + _numbers[5]; // positions 1,3,4,5
      ids[21] = (_numbers[2] << 24) + (_numbers[3] << 18) + (_numbers[4] << 12) + _numbers[5]; // positions 2,3,4,5

      emit generatedIDs(ids);

      return ids;
    }

    // Function to view tickets held by a participant for the current round.
    function viewTickets(uint _competition) public view returns(uint[100] memory _ticketNumbers, uint64[600] memory _numbers){
        uint16 numbersIndex = 0;
        uint16 i;
        uint16 j;
        for(i = 0; i < lottoRounds[_competition].participantTickets[msg.sender].length && i < 100; i++){
          _ticketNumbers[i] = (lottoRounds[_competition].participantTickets[msg.sender][i].ticketNumber);
          for(j = 0; j < lottoRounds[_competition].participantTickets[msg.sender][i].numbers.length && i < 600; j++){
            _numbers[numbersIndex] = lottoRounds[_competition].participantTickets[msg.sender][i].numbers[j];
            numbersIndex++;
          }
        }
    }

    // Function to distribute winnings 
    function distributeWinnings(uint _competition) private {
      if(!lottoRounds[_competition].distributed && _competition < competitionNumber){
  
        uint jackpotWinners;
        uint tierTwoWinners;
        uint tierThreeWinners;

        (jackpotWinners, tierTwoWinners, tierThreeWinners) = numberOfWinners(_competition);

        uint jackpotValue; 
        uint tierTwoValue;
        uint tierThreeValue;
       
        (jackpotValue, tierTwoValue, tierThreeValue) = ticketValues(_competition, jackpotWinners, tierTwoWinners, tierThreeWinners);

        emit distributeVariables(jackpotValue, tierTwoValue, tierThreeValue, jackpotWinners, tierTwoWinners, tierThreeWinners);
        
        // Loop to transfer prizes to jackpot winners;
        uint32 i;
        for(i = 0; i < lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[0]].length; i++){
          // Place winner in jackpotWinners mapping
          lottoRounds[_competition].jackpotWinners[lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[0]][i].ticketNumber] = true; 
          tellor.transfer(lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[0]][i].owner, jackpotValue);
          emit winningsDistributed(_competition, lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[0]][i].owner, jackpotValue);
        }

        // Loop to transfer prizes to winners with 5 matches.
        uint32 j;
        for(i = 1; i < 7; i++){
          for(j = 0; j < lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[i]].length; j++){
            // Condition to only transfer 5 match prizes no non jackpot winners;
            if(!lottoRounds[_competition].jackpotWinners[lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[i]][j].ticketNumber]){
              // Place winner in tierTwoWinners mapping
              lottoRounds[_competition].tierTwoWinners[lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[i]][j].ticketNumber] = true; 
              tellor.transfer(lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[i]][j].owner, tierTwoValue);
              emit winningsDistributed(_competition, lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[i]][j].owner, tierTwoValue);
            }
          }
        }

        // Loop to transfer prizes to winners with 4 matches.
        for(i = 7; i < 22; i++){
          for(j = 0; j < lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[i]].length; j++){
            // Condition to only transfer 4 match prizes, no non jackpot or 5 match winners;
            if(!lottoRounds[_competition].jackpotWinners[lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[i]][j].ticketNumber]
               && !lottoRounds[_competition].tierTwoWinners[lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[i]][j].ticketNumber]){
              tellor.transfer(lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[i]][j].owner, tierThreeValue);
              emit winningsDistributed(_competition, lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[i]][j].owner, tierThreeValue);
            }
          }
        }
        // get value to update lottery pool.
        uint poolValue = (lottoRounds[_competition].prizePool / 100) * 25;
        
        tellor.approve(address(lotteryPool), poolValue);

        lotteryPool.updatePool();

        lottoRounds[_competition].distributed = true;
      }
    }


    // Function to get the values of winning tickets.
    function ticketValues(uint _competition, uint _jackpotWinners, uint _tierTwoWinners, uint _tierThreeWinners) private returns (uint _jackpotValue, uint _tierTwoValue, uint _tierThreeValue){
      uint c = _competition;

      // determine if there was a jackpot winner in the competition round. if not add jackpotValue of _competition to current round.
      _jackpotWinners == 0 ? lottoRounds[c+1].prizePool += (lottoRounds[c].prizePool / 1000) * 450 
                           : _jackpotValue = (( lottoRounds[c].prizePool / 1000) * 450) / _jackpotWinners;

      // determine if there was a tier two winner in the competition round. if not add tierTwoValue of _competition to current round.
      _tierTwoWinners == 0 ? lottoRounds[c+1].prizePool += (lottoRounds[c].prizePool / 1000) * 225 
                           : _tierTwoValue = ((lottoRounds[c].prizePool / 1000) * 225) / _tierTwoWinners;

      // determine if there was a tier three winner in the competition round. if not add tierThreeValue of _competition to current round.
      _tierThreeWinners == 0 ? lottoRounds[c+1].prizePool += ( lottoRounds[c].prizePool / 1000) * 75 
                             : _tierThreeValue = ((lottoRounds[c].prizePool / 1000) * 75) / _tierThreeWinners;

      return (_jackpotValue, _tierTwoValue, _tierThreeValue);
    }

    function numberOfWinners(uint _competition) public view returns(uint _jackpotWinners, uint _tierTwoWinners, uint _tierThreeWinners){
      // number of jackpot winners 
      _jackpotWinners = lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[0]].length;

      uint8 i;
      // number of winners with five . Tier 2
      for(i = 1; i < 7; i++){
          _tierTwoWinners += lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[i]].length - lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[0]].length;
      }

      uint index = 21;
      uint8 j;
      // number of winners with four matches. Tier 3
      for(i = 6; i > 0; i--){
        for(j = i-1; j > 0; j--){
          _tierThreeWinners += lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[index]].length 
                              - lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[0]].length
                              - (lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[i]].length - lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[0]].length)
                              - (lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[j]].length - lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[0]].length);
          index--;
        }
      }

      return (_jackpotWinners, _tierTwoWinners, _tierThreeWinners);
    }

    // Function to update the lotto contract.
    function updateLotto() public {
      bool _didGet;
      uint _timestamp;
      uint _value;
      (_didGet, _value, _timestamp) =  getDataBefore(requestId, now - disputePeriod); //getCurrentValue(requestId);

      // Condition to determine of the query was successful.
      if(_didGet){
        uint cNumber = _value & competitionMask;

        // Condition to determine if the current round is over.
        if(cNumber == competitionNumber){
          uint64 lottoNumbers = uint64(_value >> 16);

          // Setting the current lotto rounds winning numbers.
          lottoRounds[competitionNumber].winningNumbers = [uint64(0), 0, 0, 0, 0, 0];
          int8 i;
          for(i = 5; i >= 0; i--){
            lottoRounds[competitionNumber].winningNumbers[uint(i)] = uint8(lottoNumbers) & mask;
            lottoNumbers = lottoNumbers >> 6;
          }
          lottoRounds[competitionNumber].winningIDs = generateTicketIDs(lottoRounds[competitionNumber].winningNumbers);
          emit lottoUpdate(cNumber, lottoRounds[competitionNumber].winningIDs, lottoRounds[competitionNumber].winningNumbers);
          competitionNumber++;
          distributeWinnings(competitionNumber - 1);
        } 
      }
    }

    // Function to view the winning numbers.
    function winningNumbers(uint _competition) public view returns(uint64[6] memory numbers){
        return lottoRounds[_competition].winningNumbers;
    }
    // Function to view the prize pool.
    function prizePool(uint _competition) public view returns(uint prize){
      return lottoRounds[_competition].prizePool;
    }

  // loto api https://apiloterias.com.br/app/resultado?loteria=megasena&token=TellorSB&concurso=
}
