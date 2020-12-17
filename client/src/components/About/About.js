import React, { Component} from "react";

import { Table, Input } from 'semantic-ui-react';

import "./About.css";

class About extends Component{

    constructor(props){
        super();
        this.props = props;
    }

    faucet = async () =>{
        if(this.props.web3 !== null){
            await this.props.tellorPlayGround.methods.faucet(this.props.accounts[0]).send({from: this.props.accounts[0]});
        }
    }

    update = async() =>{
        if(this.props.web3 !== null){
            var update = await this.props.tellorTime.methods.updateLotto().send({from: this.props.accounts[0]});
            console.log(update)
        }
    }

    render(){
        return(
            <div className="About">
                <p id="aboutText">Tellor lottery is a lottery game which utilizes Tellor Playground to recieve lottery numbers produced by <a href="https://apiloterias.com.br">https://apiloterias.com.br</a> which tracks brazils largest lottery Mega Sena. 
                Tellor lottery follows the Mega Sena lottery drawing times and provided numbers distributing winnings accordingly.<br/><br/>
                Prizes are distritributed to lottery participants those who correctly guessed all(six) or five of the Lottery numbers. 
                60% of the prize pool is distributed equally to winners who guessed all six numbers and 40% of the prize pool is distributed 
                equally to the participants to guessed five numbers. If either portion of the prize pool does not have a winner that portion is rolled over into the next round.
                Prizes are distributed to winners automatically via the Tellor Lottery smart contract.
                Tellor lottery currently utilizes TRBP to buy tickets on the Ethereum Kovan testnet which can be recieved via the button bellow.
                (make sure you connect your web3 wallet and have kovan testnet ether) </p>
                <div id = "faucetButton" onClick= {this.faucet}>
                    <p id="faucetText">Faucet</p>
                </div>
                <p>The button bellow is utilized for demonstration purposes, showing the tellor lottery update.</p>
                <div id = "updateButton" onClick= {this.update}>
                    <p id="faucetText">Update</p>
                </div>
            </div>
        )
    }
}

export default About;