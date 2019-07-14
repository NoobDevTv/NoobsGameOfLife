using System;
using System.Collections.Generic;
using System.Text;

namespace NoobsGameOfLife.Core.Biology
{
    public sealed class YChromosome : GenderChromosome
    {
        public override Chromosome Copy() 
            => new YChromosome();
    }
}
