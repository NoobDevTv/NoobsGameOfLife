using NoobsGameOfLife.Core.Physics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NoobsGameOfLife.Core.Biology
{
    public sealed class Genom
    {
        public const byte LENGTH = 4;
        public const byte SEQUENCE_LENGTH = LENGTH / 2;

        private static readonly Random random;

        static Genom()
        {
            random = new Random();
        }

        public Chromosome this[int index]
        {
            get
            {
                if (index >= SEQUENCE_LENGTH)
                    return SequenceB[index - SEQUENCE_LENGTH];
                else
                    return SequenceA[index];
            }
        }
        
        public Chromosome[] SequenceA { get; }
        public Chromosome[] SequenceB { get; }
        public Gender Sex { get; }

        public int Seed { get; }
        
        public Genom(Chromosome[] sequenceA, Chromosome[] sequenceB)
        {
            SequenceA = sequenceA;
            SequenceB = sequenceB;

            Sex = GetGender();
            Seed = GetSeed();
        }

        public IEnumerable<T> Get<T>() where T : Chromosome 
            => SequenceA.Where(c => c is T).Concat(SequenceB.Where(c => c is T)).Select(c => c as T);

        public Genom Copy()
            => new Genom(SequenceA.ToArray(), SequenceB.ToArray());

        public Chromosome[] Split()
        {
            var target = new Chromosome[SEQUENCE_LENGTH];

            for (int i = 0; i < SEQUENCE_LENGTH; i++)
                target[i] = (random.Next(0, 2) == 1) ? SequenceA[i] : SequenceB[i];

            return target;
        }

        public override string ToString()
            => $"{Sex} | {Seed}";

        private Gender GetGender()
        {
            //X + X => Female
            //Y + Y => Unknown
            //Y + X => Male
            //X + Y => Male

            if (SequenceA.Any(c => c is XChromosome) && SequenceB.Any(c => c is XChromosome))
            {
                return Gender.Female;
            }
            else if ((SequenceA.Any(c => c is YChromosome) && SequenceB.Any(c => c is XChromosome)) ||
                    (SequenceA.Any(c => c is XChromosome) && SequenceB.Any(c => c is YChromosome)))
            {
                return Gender.Male;
            }

            //(SequenceA.Any(c => c is YChromosome) && SequenceB.Any(c => c is YChromosome))
            return Gender.Unknown;
        }

        private int GetSeed()
        {
            var aCell = SequenceA.First(c => c is GenderChromosome) as GenderChromosome;
            var bCell = SequenceB.First(c => c is GenderChromosome) as GenderChromosome;
            return aCell.Seed + bCell.Seed;
        }

        public static Genom operator +(Genom female, Genom male)
        {
            //var foodDigestibilities = new Dictionary<Element, float>();

            //foreach (var element in male.FoodDigestibility)
            //    foodDigestibilities.Add(element.Key, (element.Value + female.FoodDigestibility[element.Key]) / 2);

            //var foodDigestibility = foodDigestibilities.OrderBy(x => x.Value);

            //var entry = foodDigestibility.First();
            //foodDigestibilities[entry.Key] -= 0.5f;
            //entry = foodDigestibility.Last();
            //foodDigestibilities[entry.Key] += 0.5f;

            return new Genom(female.Split(), male.Split());
        }

        public enum Gender
        {
            Unknown,
            Male,
            Female
        }
    }
}