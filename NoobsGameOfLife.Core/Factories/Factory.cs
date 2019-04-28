using System;
using System.Collections.Generic;

namespace NoobsGameOfLife.Core.Factories
{
    public abstract class Factory
    {
        public Type Type { get; }
        
        protected readonly int width;
        protected readonly int height;
        protected readonly Random random;

        public Factory(int width, int height, Type type)
        {
            this.width = width;
            this.height = height;
            random = new Random();
            Type = type;
        }

        public IEnumerable<T> Generate<T>(int count)
        {
            for (var i = 0; i < count; i++)
                yield return (T)GetNew();
        }

        public abstract object GetNew();

    }
    public abstract class Factory<T> : Factory
    {
        public Factory(int width, int height) : base(width, height, typeof(T))
        {

        }

        public IEnumerable<T> Generate(int count)
        {
            for (var i = 0; i < count; i++)
                yield return GetNext();
        }

        public override object GetNew()
            => GetNext();

        public abstract T GetNext();
    }
}