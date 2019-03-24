using System;
using System.Collections.Generic;
using System.Text;

namespace NoobsGameOfLife.Core
{
    public class NutrientFactory
    {
        private readonly int width;
        private readonly int height;

        public NutrientFactory(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public IEnumerable<Nutrient> GenerateNutrients(int count)
        {
            var random = new Random();

            for (int i = 0; i < count; i++)
                yield return new Nutrient(new Location(random.Next(0, width), random.Next(0, height)), 
                    (Element.Carbon, (byte)random.Next(3,20)),
                    (Element.Hydrogen, (byte)random.Next(3, 20)),
                    (Element.Oxygen, (byte)random.Next(3, 20)));
        }
    }
}
