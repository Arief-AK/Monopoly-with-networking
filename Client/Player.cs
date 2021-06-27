using System.Collections.Generic;
using System.Text.Json.Serialization;
using Client.Interfaces;

namespace Client
{
    public class Player
    {
        [JsonPropertyName("_id")]
        public string ID { get; set; }
        public int PlayerNumber { get; set; }
        public string Name { get; set; }
        public int AccountBalance { get; set; }
        public int Position { get; set; }
        
        public List<IProperty> _propertyList = new List<IProperty>();
        public bool InJail { get; set; }
        public string InGameKey { get; set; }
        public bool Taken { get; set; }
        public bool TurnSkip { get; set; }
        public bool Ready { get; set; }
        
        public Player (int playerNumber, string name, int accountBalance, string inGameKey)
        {
            PlayerNumber = playerNumber;
            Name = name;
            AccountBalance = accountBalance;
            InGameKey = inGameKey;
            Taken = true;
            TurnSkip = false;
            Ready = true;
        }

        public string GetName()
        {
            return Name;
        }
        public int GetPosition()
        {
            return Position;
        }

        public void SetPosition(int newPosIndex)
        {
            Position = newPosIndex;
        }
        
        public void AddBalance(int amount)
        {
            AccountBalance += amount;
        }

        public int GetBalance()
        {
            return AccountBalance;
        }
        
        public void AddProperty(IProperty thisProperty)
        {
            _propertyList.Add(thisProperty);
        }

        public void RemoveProperty(IProperty thisProperty)
        {
            // Attempt to find the passed argument in the list
            if (_propertyList.Exists(thisProperty.Equals))
            {
                // Remove the property from the list
                _propertyList.Remove(thisProperty);
            }
        }

        public bool PropertyOwned(IProperty thisProperty)
        {
            // Attempt to find the passed argument in the list
            if (_propertyList.Exists(thisProperty.Equals))
            {
                return true;
            }
            return false;
        }
        
        public void SetJailStatus(bool value)
        {
            InJail = value;
        }

        public bool GetJailStatus()
        {
            return InJail;
        }
    }
}