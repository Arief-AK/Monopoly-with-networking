using System;
using ServerConcurrent.Database.Cards;
using ServerConcurrent.Interfaces;

namespace ServerConcurrent.Database.Handlers
{
    public class CardHandler
    {
        public ICards[] Cards =
        {
            new CommunityChest(2),
            new Tax(4,"INCOME TAX"),
            new Chance(7),
            new Tax(12,"ELECTRICITY TAX"),
            new CommunityChest(17),
            new Chance(22),
            new Tax(28,"WATER TAX"),
            new CommunityChest(33),
            new Chance(36),
            new Tax(38,"LUXURY TAX")
        };
        
        public int GetACard(ICards fromThisProperty, int propertyNumber)
        {
            // Check what type of card is supposed to be given
            if (fromThisProperty.isChance() || fromThisProperty.isCommunity() || fromThisProperty.isTax())
            {
                // Perform the consequence
                return fromThisProperty.Consequence_Result();
            }
            throw new Exception("Invalid");
        }
    }
}