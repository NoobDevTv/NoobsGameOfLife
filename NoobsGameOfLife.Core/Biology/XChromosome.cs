using System;
using System.Collections.Generic;
using System.Text;

namespace NoobsGameOfLife.Core.Biology
{
    public sealed class XChromosome : GenderChromosome
    {
        public override Chromosome Copy() 
            => new XChromosome();
    }
}
