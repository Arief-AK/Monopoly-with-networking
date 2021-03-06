using System;
using ServerConcurrent.Interfaces;

namespace ServerConcurrent.Database.Cards
{
    public class Tax:ICards
    {
        public int PropertyNumber { get; set; }
        public int Value { get; set; }
        public string Description { get; set; }

        private readonly Random _random = new Random();
        public bool Consequence()
        {
            var number = _random.Next(1, 10);
            return number % 2 == 0;
        }

        public int Consequence_Result()
        {
            return Consequence() ? _random.Next(-125, -10) : 0;
        }

        public bool isChance()
        {
            return false;
        }

        public bool isCommunity()
        {
            return false;
        }

        public bool isTax()
        {
            return true;
        }

        public Tax(int propertyNumber, string name)
        {
            PropertyNumber = propertyNumber;
        }
    }
}