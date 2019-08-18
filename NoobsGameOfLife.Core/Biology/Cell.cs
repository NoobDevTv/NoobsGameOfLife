using NoobsGameOfLife.Core.Extensions;
using NoobsGameOfLife.Core.Information;
using NoobsGameOfLife.Core.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace NoobsGameOfLife.Core.Biology
{
    public class Cell : IVisible
    {
        private static ulong NextId => nextId++;
        private static ulong nextId = 0;

        public ulong Id { get;  }

        public Location Position { get; set; }
        public bool IsAlive { get; private set; }

        public Genom.Gender Sex => dna.Sex;

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
        private readonly Genom dna;
        private readonly Random random;
        private readonly SemaphoreSlim semaphore;
        private readonly ushort saturaded;
        private readonly double maxEnergy;
        private readonly Dictionary<Element, float> foodDigestibility;
        private readonly Genom gamete;

        private IVisible currentTarget;

        private int timeToNextSexyTimeWithOtherCellUlala = 40;

        public Cell(Genom dna) : this(dna, new Location(100, 100))
        {
        }

        public Cell(Genom dna, Location position)
        {
            Id = NextId;
            random = new Random(dna.Seed);
            Position = position;
            this.dna = dna;
            gamete = dna.Copy();
            energy = 1000;
            IsAlive = true;
            semaphore = new SemaphoreSlim(1, 1);

            var dChromosome = dna.Get<DChromosome>();
            saturaded = (ushort)dChromosome.Average(c => c.Saturated);
            maxEnergy = dChromosome.Average(c => c.MaxEnergy);
            foodDigestibility = CombineDigestibility(dChromosome);
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
                if (nutrient.IsCollected || saturaded < Energy)
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

        public override string ToString() 
            => $"{Sex} | {IsAlive} | {Position}";

        internal bool TryBreeding(Cell otherCell, out Cell child)
        {
            semaphore.Wait();
            child = null;

            if (!Collide(otherCell.Position))
            {
                semaphore.Release();
                return false;
            }

            if (timeToNextSexyTimeWithOtherCellUlala > 0 ||
                otherCell.Energy - otherCell.Energy / 2 <= 0 ||
                Energy - Energy / 2 <= 0)
            {
                currentTarget = null;
                semaphore.Release();
                return false;
            }

            if (otherCell.GetFertility() == GetFertility())
            {
                child = this + otherCell;
                timeToNextSexyTimeWithOtherCellUlala = 600;
                otherCell.timeToNextSexyTimeWithOtherCellUlala = 600;
                currentTarget = null;
                otherCell.currentTarget = null;
                child.Energy = Energy / 2 + otherCell.Energy / 2;
                otherCell.Energy /= 2;
                Energy /= 2;
            }
            else
            {
                timeToNextSexyTimeWithOtherCellUlala = 300;
                otherCell.timeToNextSexyTimeWithOtherCellUlala = 300;
                semaphore.Release();
                return false;
            }

            semaphore.Release();
            return true;
        }

        internal bool TryCollect(Nutrient nutrient)
        {
            if (nutrient.IsCollected)
                return false;

            if (energy >= saturaded && digesting != null)
                return false;

            if (!Collide(nutrient.Position))
                return false;

            digesting = nutrient;
            return true;
        }

        internal bool Collide(Location otherLocation)
            => Position.X <= otherLocation.X && (Position.X + 10) >= otherLocation.X &&
               Position.Y <= otherLocation.Y && (Position.Y + 10) >= otherLocation.Y;

        internal void Sees(IEnumerable<IVisible> visibles)
        {
            if (currentTarget != null)
                return;

            currentTarget = visibles
                .OfType<Nutrient>()
                .Where(n => foodDigestibility.Any(k => k.Value >= 0 && n.Elements.ContainsKey(k.Key)))
                .OrderByDescending(x => (float)(int)(x.Position - Position))
                .FirstOrDefault();

            if (currentTarget == null && timeToNextSexyTimeWithOtherCellUlala == 0)
                currentTarget = visibles.OfType<Cell>().Where(x => x.Sex != Sex)
                    .OrderByDescending(x => x.Energy / (float)(int)(x.Position - Position)).FirstOrDefault();
        }

        private Dictionary<Element, float> CombineDigestibility(IEnumerable<DChromosome> dChromosome)
        {
            var digestibility = new Dictionary<Element, float>();

            foreach (var chromosome in dChromosome)
            {
                foreach (var element in chromosome.FoodDigestibility)
                {
                    if (digestibility.TryGetValue(element.Key, out float value))
                        digestibility[element.Key] = (value + element.Value) / 2;
                    else
                        digestibility.Add(element.Key, element.Value);
                }
            }

            return digestibility;
        }

        private int GetFertility()
            => random.Next(0, 10);

        private bool Digest()
        {
            if (digesting != null && energy < maxEnergy)
            {
                foreach (KeyValuePair<Element, float> digestibilityElement in foodDigestibility.OrderBy(x => x.Value))
                {
                    if (digesting.Elements.TryGetValue(digestibilityElement.Key, out var amount))
                    {
                        byte count;
                        for (count = 0; count < amount; count++)
                        {
                            energy += digestibilityElement.Key.Energy;

                            if (energy >= maxEnergy)
                                break;
                        }

                        digesting.Elements[digestibilityElement.Key] -= count;
                    }

                    if (energy >= maxEnergy)
                        return true;
                }

                if (digesting.Elements.Where(x => foodDigestibility.ContainsKey(x.Key)).All(x => x.Value <= 0))
                    return true;
            }
            return false;
        }


        public static Cell operator +(Cell male, Cell female)
            => new Cell(female.gamete + male.gamete, female.Position);
    }
}
