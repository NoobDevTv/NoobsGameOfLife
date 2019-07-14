using NoobsGameOfLife.Core.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoobsGameOfLife.Core.Biology
{
    public sealed class DChromosome : Chromosome
    {
        public Dictionary<Element, float> FoodDigestibility { get; set; }

        public ushort DigestSpeed { get; set; }
        public double MaxEnergy { get; set; }
        public ushort Saturated { get; set; }

        public DChromosome()
        {
            FoodDigestibility = new Dictionary<Element, float>();
        }

        public override Chromosome Copy()
            => new DChromosome
            {
                MaxEnergy = MaxEnergy,
                Saturated = Saturated,
                FoodDigestibility = FoodDigestibility.ToDictionary(e => e.Key, e => e.Value)
            };
    }
}
