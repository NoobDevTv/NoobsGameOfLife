using System.Collections.Generic;

namespace NoobsGameOfLife.Core
{
    internal class DNA
    {
        public Dictionary<Element, int> FoodDigestibility { get; set; }
        public ushort DigestSpeed { get; set; }
        public ushort MaxEnergy { get; set; }
        public ushort Saturated { get; set; }

        public DNA()
        {
            FoodDigestibility = new Dictionary<Element, int>();
        }
    }
}