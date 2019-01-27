using NoobsGameOfLife.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NoobsGameOfLife
{
    public partial class MainForm : Form
    {
        private Simulation simulation;

        public MainForm()
        {            
            InitializeComponent();
            simulation = new Simulation(500, 500);
            renderControl.Simulation = simulation;
            simulation.Start();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            simulation.Stop();
            base.OnFormClosing(e);
        }
    }
}
