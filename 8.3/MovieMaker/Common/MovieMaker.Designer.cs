namespace MovieMaker
{
    partial class MovieMaker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MovieMaker));
            this.m_Timer = new System.Windows.Forms.Timer(this.components);
            this.m_Status = new System.Windows.Forms.Label();
            this.m_ImageList = new System.Windows.Forms.ImageList(this.components);
            this.m_CurrentForm = new System.Windows.Forms.PictureBox();
            this.m_Ok = new System.Windows.Forms.Label();
            this.m_Options = new System.Windows.Forms.Label();
            this.m_Border = new System.Windows.Forms.Panel();
            this.m_BossMode = new System.Windows.Forms.Panel();
            this.m_Fps = new System.Windows.Forms.Label();
            this.m_BossModeText = new System.Windows.Forms.Label();
            this.m_TalentsLabel = new System.Windows.Forms.Label();
            this.m_StatusLabel = new System.Windows.Forms.Label();
            this.m_BindingLabel = new System.Windows.Forms.Label();
            this.m_BasePanel = new System.Windows.Forms.Panel();
            this.m_ToolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.m_CurrentForm)).BeginInit();
            this.m_Border.SuspendLayout();
            this.m_BossMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_Timer
            // 
            this.m_Timer.Interval = 10;
            this.m_Timer.Tick += new System.EventHandler(this.m_Timer_Tick);
            // 
            // m_Status
            // 
            this.m_Status.AutoSize = true;
            this.m_Status.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_Status.Location = new System.Drawing.Point(110, 7);
            this.m_Status.Name = "m_Status";
            this.m_Status.Size = new System.Drawing.Size(0, 13);
            this.m_Status.TabIndex = 4;
            // 
            // m_ImageList
            // 
            this.m_ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ImageList.ImageStream")));
            this.m_ImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.m_ImageList.Images.SetKeyName(0, "question.png");
            this.m_ImageList.Images.SetKeyName(1, "cat.png");
            this.m_ImageList.Images.SetKeyName(2, "bear.png");
            // 
            // m_CurrentForm
            // 
            this.m_CurrentForm.Location = new System.Drawing.Point(14, 13);
            this.m_CurrentForm.Name = "m_CurrentForm";
            this.m_CurrentForm.Size = new System.Drawing.Size(40, 40);
            this.m_CurrentForm.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.m_CurrentForm.TabIndex = 5;
            this.m_CurrentForm.TabStop = false;
            // 
            // m_Ok
            // 
            this.m_Ok.AutoSize = true;
            this.m_Ok.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_Ok.Location = new System.Drawing.Point(110, 24);
            this.m_Ok.Name = "m_Ok";
            this.m_Ok.Size = new System.Drawing.Size(0, 13);
            this.m_Ok.TabIndex = 7;
            // 
            // m_Options
            // 
            this.m_Options.AutoSize = true;
            this.m_Options.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_Options.Location = new System.Drawing.Point(110, 41);
            this.m_Options.Name = "m_Options";
            this.m_Options.Size = new System.Drawing.Size(0, 13);
            this.m_Options.TabIndex = 8;
            // 
            // m_Border
            // 
            this.m_Border.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_Border.Controls.Add(this.m_BossMode);
            this.m_Border.Controls.Add(this.m_TalentsLabel);
            this.m_Border.Controls.Add(this.m_StatusLabel);
            this.m_Border.Controls.Add(this.m_BindingLabel);
            this.m_Border.Controls.Add(this.m_Options);
            this.m_Border.Controls.Add(this.m_Status);
            this.m_Border.Controls.Add(this.m_Ok);
            this.m_Border.Controls.Add(this.m_BasePanel);
            this.m_Border.Location = new System.Drawing.Point(1, 1);
            this.m_Border.Name = "m_Border";
            this.m_Border.Padding = new System.Windows.Forms.Padding(3);
            this.m_Border.Size = new System.Drawing.Size(180, 86);
            this.m_Border.TabIndex = 9;
            // 
            // m_BossMode
            // 
            this.m_BossMode.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.m_BossMode.Controls.Add(this.m_Fps);
            this.m_BossMode.Controls.Add(this.m_BossModeText);
            this.m_BossMode.Location = new System.Drawing.Point(0, 63);
            this.m_BossMode.Name = "m_BossMode";
            this.m_BossMode.Size = new System.Drawing.Size(179, 21);
            this.m_BossMode.TabIndex = 13;
            // 
            // m_Fps
            // 
            this.m_Fps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_Fps.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_Fps.Location = new System.Drawing.Point(95, 0);
            this.m_Fps.Name = "m_Fps";
            this.m_Fps.Size = new System.Drawing.Size(81, 21);
            this.m_Fps.TabIndex = 8;
            this.m_Fps.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_BossModeText
            // 
            this.m_BossModeText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_BossModeText.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_BossModeText.Location = new System.Drawing.Point(2, 0);
            this.m_BossModeText.Name = "m_BossModeText";
            this.m_BossModeText.Size = new System.Drawing.Size(112, 21);
            this.m_BossModeText.TabIndex = 7;
            this.m_BossModeText.Text = "Life: Dead";
            this.m_BossModeText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_TalentsLabel
            // 
            this.m_TalentsLabel.AutoSize = true;
            this.m_TalentsLabel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_TalentsLabel.Location = new System.Drawing.Point(60, 41);
            this.m_TalentsLabel.Name = "m_TalentsLabel";
            this.m_TalentsLabel.Size = new System.Drawing.Size(52, 13);
            this.m_TalentsLabel.TabIndex = 11;
            this.m_TalentsLabel.Text = "Talents:";
            // 
            // m_StatusLabel
            // 
            this.m_StatusLabel.AutoSize = true;
            this.m_StatusLabel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_StatusLabel.Location = new System.Drawing.Point(60, 7);
            this.m_StatusLabel.Name = "m_StatusLabel";
            this.m_StatusLabel.Size = new System.Drawing.Size(48, 13);
            this.m_StatusLabel.TabIndex = 9;
            this.m_StatusLabel.Text = "Status:";
            // 
            // m_BindingLabel
            // 
            this.m_BindingLabel.AutoSize = true;
            this.m_BindingLabel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_BindingLabel.Location = new System.Drawing.Point(60, 24);
            this.m_BindingLabel.Name = "m_BindingLabel";
            this.m_BindingLabel.Size = new System.Drawing.Size(54, 13);
            this.m_BindingLabel.TabIndex = 10;
            this.m_BindingLabel.Text = "Binding:";
            // 
            // m_BasePanel
            // 
            this.m_BasePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_BasePanel.Location = new System.Drawing.Point(-1, -1);
            this.m_BasePanel.Name = "m_BasePanel";
            this.m_BasePanel.Padding = new System.Windows.Forms.Padding(3);
            this.m_BasePanel.Size = new System.Drawing.Size(180, 64);
            this.m_BasePanel.TabIndex = 12;
            // 
            // m_ToolTip
            // 
            this.m_ToolTip.ToolTipTitle = "Micro Movie";
            // 
            // MovieMaker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(182, 88);
            this.Controls.Add(this.m_CurrentForm);
            this.Controls.Add(this.m_Border);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MovieMaker";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Micro Movie";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MicroMovie_FormClosing);
            this.Load += new System.EventHandler(this.MicroMovie_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MicroMovie_KeyPress);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MicroMovie_MouseClick);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.MicroMovie_MouseDoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MicroMovie_MouseDown);
            this.MouseLeave += new System.EventHandler(this.MicroMovie_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MicroMovie_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MicroMovie_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.m_CurrentForm)).EndInit();
            this.m_Border.ResumeLayout(false);
            this.m_Border.PerformLayout();
            this.m_BossMode.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer m_Timer;
        private System.Windows.Forms.Label m_Status;
        private System.Windows.Forms.ImageList m_ImageList;
        private System.Windows.Forms.PictureBox m_CurrentForm;
        private System.Windows.Forms.Label m_Ok;
        private System.Windows.Forms.Label m_Options;
        private System.Windows.Forms.Panel m_Border;
        private System.Windows.Forms.Label m_StatusLabel;
        private System.Windows.Forms.Label m_BindingLabel;
        private System.Windows.Forms.Label m_TalentsLabel;
        private System.Windows.Forms.Panel m_BasePanel;
        private System.Windows.Forms.ToolTip m_ToolTip;
        private System.Windows.Forms.Panel m_BossMode;
        private System.Windows.Forms.Label m_BossModeText;
        private System.Windows.Forms.Label m_Fps;
    }
}

