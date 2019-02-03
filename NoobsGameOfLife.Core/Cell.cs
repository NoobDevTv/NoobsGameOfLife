using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoobsGameOfLife.Core
{
    public class Cell : IVisible
    {
        public Location Position { get; set; }
        public bool IsAlive { get; private set; }

        public Gender Sex { get; set; }

        public int Energy
        {
            get => energy; private set
            {
                IsAlive = value > 0;
                energy = value;
            }
        }

        private readonly Random random;
        private readonly int seed;
        private int energy;

        private IVisible currentTarget;

        private int timeToNextSexyTimeWithOtherCellUlala = 1000;

        public Cell(int seed)
        {
            random = new Random(seed);
            this.seed = seed;
            Position = new Location(100, 100);
            energy = 1000;
            IsAlive = true;
            Sex = (Gender)random.Next(1, 3);
        }

        public void Update(Simulation simulation)
        {
            if (timeToNextSexyTimeWithOtherCellUlala > 0)
                timeToNextSexyTimeWithOtherCellUlala -= 1;

            int x = 0;
            int y = 0;

            if (currentTarget is Nutrient nutrient)
            {
                if (nutrient.IsCollected)
                    currentTarget = null;
            }

            if (currentTarget == null)
            {
                x = random.Next(-5, 6);
                y = random.Next(-5, 6);
            }
            else
            {
                var count = random.Next(0, 6);

                var way = currentTarget.Position - Position;
                x += way.X > 0 ? Math.Min(count, way.X) : Math.Max(-count, way.X);
                y += way.Y > 0 ? Math.Min(count, way.Y) : Math.Max(-count, way.Y);
            }

            if (x + Position.X >= simulation.Width)
                x = simulation.Width - Position.X;

            if (y + Position.Y >= simulation.Height)
                y = simulation.Height - Position.Y;

            if (Position.X + x <= 0)
                x = -Position.X;

            if (Position.Y + y <= 0)
                y = -Position.Y;

            Energy -= Math.Abs(x) + Math.Abs(y);

            Position += new Location(x, y);
        }

        internal bool TryBreeding(Cell otherCell, out Cell child)
        {
            child = null;

            if (!Collide(otherCell.Position))
                return false;

            if (timeToNextSexyTimeWithOtherCellUlala > 0)
            {
                currentTarget = null;
                return false;
            }

            if (otherCell.GetFertility() == GetFertility())
            {
                child = new Cell(seed + otherCell.seed) { };
                timeToNextSexyTimeWithOtherCellUlala = 1000;
                otherCell.timeToNextSexyTimeWithOtherCellUlala = 1000;
                currentTarget = null;
                otherCell.currentTarget = null;
            }
            else
            {
                timeToNextSexyTimeWithOtherCellUlala = 300;
                otherCell.timeToNextSexyTimeWithOtherCellUlala = 300;
                return false;
            }


            return true;
        }

        internal void TryCollect(Nutrient nutrient)
        {
            if (!Collide(nutrient.Position))
                return;

            energy += nutrient.Collect();
            //10
        }

        private bool Collide(Location otherLocation)
            => Position.X <= otherLocation.X && (Position.X + 10) >= otherLocation.X &&
               Position.Y <= otherLocation.Y && (Position.Y + 10) >= otherLocation.Y;

        internal void Sees(IList<IVisible> visibles)
        {
            if (currentTarget != null || visibles.Count == 0)
                return;

            currentTarget = visibles.Where(x => x is Nutrient).OrderByDescending(x => ((Nutrient)x).Energy / (float)(int)(x.Position - Position)).FirstOrDefault();//. FirstOrDefault(v => v is Nutrient);

            if (currentTarget == null && timeToNextSexyTimeWithOtherCellUlala == 0)
                currentTarget = visibles.Where(x => ((Cell)x).Sex != Sex).OrderByDescending(x => ((Cell)x).Energy / (float)(int)(x.Position - Position)).FirstOrDefault(); ;
        }

        private int GetFertility()
        {
            return random.Next(0, 500);
        }

        public enum Gender
        {
            Unknown,
            Male,
            Female
        }
    }
}
