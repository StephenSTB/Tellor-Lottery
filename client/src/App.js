import React, { Component } from "react";
import TellorTime from "./contracts/TellorTime.json";
import TellorPlayGround from "./contracts/TellorPlayground.json";
import getWeb3 from "./getWeb3";
import Web3 from "web3";

import "./App.css";

import {
  Route,
  HashRouter,
} from "react-router-dom";

import TopBar from "./components/TopBar/TopBar";

import Main from "./components/Main/Main";

import About from "./components/About/About";

class App extends Component {
  state = {web3: null, accounts: null, tellorTime: null, tellorPlayGround: null};

  constructor(){
    super();
    this.updateWeb3 = this.updateWeb3.bind(this);
  }

  updateWeb3 = async () =>{
    if (window.ethereum) {

      try{
        await window.ethereum.enable();
      }
      catch(error){
        console.error(error);
      }
      const web3  = new Web3(window.ethereum);

      const accounts = await web3.eth.getAccounts();
      
      console.log(accounts)

      var ttdInstance = new web3.eth.Contract(TellorTime.abi, "0x171d8736037e0bca77b1f9bfcd371a92ffc5e792");

      console.log("Tellor Time Address: " + ttdInstance.options.address);

      var tpInstance = new web3.eth.Contract(TellorPlayGround.abi, "0x20374E579832859f180536A69093A126Db1c8aE9");

      this.setState({web3, accounts, tellorTime: ttdInstance, tellorPlayGround: tpInstance});

      window.ethereum.on('update', function (accounts) {
        // Time to reload your interface with accounts[0]!
          window.location.reload();
      });
      
    }
  }

  render() {
    var account = this.state.accounts ?  this.state.accounts[0] : "Connect Account";
    return (
      <div className="App">
        <HashRouter>
          <TopBar {...this.state} updateWeb3 = {this.updateWeb3} account = {account}></TopBar>
          <Route exact path="/" render = {(routeProps)=>(<Main {...routeProps} {...this.state}/>)}/>
          <Route exact path="/About" render = {(routeProps)=>(<About {...routeProps} {...this.state}/>)}/>
        </HashRouter>

        <p id="copyright">&copy; 2020 Tellor Lottery</p>
      </div>
    );
  }
}

export default App;
