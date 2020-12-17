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
using TellorTimeContracts.Contracts.TellorTime;
using TellorTimeContracts.Contracts.TellorTime.ContractDefinition;

namespace TellorTimeConsole
{
    class Program
    {

        static Account account = new Account("5e525b2ce341f0f664d42d7a046aca38a76f8d75e23e96d728213089901021fd", null);

        static Web3 web3 = new Web3(account, "https://kovan.infura.io/v3/11319a0ca8664762980ff25ba91d99b4"); // kovan

        static string tellorPlaygroundAddress = "0x20374E579832859f180536A69093A126Db1c8aE9";

        static BigInteger requestId = new BigInteger(77);

        // Contract Addresses
        static string tellorTimeAddress;

        // Contract Services

        static TellorTimeService ttdService;

        static TellorPlaygroundService tpService;


        static void Main(string[] args)
        {
            Console.WriteLine("Tellor Time! Console Testing!");

            InitializeSettings();

            ThreadStart updateStart = new ThreadStart(start);

            Thread update = new Thread(updateStart);

            update.Start();

            while (true)
            {
                Console.WriteLine("Command: ");

                string commandString = Console.ReadLine();

                string[] command = commandString.Split(" ");

                Console.WriteLine();


                switch (command[0].ToLower())
                {
                    case "deploy":
                        DeployTellorTimeContract().Wait();
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
                        Console.WriteLine($"command args {command.Length}");
                        List<ulong> numbers = new List<ulong>(){ 10, 16, 27, 34, 36, 57 };
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
                        Console.Write($"buy numbers:");
                        foreach (ulong n in numbers) Console.Write($"{n},");
                        Console.WriteLine();
                        BuyTicket(numbers).Wait();
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
                        Faucet().Wait();
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

            tellorTimeAddress = Settings["TellorTimeAddress"].Value;

            ttdService = new TellorTimeService(web3, tellorTimeAddress);

            tpService = new TellorPlaygroundService(web3, tellorPlaygroundAddress);

        }

        private static async void start()
        {
            while (true)
            {
                Console.WriteLine("Updater");

                RequestAndUpdateLatestNumbers().Wait();

                Console.WriteLine("Command: ");
               
                Thread.Sleep(36000000);
            }

        }

        private static async Task ViewTickets()
        {
            var viewTicketsReceipt = await ttdService.ViewTicketsRequestAndWaitForReceiptAsync(getCompetitionStart());
            

        }

        public static async Task ViewPrizePool()
        {
            var poolReciept = await ttdService.PrizePoolQueryAsync(await ttdService.CompetitionNumberQueryAsync());
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

            var generateIDsReceipt = await ttdService.GenerateTicketIDsRequestAndWaitForReceiptAsync(numbers);

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
            List<ulong> numbers = new List<ulong>() { 0, 0, 0, 0, 0, 0};

            BigInteger numberID = new BigInteger(0);

            var ethBalance = Web3.Convert.FromWei(await web3.Eth.GetBalance.SendRequestAsync(account.Address));
            var tpBalance = Web3.Convert.FromWei(await tpService.BalanceOfQueryAsync(account.Address));
            Console.WriteLine($"{account.Address} Eth: {ethBalance} TP: {tpBalance}");

            BigInteger competition = new BigInteger(0);

            Random rng = new Random();
            try
            {
                for (int i = 0; i < 4; i++) {

                    int start = 1, end = 55;
                    Console.Write("Ticket numbers: ( ");
                    for(int j = 0; j < 6; j++)
                    {
                        numbers[j] = (ulong)rng.Next(start, end);
                        numberID += numbers[j];

                        start = (int)numbers[j] + 1;
                        end++;

                        numberID = j == 5 ? numberID : numberID << 6;

                        Console.Write($"{numbers[j]}");

                        if (j != 5)
                            Console.Write(",");
                    }
                    Console.Write(" )");
                    Console.Write($" {numberID}");

                    var ticketCost = await ttdService.TicketCostQueryAsync();

                    await tpService.ApproveRequestAndWaitForReceiptAsync(tellorTimeAddress, ticketCost);

                    var allowance = await tpService.AllowanceQueryAsync(account.Address, tellorTimeAddress);

                    Console.Write($" {Web3.Convert.FromWei(allowance)}\n");

                    BuyTicketFunction BTF = new BuyTicketFunction
                    {
                        FromAddress = account.Address,
                        Numbers = numbers,
                    };

                    var buyReceipt = await ttdService.BuyTicketRequestAndWaitForReceiptAsync(BTF);

                    var buySuccessEvents = buyReceipt.DecodeAllEvents<BuyTicketSuccessEventDTO>();

                    foreach(EventLog<BuyTicketSuccessEventDTO> e in buySuccessEvents)
                    {
                        Console.WriteLine($"{e.Event.CompetitionNumber}, {e.Event.TicketNumber}, ({string.Join(",", e.Event.Numbers)}), {e.Event.Sender}, {Web3.Convert.FromWei(e.Event.PrizePool)}");
                    }

                    var buyErrorEvents = buyReceipt.DecodeAllEvents<BuyTicketErrorEventDTO>();

                    foreach(EventLog<BuyTicketErrorEventDTO> e in buyErrorEvents)
                    {
                        Console.WriteLine($"{e.Event.Error}");
                    }

                    numberID = new BigInteger(0);
                }

            
                BigInteger winNumberId = new BigInteger(0); // 
                List<ulong> winNumbers = new List<ulong>() { 08, 11, 14, 39, 48, 53 };
                List<ulong> fiveMatchNumbers = new List<ulong>() { 10, 11, 14, 39, 48, 53 };

                for (int i = 0; i < 6; i++)
                {
                    winNumberId += winNumbers[i];
                    winNumberId = i == 5 ? winNumberId : winNumberId << 6;
                }

                Console.WriteLine($"buying winning ticket...");

                var ticketC = await ttdService.TicketCostQueryAsync();

                await tpService.ApproveRequestAndWaitForReceiptAsync(tellorTimeAddress, ticketC);

                var allow = await tpService.AllowanceQueryAsync(account.Address, tellorTimeAddress);

                Console.Write($" {Web3.Convert.FromWei(allow)}\n");

                BuyTicketFunction BT = new BuyTicketFunction
                {
                    FromAddress = account.Address,
                    Numbers = fiveMatchNumbers,
                };

                var buy = await ttdService.BuyTicketRequestAndWaitForReceiptAsync(BT);

                var buyEvents = buy.DecodeAllEvents<BuyTicketSuccessEventDTO>();

                foreach (EventLog<BuyTicketSuccessEventDTO> e in buyEvents)
                {
                    Console.WriteLine($"{e.Event.CompetitionNumber}, {string.Join(",", e.Event.Numbers)}, {e.Event.Sender}, {Web3.Convert.FromWei(e.Event.PrizePool)}");
                    competition = e.Event.CompetitionNumber;
                }


                ethBalance = Web3.Convert.FromWei(await web3.Eth.GetBalance.SendRequestAsync(account.Address));
                tpBalance = Web3.Convert.FromWei(await tpService.BalanceOfQueryAsync(account.Address));
                Console.WriteLine($"{account.Address} Eth: {ethBalance} TP: {tpBalance}");

                var ttBalance = Web3.Convert.FromWei(await tpService.BalanceOfQueryAsync(tellorTimeAddress));
                Console.WriteLine($"Tellor Time TP: {ttBalance}");

                var updateReceipt = await ttdService.UpdateLottoRequestAndWaitForReceiptAsync();

                var updateEvent = updateReceipt.DecodeAllEvents<LottoUpdateEventDTO>();
                foreach (EventLog<LottoUpdateEventDTO> e in updateEvent)
                {
                    Console.WriteLine($"{e.Event.CompetitionNumber} , {e.Event.WinningIDs[0]}");
                }

                var distributeWinnings = await ttdService.DistributeWinningsRequestAndWaitForReceiptAsync(competition);

                var distributeVars = distributeWinnings.DecodeAllEvents<DistributeVariablesEventDTO>();

                foreach (EventLog<DistributeVariablesEventDTO> e in distributeVars)
                {
                    Console.WriteLine($"jackpot pool: {e.Event.JackpotPrizePool}, five matches pool: {e.Event.FiveMatchesPrizepool}, jackpot winners {e.Event.JackpotWinnerNum}, five match winners {e.Event.FiveMatchWinnerNum}");
                }

                var winningsEvents = distributeWinnings.DecodeAllEvents<WinningsDistributedEventDTO>();

                foreach (EventLog<WinningsDistributedEventDTO> e in winningsEvents)
                {
                    Console.WriteLine($"{e.Event.CompetitionNumber}, {e.Event.Winner}, {e.Event.Prize}");
                }

                ethBalance = Web3.Convert.FromWei(await web3.Eth.GetBalance.SendRequestAsync(account.Address));
                tpBalance = Web3.Convert.FromWei(await tpService.BalanceOfQueryAsync(account.Address));
                Console.WriteLine($"{account.Address} Eth: {ethBalance} TP: {tpBalance}");
                ttBalance = Web3.Convert.FromWei(await tpService.BalanceOfQueryAsync(tellorTimeAddress));
                Console.WriteLine($"Tellor Time TP: {ttBalance}");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

        private static async Task BuyTicket(List<ulong> numbers)
        {
            try
            {
                var allowance = await tpService.AllowanceQueryAsync(account.Address, tellorTimeAddress);

                var ticketCost = await ttdService.TicketCostQueryAsync();

                //Console.WriteLine($"Ticket Cost: {ticketCost.ToString()} Allowance: {allowance}");

                if (allowance < ticketCost)
                    await tpService.ApproveRequestAndWaitForReceiptAsync(tellorTimeAddress, ticketCost);

                allowance = await tpService.AllowanceQueryAsync(account.Address, tellorTimeAddress);

                Console.WriteLine($"{account.Address} allowance for Tellor Time: {allowance}");

                // Create ticket numbers
                //uint[] numbers = { 6, 10, 22, 34, 55, 58 };
                //uint[] numbers = { 10, 16, 27, 34, 36, 57 };

                BigInteger number = new BigInteger(0);

                for (int i = 0; i < numbers.Count; i++)
                {
                    number += numbers[i];
                    number = (i == numbers.Count-1) ? number : number << 6;
                }

                BuyTicketFunction BTF = new BuyTicketFunction
                {
                    FromAddress = account.Address,
                    Numbers = numbers,
                };

                Console.WriteLine($"number: {number}");

                /*
                var balance = Web3.Convert.FromWei(await ttdService.TellorTestQueryAsync());

                Console.WriteLine($"Tellor p total supply {balance}");*/

                var buyTicketReceipt = await ttdService.BuyTicketRequestAndWaitForReceiptAsync(BTF);

                var buyTicketEvents = buyTicketReceipt.DecodeAllEvents<BuyTicketSuccessEventDTO>();

                foreach(EventLog<BuyTicketSuccessEventDTO> e in buyTicketEvents)
                {
                    Console.WriteLine($"Competition number: {e.Event.CompetitionNumber} Address: {e.Event.Sender} Prize Pool: {e.Event.PrizePool} Ticket Number: {e.Event.TicketNumber} Numbers: {string.Join(",", e.Event.Numbers)}");
                }

                var buyTicketErrorEvents = buyTicketReceipt.DecodeAllEvents<BuyTicketErrorEventDTO>();

                foreach(EventLog<BuyTicketErrorEventDTO> e in buyTicketErrorEvents)
                {
                    Console.WriteLine($"error: {e.Event.Error} {e.Event.N}");
                }

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }


        private static async Task RequestAndUpdateLatestNumbers()
        {
            WebRequest request = WebRequest.Create("https://apiloterias.com.br/app/resultado.php?loteria=megasena&token=TellorSB&concurso=");
            WebResponse response = request.GetResponse();

            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            using (Stream dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                Console.WriteLine(responseFromServer + "\n");

                string[] responseDenzasSplit = responseFromServer.Split("dezenas");

                string[] responsePremiacaoSplit = responseDenzasSplit[1].Split("premiacao");

                string numbersUnparsedString = responsePremiacaoSplit[0];

                //Console.WriteLine($" {numbersUnparsedString}");

                char[] numbersUnparsed = numbersUnparsedString.ToCharArray();

                /*
                foreach(char c in numbersUnparsed)
                {
                    Console.Write(c);
                }*/

                //Console.WriteLine();

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

                Console.Write("Latest winning numbers: ( ");
                for(int i = 0; i < numbers.Length; i++)
                {
                    Console.Write(numbers[i]);
                    if(i < numbers.Length - 1)
                    {
                        Console.Write(", ");
                    }
                }
                Console.Write(" )\n\n");

                //uint mask = 63;

                BigInteger lottoValue = new BigInteger(0);

                // Create lottoNumbers value to be set in the TellorPlayGround contract.
                foreach(uint n in numbers)
                {
                    lottoValue += n;
                    lottoValue = (n == numbers[5]) ? lottoValue : lottoValue << 6;
                }

                Console.WriteLine($"Tellor Playground lotto value: {lottoValue}");

                // Retrieve the competition number from the responseFromServer.
                var competitionNumber = new BigInteger(int.Parse(responseFromServer.Split("numero_concurso\":")[1].Split(",\"data_concurso")[0]));

                Console.WriteLine($"{competitionNumber}");

                lottoValue = lottoValue << 16;

                lottoValue += competitionNumber;

                var cNumber = await ttdService.CompetitionNumberQueryAsync();

                Console.WriteLine(cNumber);

                // Condition to determine if the lotto numbers need to be updated in the Tellor Playground.
                if (cNumber.Equals(competitionNumber))
                {
                    try
                    {
                        // Submit the new lotto numbers.
                        await tpService.SubmitValueRequestAndWaitForReceiptAsync(requestId, lottoValue);

                        // Update tellor time contract.
                        var updateLottoReceipt = await ttdService.UpdateLottoRequestAndWaitForReceiptAsync();

                        var events = updateLottoReceipt.DecodeAllEvents<LottoUpdateEventDTO>();

                        foreach(EventLog<LottoUpdateEventDTO> e in events)
                        {
                            Console.WriteLine($"Contract competition number: {e.Event.CompetitionNumber} winning Numbers: ");
                            foreach(BigInteger b in e.Event.Numbers)
                            {
                                Console.Write($"{b},");
                            }
                            Console.Write($" winning ids: ");
                            for(int i = 0; i < e.Event.WinningIDs.Count; i++)
                            {
                                Console.Write($"{e.Event.WinningIDs[i]} ");
                            }
                        }
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }

                // lotto = await ttdService.LottoNumbersQueryAsync();

                //Console.WriteLine($"\nTellor Time lotto value: {lotto}");

                /*
                BigInteger retrieveLottoNumbers = lottoNumbers;

                for(int i = 0; i < numbers.Length; i ++)
                {
                    Console.Write($"{retrieveLottoNumbers & mask},");
                    retrieveLottoNumbers = retrieveLottoNumbers >> 6;
                }*/
            }
            response.Close();
        }


        // Function to get TellorPlayground coin.
        private static async Task Faucet()
        {
            try
            {
                var faucetReceipt = await tpService.FaucetRequestAndWaitForReceiptAsync(account.Address);

                var playgroundBalance = Web3.Convert.FromWei(await tpService.BalanceOfQueryAsync(account.Address));

                var totalSupply = Web3.Convert.FromWei(await tpService.TotalSupplyQueryAsync());

                Console.WriteLine($"TotalSupply: {totalSupply} Balance: {playgroundBalance}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
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
                Console.WriteLine(responseFromServer + "\n");

                // Retrieve the competition number from the responseFromServer.
                return uint.Parse(responseFromServer.Split("numero_concurso\":")[1].Split(",\"data_concurso")[0]) + 1;
            }

        }

        private static async Task DeployTellorTimeContract()
        {
            Console.WriteLine("TellorTime contract deployment.....\n");

            try
            {
                TellorTimeDeployment TTD = new TellorTimeDeployment
                {
                    TellorAddress = tellorPlaygroundAddress,
                    CompetitionStart = getCompetitionStart()-1,
                };

                Console.WriteLine($"Competition Start: {TTD.CompetitionStart}\n");

                // Deploy Tellor Time Contract;
                var ttdReciept = await TellorTimeService.DeployContractAndWaitForReceiptAsync(web3, TTD);

                // Initialize tellorTime Address
                tellorTimeAddress = ttdReciept.ContractAddress;

                // Open Settings.
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var Settings = configFile.AppSettings.Settings;

                Settings["TellorTimeAddress"].Value = tellorTimeAddress;

                // Save Settings
                configFile.Save(ConfigurationSaveMode.Modified);

                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);

                // Initialize ttdService
                ttdService = new TellorTimeService(web3, tellorTimeAddress);

                // Get account balance 
                var balance = Web3.Convert.FromWei(await web3.Eth.GetBalance.SendRequestAsync(account.Address));

                Console.WriteLine($"    Tellor Time address: {tellorTimeAddress}\n  Gas used: {Web3.Convert.FromWei(ttdReciept.GasUsed)}\n  {account.Address} balance: {balance}");

                /*var name  = await tpService.NameQueryAsync();

                Console.WriteLine(name);
                
                var submitReceipt =  await tpService.SubmitValueRequestAndWaitForReceiptAsync(new BigInteger(2), new BigInteger(19000));

                var events = submitReceipt.DecodeAllEvents<NewValueEventDTO>();

                foreach (EventLog<NewValueEventDTO> e in events)
                {
                    Console.WriteLine($"Event data: {e.Event.RequestId} {e.Event.Time} {e.Event.Value}");
                }

                var getbtcPirce = await ttdService.SetBtcPriceRequestAndWaitForReceiptAsync();

                var btcPrice = await ttdService.BtcPriceQueryAsync();

                var didGet = await ttdService.DidGetQueryAsync();

                Console.WriteLine($"{btcPrice} {didGet}");*/
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        
    }
}
