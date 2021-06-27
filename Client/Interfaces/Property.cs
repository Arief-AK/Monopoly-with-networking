namespace Client.Interfaces
{
    public interface IProperty
    {
        string Ownership { get; set; }
        int PropertyNumber { get; set; }
        string Color { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        int Rent { get; set; }
        int Houses { get; set; }
        int Hotels { get; set; }
        int HouseCost { get; set; }
        int HotelCost { get; set; }
        bool Renovate(int choice); // If choice is 0 perform something with house. If choice is 1 perform something with hotel
    }
}