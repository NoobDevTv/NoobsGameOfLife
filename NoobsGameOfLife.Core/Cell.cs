using System;
using System.Collections.Generic;
using System.Text;

namespace NoobsGameOfLife.Core
{
    public class Cell
    {
        public Location Position { get; set; }
        public bool IsAlive { get; private set; }

        public int Energy
        {
            get => energy; private set
            {
                IsAlive = value > 0;
                energy = value;
            }
        }

        private readonly Random random;
        private int energy;


        public Cell(int seed)
        {
            random = new Random(seed);
            Position = new Location(100, 100);
            energy = 10000;
            IsAlive = true;

        }

        public void Update(Simulation simulation)
        {
            var x = Position.X + random.Next(-5, 6);
            var y = Position.Y + random.Next(-5, 6);

            if (x >= simulation.Width)
                x = simulation.Width;

            if (y >= simulation.Height)
                y = simulation.Height;

            if (x <= 0)
                x = 0;

            if (y <= 0)
                y = 0;

            Energy -= Math.Abs(Position.X - x) + Math.Abs(Position.Y - y);

            Position = new Location(x, y);            
        }

        internal void TryCollect(Nutrient nutrient)
        {
            var collide = Position.X <= nutrient.Position.X && (Position.X + 10) >= nutrient.Position.X &&
                          Position.Y <= nutrient.Position.Y && (Position.Y + 10) >= nutrient.Position.Y;

            if (!collide)
                return;

            energy += nutrient.Collect();
            //10
        }
    }
}
