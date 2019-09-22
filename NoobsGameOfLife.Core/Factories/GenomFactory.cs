using NoobsGameOfLife.Core.Biology;
using NoobsGameOfLife.Core.Physics;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoobsGameOfLife.Core.Factories
{
    public class GenomFactory : Factory<Genom>
    {
        public Chromosome[] Male => new Chromosome[]
        {
            (random.Next(0,2) == 1 ? X : Y),
            D
        };

        public Chromosome[] Female => new Chromosome[]
        {
            X,
            D
        };

        public Chromosome X => new XChromosome() { Seed = random.Next() };
        public Chromosome Y => new YChromosome() { Seed = random.Next() };
        public Chromosome D => new DChromosome()
        {
            FoodDigestibility = new Dictionary<Element, float> //Zuweisung
            {
                [Element.Carbon] = 1, //1 == 100%
                [Element.Oxygen] = 1, //1 == 100%
                [Element.Hydrogen] = 1 //1 == 100%
            },
            Saturated = 800,
            MaxEnergy = 1000
        };

        public GenomFactory(int width, int height) : base(width, height)
        {
        }

        public override Genom GetNext() 
            => new Genom(Female, Male);

    }
}
