pragma solidity >=0.4.21 <0.7.0;

import "./node_modules/usingtellor/contracts/UsingTellor.sol";

contract TellorTime is UsingTellor{

    // Variable to hold the requestId.
    uint requestId = 77;

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
      uint64[7] winningIDs;
      // Prize Pool of the LottoRound.
      uint prizePool;
      // Variable to determine if the winnings have been distributed.
      bool distributed;
      // Mapping to track ticketIDs to ticket.
      mapping(uint64 => Ticket[]) tickets;
      // Mapping to hold jackpot winners from ticket numbers;
      mapping(uint => bool) jackpotWinners;
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
    event lottoUpdate(uint competitionNumber ,uint64[7] winningIDs, uint64[6] numbers);

    // Event to be emitted when there is an error buying a ticket.
    event buyTicketError(string error, uint n);

    // Event emitted upon a successful ticket buy.
    event buyTicketSuccess(uint competitionNumber, address sender, uint ticketNumber, uint64[6] numbers, uint prizePool);

    // Event emitted when winnings are distributed.
    event winningsDistributed(uint competitionNumber, address winner, uint prize);

    // Event emitted when distribute winnings function is called.
    event distributeVariables(uint jackpotPrizePool, uint fiveMatchesPrizepool, uint jackpotWinnerNum, uint fiveMatchWinnerNum);

    // Event emitted when ticket ids are generated
    event generatedIDs(uint64[7] ticketIDs);

    // Event emitted to view ticket numbers;
    event viewParticipantTickets(address owner, uint ticketNumber, uint64[6] numbers);

    // Constructor.
    constructor(address payable _tellorAddress, uint _competitionStart) UsingTellor(_tellorAddress) public {
      competitionNumber = _competitionStart;
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
          lottoRounds[competitionNumber].prizePool += ticketCost;
          uint64[7] memory ticketIDs = generateTicketIDs(_numbers);
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

    // Function to generat ticket ids for easy distribution.
    function generateTicketIDs(uint64[6] memory _numbers) public returns(uint64[7] memory ticketIDs){
      uint64[7] memory ids;

      // Full ticket id;
      ids[0] = (_numbers[0] << 30) + (_numbers[1] << 24) + (_numbers[2] << 18) + (_numbers[3] << 12) + (_numbers[4] << 6) + _numbers[5];

      // 5 of 6 ticket ids.
      ids[1] = (_numbers[0] << 24) + (_numbers[1] << 18) + (_numbers[2] << 12) + (_numbers[3] << 6) + _numbers[4]; // First 5 numbers
      ids[2] = (_numbers[0] << 24) + (_numbers[1] << 18) + (_numbers[2] << 12) + (_numbers[3] << 6) + _numbers[5]; // First 4 and last numbers
      ids[3] = (_numbers[0] << 24) + (_numbers[1] << 18) + (_numbers[2] << 12) + (_numbers[4] << 6) + _numbers[5]; // First 3 and  last 2 numbers
      ids[4] = (_numbers[0] << 24) + (_numbers[1] << 18) + (_numbers[3] << 12) + (_numbers[4] << 6) + _numbers[5]; // First 2 and last 3 numbers
      ids[5] = (_numbers[0] << 24) + (_numbers[2] << 18) + (_numbers[3] << 12) + (_numbers[4] << 6) + _numbers[5]; // First and last 4 numbers
      ids[6] = (_numbers[1] << 24) + (_numbers[2] << 18) + (_numbers[3] << 12) + (_numbers[4] << 6) + _numbers[5]; // Last 5 numbers

      emit generatedIDs(ids);

      return ids;
    }

    // Function to view tickets held by a participant for the current round.
    function viewTickets(uint _competition) public{
        uint8 i;
        for(i = 0; i < lottoRounds[_competition].participantTickets[msg.sender].length; i++){
          emit viewParticipantTickets(lottoRounds[_competition].participantTickets[msg.sender][i].owner,
           lottoRounds[_competition].participantTickets[msg.sender][i].ticketNumber, lottoRounds[_competition].participantTickets[msg.sender][i].numbers);
        }
    }

    // Function to distribute winnings 
    function distributeWinnings(uint _competition) public {
      if(!lottoRounds[_competition].distributed && _competition < competitionNumber){
        
        uint jackpotValue; 
        
        uint fiveMatchTicketNumber = 0;
        uint fiveMatchValue;

        // get tickets with only 5 matches.
        uint8 i;
        for(i = 1; i < lottoRounds[_competition].winningIDs.length; i++){
           fiveMatchTicketNumber += lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[i]].length - lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[0]].length;
        }

        // determine if there was a jackpot winner this in the competition round. if not add jackpotValue of _competition to current round.
        if(lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[0]].length == 0){
          lottoRounds[competitionNumber].prizePool += (lottoRounds[_competition].prizePool / 100) * 60;
        }
        else{
          jackpotValue = ((lottoRounds[_competition].prizePool / 100) * 60) / lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[0]].length;
        }
        if(fiveMatchTicketNumber == 0)
        {
          lottoRounds[competitionNumber].prizePool += (lottoRounds[_competition].prizePool / 100) * 40;
        }
        else{
          fiveMatchValue = ((lottoRounds[_competition].prizePool / 100) * 40) / fiveMatchTicketNumber;
        }

        emit distributeVariables(jackpotValue, fiveMatchValue, lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[0]].length, fiveMatchTicketNumber);
        
        // Loop to transfer prizes to jackpot winners;
        for(i = 0; i < lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[0]].length; i++){
          lottoRounds[_competition].jackpotWinners[lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[0]][i].ticketNumber] = true; // Place winner in jackpotWinners mapping
          tellor.transfer(lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[0]][i].owner, jackpotValue);
          emit winningsDistributed(_competition, lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[0]][i].owner, jackpotValue);
        }

        // Loop to transfer prizes to winners with 5 matches.
        for(i = 1; i < lottoRounds[_competition].winningIDs.length; i++){
          uint8 j;
          for(j = 0; j < lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[i]].length; j++){
            // Condition to only transfer 5 match prizes no non jackpot winners;
            if(!lottoRounds[_competition].jackpotWinners[lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[i]][j].ticketNumber]){
              tellor.transfer(lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[i]][j].owner, fiveMatchValue);
              emit winningsDistributed(_competition, lottoRounds[_competition].tickets[lottoRounds[_competition].winningIDs[i]][j].owner, fiveMatchValue);
            }
          }
        }
        lottoRounds[_competition].distributed = true;
      }
    }

    // Function to update the lotto contract.
    function updateLotto() public {
      bool _didGet;
      uint _timestamp;
      uint _value;
      (_didGet, _value, _timestamp) = getCurrentValue(requestId);

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
