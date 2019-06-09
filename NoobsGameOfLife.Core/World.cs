using NoobsGameOfLife.Core.Biology;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoobsGameOfLife.Core
{
    public class World
    {
        public int Height { get; }
        public int Width { get; }


        public ConcurrentCollection<Cell> Cells { get; }
        public ConcurrentCollection<Nutrient> Nutrients { get; }

        public World(int width, int height)
        {
            Height = height;
            Width = width;

            Cells = new ConcurrentCollection<Cell>();
            Nutrients = new ConcurrentCollection<Nutrient>();
        }
    }
}
