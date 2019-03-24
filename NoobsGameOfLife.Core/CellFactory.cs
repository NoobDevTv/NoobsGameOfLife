using System;
using System.Collections.Generic;
using System.Text;

namespace NoobsGameOfLife.Core
{
    public class CellFactory
    {
        private readonly DNA originDna;
        private readonly int width;
        private readonly int height;

        public CellFactory(int width, int height)
        {
            this.width = width;
            this.height = height;

            originDna = new DNA
            {
                FoodDigestibility = new Dictionary<Element, int>
                {
                    [Element.Carbon] = 0,
                    [Element.Oxygen] = 0,
                    [Element.Hydrogen] = 0
                },
                Saturated = 800,
                MaxEnergy = 1000
            };
        }

        public IEnumerable<Cell> GenerateCells(int count)
        {
            for (int i = 0; i < count; i++)
                yield return new Cell(originDna);
        }
    }
}
