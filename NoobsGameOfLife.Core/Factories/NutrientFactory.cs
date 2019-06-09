using NoobsGameOfLife.Core.Biology;
using NoobsGameOfLife.Core.Physics;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoobsGameOfLife.Core.Factories
{
    public class NutrientFactory : Factory<Nutrient>
    {       
        public NutrientFactory(int width, int height) : base(width, height)
        {
            
        }

        public override Nutrient GetNext() 
            => new Nutrient(new Location(random.Next(0, width), random.Next(0, height)),
                    (Element.Carbon, (byte)random.Next(3, 20)),
                    (Element.Hydrogen, (byte)random.Next(3, 20)),
                    (Element.Oxygen, (byte)random.Next(3, 20)));
    }
}
