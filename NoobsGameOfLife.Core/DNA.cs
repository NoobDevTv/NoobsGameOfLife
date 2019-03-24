using System.Collections.Generic;

namespace NoobsGameOfLife.Core
{
    public class DNA
    {
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

        public static DNA operator +(DNA male, DNA female)
            => new DNA
            {
                Seed = male.Seed + female.Seed,
                MaxEnergy = (ushort)((male.MaxEnergy + female.MaxEnergy) / 2),
                Saturated = (ushort)((male.Saturated + female.Saturated) / 2),
                FoodDigestibility = male.FoodDigestibility,
                Sex = Gender.Unknown
            };

        public enum Gender
        {
            Unknown,
            Male,
            Female
        }
    }
}