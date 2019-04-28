using NoobsGameOfLife.Core;
using System;
using System.Collections.Generic;
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

        public MainForm()
        {
            dispatcher = Dispatcher.CurrentDispatcher;

            InitializeComponent();
            simpleBinding = new Dictionary<string, Action<string>>();
            statItems = new Dictionary<string, ListViewItem>();
            simulation = new Simulation(500, 500);
            renderControl.Simulation = simulation;
            Subscribe();
            simulation.Start();
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
            simpleBinding.Add(nameof(Simulation.CellInfos), (p) => statItems[p].Text = "Cells: " + simulation.CellInfos.Length);

            listItem = new ListViewItem();
            statItems.Add(nameof(Simulation.NutrientInfos), listItem);
            simpleBinding.Add(nameof(Simulation.NutrientInfos), (p) => statItems[p].Text = "Nutrients: " + simulation.NutrientInfos.Length);

            simulation.PropertyChanged += (s, e) => Task.Run(() => Dispatch(simpleBinding[e.PropertyName], e.PropertyName));

            foreach (ListViewItem item in statItems.Values)
                statsView.Items.Add(item);
        }

        private void Dispatch<T>(Action<T> action, T param)
        {
            if (DispatchRequired)
                dispatcher.Invoke(action, param);
            else
                action(param);
        }
    }
}
