using System;
using ServerConcurrent.Database.Streets;
using ServerConcurrent.Interfaces;

namespace ServerConcurrent.Database.Handlers
{
    public class PropertyHandler
    {
        public IProperty[] HabitableProperties =
        {
            new Street(0,"WHITE","GO","START",200,0,0),
            new Street(1, "BROWN", "Mediterranean Avenue", "TBA", 2, 50, 50),
            new Street(3, "BROWN", "Baltic Avenue", "TBA", 4, 50, 50),
            new Stations.Stations(5, "BLACK", "Reading Railroad", "TBA", 25, 0, 0),
            new Street(6, "LIGHT-BLUE", "Oriental Avenue", "TBA", 6, 50, 50),
            new Street(8, "LIGHT-BLUE", "Vermont Avenue", "TBA", 6, 50, 50),
            new Street(9, "LIGHT-BLUE", "Connecticut Avenue", "TBA", 8, 50, 50),
            new Street(10,"WHITE-BLACK","JAIL","JAIL",0,0,0),
            new Street(11, "PINK", "St. Charles Place", "TBA", 10, 100, 100),
            new Street(13, "PINK", "States Avenue", "TBA", 10, 100, 100),
            new Street(14, "PINK", "Virginia Avenue", "TBA", 12, 100, 100),
            new Stations.Stations(15, "BLACK", "Pennsylvania Railroad", "TBA", 25, 0, 0),
            new Street(16, "ORANGE", " St. James Place", "TBA", 14, 100, 100),
            new Street(18, "ORANGE", "Tennessee Avenue", "TBA", 14, 100, 100),
            new Street(19, "ORANGE", "New York Avenue", "TBA", 16, 100, 100),
            new Street(20, "WHITE-RED", "FREE PARKING", "TBA", 0, 0, 0),
            new Street(21, "RED", "Kentucky Avenue", "TBA", 18, 150, 150),
            new Street(23, "RED", "Indiana Avenue", "TBA", 18, 150, 150),
            new Street(24, "RED", "Illinois Avenue", "TBA", 20, 150, 150),
            new Stations.Stations(25, "BLACK", "Boston Railroad", "TBA", 25, 0, 0),
            new Street(26, "YELLOW", "Atlantic Avenue", "TBA", 22, 150, 150),
            new Street(27, "YELLOW", "Vendor Avenue", "TBA", 22, 150, 150),
            new Street(29, "YELLOW", "Marvin Gardens", "TBA", 24, 150, 150),
            new Street(30, "WHITE-BLUE", "GO TO JAIL", "TBA", 0, 0, 0),
            new Street(31, "GREEN", "Pacific Avenue", "TBA", 26, 200, 200),
            new Street(32, "GREEN", "North Carolina Avenue", "TBA", 26, 200, 200),
            new Street(34, "GREEN", "Pennsylvania Avenue", "TBA", 28, 200, 200),
            new Stations.Stations(35, "BLACK", "Short-line Railroad", "TBA", 25, 0, 0),
            new Street(37, "DARK-BLUE", "Park Place Avenue", "TBA", 35, 200, 200),
            new Street(39, "DARK-BLUE", "Boardwalk Avenue", "TBA", 50, 200, 200)
        };

        public bool EligiblePurchase(int propertyIndex, Player thisPlayer)
        {
            if (thisPlayer.GetBalance() >= HabitableProperties[propertyIndex].Rent)
                return true;

            return false;
        }
        
        public bool EligibleHouseHotelPurchase(int choice, int propertyIndex, Player thisPlayer)
        {
            switch (choice)
            {
                // Buy a house
                case 0:
                {
                    if (thisPlayer.GetBalance() >= HabitableProperties[propertyIndex].HouseCost)
                    {
                        if (AddHouse(propertyIndex))
                        {
                            Console.WriteLine("\nYou have successfully purchased a house");
                            return true;
                        }
                    }
                    break;
                }
                // Buy a hotel
                case 1 when AddHotel(propertyIndex):
                    Console.WriteLine("\nYou have successfully purchased a hotel");
                    return true;
            }
            return false;
        }

        public bool AddHouse(int propertyIndex)
        {
            // If so, add a house at given property
            HabitableProperties[propertyIndex].Renovate(0);
            return true;
        }

        public bool AddHotel(int propertyIndex)
        {
            // If so, add a hotel at given property
            HabitableProperties[propertyIndex].Renovate(1);
            return true;
        }

        public bool RemoveHouse(int propertyIndex)
        {
            // TODO: Check if player has a house(s)
            
            //If so, remove a house at given property
            HabitableProperties[propertyIndex].Renovate(0);
            return true;
        }

        public bool RemoveHotel(int propertyIndex)
        {
            // TODO: Check if player has a hotel
            
            //If so, remove a hotel at given property
            HabitableProperties[propertyIndex].Renovate(1);
            return true;
        }
    }
}