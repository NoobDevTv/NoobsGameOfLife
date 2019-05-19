using NoobsGameOfLife.Core.Factories;
using NoobsGameOfLife.Core.Information;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoobsGameOfLife.Core
{
    public class Simulation : INotifyPropertyChanged
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int SleepTime { get; set; }

        public CellInfo[] CellInfos { get => cellInfo; set => SetValue(value, ref cellInfo); }
        public NutrientInfo[] NutrientInfos { get => nutrientInfos; set => SetValue(value, ref nutrientInfos); }

        private readonly List<Cell> cells;
        private readonly List<Nutrient> nutrients;
        private readonly Dictionary<Type, Factory> factories;
        private CancellationTokenSource cancellationTokenSource;

        private CellInfo[] cellInfo;
        private NutrientInfo[] nutrientInfos;

        public event PropertyChangedEventHandler PropertyChanged;

        public Simulation(int width, int height)
        {
            Width = width;
            Height = height;
            SleepTime = 16;
            cells = new List<Cell>();
            nutrients = new List<Nutrient>();

            factories = new Dictionary<Type, Factory>()
            {
                [typeof(Cell)] = new CellFactory(width, height),
                [typeof(Nutrient)] = new NutrientFactory(width, height)
            };

            cells.AddRange(factories[typeof(Cell)].Generate<Cell>(30));
            nutrients.AddRange(factories[typeof(Nutrient)].Generate<Nutrient>(2000));
        }

        public void Start()
        {
            cancellationTokenSource = new CancellationTokenSource();
            new Task(Update, cancellationTokenSource.Token, TaskCreationOptions.LongRunning)
            .Start(TaskScheduler.Default);
        }

        public void Update()
        {
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                foreach (var cell in cells.ToArray())
                {
                    cell.Update(this);

                    //TODO: Use Head elements to recreate new nutrients

                    cell.Sees(CellSees(cell));

                    foreach (var nutrient in nutrients.Where(n => !n.IsCollected))
                    {
                        nutrient.IsCollected = cell.TryCollect(nutrient);
                    }

                    foreach (var otherCell in cells.Where(c => cell.Sex != c.Sex).ToArray())
                    {
                        if (cell.TryBreeding(otherCell, out var child))
                            cells.Add(child);
                    }
                }

                foreach (var cell in cells.Where(c => !c.IsAlive).ToArray())
                {
                    //nutrients.Add(new Nutrient(cell.Position));
                    cells.Remove(cell);
                }

                CellInfos = cells.Select(c => new CellInfo(c)).ToArray();
                NutrientInfos = nutrients.Where(n => !n.IsCollected).Select(n => new NutrientInfo(n)).ToArray();

                Thread.Sleep(SleepTime);
            }
        }

        private IEnumerable<IVisible> CellSees(Cell cell)
            => nutrients
                .Where(n => !n.IsCollected)
                .Select(n => (IVisible)n)
                .Concat(cells)
                .Where(c => Collided(cell, c, 50) && c != cell)
                .OrderBy(v => Math.Abs((int)(cell.Position - v.Position)));

        public void Stop()
        {
            cancellationTokenSource.Cancel();
        }

        private bool Collided(IVisible seeker, IVisible sighted, int seekerRange)
        {
            return seeker.Position.X + seekerRange >= sighted.Position.X
                && seeker.Position.X - seekerRange <= sighted.Position.X
                && seeker.Position.Y + seekerRange >= sighted.Position.Y
                && seeker.Position.Y - seekerRange <= sighted.Position.Y;
        }

        private void SetValue<T>(T value, ref T field, [CallerMemberName]string callername = "")
        {
            if (field != null)
            {
                if (field.Equals(value))
                    return;
            }

            field = value;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callername));
        }
    }
}
