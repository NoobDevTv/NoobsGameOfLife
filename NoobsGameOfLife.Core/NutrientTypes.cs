using System;
using System.Collections.Generic;
using System.Text;

namespace NoobsGameOfLife.Core
{
    public class NutrientTypes
    {
        Dictionary<Element, byte> Elements;
        public NutrientTypes()
        {
            Elements = new Dictionary<Element, byte> {};
        }
    }
}
