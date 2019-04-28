using System;
using System.Collections.Generic;
using System.Linq;

namespace NoobsGameOfLife.Core
{
    public class DNA
    {
        private static readonly Random random;

        static DNA()
        {
            random = new Random();
        }

        public Dictionary<Element, int> FoodDigestibility { get; set; }

        public ushort DigestSpeed { get; set; }
        public ushort MaxEnergy { get; set; }
        public ushort Saturated { get; set; }
        public int Seed { get; internal set; }
        public Gender Sex { get; set; }

        public DNA()
        {
            FoodDigestibility = new Dictionary<Element, int>();
        }

        public DNA Copy()
            => new DNA
            {
                Seed = Seed,
                MaxEnergy = MaxEnergy,
                Saturated = Saturated,
                FoodDigestibility = FoodDigestibility.ToDictionary(e => e.Key, e => e.Value),
                Sex = Sex
            };

        public static DNA operator +(DNA male, DNA female)
            => new DNA
            {
                Seed = male.Seed + female.Seed,
                MaxEnergy = (ushort)((male.MaxEnergy + female.MaxEnergy) / 2),
                Saturated = (ushort)((male.Saturated + female.Saturated) / 2),
                FoodDigestibility = male.FoodDigestibility,
                Sex = (Gender)random.Next(0, 3)
            };

        public enum Gender
        {
            Unknown,
            Male,
            Female
        }
    }
}