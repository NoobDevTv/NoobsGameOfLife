using NoobsGameOfLife.Core;
using NoobsGameOfLife.Core.Information;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace NoobsGameOfLife
{
    public partial class MainForm : Form
    {
        public bool DispatchRequired => dispatcher.Thread.ManagedThreadId != Thread.CurrentThread.ManagedThreadId;

        private readonly Simulation simulation;
        private readonly Dictionary<string, Action<string>> simpleBinding;
        private readonly Dictionary<string, ListViewItem> statItems;
        private readonly Dispatcher dispatcher;

        private readonly Task refreshTask;
        private readonly BindingList<CellInfo> dataGridItems;

        public MainForm()
        {
            dispatcher = Dispatcher.CurrentDispatcher;

            InitializeComponent();

            dataGridItems = new BindingList<CellInfo>();

            cellDataGrid.AutoGenerateColumns = true;
            cellDataGrid.ReadOnly = true;
            cellDataGrid.AllowUserToAddRows = false;
            cellDataGrid.DataSource = dataGridItems;
            //dataGridItems.Add(new DataGridItem() { Name = "Test" });

            simpleBinding = new Dictionary<string, Action<string>>();
            statItems = new Dictionary<string, ListViewItem>();
            simulation = new Simulation(512, 512);
            speedTrackBar.Value = simulation.SleepTime;
            renderControl.Simulation = simulation;

            Subscribe();
            renderControl.SelectionChanged += RenderControlSelectionChanged;

            simulation.Start();
            refreshTask = new Task(async () => await UpdateUi(), TaskCreationOptions.LongRunning);
            refreshTask.Start();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            simulation.Stop();
            base.OnFormClosing(e);
        }

        private void Subscribe()
        {
            var listItem = new ListViewItem();
            statItems.Add(nameof(Simulation.CellInfos), listItem);
            listItem = new ListViewItem();
            statItems.Add(nameof(Simulation.NutrientInfos), listItem);

            listItem = new ListViewItem();
            statItems.Add("Energy", listItem);


            simulation.PropertyChanged += (s, e) => Task.Run(() =>
            {
                if (simpleBinding.TryGetValue(e.PropertyName, out var binding))
                    Dispatch(binding, e.PropertyName);
            });

            foreach (ListViewItem item in statItems.Values)
                statsView.Items.Add(item);
        }

        private void RenderControlSelectionChanged(object sender, IEnumerable<CellInfo> cellInfos)
        {
            dataGridItems.Clear();

            if (cellInfos == null)
                return;

            foreach (var cell in cellInfos)
                dataGridItems.Add(cell);

        }

        private void Dispatch<T>(Action<T> action, T param)
        {
            if (DispatchRequired)
                dispatcher.Invoke(action, param);
            else
                action(param);
        }

        private async Task DispatchAsync(Action action)
        {
            if (DispatchRequired)
                await dispatcher.InvokeAsync(action);
            else
                action();
        }

        private void SpeedTrackBarValueChanged(object sender, EventArgs e)
        {
            if (sender is TrackBar tb)
                simulation.SleepTime = tb.Value;
        }

        private async Task UpdateUi()
        {
            while (true)
            {
                var cellInfos = simulation.CellInfos?.Count();
                var nutrientInfos = simulation.NutrientInfos?.Count();
                var energyResult = simulation.CellInfos?.Sum(x => x.Energy)
                    + simulation.NutrientInfos?.Sum(x => x.Carbon * 10 + x.Hydrogen * 10 + x.Oxygen * 10);

                IEnumerable<CellInfo> cells = null; 
                
                if (renderControl.SelectedCells != null)
                    cells = simulation.CellInfos?.Where(c => renderControl.SelectedCells.Contains(c));

                await DispatchAsync(() =>
                {
                    statItems[nameof(Simulation.NutrientInfos)].Text = $"Nutrients: {nutrientInfos}";
                    statItems[nameof(Simulation.CellInfos)].Text = $"Cells: {cellInfos}";
                    statItems["Energy"].Text = $"Energy: {energyResult}";
                    RenderControlSelectionChanged(this, cells);
                });

                await Task.Delay(250);
            }

        }


        private class DataGridItem
        {
            public double Energy { get; }
            public Location Position { get; }

            public DataGridItem(CellInfo cellInfo)
            {
                Energy = cellInfo.Energy;
                Position = cellInfo.Position;
            }
        }
    }
}
