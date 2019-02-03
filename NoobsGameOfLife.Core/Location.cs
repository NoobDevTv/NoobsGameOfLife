using System;

namespace NoobsGameOfLife.Core
{
    public struct Location
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Location operator +(Location loc1, Location loc2) 
            => new Location(loc1.X + loc2.X, loc1.Y + loc2.Y);

        public static Location operator -(Location loc1, Location loc2) 
            => new Location(loc1.X - loc2.X, loc1.Y - loc2.Y);

        public static explicit operator int(Location loc)
            => Math.Abs(loc.X)+ Math.Abs(loc.Y);

        public override string ToString()
        {
            return $"X:{X} | Y:{Y}";
        }
    }
}