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
using System.Diagnostics;
using NoobsGameOfLife.Core.Information;

namespace NoobsGameOfLife
{
    public partial class RenderControl : UserControl
    {
        public Simulation Simulation { get; set; }
        public Rectangle SelectionRectangle { get; private set; }
        public IReadOnlyList<CellInfo> SelectedCells { get; private set; }

        public event EventHandler<IEnumerable<CellInfo>> SelectionChanged;

        private readonly Timer timer;
        private double max;
        private bool mouseDown;

        private Point mouseStartLocation;
        private Point mouseLocation;

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
                e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, Simulation.Width, Simulation.Height));


            foreach (var nutrienInfo in Simulation.NutrientInfos)
            {
                using (var brush = new SolidBrush(Color.FromArgb(nutrienInfo.Hydrogen * 10, nutrienInfo.Carbon * 10, nutrienInfo.Oxygen * 10)))
                    e.Graphics.FillRectangle(brush, new Rectangle(nutrienInfo.Position.X, nutrienInfo.Position.Y, 5, 5));
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

                using (var brush = new SolidBrush(Color.FromArgb((byte)redValue, 0, 0)))
                    e.Graphics.FillRectangle(brush, new Rectangle(cellInfo.Position.X, cellInfo.Position.Y, 10, 10));
            }

            if (SelectedCells != null)
            {
                foreach (var selectedCell in Simulation.CellInfos?.Where(c => SelectedCells.Contains(c)))
                {
                    using (var pen = new Pen(Color.Yellow, 2))
                        e.Graphics.DrawRectangle(pen, new Rectangle(selectedCell.Position.X, selectedCell.Position.Y, 10, 10));
                }
            }

            if (mouseDown)
                DrawSelectionRectangle(e);

            base.OnPaint(e);
        }

        private void DrawSelectionRectangle(PaintEventArgs e)
        {
            using var pen = new Pen(Color.White, 3);

            int relativeWidth = Math.Abs(mouseStartLocation.X - mouseLocation.X);
            int relativeHeight = Math.Abs(mouseStartLocation.Y - mouseLocation.Y);
            int startX = Math.Min(mouseLocation.X, mouseStartLocation.X);
            int startY = Math.Min(mouseLocation.Y, mouseStartLocation.Y);

            SelectionRectangle = new Rectangle(new Point(startX, startY), new Size(relativeWidth, relativeHeight));

            e.Graphics.DrawRectangle(pen, SelectionRectangle);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (mouseDown)
            {
                SelectedCells = Simulation.CellInfos.Where(c => SelectionRectangle.Contains(c.Position.X, c.Position.Y)).ToList();
                mouseDown = false;
                SelectionChanged?.Invoke(this, SelectedCells);
            }

            base.OnMouseUp(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            mouseDown = true;
            mouseStartLocation = e.Location;

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            mouseLocation = e.Location;
            base.OnMouseMove(e);
        }
    }
}
