import React, { Component} from "react";

import { Table, Input } from 'semantic-ui-react';

import "./Main.css";

class Main extends Component{

    state = {balance: 0, prizePool: 0, latestNumbers: [0,0,0,0,0,0], buyNumbers: [0,0,0,0,0,0], cells: [0,1,2,3,4,5], transactionText: ""}
    constructor(props){
        super();
        this.props = props;
        setInterval(() => {
            this.latestWinningNumbers();
            this.getBalance();
            this.getPrizePool();
        },10000);
    }

    updateBuyNumbers = async (n, e) =>{
        var numbers = this.state.buyNumbers;
        numbers[n] = e.target.value;
        this.setState({buyNumbers: numbers});
        var i;
        for(i = 0; i < this.state.buyNumbers.length; i++ ){
            //console.log(this.state.buyNumbers[i]);
        }
    }

    buyTicket = async () =>{
        if(this.props.web3 !== null){
            var numbers = this.state.buyNumbers;
            numbers.sort((a,b) => a - b);

            console.log(numbers)

            this.setState({buyNumbers: numbers});

            var i, last = 0;
            for(i = 0; i < numbers.length; i++){
                if(parseInt(numbers[i]) < 61 && parseInt(numbers[i]) > parseInt(last)){
                    last = numbers[i];
                }
                else{
                    this.setState({transactionText:"Invalid ticket. (Make sure numbers are from 1 to 60 with no repeats.)"});
                    return;
                }
            }
            console.log(numbers);

            var tellorLotto = this.props.tellorTime;
            var playground = this.props.tellorPlayGround;
            var web3 = this.props.web3;

            this.setState({transactionText:"Approving 1 TRBP to buy a ticket..."});
            // allow TRBP to be used by the contract
            await playground.methods.approve(tellorLotto.options.address,  web3.utils.toWei('1', 'ether')).send({from: this.props.accounts[0]});

            await playground.methods.allowance(this.props.accounts[0], tellorLotto.options.address).call().then(function(result){
                console.log("allowance " + result)
            });

            this.setState({transactionText:"Buying Ticket..."});

            var buyTicketReceipt = await this.props.tellorTime.methods.buyTicket(numbers).send({from: this.props.accounts[0]});

            console.log(buyTicketReceipt);

            this.setState({transactionText:"Ticket Bought!"});
        }
    }

    getPrizePool = async () =>{
        if(this.props.web3 !== null){
            var competitionNumber = await this.props.tellorTime.methods.competitionNumber.call().call();
            var pool = await this.props.tellorTime.methods.prizePool(competitionNumber).call();
            pool = this.props.web3.utils.fromWei(pool);
            this.setState({prizePool: pool});
        }
    }

    getBalance = async () =>{
        if(this.props.web3 !== null){
            var bal =  await this.props.tellorPlayGround.methods.balanceOf(this.props.accounts[0]).call();
            bal = this.props.web3.utils.fromWei(bal);
            this.setState({balance: bal});
        }
    }

    latestWinningNumbers = async () => {
        if(this.props.web3 !== null){
            var competitionNumber = await this.props.tellorTime.methods.competitionNumber.call().call();
            //console.log(currentRound);
            var winningNumbers = await this.props.tellorTime.methods.winningNumbers(--competitionNumber).call();
            //console.log(winningNumbers);
            this.setState({latestNumbers: winningNumbers});
        }
    }

    render(){
        return(
            <div className="Main">
                <h2>Latest Winning Numbers</h2>
              <Table celled id="buyTable">
                    <Table.Body>
                        <Table.Row>
                            {this.state.cells.map(number =>
                                <Table.Cell id="cell">{this.state.latestNumbers[number]}</Table.Cell>) }
                        </Table.Row>    
                    </Table.Body>
               </Table>
               <h2> Current Prize Pool: {this.state.prizePool}</h2>
                
              <h2>Enter Lottery Numbers (1 - 60) Bellow</h2>
              <div id="buyTicket">
                <Table celled id="buyTable">
                    <Table.Body>
                        <Table.Row>
                            {this.state.cells.map(number =>
                                <Table.Cell id="cell">
                                <form>
                                    <input id="numberInput" type ="text" autocomplete="off"  onChange = {(e) => this.updateBuyNumbers(number, e) }/>
                                </form>
                            </Table.Cell>) }
                        </Table.Row>    
                    </Table.Body>
                </Table>
                <div id="trBalance"><p>Tellor Playground Balance: {this.state.balance}</p></div>
                <div id = "buyButton" onClick= {this.buyTicket}>
                    <p id="buyText">Buy Ticket</p>
                </div>
                <div id= "transactionMessage"><p>{this.state.transactionText}</p></div>
              </div>
            </div>
        )
    }
}

export default Main;