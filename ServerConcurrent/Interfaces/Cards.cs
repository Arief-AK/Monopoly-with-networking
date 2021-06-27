namespace ServerConcurrent.Interfaces
{
    public interface ICards
    {
        int PropertyNumber { get; set; }
        int Value { get; set; }
        string Description { get; set; }
        bool Consequence();
        int Consequence_Result();

        bool isChance();
        bool isCommunity();
        bool isTax();
    }
}