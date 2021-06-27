using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Client.Database;
using Client.Interfaces;

namespace Client
{
    public class ClientController
    {
        private readonly Random _random = new();
        private readonly HttpClient _client = new();
        private readonly ClientKey _userClientKey = new();
        private int CurrentGamePlayer { get; set; }
        private bool GameInitialised { get; set; }
        
        private bool Waiting { get; set; }

        private Player _clientPlayer = new Player(-1, "", 0,"");

        public ClientController()
        {
            _client.BaseAddress = new Uri("https://monopoly-135c.restdb.io/rest/");
            _client.DefaultRequestHeaders.Add("x-apikey","3f001906772c32780b5cd070c52afbd09d430");
        }
        
        // Function to wait for client response
        private async Task<bool> GetClientReady(Player thisPlayer)
        {
            var clientReady = false;

            while (!clientReady)
            {
                var player = await GetPlayerData(thisPlayer.PlayerNumber,_userClientKey.InGameKey);

                if (player.Ready)
                    clientReady = true;
            }
            return true;
        }
        
        // Function that gets the 'playerNumber' of this client from server side
        private async Task<int> GetClientPlayerNumber(bool saveGame)
        {
            if (saveGame)
            {
                // Get data from server
                var response = await _client.GetAsync("players");
                var content = await response.Content.ReadAsStringAsync();
                var obj = JsonSerializer.Deserialize<List<Player>>(content);

                var found = false;
                var index = 0;

                while (!found)
                {
                    if (obj != null && obj[index].Taken && obj[index].Name == _clientPlayer.Name)
                    {
                        found = true;
                    }
                    index++;
                }

                return obj[index].PlayerNumber;
            }
            else
            {
                // Get data from server
                var response = await _client.GetAsync("players");
                var content = await response.Content.ReadAsStringAsync();
                var obj = JsonSerializer.Deserialize<List<Player>>(content);
                
                var found = false;
                var index = 0;

                while (!found)
                {
                    if (!obj[index].Taken)
                    {
                        found = true;
                    }
                    else
                    {
                        index++;   
                    }
                }

                return obj[index].PlayerNumber;
            }
        }
        
        // Function that gets 'invitation' key from server-side
        private async Task<string> GetInvitationKey(bool saveGame)
        {
            if (saveGame)
            {
                // Get saved-game key from server
                var response = await _client.GetAsync("saved-game-keys");
                var content = await response.Content.ReadAsStringAsync();
                var obj = JsonSerializer.Deserialize<List<ClientKey>>(content);
                
                var found = false;
                var index = 0;

                while (!found)
                {
                    // Compare client 'key' and server 'key'
                    if (obj != null && obj[index].InGameKey == _userClientKey.InGameKey)
                    {
                        found = true;
                    }
                    else
                    {
                        index++;
                    }
                }

                return obj[index].InGameKey;
            }
            else
            {
                // Get invitation key from server
                var response = await _client.GetAsync("latest-key");
                var content = await response.Content.ReadAsStringAsync();
                var obj = JsonSerializer.Deserialize<List<ClientKey>>(content);
                if (obj != null) return obj[0].InGameKey;
            }
            throw new Exception("Something went wrong!");
        }
        
        // Function to get 'current player' number of the game from server-side
        private async Task<int> GetCurrentPlayerNumber(bool saveGame)
        {
            if (saveGame)
            {
                // Get current-player from server
                var response = await _client.GetAsync("saved-game-keys");
                var content = await response.Content.ReadAsStringAsync();
                var obj = JsonSerializer.Deserialize<List<ClientKey>>(content);
                
                var found = false;
                var index = 0;

                while (!found)
                {
                    // Compare client 'key' and server 'key'
                    if (obj != null && obj[index].InGameKey == _userClientKey.InGameKey)
                    {
                        _userClientKey.CurrentPlayer = obj[index].CurrentPlayer;
                        found = true;
                    }
                    index++;
                }

                return obj[index].CurrentPlayer;
            }
            else
            {
                // Get invitation key from server
                var response = await _client.GetAsync("latest-key");
                var content = await response.Content.ReadAsStringAsync();
                var obj = JsonSerializer.Deserialize<List<ClientKey>>(content);
                if (obj != null) return obj[0].CurrentPlayer;
            }
            throw new Exception("Something went wrong!");
        }
        
        // Function that prompts user for game 'state'
        private async Task InitialiseGame()
        {
            var serverPlayerNumber = -1;
            
            Console.WriteLine("\nWelcome to monopoly!");
            
            Console.WriteLine("\nStarting a new game!");
            
            // Game is not saved. Get new invitation key from server and assign to client key
            _userClientKey.InGameKey = await GetInvitationKey(false);
            
            // Function that gets the 'playerNumber' of this client from server side
            serverPlayerNumber = await GetClientPlayerNumber(false);
            
            // Create and initialise player
            _clientPlayer = InitialisePlayer(serverPlayerNumber);
            
            // Get player data back to receive 'ID'
            var player = await GetPlayerData(_clientPlayer.PlayerNumber, _userClientKey.InGameKey);
            _clientPlayer.ID = player.ID;

            // Update player data back to server
            if (!await UpdatePlayerData(_clientPlayer))
                throw new Exception("\nSomething went wrong!");
            
            _clientPlayer = await GetPlayerData(_clientPlayer.PlayerNumber, "abcdef");
            
            // Function to get 'current player' number of the game from server-side
            CurrentGamePlayer = await GetCurrentPlayerNumber(false);
            
            GameInitialised = true;
        }

        // Function to prompt and enter user details
        private Player InitialisePlayer(int playerNumber)
        {
            Console.WriteLine("\nPlease enter a name:");
            var name = Console.ReadLine();
            
            return new Player(playerNumber, name, 100,_userClientKey.InGameKey);
        }
        
        // Function to get player data from server
        private async Task<Player> GetPlayerData(int thisPlayerNumber,string clientGameKey)
        {
            var playerReceived = new Player(0, "", 0,_userClientKey.InGameKey);
            
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
                
                Console.WriteLine($"JSON response: All good!");
            }
            catch (Exception PostException)
            {
                Console.WriteLine(PostException);
            }
            return true;
        }
        
        // Function to get location data
        private async Task<string> GetLocationName(int propertyNumber)
        {
            // Get data from server
            var response = await _client.GetAsync("board-properties");
            var content = await response.Content.ReadAsStringAsync();
            var obj = JsonSerializer.Deserialize<List<Street>>(content);

            var index = 0;
            var found = false;
            
            if (obj != null)
            {
               while (!found)
               {
                   if (propertyNumber == obj[index].PropertyNumber)
                   {
                       found = true;
                   }
                   else
                   {
                       index++;
                   }
               }
            }

            return obj[index].Name;
        }
        
        // Function to update player data to the server
        private async Task<bool> UpdatePlayerData(Player thisPlayer)
        {
            try
            {
                string json = JsonSerializer.Serialize(thisPlayer);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PutAsync($"players/{thisPlayer.ID}", content);
                if (response.StatusCode == (HttpStatusCode) 200)
                {
                    Console.WriteLine($"JSON response: All good!");
                }
            }
            catch (Exception putException)
            {
                Console.WriteLine(putException);
                throw;
            }
            
            return true;
        }
        
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
        
        // Function to send a message to a specific client
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
        
        // Function that runs client-side of Monopoly
        public async Task Run()
        {
            if (!GameInitialised)
            {
                await InitialiseGame();   
            }

            var msg = await GetMessageClient(_clientPlayer.PlayerNumber);

            // Server requests response
            if (msg.StatusCode == -1)
            {
                // Show the request
                Console.WriteLine(msg.Text);

                var answer = Console.ReadLine();
                
                // Send response back to server
                msg.Text = answer;
                msg.StatusCode = 0;

                if (!await UpdateClientMessage(msg, msg.ID))
                    throw new Exception("\nError occurred in updating message");
            }
            
            // Summarise information from server
            if (msg.StatusCode == 1)
            {
                if (await GetClientReady(_clientPlayer))
                {
                    // Get player
                    _clientPlayer = await GetPlayerData(_clientPlayer.PlayerNumber, _userClientKey.InGameKey);
                    
                    // Show information
                    Console.WriteLine("\nHere are your stats");
                    Console.WriteLine($"Name:{_clientPlayer.Name}");
                    Console.WriteLine($"Account balance:{_clientPlayer.AccountBalance}");
                    Console.WriteLine($"Properties owned:{_clientPlayer._propertyList.Count}");

                    if (!_clientPlayer.InJail)
                    {
                        if (_clientPlayer.Position == 10)
                        {
                            Console.WriteLine($"\nPosition: {await GetLocationName(_clientPlayer.Position)} (just-visiting)");
                        }
                        else
                        {
                            Console.WriteLine($"\nPosition: {await GetLocationName(_clientPlayer.Position)}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nLocation: JAIL");
                    }

                    msg.Text = "";
                    msg.StatusCode = 0;
                    if(! await UpdateClientMessage(msg,msg.ID)) 
                        throw new Exception("\nError occurred when updating the message");
                    
                    Console.WriteLine("\nYour turn has ended");
                }
            }
            // TURN LOOP END
        }
    }
}