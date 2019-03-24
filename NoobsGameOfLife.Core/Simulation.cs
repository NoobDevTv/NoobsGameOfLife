using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoobsGameOfLife.Core
{
    public class Simulation
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public CellInfo[] CellInfos { get; set; }
        public NutrientInfo[] NutrientInfos { get; set; }

        private readonly List<Cell> cells;
        private readonly List<Nutrient> nutrients;
        private readonly NutrientFactory nutrientFactory;
        private readonly CellFactory cellFactory;
        private CancellationTokenSource cancellationTokenSource;

        public Simulation(int width, int height)
        {
            Width = width;
            Height = height;
                        
            cells = new List<Cell>();
            nutrients = new List<Nutrient>();
            nutrientFactory = new NutrientFactory(width, height);
            cellFactory = new CellFactory(width, height);

            cells.AddRange(cellFactory.GenerateCells(30));
            nutrients.AddRange(nutrientFactory.GenerateNutrients(2000));
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

                    cell.Sees(CellSees(cell));

                    foreach (var nutrient in nutrients.Where(n => !n.IsCollected))
                    {
                        cell.TryCollect(nutrient);
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

                Thread.Sleep(16);
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
    }
}
