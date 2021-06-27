﻿using ServerConcurrent.Interfaces;

namespace ServerConcurrent.Database.Streets
{
    public class Street : IProperty
    {
        public Player Ownership { get; set; }
        public int PropertyNumber { get; set; }
        public string Color { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Rent { get; set; }
        public int Houses { get; set; }
        public int Hotels { get; set; }
        public int HouseCost { get; set; }
        public int HotelCost { get; set; }

        public Street(int propertyNumber, string color, string name, string description, int rent, int houseCost,
            int hotelCost)
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
            switch (choice)
            {
                //If 'choice' is '0' then add a house
                case 0:
                    Rent += HouseCost;
                    Houses++;
                    return true;
                case 1:
                    Rent += HotelCost;
                    Hotels++;
                    return true;
                default:
                    return false;
            }
        }
    }
}