namespace Client.Interfaces
{
    public interface ICards
    {
        int PropertyNumber { get; set; }
        int Value { get; set; }
        string Description { get; set; }
        bool Consequence();
        int Consequence_Result();

        bool IsChance();
        bool IsCommunity();
        bool IsTax();
    }
}