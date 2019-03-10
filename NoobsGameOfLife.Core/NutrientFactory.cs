using System;
using System.Collections.Generic;
using System.Text;

namespace NoobsGameOfLife.Core
{
    public class NutrientFactory
    {
        public IEnumerable<Nutrient> GenerateNutrient(int count)
        {
            var random = new Random();

            for (int i = 0; i < count; i++)
                yield return new Nutrient(random);
        }
    }
}
