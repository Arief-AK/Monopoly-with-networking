using Client.Interfaces;

namespace Client.Database
{
    public class Stations:IProperty
    {
        public string Ownership { get; set; }
        public int PropertyNumber { get; set; }
        public string Color { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Rent { get; set; }
        public int Houses { get; set; }
        public int Hotels { get; set; }
        public int HouseCost { get; set; }
        public int HotelCost { get; set; }
        
        public Stations(int propertyNumber, string color, string name, string description, int rent, int houseCost, int hotelCost)
        {
            PropertyNumber = propertyNumber;
            Color = color;
            Name = name;
            Description = description;
            Rent = rent;
            HouseCost = houseCost;
            HotelCost = hotelCost;
        }
        
        public bool Renovate(int choice)
        {
            // TODO: If player owns other stations, the rent of the this station is increased
            return true;
        }
    }
}