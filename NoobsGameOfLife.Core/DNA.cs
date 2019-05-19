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

        public Dictionary<Element, float> FoodDigestibility { get; set; }

        public ushort DigestSpeed { get; set; }
        public double MaxEnergy { get; set; }
        public ushort Saturated { get; set; }
        public int Seed { get; internal set; }
        public Gender Sex { get; set; }

        public DNA()
        {
            FoodDigestibility = new Dictionary<Element, float>();
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

        public override string ToString() => $"{Sex} | {MaxEnergy} | {Saturated}";

        public static DNA operator +(DNA male, DNA female)
        {
            var foodDigestibilities = new Dictionary<Element, float>();

            foreach (var element in male.FoodDigestibility)
                foodDigestibilities.Add(element.Key, (element.Value + female.FoodDigestibility[element.Key]) / 2);

            var foodDigestibility = foodDigestibilities.OrderBy(x => x.Value);

            var entry = foodDigestibility.First();
            foodDigestibilities[entry.Key] -= 0.5f;
            entry = foodDigestibility.Last();
            foodDigestibilities[entry.Key] += 0.5f;

            return new DNA
            {
                Seed = male.Seed + female.Seed,
                MaxEnergy = (ushort)((male.MaxEnergy + female.MaxEnergy) / 2),
                Saturated = (ushort)((male.Saturated + female.Saturated) / 2),
                FoodDigestibility = foodDigestibilities,
                Sex = (Gender)random.Next(0, 3)
            };
        }

        public enum Gender
        {
            Unknown,
            Male,
            Female
        }
    }
}