using NoobsGameOfLife.Core.Biology;
using NoobsGameOfLife.Core.Extensions;
using NoobsGameOfLife.Core.Factories;
using NoobsGameOfLife.Core.Gods;
using NoobsGameOfLife.Core.Information;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoobsGameOfLife.Core
{
    public sealed class Simulation : INotifyPropertyChanged
    {
        public World World { get; }
        public God God { get; }

        public int Width => World.Width;
        public int Height => World.Height;
        public int SleepTime { get; set; }

        public IEnumerable<CellInfo> CellInfos { get => cellInfo; set => SetValue(value, ref cellInfo); }
        public IEnumerable<NutrientInfo> NutrientInfos { get => nutrientInfos; set => SetValue(value, ref nutrientInfos); }

        private readonly Dictionary<Type, Factory> factories;
        private CancellationTokenSource cancellationTokenSource;

        private IEnumerable<CellInfo> cellInfo;
        private IEnumerable<NutrientInfo> nutrientInfos;
        private readonly List<Chunk> chunks;

        public event PropertyChangedEventHandler PropertyChanged;

        public Simulation(int width, int height)
        {
            World = new World(width, height);
            God = new DefaultGod();

            SleepTime = 16;

            factories = new Dictionary<Type, Factory>()
            {
                [typeof(Cell)] = new CellFactory(width, height),
                [typeof(Nutrient)] = new NutrientFactory(width, height)
            };

            int chunkWidth = 8;
            int chunkHeight = 8;
            chunks = new List<Chunk>();
            for (int w = 0; w < width; w += chunkWidth)
                for (int h = 0; h < height; h += chunkHeight)
                {
                    chunks.Add(new Chunk(
                        chunkWidth,
                        chunkHeight,
                        new Location(w, h),
                        (Factory<Nutrient>)factories[typeof(Nutrient)]));
                }


            World.Cells.AddRange(factories[typeof(Cell)].Generate<Cell>(30));
            World.Nutrients.AddRange(factories[typeof(Nutrient)].Generate<Nutrient>(2000));
        }

        public void Start()
        {
            cancellationTokenSource = new CancellationTokenSource();
            God.Start(cancellationTokenSource.Token);
            new Task(async () => await Update(cancellationTokenSource.Token),
                cancellationTokenSource.Token, TaskCreationOptions.LongRunning)
            .Start(TaskScheduler.Default);
        }

        public async Task Update(CancellationToken token)
        {
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                Parallel.ForEach(World.Cells, c => { c.Update(this); c.Sees(CellSees(c)); });
                //Parallel.ForEach(World.Cells, c => );


                Parallel.ForEach(World.Cells, (cell) =>
                {
                    foreach (var nutrient in World.Nutrients.Where(n => !n.IsCollected))
                    {
                        nutrient.IsCollected = cell.TryCollect(nutrient);

                        if (nutrient.IsCollected)
                            break;
                    }

                    foreach (var otherCell in World.Cells.Where(c => cell.Sex != c.Sex))
                    {
                        if (cell.TryBreeding(otherCell, out var child))
                        {
                            World.Cells.Add(child);
                            break;
                        }
                    }
                });

                Parallel.ForEach(chunks, (c) => c.Update(World));

                foreach (var cell in World.Cells.Where(c => !c.IsAlive))
                {
                    //nutrients.Add(new Nutrient(cell.Position));
                    World.Cells.Remove(cell);
                }

                if (World.Cells.Count > 0)
                    CellInfos = World.Cells.Select(c => new CellInfo(c));

                if (World.Cells.Count > 0)
                    NutrientInfos = World.Nutrients.Where(n => !n.IsCollected).Select(n => new NutrientInfo(n));

                await Task.Delay(SleepTime, token);
            }
        }

        internal void Add(HeatInformation heat)
            => chunks.First(x => x.Intersects(heat)).Heat.Enqueue(heat);

        public void Stop()
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;
        }

        private IEnumerable<IVisible> CellSees(Cell cell)
            => World.Nutrients
                .Where(n => !n.IsCollected)
                .Select(n => (IVisible)n)
                .Concat(World.Cells)
                .Where(c => Collided(cell, c, 50) && c != cell)
                .OrderBy(v => Math.Abs((int)(cell.Position - v.Position)));

        private bool Collided(IVisible seeker, IVisible sighted, int seekerRange)
        {
            return seeker.Position.X + seekerRange >= sighted.Position.X
                && seeker.Position.X - seekerRange <= sighted.Position.X
                && seeker.Position.Y + seekerRange >= sighted.Position.Y
                && seeker.Position.Y - seekerRange <= sighted.Position.Y;
        }

        private void SetValue<T>(T value, ref T field, [CallerMemberName]string callername = "")
        {
            if (field != null && field.Equals(value))
                return;

            field = value;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callername));
        }
    }
}
