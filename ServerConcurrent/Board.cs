/* Name: Board.cs
 * Desc: This is the board class where the physical board elements are initialised.
 * 
 */

using ServerConcurrent.Database.Handlers;
using ServerConcurrent.Interfaces;

public struct Area
{
    public int Position;
    public IProperty Property;
    public ICards Card;
}

namespace ServerConcurrent
{
    public class Board
    {
        // Create an array representation of the board
        public Area[] BoardMap = new Area[40];
        
        // Create a 'PropertyHandler'
        public PropertyHandler PropertyHandle = new PropertyHandler();
        
        // Create a 'CardHandler'
        private CardHandler CardHandle = new CardHandler();
        
        // Create an array of 'PlayerPositions'
        private int[] _playerPositions = new int[8];

        private void InitialiseCoordinates(int numberOfPlayers)
        {
            // Initially, all players start from 'GO' property
            for (var i = 0; i < numberOfPlayers; i++)
            {
                _playerPositions[i]= 0;
            }
        }

        private void AssignHabitablePropertiesToMap()
        {
            var index = 0;
            var propertyIndex = 0;

            while (index < 40)
            {
                if (index == PropertyHandle.HabitableProperties[propertyIndex].PropertyNumber)
                {
                    BoardMap[index].Property = PropertyHandle.HabitableProperties[propertyIndex];
                    propertyIndex++;
                }
                index++;
            }
        }

        private void AssignCardsToMap()
        {
            var index = 0;
            var arrayIndex = 0;

            while (arrayIndex < 10)
            {
                if (index == CardHandle.Cards[arrayIndex].PropertyNumber)
                {
                    BoardMap[index].Card = CardHandle.Cards[arrayIndex];
                    arrayIndex++;
                }
                index++;
            }
            
        }

        private void BuildMap()
        {
            AssignHabitablePropertiesToMap();
            AssignCardsToMap();
        }

        public Board(int numberOfPlayers)
        {
            // Build the map
            BuildMap();
            
            // Initialise Player coordinates
            InitialiseCoordinates(numberOfPlayers);
        }

        public void UpdateBoard(Player thisPlayer)
        {
            // Assign position of players to respective position in '_playerPositions'
            _playerPositions[thisPlayer.PlayerNumber] = thisPlayer.GetPosition();
        }
        
        public Area GetBoardProperty(Player thisPlayer)
        {
            return BoardMap[thisPlayer.GetPosition()];
        }
    }
}