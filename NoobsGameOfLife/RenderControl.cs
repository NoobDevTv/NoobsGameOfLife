using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NoobsGameOfLife.Core;

namespace NoobsGameOfLife
{
    public partial class RenderControl : UserControl
    {
        public Simulation Simulation { get; set; }

        private readonly Timer timer;
        private int max;

        public RenderControl()
        {
            InitializeComponent();
            timer = new Timer();
            timer.Tick += (s, e) => Invalidate();
            timer.Interval = 1;
            timer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Color.CornflowerBlue);

            if (Simulation == null)
                return;

            using (var pen = new Pen(Color.Red))
            {
                e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, Simulation.Width, Simulation.Height));
            }

            using (var brush = new SolidBrush(Color.DarkGreen))
            {
                foreach (var nutrienInfo in Simulation.NutrientInfos)
                {
                    e.Graphics.FillRectangle(brush, new Rectangle(nutrienInfo.Position.X, nutrienInfo.Position.Y, 5, 5));
                }
            }
            

            foreach (var cellInfo in Simulation.CellInfos)
            {
                if (max < cellInfo.Energy)
                    max = cellInfo.Energy;

                var redValue = cellInfo.Energy * 255 / max;

                if (redValue < 0)
                    redValue = 0;
                else if (redValue > 255)
                    redValue = 255;

                using (var brush = new SolidBrush(Color.FromArgb(redValue, 0, 0)))
                {
                    e.Graphics.FillRectangle(brush, new Rectangle(cellInfo.Position.X, cellInfo.Position.Y, 10, 10));
                }
            }

            base.OnPaint(e);
        }
    }
}
