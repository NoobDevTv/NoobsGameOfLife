namespace NoobsGameOfLife
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.renderControl = new NoobsGameOfLife.RenderControl();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.statsView = new System.Windows.Forms.ListView();
            this.simulationPage = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.speedTrackBar = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.simulationPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.speedTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.renderControl);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(1314, 882);
            this.splitContainer1.SplitterDistance = 728;
            this.splitContainer1.TabIndex = 1;
            // 
            // renderControl
            // 
            this.renderControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.renderControl.Location = new System.Drawing.Point(0, 0);
            this.renderControl.Name = "renderControl";
            this.renderControl.Simulation = null;
            this.renderControl.Size = new System.Drawing.Size(728, 882);
            this.renderControl.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.simulationPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(582, 882);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.statsView);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(574, 853);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Stats";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // statsView
            // 
            this.statsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statsView.HideSelection = false;
            this.statsView.Location = new System.Drawing.Point(3, 3);
            this.statsView.Name = "statsView";
            this.statsView.Size = new System.Drawing.Size(568, 847);
            this.statsView.TabIndex = 0;
            this.statsView.UseCompatibleStateImageBehavior = false;
            // 
            // simulationPage
            // 
            this.simulationPage.Controls.Add(this.label1);
            this.simulationPage.Controls.Add(this.speedTrackBar);
            this.simulationPage.Location = new System.Drawing.Point(4, 25);
            this.simulationPage.Name = "simulationPage";
            this.simulationPage.Size = new System.Drawing.Size(574, 853);
            this.simulationPage.TabIndex = 1;
            this.simulationPage.Text = "Simulation";
            this.simulationPage.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Simulationspeed:";
            // 
            // speedTrackBar
            // 
            this.speedTrackBar.Location = new System.Drawing.Point(136, 16);
            this.speedTrackBar.Maximum = 100;
            this.speedTrackBar.Name = "speedTrackBar";
            this.speedTrackBar.Size = new System.Drawing.Size(151, 56);
            this.speedTrackBar.TabIndex = 0;
            this.speedTrackBar.Value = 16;
            this.speedTrackBar.ValueChanged += new System.EventHandler(this.SpeedTrackBar_ValueChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1314, 882);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.Name = "MainForm";
            this.Text = "Noobs Game of Life";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.simulationPage.ResumeLayout(false);
            this.simulationPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.speedTrackBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private RenderControl renderControl;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListView statsView;
        private System.Windows.Forms.TabPage simulationPage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar speedTrackBar;
    }
}

