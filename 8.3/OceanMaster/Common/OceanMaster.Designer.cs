namespace OceanMaster
{
    partial class OceanMaster
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OceanMaster));
            this.m_Timer = new System.Windows.Forms.Timer(this.components);
            this.pnlDraw = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pnlDraw.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_Timer
            // 
            this.m_Timer.Interval = 10;
            this.m_Timer.Tick += new System.EventHandler(this.m_Timer_Tick);
            // 
            // pnlDraw
            // 
            this.pnlDraw.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnlDraw.Controls.Add(this.panel2);
            this.pnlDraw.Controls.Add(this.panel1);
            this.pnlDraw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDraw.Location = new System.Drawing.Point(0, 0);
            this.pnlDraw.Name = "pnlDraw";
            this.pnlDraw.Size = new System.Drawing.Size(800, 800);
            this.pnlDraw.TabIndex = 0;
            this.pnlDraw.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlDraw_Paint);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel1.Location = new System.Drawing.Point(100, 400);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(3, 3);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel2.Location = new System.Drawing.Point(700, 400);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(3, 3);
            this.panel2.TabIndex = 1;
            // 
            // OceanMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(800, 800);
            this.Controls.Add(this.pnlDraw);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OceanMaster";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Micro Movie";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OceanMaster_FormClosing);
            this.Load += new System.EventHandler(this.OceanMaster_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OceanMaster_KeyPress);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OceanMaster_MouseDoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OceanMaster_MouseDown);
            this.MouseLeave += new System.EventHandler(this.OceanMaster_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OceanMaster_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OceanMaster_MouseUp);
            this.pnlDraw.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer m_Timer;
        private System.Windows.Forms.Panel pnlDraw;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}

