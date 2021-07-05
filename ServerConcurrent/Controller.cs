/* Name: Controller.cs
 * Desc: This is our game controller, events are captured here but handled elsewhere.
 */

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServerConcurrent
{
    public class Controller
    {
        private readonly HttpClient _client = new();
        private readonly HostKey _hostKey = new();
        
        private bool GameInitialised { get; set; }
        
        private Player[] _inGamePlayers = new Player[2];
        private int CurrentPlayer { get; set; }
        private bool Quit { get; set; }
        private int Dices { get; set; }

        private static Board _board;
        
        private readonly Random _random = new Random();
        private int PlayerAmount { get; set; }

        public Controller()
        {
            _client.BaseAddress = new Uri("https://monopoly-135c.restdb.io/rest/");
            _client.DefaultRequestHeaders.Add("x-apikey","3f001906772c32780b5cd070c52afbd09d430");
        }
        
        // Function to roll dices
        private int RollDices()
        {
            return _random.Next(7, 13);
        }
        
        // Function to input player details
        private void InitialisePlayers()
        {
            Console.WriteLine("\nHost, please enter your name:");
            var name = Console.ReadLine();

            Player host = new Player(0, name, 100, _hostKey.InGameKey);
            host.Ready = true;
            host.Taken = true;
            
            _inGamePlayers[0] = host;
            
            for (var i = 1; i < PlayerAmount; i++)
            {
                Player newPlayer = new Player(i,"",100, _hostKey.InGameKey);
                _inGamePlayers[i] = newPlayer;
            }
        }
        
        // Function to wait for client response
        private async Task<bool> GetClientReady(Player thisPlayer)
        {
            var clientReady = false;

            while (!clientReady)
            {
                var player = await GetPlayerData(thisPlayer.PlayerNumber,_hostKey.InGameKey);

                if (player.Ready)
                    clientReady = true;
            }
            return true;
        }
        
        // Function to wait for client response
        private async Task<bool> GetClientResponse(Player thisPlayer)
        {
            var clientReady = false;

            while (!clientReady)
            {
                var player = await GetPlayerData(thisPlayer.PlayerNumber,_hostKey.InGameKey);

                if (player.Ready)
                    clientReady = true;
            }
            return true;
        }
        
        // Function to send invitation key to database
        private async Task<bool> SendHostInvitation(HostKey thisKey)
        {
            try
            {
                string json = JsonSerializer.Serialize(thisKey);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("latest-key", content);

                if (response.StatusCode == (HttpStatusCode) 200)
                {
                    Console.WriteLine("JSON response: All good!");
                }
            }
            catch (Exception postException)
            {
                Console.WriteLine(postException);
            }
            return true;
        }
        
        // Function to get player data from server
        private async Task<Player> GetPlayerData(int thisPlayerNumber,string clientGameKey)
        {
            var playerReceived = new Player(0, "", 0,_hostKey.InGameKey);
            
            // Get data from server
            var response = await _client.GetAsync("players");
            var content = await response.Content.ReadAsStringAsync();
            var obj = JsonSerializer.Deserialize<List<Player>>(content);

            if (obj != null)
            {
                foreach (var player in obj)
                {
                    if (player.PlayerNumber == thisPlayerNumber && player.InGameKey == clientGameKey)
                    {
                        playerReceived = player;
                    }
                }
            }
            else
            {
                throw new Exception("\nError getting data from server");
            }

            return playerReceived;
        }
        
        // Function to send player data to server
        private async Task<bool> SendPlayerData(Player thisPlayer)
        {
            try
            {
                string json = JsonSerializer.Serialize(thisPlayer);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("players", content);

                if (response.StatusCode == (HttpStatusCode) 200)
                {
                    Console.WriteLine($"JSON response: All good!");
                }
            }
            catch (Exception postException)
            {
                Console.WriteLine(postException);
            }
            return true;
        }
        
        // Function to update player data to the server
        private async Task<bool> UpdatePlayerData(Player thisPlayer)
        {
            try
            {
                // Get ID of this player
                var tempPlayer = await GetPlayerData(thisPlayer.PlayerNumber, thisPlayer.InGameKey);
                thisPlayer.ID = tempPlayer.ID;
                
                string json = JsonSerializer.Serialize(thisPlayer);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PutAsync($"players/{thisPlayer.ID}", content);
                if (response.StatusCode == (HttpStatusCode) 200)
                {
                    Console.WriteLine("JSON response: Successfully updated player");
                }
            }
            catch (Exception putException)
            {
                Console.WriteLine(putException);
                throw;
            }
            
            return true;
        }
        
        // Function to get the get 'currentPlayer' of the game
        private async Task<HostKey> GetCurrentPlayer()
        {
            // Get invitation key from server
            var response = await _client.GetAsync("latest-key");
            var content = await response.Content.ReadAsStringAsync();
            var obj = JsonSerializer.Deserialize<List<HostKey>>(content);
            if (obj != null)
            {
                return obj[0];
            }
            throw new Exception("\nError in getting data from database");
        }
        
        // Function to get message from client
        private async Task<Message> GetMessageClient(int currentPlayer)
        {
            var messageReceived = new Message(-1);
            
            // Get data from server
            var response = await _client.GetAsync("messages");
            var content = await response.Content.ReadAsStringAsync();
            var obj = JsonSerializer.Deserialize<List<Message>>(content);

            foreach (var msg in obj)
            {
                if (msg.PlayerNumber == currentPlayer)
                {
                    messageReceived = msg;
                }
            }

            return messageReceived;
        }
        
        // Function to send a message to the database
        private async Task<bool> SendClientMessage(Message thisMessage)
        {
            try
            {
                string json = JsonSerializer.Serialize(thisMessage);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("messages", content);

                if (response.StatusCode == (HttpStatusCode) 200)
                {
                    Console.WriteLine($"JSON response: All good!");
                }
            }
            catch (Exception postException)
            {
                Console.WriteLine(postException);
            }
            return true;
        }
        
        // Function to update a message to a specific client
        private async Task<bool> UpdateClientMessage(Message thisMessage,string id)
        {
            try
            {
                string json = JsonSerializer.Serialize(thisMessage);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PutAsync($"messages/{id}", content);
                if (response.StatusCode == (HttpStatusCode) 200)
                {
                    Console.WriteLine("JSON response: All good!");
                }
            }
            catch (Exception putException)
            {
                Console.WriteLine(putException);
                throw;
            }
            return true;
        }
        
        // Function to delete message on the database
        private async Task<bool> DeleteAllMessages()
        {
            for (int i = 0; i < PlayerAmount; i++)
            {
                var msg = await GetMessageClient(i);
                string json = JsonSerializer.Serialize(msg);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.DeleteAsync($"messages/{msg.ID}");
            }

            return true;
        }
        
        // FUnction to delete players on the database
        private async Task<bool> DeleteAllPlayers()
        {
            for (int i = 0; i < PlayerAmount; i++)
            {
                var player = await GetPlayerData(i,_hostKey.InGameKey);
                string json = JsonSerializer.Serialize(player);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.DeleteAsync($"players/{player.ID}");
            }

            return true;
        }
        
        // FUnction to delete players on the database
        private async Task<bool> DeleteAllKeys()
        {
            var key = await GetCurrentPlayer();
            string json = JsonSerializer.Serialize(key);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.DeleteAsync($"latest-key/{key.ID}");
            
            return true;
        }

        // Function to initialise game
        private async Task Intialise()
        {
            var valid = false;
            
            Console.WriteLine("\nWelcome to monopoly!");
            
            // Prompt host to create 'invitation' key
            Console.WriteLine("\nYou are a host, please create an invitation key:");
            _hostKey.InGameKey = Console.ReadLine();
            _hostKey.CurrentPlayer = 0;
            
            // Send host invitation key to database
            if (!await SendHostInvitation(_hostKey))
                throw new Exception("\nError occured in sending invitation");

            while (!valid)
            {
                // Prompt user the amount of players in the game
                Console.WriteLine("\nPlease enter number of players in this game: (2-8)");
                PlayerAmount = Convert.ToInt32(Console.ReadLine());

                if (PlayerAmount is >= 1 and <= 8) // Change me!
                {
                    valid = true;
                }
            }
            
            // Set the amount of players as the amount of 'InGamePlayers'
            _inGamePlayers = new Player[PlayerAmount];
            
            // Set the amount of clients as the amount of 'Clients'
            var clients = new Message[PlayerAmount];
            
            // Initialise the players in the array
            InitialisePlayers();

            for (int i = 0; i < PlayerAmount; i++)
            {
                var initial = new Message(i);
                clients[i] = initial;
            }
            
            // Send all players to the database
            for (var i = 0; i < PlayerAmount; i++)
            {
                if (!await SendPlayerData(_inGamePlayers[i]))
                    throw new Exception("\nError occured when transferring data");
                if (!await SendClientMessage(clients[i]))
                    throw new Exception("\nError occured when transferring data");
            }

            // Wait for client responses
            for (var i = 0; i < PlayerAmount; i++)
            {
                Console.WriteLine($"\nWaiting on client:{i}");
                // Function to check if client responds
                if (await GetClientReady(_inGamePlayers[i]))
                {
                    // Get all player data from database
                    _inGamePlayers[i] = await GetPlayerData(_inGamePlayers[i].PlayerNumber, _hostKey.InGameKey);
                    
                    // Get all messages from database
                    clients[i] = await GetMessageClient(i);
                }
            }
            
            // Create a board for number of 'PlayerAmount'
            _board = new Board(PlayerAmount);

            // Assign current player
            CurrentPlayer = 0; // With use of InGamePlayers index position

            // Assign initialised game flag
            GameInitialised = true;
            
            // 'Quit' as false to start the game
            Quit = false;
        }

        private void UpdatePosition( int diceAmount, Player thisPlayer)
        {
            // Get current position of current player
            var newPosition = thisPlayer.GetPosition() + diceAmount;
            
            // Check if the new position will fit inside the board index
            if (newPosition <= 39)
            {
                // Update the position of players in terms of 'PropertyNumber'
                thisPlayer.SetPosition(newPosition);   
            }
            else
            {
                // Modify the new position to fit board index
                newPosition -= 39;
                
                // If positive
                if (newPosition >= 0)
                {
                    thisPlayer.SetPosition(newPosition);
                }
                // If negative
                else
                {
                    newPosition -= 1;
                    thisPlayer.SetPosition(newPosition);
                }
            }
        }

        // Case: Landing on a property
        private async Task LandOnProperty(Player thisPlayer)
        {
            var found = false;
            var index = 0;
            var currentProperty = _board.GetBoardProperty(thisPlayer).Property;
            
            // Create a case for landing on jail (visiting)
            if (currentProperty.Name == "JAIL")
            {
                Console.WriteLine("\nYou have landed on 'Just visiting Jail'");
                found = true;
            }
            
            // Case for landing on go-to-jail
            else if (currentProperty.Name == "GO TO JAIL")
            {
                Console.WriteLine($"\n{thisPlayer.GetName()}, you have landed on {currentProperty.Name}. So, you" +
                                  $"are now in jail.");
                
                // Set current player position to JAIL
                thisPlayer.SetPosition(10); // 'index' 10 is 'JAIL'
                
                // Set current player 'inJail' status to 'true'
                thisPlayer.SetJailStatus(true);
            }

            // Property is owned by current player
            else if (thisPlayer.PropertyOwned(currentProperty))
            {
                // Prompt user to buy a house or hotel
                Console.WriteLine("\nWould you like to purchase a house (1) or hotel (2), if not (0)");
                var answer = Console.Read();

                switch (answer)
                {
                    case 1:
                        // Check if current player is eligible to buy a house or hotel
                        if (_board.PropertyHandle.EligibleHouseHotelPurchase(0, currentProperty.PropertyNumber,
                            thisPlayer))
                        {
                            found = true;
                            var msg = await GetMessageClient(CurrentPlayer);

                            msg.Text = $"{_inGamePlayers[CurrentPlayer].Name}, you just bought a house" +
                                       $"for {currentProperty.HouseCost}";

                            msg.StatusCode = 10;

                            if (!await UpdateClientMessage(msg, msg.ID))
                                throw new Exception("\nError occurred in updating message");
                        }
                            
                        break;
                    case 2:
                        // Check if current player is eligible to buy a house or hotel
                        if (_board.PropertyHandle.EligibleHouseHotelPurchase(1, currentProperty.PropertyNumber, thisPlayer))
                        {
                            found = true;
                            var msg = await GetMessageClient(CurrentPlayer);

                            msg.Text = $"{_inGamePlayers[CurrentPlayer].Name}, you just bought a house" +
                                       $"for {currentProperty.HotelCost}";

                            msg.StatusCode = 10;

                            if (!await UpdateClientMessage(msg, msg.ID))
                                throw new Exception("\nError occurred in updating message");
                        }
                        break;
                }
            }

            else if(currentProperty.PropertyNumber == 10)
            {
                found = true;
            }

            // Attempt to find ownership of 'property'
            while (!found && index < PlayerAmount)
            {
                // Property is not owned by current player, find who owns the property
                if (_inGamePlayers[index].PropertyOwned(currentProperty))
                {
                    // Get rent of '_currentProperty'
                    var rent = currentProperty.Rent;

                    // Current player pays rent to player who owns property
                    _inGamePlayers[CurrentPlayer].AddBalance(rent * (-1));
                    _inGamePlayers[index].AddBalance(rent);
                    
                    Console.WriteLine($"\n{_inGamePlayers[CurrentPlayer].Name} paid the amount of {rent} to" +
                                      $" {_inGamePlayers[index].GetName()}");

                    // Come out of loop
                    found = true;
                }
                index++;
            }
            
            // Property is not owned by any player
            if (found) return;
            {
                // Prompt current player to buy the property
                Console.WriteLine("\nWould you like to buy this property?");
                Console.WriteLine($"\nName:{currentProperty.Name}");
                Console.WriteLine($"Description:{currentProperty.Description}");
                Console.WriteLine($"Rent:${currentProperty.Rent}");
                Console.WriteLine($"House cost:${currentProperty.HouseCost}");
                Console.WriteLine($"Hotel cost:${currentProperty.HotelCost}");

                var msg = await GetMessageClient(CurrentPlayer);
                var answer = msg.Text;
                
                if (CurrentPlayer != 0)
                {
                    msg.Text = "\nAnswer (Y) or (N):";
                    msg.StatusCode = -1;

                    if (!await UpdateClientMessage(msg, msg.ID))
                        throw new Exception("\nError in updating message");

                    var waiting = true;

                    while (waiting)
                    {
                        msg = await GetMessageClient(CurrentPlayer);

                        if (msg.StatusCode == 0)
                        {
                            waiting = false;
                        }
                    }

                    answer = msg.Text;
                }
                else
                {
                    Console.WriteLine("\n(Y)Yes or (N)No");
                    answer = Console.ReadLine();
                }

                // If agree:
                if (answer == "Y" || answer == "y")
                {
                    // Check if current player is eligible to buy the property
                    if (_board.PropertyHandle.EligiblePurchase(currentProperty.PropertyNumber, thisPlayer))
                    {
                        // If so, buy the property
                        thisPlayer.AddProperty(currentProperty);
                        thisPlayer.AddBalance(currentProperty.Rent * (-1));
                        currentProperty.Ownership = thisPlayer;
                        
                        Console.WriteLine($"\n{_inGamePlayers[CurrentPlayer].Name} has successfully purchased this property");
                        Console.WriteLine($"Your balance is now:${thisPlayer.GetBalance()}");

                        msg.Text = $"Your balance is now:${thisPlayer.GetBalance()}";
                        msg.StatusCode = 0;

                        if (!await UpdateClientMessage(msg, msg.ID))
                            throw new Exception("\nError in updating message");
                    }
                    else
                    {
                        // If not, prompt current player they are not eligible to buy the property
                        Console.WriteLine("\nYou are not eligible to purchase this property.");
                    }
                }
            }
        }

        // Case: Landing on chance case
        private async Task LandOnCard(Player thisPlayer)
        {
            var card = _board.GetBoardProperty(thisPlayer).Card;
            var response = $"\nYou have good luck, your balance is now:{thisPlayer.GetBalance()}";
            var msg = await GetMessageClient(CurrentPlayer);
            
            if (_board.GetBoardProperty(thisPlayer).Card.isChance())
            {
                if (CurrentPlayer == 0)
                {
                    Console.WriteLine("\nYou landed on a chance!");   
                }
                
                msg.StatusCode = 0;
                msg.Text = response;
                
                if (!await UpdateClientMessage(msg, msg.ID))
                    throw new Exception("\nError occurred with sending message");
            }
            else if(_board.GetBoardProperty(thisPlayer).Card.isCommunity())
            {
                if (CurrentPlayer == 0)
                {
                    Console.WriteLine("\nYou landed on a community chest!");   
                }
                
                msg.StatusCode = 0;
                msg.Text = response;
                
                if (!await UpdateClientMessage(msg, msg.ID))
                    throw new Exception("\nError occurred with sending message");
            }
            else if(_board.GetBoardProperty(thisPlayer).Card.isTax())
            {
                if (CurrentPlayer == 0)
                {
                    response = $"\nYou have to pay tax, your balance is now:{thisPlayer.GetBalance()}";
                    
                    if (CurrentPlayer == 0)
                    {
                        Console.WriteLine("\nYou landed on a tax!");
                        Console.WriteLine(response);
                    }
                    
                    msg.StatusCode = 0;
                    msg.Text = response;
                
                    if (!await UpdateClientMessage(msg, msg.ID))
                        throw new Exception("\nError occurred with sending message");
                }
            }
            else
            {
                throw new Exception("Something went wrong.");
            }
            
            // Check if the card has a consequence
            if (card.Consequence_Result() <= 0 && !card.isTax())
            {
                response = "\nUnlucky, nothing for now";
                if (CurrentPlayer == 0)
                {
                    Console.WriteLine(response);
                }
                
                msg.StatusCode = 0;
                msg.Text = response;
                
                if (!await UpdateClientMessage(msg, msg.ID))
                    throw new Exception("\nError occurred with sending message");
            }
            else if(!card.isTax())
            {
                response = $"\nYou have good luck, your balance is" +
                           $"now:{thisPlayer.GetBalance()+card.Consequence_Result()}";
                
                thisPlayer.AddBalance(thisPlayer.GetBalance() + card.Consequence_Result()); 
                
                if (CurrentPlayer == 0)
                {
                    Console.WriteLine(response);   
                }
                
                msg.StatusCode = 0;
                msg.Text = response;
                
                if (!await UpdateClientMessage(msg, msg.ID))
                    throw new Exception("\nError occurred with sending message");
                
            }
            
        }

        private async Task InJail()
        {
            // Check if the current player has already skipped a turn
            if (!_inGamePlayers[CurrentPlayer].TurnSkip)
            {
                Console.WriteLine($"\n{_inGamePlayers[CurrentPlayer].GetName()}, you are currently in jail." +
                                  $"You have a chance to get out of jail if you roll a double (12) on the dices." +
                                  $"\nWould you like to try?");

                var msg = await GetMessageClient(CurrentPlayer);
                var rollDiceFlag = "";
                
                if (CurrentPlayer == 0)
                {
                    rollDiceFlag = Console.ReadLine();
                }
                else
                {
                    msg.Text = "\nAnswer (Y) or (N):";
                    msg.StatusCode = -1;

                    if (!await UpdateClientMessage(msg, msg.ID))
                        throw new Exception("\nError in updating message");

                    var waiting = true;

                    while (waiting)
                    {
                        msg = await GetMessageClient(CurrentPlayer);

                        if (msg.StatusCode == 0)
                        {
                            waiting = false;
                        }
                    }

                    rollDiceFlag = msg.Text;
                }

                if (rollDiceFlag == "Y" || rollDiceFlag == "y")
                {
                    // If current player is in jail, player must skip at least one turn
                    // Unless current player rolls a '12' with dices
                    var diceValue = RollDices();
                    if (diceValue == 12)
                    {
                        _inGamePlayers[CurrentPlayer].SetJailStatus(false);
                        _inGamePlayers[CurrentPlayer].SetPosition(20); // 'index' 20 is 'Free parking'
                            
                        Console.WriteLine($"\n{_inGamePlayers[CurrentPlayer].GetName()}, you are now in" +
                                          $"'Free parking'");
                    }
                    else
                    {
                        Console.WriteLine("\nYou have not rolled the correct amount. Your turn will be skipped.");
                        _inGamePlayers[CurrentPlayer].TurnSkip = true;
                    }
                }   
            }
            else
            {
                _inGamePlayers[CurrentPlayer].SetJailStatus(false);
                _inGamePlayers[CurrentPlayer].TurnSkip = false;
                _inGamePlayers[CurrentPlayer].SetPosition(20); // 'index' 20 is 'Free parking'
                            
                Console.WriteLine($"\n{_inGamePlayers[CurrentPlayer].GetName()}, you are now in" +
                                  $"'Free parking'");
            }
        }
        
        public async Task Run()
        {
            if (!GameInitialised)
            {
                await Intialise();

                for (int i = 0; i < PlayerAmount; i++)
                {
                    _inGamePlayers[CurrentPlayer].Ready = false;

                    if (!await UpdatePlayerData(_inGamePlayers[CurrentPlayer]))
                        throw new Exception("\nError occurred when updating players");
                }
            }

            while (!Quit)
            {
                Console.WriteLine($"\nCurrent player:{CurrentPlayer+1}");
                Console.WriteLine($"Name: {_inGamePlayers[CurrentPlayer].GetName()}");
                
                // Check if current player is in jail
                if (!_inGamePlayers[CurrentPlayer].GetJailStatus())
                {
                    // Roll the dice (random 7 - 12) as there are 2 dices
                    Dices = RollDices();
                    Console.WriteLine($"\n{_inGamePlayers[CurrentPlayer].GetName()}, You have rolled the amount:{Dices}");

                    // Update current player's coordinates
                    UpdatePosition(Dices,_inGamePlayers[CurrentPlayer]);
                
                    // Handle what happens when current player lands
                    var property = _board.GetBoardProperty(_inGamePlayers[CurrentPlayer]);

                    if (property.Position == 30)
                    {
                        Console.WriteLine($"\n{_inGamePlayers[CurrentPlayer].GetName()}, you have landed on 'GO TO JAIL'");
                        _inGamePlayers[CurrentPlayer].SetJailStatus(true);
                    }

                    if (property.Property != null)
                    {
                        await LandOnProperty(_inGamePlayers[CurrentPlayer]);
                    }
                    else if(property.Card != null)
                    {
                        await LandOnCard(_inGamePlayers[CurrentPlayer]);
                    }
                    else
                    {
                        throw new Exception("Something went wrong");
                    }
                }
                
                // Current player is in jail
                else
                {
                    await InJail();
                }
                
                // Update the board
                _board.UpdateBoard(_inGamePlayers[CurrentPlayer]);

                var msg = await GetMessageClient(CurrentPlayer);
                msg.StatusCode = 1;
                msg.Text = "Done";

                if (!await UpdateClientMessage(msg, msg.ID))
                    throw new Exception("\nError occurred with sending message");

                if (CurrentPlayer == 0)
                {
                    msg.StatusCode = 0;
                    msg.Text = "";
                    
                    if (!await UpdateClientMessage(msg, msg.ID))
                        throw new Exception("\nError occurred with sending message");
                }
                
                // Make the current player ready
                _inGamePlayers[CurrentPlayer].Ready = true;

                if (!await UpdatePlayerData(_inGamePlayers[CurrentPlayer]))
                    throw new Exception("\nError occurred in updating player data.");

                // Change to next player using CurrentPlayer
                if (CurrentPlayer < PlayerAmount-1)
                {
                    CurrentPlayer++;
                }
                else
                {
                    CurrentPlayer = 0;
                }
                
                // Prompt current player user to quit
                Console.WriteLine("\nWould you like to quit? (Y) or (N)");
                var answer = Console.ReadLine();
                
                if (answer == "Y" || answer == "y")
                {
                    for (var clients = 0; clients < PlayerAmount; clients++)
                    {
                        var quitMsg = await GetMessageClient(clients);
                        quitMsg.StatusCode = -2;
                        quitMsg.Text = "\nThanks for playing!\nQuiting...";
                        
                        if (!await UpdateClientMessage(quitMsg, quitMsg.ID))
                            throw new Exception("\nError occurred with sending quit message");
                    }
                    
                    Console.WriteLine("\nThanks for playing!");

                    await DeleteAllMessages();
                    await DeleteAllPlayers();
                    await DeleteAllKeys();
                    
                    Console.WriteLine("\nDone cleaning up!");
                    
                    Quit = true;
                }
                else if(answer == "N" || answer == "n")
                {
                    Quit = false;   
                }
                else
                {
                    throw new Exception("Invalid input");
                }
            }
        }
    }
}