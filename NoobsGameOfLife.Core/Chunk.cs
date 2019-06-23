using NoobsGameOfLife.Core.Biology;
using NoobsGameOfLife.Core.Factories;
using NoobsGameOfLife.Core.Information;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace NoobsGameOfLife.Core
{
    internal class Chunk
    {
        public int Width { get; set; }
        public int Heigth { get; set; }
        public Location Position { get; set; }
        public ConcurrentQueue<HeatInformation> Heat { get; }
        public Factory<Nutrient> NutrientFactory { get; }

        private Nutrient nextNutrient;
        private int neededEnergy;
        private int collectedEnergy;

        public Chunk(int width, int heigth, Location position, Factory<Nutrient> factory)
        {
            Width = width;
            Heigth = heigth;
            Position = position;
            Heat = new ConcurrentQueue<HeatInformation>();
            NutrientFactory = factory;
        }

        internal bool Intersects(HeatInformation heat) 
            => heat.Position.X >= Position.X && heat.Position.X <= Position.X + Width &&
             heat.Position.Y >= Position.Y && heat.Position.Y <= Position.Y + Heigth;

        internal void Update(World world)
        {
            if (nextNutrient == null)
            {
                nextNutrient = NutrientFactory.GetNext();
                neededEnergy = nextNutrient.Elements.Sum(e => e.Value * e.Key.Energy);
            }

            while (Heat.Count > 0 && collectedEnergy < neededEnergy)
            {
                if (Heat.TryDequeue(out HeatInformation heat))
                    collectedEnergy += heat.Value;
                else
                    break;
            }

            if (collectedEnergy >= neededEnergy)
            {
                nextNutrient.Position = new Location(
                    nextNutrient.Position.X % Width + Position.X, 
                    nextNutrient.Position.Y % Heigth + Position.Y);
                world.Nutrients.Add(nextNutrient);
                nextNutrient = null;
                collectedEnergy -= neededEnergy;
            }
        }
        public override string ToString() 
            => $"{Position.ToString()}";
    }
}