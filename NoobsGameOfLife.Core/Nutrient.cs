using System;
using System.Collections.Generic;
using System.Text;

namespace NoobsGameOfLife.Core
{
    public class Nutrient 
    {

        public Location Position { get; set; }

        public int Energy { get; private set; }
        public bool IsCollected { get; private set; }
        
        public Nutrient(Random rand)
        {
            ReInitalized(rand);
        }
        public Nutrient(Location location, Random rand)
        {
            ReInitalized(location, rand);
        }

        public void ReInitalized(Random rand)
        {
            ReInitalized(new Location(rand.Next(0, 500), rand.Next(0, 500)), rand);
        }
        public void ReInitalized(Location location, Random rand)
        {
            Position = location;
            Energy = rand.Next(500, 1000);

            IsCollected = false;
        }

        internal int Collect()
        {
            IsCollected = true;
            return Energy;
        }
    }
}
