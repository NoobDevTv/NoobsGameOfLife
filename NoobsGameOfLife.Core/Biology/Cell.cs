using NoobsGameOfLife.Core.Information;
using NoobsGameOfLife.Core.Physics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NoobsGameOfLife.Core.Biology
{
    public class Cell : IVisible
    {
        public Location Position { get; set; }
        public bool IsAlive { get; private set; }

        public DNA.Gender Sex => dna.Sex;

        public double Energy
        {
            get => energy; private set
            {
                IsAlive = value > 0;
                energy = value;
            }
        }

        private double energy;
        private Nutrient digesting;
        private readonly DNA dna;
        private readonly Random random;

        private readonly DNA gamete;

        private IVisible currentTarget;

        private int timeToNextSexyTimeWithOtherCellUlala = 40;

        public Cell(DNA dna) : this(dna, new Location(100, 100))
        {
        }

        public Cell(DNA dna, Location position)
        {
            random = new Random(dna.Seed);
            Position = position;
            this.dna = dna;
            gamete = dna.Copy();
            energy = 1000;
            IsAlive = true;
        }

        public void Update(Simulation simulation)
        {
            if (timeToNextSexyTimeWithOtherCellUlala > 0)
                timeToNextSexyTimeWithOtherCellUlala -= 1;

            if (Digest())
            {
                digesting.PoopOut(Position);
                digesting = null;
            }

            var x = 0;
            var y = 0;

            if (currentTarget is Nutrient nutrient)
            {
                if (nutrient.IsCollected || dna.Saturated < Energy)
                    currentTarget = null;
            }
            else if (currentTarget is Cell cell)
            {
                if (!cell.IsAlive)
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

                Location way = currentTarget.Position - Position;
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

            var value = Math.Abs(x) + Math.Abs(y);
            Energy -= value;

            var heat = new HeatInformation
            {
                Position = Position,
                Value = value
            };

            simulation.Add(heat);
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
                child = this + otherCell;
                timeToNextSexyTimeWithOtherCellUlala = 600;
                otherCell.timeToNextSexyTimeWithOtherCellUlala = 600;
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

        internal bool TryCollect(Nutrient nutrient)
        {
            if (nutrient.IsCollected)
                return false;

            if (energy >= dna.Saturated && digesting != null)
                return false;

            if (!Collide(nutrient.Position))
                return false;

            digesting = nutrient;
            return true;
        }

        private bool Collide(Location otherLocation)
            => Position.X <= otherLocation.X && (Position.X + 10) >= otherLocation.X &&
               Position.Y <= otherLocation.Y && (Position.Y + 10) >= otherLocation.Y;

        internal void Sees(IVisible[] visibles)
        {
            if (currentTarget != null || visibles.Length == 0)
                return;

            currentTarget = visibles
                .OfType<Nutrient>()
                .Where(n => dna.FoodDigestibility.Any(k => k.Value >= 0 && n.Elements.ContainsKey(k.Key)))
                .OrderByDescending(x => (float)(int)(x.Position - Position))
                .FirstOrDefault();

            if (currentTarget == null && timeToNextSexyTimeWithOtherCellUlala == 0)
                currentTarget = visibles.Where(x => ((Cell)x).Sex != Sex).OrderByDescending(x => ((Cell)x).Energy / (float)(int)(x.Position - Position)).FirstOrDefault(); ;
        }

        private int GetFertility()
            => random.Next(0, 10);

        private bool Digest()
        {
            if (digesting != null && energy < dna.MaxEnergy)
            {
                foreach (KeyValuePair<Element, float> digestibilityElement in dna.FoodDigestibility.OrderBy(x => x.Value))
                {
                    if (digesting.Elements.TryGetValue(digestibilityElement.Key, out var amount))
                    {
                        byte count;
                        for (count = 0; count < amount; count++)
                        {
                            energy += digestibilityElement.Key.Energy * digestibilityElement.Value;
                            gamete.FoodDigestibility[digestibilityElement.Key] += 0.001f;

                            if (energy >= dna.MaxEnergy)
                                break;
                        }

                        digesting.Elements[digestibilityElement.Key] -= count;
                    }

                    if (energy >= dna.MaxEnergy)
                        return true;
                }

                if (digesting.Elements.Where(x => dna.FoodDigestibility.ContainsKey(x.Key)).All(x => x.Value <= 0))
                    return true;
            }
            return false;
        }


        public static Cell operator +(Cell male, Cell female)
            => new Cell(male.gamete + female.gamete, female.Position);
    }
}
