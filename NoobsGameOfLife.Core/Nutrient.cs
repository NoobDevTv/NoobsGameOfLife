using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NoobsGameOfLife.Core
{
    public class Nutrient : IVisible
    {
        public Location Position { get; set; }

        public bool IsCollected { get; private set; }

        public Dictionary<Element, byte> Elements { get; set; }

        public Nutrient(Location location, Dictionary<Element, byte> elements)
        {
            Position = location;
            IsCollected = false;
            Elements = elements;
        }

        public void PoopOut(Location position)
        {
            if (Elements.All(x => x.Value == 0))
                return;

            Position = position;
            IsCollected = false;
        }
    }
}
