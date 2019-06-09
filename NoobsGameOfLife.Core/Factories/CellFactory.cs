using NoobsGameOfLife.Core.Biology;
using NoobsGameOfLife.Core.Physics;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoobsGameOfLife.Core.Factories
{
    public class CellFactory : Factory<Cell>
    {
        public DNA OriginDNA => new DNA
        {
            FoodDigestibility = new Dictionary<Element, float> //Zuweisung
            {
                [Element.Carbon] = 1, //1 == 100%
                [Element.Oxygen] = 1, //1 == 100%
                [Element.Hydrogen] = 1 //1 == 100%
            },
            Saturated = 800,
            MaxEnergy = 1000,
            Sex = (DNA.Gender)random.Next(1, 3),
            Seed = random.Next()
        };
        
        public CellFactory(int width, int height) : base(width, height)
        {
        }

        public override Cell GetNext() 
            => new Cell(OriginDNA);
    }
}
