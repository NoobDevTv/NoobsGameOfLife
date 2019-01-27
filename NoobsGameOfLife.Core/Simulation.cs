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
        private readonly List<Cell> cells;
        private readonly List<Nutrient> nutrients;
        private CancellationTokenSource cancellationTokenSource;

        public int Width { get; private set; }
        public int Height { get; private set; }

        private readonly Random random;

        public Simulation(int width, int height)
        {

            Width = width;
            Height = height;

            random = new Random();

            cells = new List<Cell>();
            nutrients = new List<Nutrient>();

            for (int i = 0; i < 1000; i++)
                cells.Add(new Cell(random.Next()));

            for (int i = 0; i < 500; i++)
                nutrients.Add(new Nutrient(random));
        }

        public IEnumerable<CellInfo> GetCellInfos()
        {
            foreach (var cell in cells.ToArray())
                yield return new CellInfo(cell);
        }

        public IEnumerable<NutrientInfo> GetNutrientInfos()
        {
            foreach (var nutrient in nutrients.Where(n => !n.IsCollected).ToArray())
                yield return new NutrientInfo(nutrient);
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
                foreach (var cell in cells)
                {
                    cell.Update(this);

                    foreach (var nutrient in nutrients.Where(n => !n.IsCollected))
                    {
                        cell.TryCollect(nutrient);
                    }
                }

                foreach (var cell in cells.Where(c => !c.IsAlive).ToArray())
                {
                    nutrients.Add(new Nutrient(cell.Position, random));
                    cells.Remove(cell);
                }

                Thread.Sleep(16);
            }
        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
        }

    }
}
