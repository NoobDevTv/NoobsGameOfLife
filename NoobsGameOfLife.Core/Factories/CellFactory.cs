using NoobsGameOfLife.Core.Biology;
using NoobsGameOfLife.Core.Physics;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoobsGameOfLife.Core.Factories
{
    public class CellFactory : Factory<Cell>
    {
        public Factory<Genom> GenomFactory { get; }

        public CellFactory(Factory<Genom> genomFactory, int width, int height) : base(width, height)
        {
            GenomFactory = genomFactory;
        }


        public override Cell GetNext()
            => new Cell(GenomFactory.GetNext());
    }
}
