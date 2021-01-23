using Nethereum.Contracts;
using Nethereum.HdWallet;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using TellorTimeContracts.Contracts.TellorPlayground;
using TellorTimeContracts.Contracts.TellorPlayground.ContractDefinition;
using TellorTimeContracts.Contracts.TellorLottery;
using TellorTimeContracts.Contracts.TellorLottery.ContractDefinition;
using TellorTimeContracts.Contracts.TellorLotteryPool;
using TellorTimeContracts.Contracts.TellorLotteryPool.ContractDefinition;
using System.Globalization;

namespace TellorTimeConsole
{
    class Program
    {

        //static Account account = new Account("5e525b2ce341f0f664d42d7a046aca38a76f8d75e23e96d728213089901021fd", null);


        static Account account = new Account("b2d8bc829751d1bdd00ad5a539bc3e51259bf8018a14135c526fcb3f7522e623", null);

        //static Web3 web3 = new Web3(account, "https://kovan.infura.io/v3/11319a0ca8664762980ff25ba91d99b4"); // kovan

        static Web3 web3 = new Web3(account, "HTTP://127.0.0.1:7545"); // Ganache

        //static Web3 web3 = new Web3(account, "https://rpc-mumbai.maticvigil.com/v1/857bd788405149815e9bed09e4e0ef2b41777537",null); // mumbai

        static string tellorPlaygroundAddress = "0x20374E579832859f180536A69093A126Db1c8aE9";

        static BigInteger requestId = new BigInteger(77);

        // Contract Addresses
        static string tellorLotteryAddress;

        static string tellorPoolAddress;

        // Contract Services

        static TellorLotteryService tlService;

        static TellorPlaygroundService tpgService;

        static TellorLotteryPoolService tplService;


        static void Main(string[] args)
        {
            Console.WriteLine("     Tellor Lottery! Console Testing!\n");

            InitializeSettings();

            ThreadStart updateStart = new ThreadStart(start);

            Thread update = new Thread(updateStart);

            update.Start();

            while (true)
            {
                Console.Write("Command: ");

                string commandString = Console.ReadLine();

                string[] command = commandString.Split(" ");

                Console.WriteLine();

                switch (command[0].ToLower())
                {
                    case "deploy":
                        DeployTellorLotteryContract().Wait();
                        Console.WriteLine();
                        break;
                    case "request":
                        RequestAndUpdateLatestNumbers().Wait();
                        Console.WriteLine();
                        break;
                    case "distribute":
                        DistributeWinningsTest().Wait();
                        break;
                    case "buy":
                        //Console.WriteLine($"    Command args {command.Length}");
                        List<ulong> numbers = RandomNumbers();
                        if (command.Length == 7)
                        {
                            for(int i = 1; i < 7; i++)
                            {
                                try
                                {
                                    numbers[i - 1] = ulong.Parse(command[i]);
                                }
                                catch(Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                        }
                        //Console.Write($"    buy numbers:");
                        //foreach (ulong n in numbers) Console.Write($"{n},");
                        //Console.WriteLine();
                        BuyTicket(numbers).Wait();
                        break;
                    case "winners":
                        ViewNumberOfWinners().Wait();
                        break;
                    case "view":
                        ViewTickets().Wait();
                        break;
                    case "pool":
                        ViewPrizePool().Wait();
                        break;
                    case "ids":
                        GenerateTicketIDs().Wait();
                        break;
                    case "faucet":
                        Faucet(account.Address).Wait();
                        break;
                    case "switch":
                        SwitchAccount();
                        break;
                    case "stake":
                        if(command.Length > 1)
                        {
                            var amount = new BigInteger(int.Parse(command[1]));
                            Stake(amount).Wait();
                            break;
                        }
                        Stake(new BigInteger(0)).Wait();
                        break;
                    case "unstake":
                        if (command.Length > 1)
                        {
                            var amount = new BigInteger(int.Parse(command[1]));
                            UnStake(amount).Wait();
                            break;
                        }
                        UnStake(new BigInteger(0)).Wait();
                        break;
                    case "winningnumbers":
                        WinningNumbers(true);
                        break;
                    case "rng":
                        RandomNumbers();
                        break;
                    case "dividend":
                        PoolTest().Wait();
                        break;
                    case "balance":
                        networkBalance().Wait();
                        break;
                    default:
                        Console.WriteLine("Invalid command.");
                        break;
                }
            }
        }


        private static void InitializeSettings()
        {


            // Initialize contract variables.
            Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationCollection Settings = configFile.AppSettings.Settings;

            tellorLotteryAddress = Settings["TellorLotteryAddress"].Value;

            tellorPlaygroundAddress = Settings["TellorPlaygroundAddress"].Value;

            tellorPoolAddress = Settings["TellorPoolAddress"].Value;

            tlService = new TellorLotteryService(web3, tellorLotteryAddress);

            tpgService = new TellorPlaygroundService(web3, tellorPlaygroundAddress);

            tplService = new TellorLotteryPoolService(web3, tellorPoolAddress);

        }

        private static async void start()
        {
            while (true)
            {
                Thread.Sleep(36000000);
                Console.WriteLine("Updater");

                RequestAndUpdateLatestNumbers().Wait();

                Console.WriteLine("Command: ");
               
                Thread.Sleep(36000000);
            }

        }

        private static void SwitchAccount()
        {
            if (account.PrivateKey.Equals("b2d8bc829751d1bdd00ad5a539bc3e51259bf8018a14135c526fcb3f7522e623"))
            {
                account = new Account("3ea5dca060d1fa3b60661acf3b7857ceb09082ee779b5110fe2fe57a6ebb9b39", null);
            }
            else
            {
                account = new Account("b2d8bc829751d1bdd00ad5a539bc3e51259bf8018a14135c526fcb3f7522e623", null);
            }
            
            web3 = new Web3(account, "HTTP://127.0.0.1:7545");
            InitializeSettings();
        }

        private static async Task PoolTest()
        {
            Console.WriteLine($"Testing tellor lottery pool functionality.");
            try
            {
                List<ulong> numbers = new List<ulong>(){ 0,0,0,0,0,0 };

                Console.WriteLine("   Recieve Pool Tokens..");

                var ticketCost = await tlService.TicketCostQueryAsync();
                for (int i = 0; i < 2; i++)
                {

                    numbers = RandomNumbers();

                    BuyTicket(numbers).Wait();
                }

                
                Stake(1).Wait();

                var balance = Web3.Convert.FromWei(await tpgService.BalanceOfQueryAsync(account.Address));

                RequestAndUpdateLatestNumbers().Wait();

                Console.WriteLine($"    Harvesting dividends..\n");

                Console.Write($"    Pre harvest TRBP: {balance}\n");

                await tplService.HarvestRequestAsync();

                var postHarvestBalance = Web3.Convert.FromWei(await tpgService.BalanceOfQueryAsync(account.Address));

                Console.Write($"    Post harvest: {postHarvestBalance}\n");

                Console.WriteLine($"    Unstaking tellor pool tokens..");

                await tplService.UnstakeRequestAsync(Web3.Convert.ToWei(1));

                var stake = await tplService.ViewStakeQueryAsync();

                Console.WriteLine($"    Stake: {stake.Amount}");


            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private static async Task Stake(BigInteger amount)
        {
            BigInteger amt;

            if (amount == 0)
            {
                amt = await tplService.BalanceOfQueryAsync(account.Address);
            }
            else
            {
                amt = Web3.Convert.ToWei(amount);
            }
            try
            {
                Console.WriteLine($"Staking {Web3.Convert.FromWei(amt)} Tokens..");
                //var stake = await tplService.ViewStakeQueryAsync();

                //Console.WriteLine($"    Pool number: {stake.PoolNumber}, Staked: {stake.Amount}\n");

                await tplService.ApproveRequestAndWaitForReceiptAsync(tellorPoolAddress, amt);

                //var allowance = await tplService.AllowanceQueryAsync(account.Address, tellorPoolAddress);

                //Console.WriteLine($"    Allowance: {Web3.Convert.FromWei(allowance)} amount: {Web3.Convert.FromWei(amt)}");

                var gas = Web3.Convert.FromWei(await web3.Eth.GetBalance.SendRequestAsync(account.Address));

                var stakeReceipt = await tplService.StakeRequestAndWaitForReceiptAsync(amt);

                gas -= Web3.Convert.FromWei(await web3.Eth.GetBalance.SendRequestAsync(account.Address));

                var stake = await tplService.ViewStakeQueryAsync();

                Console.WriteLine($"    Gas: {gas}");

                Console.WriteLine($"    Pool number: {stake.PoolNumber}, Staked: {stake.Amount}\n");

                var currentStaked = await tplService.CurrentStakedQueryAsync();

                var tpValue = await tplService.BalanceOfQueryAsync(tellorPoolAddress);

                Console.WriteLine($"    {account.Address} Current Staked: {Web3.Convert.FromWei(currentStaked)} Confirmed in Contract: {Web3.Convert.FromWei(tpValue)}\n");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

        private static async Task UnStake(BigInteger amount)
        {
            BigInteger amt;
            if(amount == 0)
            {
                var stake = await tplService.ViewStakeQueryAsync();
                amt = stake.Amount;
            }
            else
            {
                amt = Web3.Convert.ToWei(amount);
            }
            try
            {
                Console.Write($"Unstaking {Web3.Convert.FromWei(amt)} tokens.\n");

                var unstakeReceipt = await tplService.UnstakeRequestAsync(amt);

                var stake = await tplService.ViewStakeQueryAsync();

                Console.Write($"    {account.Address} Staked: {stake.Amount}");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private static async Task Harvest()
        {
            Console.WriteLine($"    Harvesting");
        }

        private static async Task ViewTickets()
        {
            var competitionNumber = await tlService.CompetitionNumberQueryAsync();

            var viewTickets = await tlService.ViewTicketsQueryAsync(competitionNumber);

            Console.WriteLine($"   Tickets for Lotto round: {competitionNumber}");

            for(int i = 0; i < viewTickets.TicketNumbers.Count; i++)
            {
                if (viewTickets.TicketNumbers[i] != 0)
                {
                    var numbers = viewTickets.Numbers.GetRange(i * 6, 6);
                    Console.WriteLine($"     Ticket Number: {viewTickets.TicketNumbers[i]},  Numbers: ( {string.Join(",", numbers)} )");
                    continue;
                }
                break;
            }

        }

        private static async Task ViewNumberOfWinners()
        {
            var competitionNumber = await tlService.CompetitionNumberQueryAsync();

            var winningNumbers = await tlService.NumberOfWinnersQueryAsync(competitionNumber-1);

            Console.WriteLine($"      Jackpot Winners: {winningNumbers.JackpotWinners}\n      Runner-up Winners: {winningNumbers.TierTwoWinners}\n" +
                $"      Third Tier Winners: {winningNumbers.TierThreeWinners}\n");
        }

        public static async Task ViewPrizePool()
        {
            var poolReciept = await tlService.PrizePoolQueryAsync(await tlService.CompetitionNumberQueryAsync());
            Console.WriteLine($"prize pool : {poolReciept}");

        }

        private static async Task GenerateTicketIDs()
        {
            List<ulong> numbers = new List<ulong>(){ 30, 37, 45, 54, 57, 58 };

            Console.Write("Ticket: ( ");
            for(int i = 0; i < numbers.Count; i++) {
                Console.Write(numbers[i]);
                if (i != 5)
                    Console.Write(", ");
            }
            Console.Write(" )\n");

            var generateIDsReceipt = await tlService.GenerateTicketIDsRequestAndWaitForReceiptAsync(numbers);

            var events = generateIDsReceipt.DecodeAllEvents<GeneratedIDsEventDTO>();


            Console.Write("Ticket IDs: ( ");
            foreach(EventLog<GeneratedIDsEventDTO> e in events)
            {
                for(int i = 0; i < 7; i++)
                {
                    Console.Write($"{e.Event.TicketIDs[i]}");
                    if (i != 6)
                        Console.Write(", ");
                }
            }
            Console.Write(" )\n");
        }

        private static async Task DistributeWinningsTest()
        {
            Console.WriteLine("Distribute Winnings Test..");
            List<ulong> numbers;

            var ethBalance = Web3.Convert.FromWei(await web3.Eth.GetBalance.SendRequestAsync(account.Address));
            var tpBalance = Web3.Convert.FromWei(await tpgService.BalanceOfQueryAsync(account.Address));
            Console.WriteLine($"    {account.Address} Eth: {ethBalance} TP: {tpBalance}");

            try
            {
                var ticketCost = await tlService.TicketCostQueryAsync();
                for (int i = 0; i < 2; i++) {

                    numbers = RandomNumbers();

                    BuyTicket(numbers).Wait();
                }


                List<ulong> winNumbers = WinningNumbers(false);
                List<ulong> tierTwoNumbers = WinningNumbers(false);
                List<ulong> tierThreeNumbers = WinningNumbers(false);


                Console.WriteLine($"Buying winning ticket...");

                BuyTicket(winNumbers).Wait();


                ethBalance = Web3.Convert.FromWei(await web3.Eth.GetBalance.SendRequestAsync(account.Address));
                tpBalance = Web3.Convert.FromWei(await tpgService.BalanceOfQueryAsync(account.Address));
                Console.WriteLine($"    {account.Address} Eth: {ethBalance} TP: {tpBalance}");

                var ttBalance = Web3.Convert.FromWei(await tpgService.BalanceOfQueryAsync(tellorLotteryAddress));
                Console.WriteLine($"    Tellor Lottery TP: {ttBalance}");

                // update the lotto.
                RequestAndUpdateLatestNumbers().Wait();
 
                ethBalance = Web3.Convert.FromWei(await web3.Eth.GetBalance.SendRequestAsync(account.Address));
                tpBalance = Web3.Convert.FromWei(await tpgService.BalanceOfQueryAsync(account.Address));
                Console.WriteLine($"    {account.Address} Eth: {ethBalance} TP: {tpBalance}");
                ttBalance = Web3.Convert.FromWei(await tpgService.BalanceOfQueryAsync(tellorLotteryAddress));
                Console.WriteLine($"    Tellor Lottery TP: {ttBalance}");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

        private static List<ulong> WinningNumbers(bool view)
        {
            WebRequest request = WebRequest.Create("https://apiloterias.com.br/app/resultado.php?loteria=megasena&token=TellorSB&concurso=");
            WebResponse response = request.GetResponse();

            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            
            using (Stream dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();

                char[] numbers = responseFromServer.Split("dezenas")[1].Split("premiacao")[0].ToCharArray();

                //Console.WriteLine(numbers);

                List<ulong> winningNumbers = new List<ulong>() { 0, 0, 0, 0, 0, 0 };

                int index = 0;

                for(int i = 0; i < numbers.Length; i++)
                {
                    if (Char.IsDigit(numbers[i]))
                    {
                        winningNumbers[index] = ulong.Parse(numbers[i].ToString() + numbers[++i].ToString());
                        index++;
                    }
                }
                if(view)
                    Console.WriteLine($"     Winning Numbers: ({string.Join(",", winningNumbers)})");

                return winningNumbers;
            }
        }

        private static List<ulong> RandomNumbers()
        {
            List<ulong> numbers = new List<ulong>() {0,0,0,0,0,0};

            Random rng = new Random();

            int last = 1; int mid = 27;  int end = 58;

            for(int i = 0; i < 3; i++)
            {
                numbers[i] = (ulong)rng.Next(last, mid);
                last = (int)numbers[i] + 1;
                mid++;
            }

            for(int i = 3; i < 6; i++)
            {
                numbers[i] = (ulong)rng.Next(last, end);
                last = (int)numbers[i] + 1;
                end++;
            }

            //Console.WriteLine($"    Random numbers: ({string.Join(",", numbers)})");

            return numbers;
        }

        private static async Task BuyTicket(List<ulong> numbers)
        {
            try
            {
                Console.WriteLine($"Buying Ticket...");
                var allowance = await tpgService.AllowanceQueryAsync(account.Address, tellorLotteryAddress);

                var ticketCost = await tlService.TicketCostQueryAsync();


                //Console.WriteLine($"    Ticket Cost: {ticketCost} Allowance: {Web3.Convert.FromWei(allowance)}");

                if (allowance < ticketCost)
                    await tpgService.ApproveRequestAndWaitForReceiptAsync(tellorLotteryAddress, ticketCost);

                //allowance = await tpgService.AllowanceQueryAsync(account.Address, tellorLotteryAddress);

                //Console.WriteLine($"    {account.Address} allowance for Tellor Lottery: {Web3.Convert.FromWei(allowance)}");

                BuyTicketFunction BTF = new BuyTicketFunction
                {
                    FromAddress = account.Address,
                    Numbers = numbers,
                };

                var balance = Web3.Convert.FromWei(await web3.Eth.GetBalance.SendRequestAsync(account.Address));

                var buyTicketReceipt = await tlService.BuyTicketRequestAndWaitForReceiptAsync(BTF);

                balance = balance - Web3.Convert.FromWei(await web3.Eth.GetBalance.SendRequestAsync(account.Address));

                Console.Write($"      Gas: {balance}");

                var buyTicketEvents = buyTicketReceipt.DecodeAllEvents<BuyTicketSuccessEventDTO>();

                foreach(EventLog<BuyTicketSuccessEventDTO> e in buyTicketEvents)
                {
                    //Console.WriteLine($"    Competition number: {e.Event.CompetitionNumber} Address: {e.Event.Sender} Prize Pool: {e.Event.PrizePool} Ticket Number: {e.Event.TicketNumber} Numbers: {string.Join(",", e.Event.Numbers)}");
                    Console.Write($"    Ticket Number: {e.Event.TicketNumber}, Numbers: ({string.Join(",", e.Event.Numbers)})");
                }

                var buyTicketErrorEvents = buyTicketReceipt.DecodeAllEvents<BuyTicketErrorEventDTO>();

                foreach(EventLog<BuyTicketErrorEventDTO> e in buyTicketErrorEvents)
                {
                    Console.WriteLine($"error: {e.Event.Error} {e.Event.N}");
                }

                var poolTokens = Web3.Convert.FromWei(await tplService.BalanceOfQueryAsync(account.Address));

                Console.Write($"    Lottery pool tokens: {poolTokens}\n\n");

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }


        private static async Task RequestAndUpdateLatestNumbers()
        {

            Console.WriteLine("Lottery Update.\n");
            WebRequest request = WebRequest.Create("https://apiloterias.com.br/app/resultado.php?loteria=megasena&token=TellorSB&concurso=");
            WebResponse response = request.GetResponse();

            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            using (Stream dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                //Console.WriteLine(responseFromServer + "\n");

                string numbersUnparsedString = responseFromServer.Split("dezenas")[1].Split("premiacao")[0];

                char[] numbersUnparsed = numbersUnparsedString.ToCharArray();

                int numbersStringIndex = 0;

                uint[] numbers = new uint[6];

                // Parse winning numbers
                for (int i = 0; i < numbersUnparsed.Length; i++)
                {
                    if (Char.IsDigit(numbersUnparsed[i]))
                    {
                        numbers[numbersStringIndex] = uint.Parse((numbersUnparsed[i].ToString() + numbersUnparsed[++i].ToString()));
                        numbersStringIndex++;
                        //Console.Write(numbersUnparsed[i]);
                    }
                }

                Console.Write($"    Latest winning numbers: ({string.Join(",", numbers)})\n");

                BigInteger lottoValue = new BigInteger(0);

                // Create lottoNumbers value to be set in the TellorPlayGround contract.
                foreach(uint n in numbers)
                {
                    lottoValue += n;
                    lottoValue = (n == numbers[5]) ? lottoValue : lottoValue << 6;
                }

                Console.WriteLine($"    Tellor Playground lotto value: {lottoValue}");

                // Retrieve the competition number from the responseFromServer.
                var competitionNumber = new BigInteger(int.Parse(responseFromServer.Split("numero_concurso\":")[1].Split(",\"data_concurso")[0]));

                //Console.WriteLine($"{competitionNumber}");

                lottoValue = lottoValue << 16;

                lottoValue += competitionNumber;

                var cNumber = await tlService.CompetitionNumberQueryAsync();

                //Console.WriteLine(cNumber);

                // Condition to determine if the lotto numbers need to be updated in the Tellor Playground.
                if (cNumber.Equals(competitionNumber))
                {
                    try
                    {
                        // Submit the new lotto numbers.
                        await tpgService.SubmitValueRequestAndWaitForReceiptAsync(requestId, lottoValue);

                        Thread.Sleep(3000);

                        var gas = Web3.Convert.FromWei(await web3.Eth.GetBalance.SendRequestAsync(account.Address));
                            
                        // Update tellor Lottery contract.
                        var updateLottoReceipt = await tlService.UpdateLottoRequestAndWaitForReceiptAsync();

                        gas -= Web3.Convert.FromWei(await web3.Eth.GetBalance.SendRequestAsync(account.Address));

                        Console.WriteLine($"    Update Lottery Gas: {gas}");

                        var events = updateLottoReceipt.DecodeAllEvents<LottoUpdateEventDTO>();

                        foreach(EventLog<LottoUpdateEventDTO> e in events)
                        {
                            Console.WriteLine($"    Contract competition number: {e.Event.CompetitionNumber}");

                            Console.Write($"    Winning Numbers: {string.Join(",", e.Event.Numbers)}\n");
                            Console.Write($"    Winning IDs: {string.Join(",", e.Event.WinningIDs)}\n\n");
                        }

                        var distributeVars = updateLottoReceipt.DecodeAllEvents<DistributeVariablesEventDTO>();

                        foreach (EventLog<DistributeVariablesEventDTO> e in distributeVars)
                        {
                            Console.WriteLine($"    jackpot pool: {e.Event.JackpotPrize}, tier two pool: {e.Event.TierTwoPrize}, tier Three pool: {e.Event.TierThreePrize}\n" +
                                $"    jackpot winners {e.Event.JackpotWinnerNum}, tier two winners {e.Event.TierTwoWinnerNum}, tier three winners {e.Event.TierthreeWinnerNum}");
                        }

                        var winningsEvents = updateLottoReceipt.DecodeAllEvents<WinningsDistributedEventDTO>();

                        foreach (EventLog<WinningsDistributedEventDTO> e in winningsEvents)
                        {
                            Console.WriteLine($"    Winnings Distributed Event: {e.Event.CompetitionNumber}, {e.Event.Winner}, {e.Event.Prize}");
                        }

                        var poolUpdateEvents = updateLottoReceipt.DecodeAllEvents<PoolUpdateEventDTO>();

                        foreach(EventLog<PoolUpdateEventDTO> e in poolUpdateEvents)
                        {
                            Console.WriteLine($"    Pool number: {e.Event.PoolNumber}, Staked: {e.Event.Staked}, Value: {Web3.Convert.FromWei(e.Event.Value)}");
                        }

                        Console.WriteLine();
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }
            }
            response.Close();
        }


        // Function to get TellorPlayground coin.
        private static async Task Faucet(string address)
        {
            try
            {
                var faucetReceipt = await tpgService.FaucetRequestAndWaitForReceiptAsync(address);

                var playgroundBalance = Web3.Convert.FromWei(await tpgService.BalanceOfQueryAsync(address));

                var totalSupply = Web3.Convert.FromWei(await tpgService.TotalSupplyQueryAsync());

                Console.WriteLine($"    TotalSupply: {totalSupply}, {address} Balance: {playgroundBalance}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static async Task networkBalance()
        {
            var Balance = Web3.Convert.FromWei(await web3.Eth.GetBalance.SendRequestAsync(account.Address));
            var Trbp = Web3.Convert.FromWei(await tpgService.BalanceOfQueryAsync(account.Address));
            var pool = Web3.Convert.FromWei(await tplService.BalanceOfQueryAsync(account.Address));
            Console.WriteLine($"Network Balance: {Balance}, TRBP: {Trbp}, Pool Tokens {pool}\n");
        }


        private static uint getCompetitionStart()
        {
            WebRequest request = WebRequest.Create("https://apiloterias.com.br/app/resultado.php?loteria=megasena&token=TellorSB&concurso=");
            WebResponse response = request.GetResponse();

            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            using (Stream dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                //Console.WriteLine(responseFromServer + "\n");

                // Retrieve the competition number from the responseFromServer.
                return uint.Parse(responseFromServer.Split("numero_concurso\":")[1].Split(",\"data_concurso")[0]) + 1;
            }

        }

        private static async Task DeployTellorLotteryContract()
        {
            Console.WriteLine("TellorLottery contract deployment.....\n");

            try
            {
                var balance = Web3.Convert.FromWei(await web3.Eth.GetBalance.SendRequestAsync(account.Address));

                Console.WriteLine($"    {account.Address} balance: {balance}");

                TellorPlaygroundDeployment TPD = new TellorPlaygroundDeployment
                {

                };

                var tpReceipt = await TellorPlaygroundService.DeployContractAndWaitForReceiptAsync(web3, TPD);

                tellorPlaygroundAddress = tpReceipt.ContractAddress;

                TellorLotteryDeployment tl = new TellorLotteryDeployment
                {
                    TellorAddress = tellorPlaygroundAddress,
                    CompetitionStart = getCompetitionStart()-1,
                };

                Console.WriteLine($"    Competition Start: {tl.CompetitionStart}, Winning Numbers: ({string.Join(",",WinningNumbers(false))})\n");

                // Deploy Tellor Lottery Contract;
                var tlReceipt = await TellorLotteryService.DeployContractAndWaitForReceiptAsync(web3, tl);

                // Initialize tellorLottery Address
                tellorLotteryAddress = tlReceipt.ContractAddress;

                // Initialize tlService
                tlService = new TellorLotteryService(web3, tellorLotteryAddress);

                // Initialize tpgService
                tpgService = new TellorPlaygroundService(web3, tellorPlaygroundAddress);

                tellorPoolAddress = await tlService.LotteryPoolQueryAsync();

                // Initialize tplService
                tplService = new TellorLotteryPoolService(web3, tellorPoolAddress);


                // Open Settings.
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var Settings = configFile.AppSettings.Settings;

                Settings["TellorLotteryAddress"].Value = tellorLotteryAddress;

                Settings["TellorPlaygroundAddress"].Value = tellorPlaygroundAddress;

                Settings["TellorPoolAddress"].Value = tellorPoolAddress;

                // Save Settings
                configFile.Save(ConfigurationSaveMode.Modified);

                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);

                // Get account balance 

                var gas =  balance - Web3.Convert.FromWei(await web3.Eth.GetBalance.SendRequestAsync(account.Address));

                balance = Web3.Convert.FromWei(await web3.Eth.GetBalance.SendRequestAsync(account.Address));

                Console.WriteLine($"    Tellor Lottery address: {tellorLotteryAddress}\n    Gas used: {gas} \n    {account.Address} balance: {balance}");

                Console.WriteLine($"    Tellor Playground address: {tellorPlaygroundAddress}");

                Console.WriteLine($"    Tellor Pool address: {tellorPoolAddress}\n");

                Faucet(account.Address).Wait();

                Faucet(tellorPoolAddress).Wait();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        
    }
}
