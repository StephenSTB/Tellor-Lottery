# Tellor-Lottery

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Tellor lottery is a lottery game which utilizes Tellor Playground to recieve lottery numbers produced by <a href="https://apiloterias.com.br">https://apiloterias.com.br</a> which tracks brazils largest lottery Mega Sena. Tellor lottery follows the Mega Sena lottery drawing times and provided numbers distributing winnings accordingly.Prizes are distritributed to lottery participants those who correctly guessed all(six) or five of the Lottery numbers. 60% of the prize pool is distributed equally to winners who guessed all six numbers and 40% of the prize pool is distributed equally to the participants to guessed five numbers. If either portion of the prize pool does not have a winner that portion is rolled over into the next round. Prizes are distributed to winners automatically via the Tellor Lottery smart contract.Tellor lottery currently utilizes TRBP to buy tickets on the Ethereum Kovan testnet.

## Demo
Website deployed at:

https://tellor-lottery.runkodapps.com/#/

Demonstation of some of the Tellor Lottery smart contract functionality:

[![Watch the video](https://img.youtube.com/vi/pVxzg3aQ4D0/hqdefault.jpg)](https://youtu.be/pVxzg3aQ4D0)

## Gitcoin
This project is a submission for gitcoin grants 8 hackathon

https://gitcoin.co/issue/tellor-io/usingtellor/30/100024323

## Contracts
Tellor Lottery contract can be found at:

https://github.com/StephenSTB/Tellor-Lottery/blob/main/TellorTimeConsole/TellorTimeConsole/TellorTimeContracts/TellorTime.sol

Contract is deployed on Kovan testnet.

## Run Locally

### Front End
Enter Tellor-Lottery/client via terminal and enter.

``` npm install ```
then,
``` npm start ```
### Lottery Updater
Run: TellorTimeConsole.exe at 

Tellor-Lottery\TellorTimeConsole\TellorTimeConsole\bin\Debug\netcoreapp3.1\TellorTimeConsole 

to automatically update the tellor lottery contract.

