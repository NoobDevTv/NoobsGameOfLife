using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NoobsGameOfLife.Core
{
    public class Nutrient : IVisible
    {
        public Location Position { get; set; }

        public bool IsCollected { get; set; }

        public Dictionary<Element, byte> Elements { get; set; }

        public Nutrient(Location location, params (Element Element, byte Count)[] elements)
        {
            Position = location;
            IsCollected = false;
            Elements = new Dictionary<Element, byte>();

            foreach (var element in elements)
            {
                if (Elements.ContainsKey(element.Element))
                    Elements[element.Element] = element.Count;
                else
                    Elements.Add(element.Element, element.Count);
            }
        }

        public void PoopOut(Location position)
        {
            if (Elements.All(x => x.Value == 0))
            {
                IsCollected = true;
                return;
            }

            Position = position;
            IsCollected = false;
        }

    }
}
