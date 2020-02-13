namespace LittleBattles
{
    partial class LittleBattles
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LittleBattles));
            this.m_Timer = new System.Windows.Forms.Timer(this.components);
            this.m_ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.txtLog = new System.Windows.Forms.TextBox();
            this.cbSafariHat = new System.Windows.Forms.CheckBox();
            this.cbPetTreat = new System.Windows.Forms.CheckBox();
            this.cbLesserTreat = new System.Windows.Forms.CheckBox();
            this.pnlDataValid = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlInBattle = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlTurnReady = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlSafariHat = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlLesserTreat = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.pnlPetTreat = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSingle = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnAuto = new System.Windows.Forms.Button();
            this.pnlRevive = new System.Windows.Forms.Panel();
            this.txtReviveCooldown = new System.Windows.Forms.Label();
            this.pnlTeensy = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.pnlDataValid.SuspendLayout();
            this.pnlInBattle.SuspendLayout();
            this.pnlTurnReady.SuspendLayout();
            this.pnlSafariHat.SuspendLayout();
            this.pnlLesserTreat.SuspendLayout();
            this.pnlPetTreat.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlRevive.SuspendLayout();
            this.pnlTeensy.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_Timer
            // 
            this.m_Timer.Interval = 10;
            this.m_Timer.Tick += new System.EventHandler(this.m_Timer_Tick);
            // 
            // m_ToolTip
            // 
            this.m_ToolTip.ToolTipTitle = "Micro Movie";
            // 
            // txtLog
            // 
            this.txtLog.AcceptsReturn = true;
            this.txtLog.Location = new System.Drawing.Point(14, 128);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(259, 276);
            this.txtLog.TabIndex = 4;
            // 
            // cbSafariHat
            // 
            this.cbSafariHat.AutoSize = true;
            this.cbSafariHat.Checked = true;
            this.cbSafariHat.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSafariHat.Location = new System.Drawing.Point(146, 19);
            this.cbSafariHat.Name = "cbSafariHat";
            this.cbSafariHat.Size = new System.Drawing.Size(15, 14);
            this.cbSafariHat.TabIndex = 5;
            this.cbSafariHat.UseVisualStyleBackColor = true;
            // 
            // cbPetTreat
            // 
            this.cbPetTreat.AutoSize = true;
            this.cbPetTreat.Location = new System.Drawing.Point(146, 75);
            this.cbPetTreat.Name = "cbPetTreat";
            this.cbPetTreat.Size = new System.Drawing.Size(15, 14);
            this.cbPetTreat.TabIndex = 7;
            this.cbPetTreat.UseVisualStyleBackColor = true;
            // 
            // cbLesserTreat
            // 
            this.cbLesserTreat.AutoSize = true;
            this.cbLesserTreat.Location = new System.Drawing.Point(146, 46);
            this.cbLesserTreat.Name = "cbLesserTreat";
            this.cbLesserTreat.Size = new System.Drawing.Size(15, 14);
            this.cbLesserTreat.TabIndex = 8;
            this.cbLesserTreat.UseVisualStyleBackColor = true;
            // 
            // pnlDataValid
            // 
            this.pnlDataValid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDataValid.Controls.Add(this.label1);
            this.pnlDataValid.Location = new System.Drawing.Point(14, 12);
            this.pnlDataValid.Name = "pnlDataValid";
            this.pnlDataValid.Size = new System.Drawing.Size(109, 25);
            this.pnlDataValid.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Data Valid";
            // 
            // pnlInBattle
            // 
            this.pnlInBattle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlInBattle.Controls.Add(this.label2);
            this.pnlInBattle.Location = new System.Drawing.Point(14, 40);
            this.pnlInBattle.Name = "pnlInBattle";
            this.pnlInBattle.Size = new System.Drawing.Size(109, 25);
            this.pnlInBattle.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "In Battle";
            // 
            // pnlTurnReady
            // 
            this.pnlTurnReady.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTurnReady.Controls.Add(this.label3);
            this.pnlTurnReady.Location = new System.Drawing.Point(14, 68);
            this.pnlTurnReady.Name = "pnlTurnReady";
            this.pnlTurnReady.Size = new System.Drawing.Size(109, 25);
            this.pnlTurnReady.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Turn Ready";
            // 
            // pnlSafariHat
            // 
            this.pnlSafariHat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSafariHat.Controls.Add(this.label4);
            this.pnlSafariHat.Location = new System.Drawing.Point(164, 12);
            this.pnlSafariHat.Name = "pnlSafariHat";
            this.pnlSafariHat.Size = new System.Drawing.Size(109, 25);
            this.pnlSafariHat.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Safari Hat";
            // 
            // pnlLesserTreat
            // 
            this.pnlLesserTreat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLesserTreat.Controls.Add(this.label5);
            this.pnlLesserTreat.Location = new System.Drawing.Point(164, 40);
            this.pnlLesserTreat.Name = "pnlLesserTreat";
            this.pnlLesserTreat.Size = new System.Drawing.Size(109, 25);
            this.pnlLesserTreat.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Lesser Treat";
            // 
            // pnlPetTreat
            // 
            this.pnlPetTreat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPetTreat.Controls.Add(this.label6);
            this.pnlPetTreat.Location = new System.Drawing.Point(164, 68);
            this.pnlPetTreat.Name = "pnlPetTreat";
            this.pnlPetTreat.Size = new System.Drawing.Size(109, 25);
            this.pnlPetTreat.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(26, 5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Pet Treat";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSingle);
            this.panel1.Controls.Add(this.btnStop);
            this.panel1.Controls.Add(this.btnAuto);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 410);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(286, 45);
            this.panel1.TabIndex = 14;
            // 
            // btnSingle
            // 
            this.btnSingle.Location = new System.Drawing.Point(104, 3);
            this.btnSingle.Name = "btnSingle";
            this.btnSingle.Size = new System.Drawing.Size(82, 32);
            this.btnSingle.TabIndex = 13;
            this.btnSingle.Text = "Once";
            this.btnSingle.UseVisualStyleBackColor = true;
            this.btnSingle.Click += new System.EventHandler(this.btnSingle_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(192, 3);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(82, 32);
            this.btnStop.TabIndex = 12;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnAuto
            // 
            this.btnAuto.Location = new System.Drawing.Point(13, 3);
            this.btnAuto.Name = "btnAuto";
            this.btnAuto.Size = new System.Drawing.Size(85, 32);
            this.btnAuto.TabIndex = 11;
            this.btnAuto.Text = "Auto";
            this.btnAuto.UseVisualStyleBackColor = true;
            this.btnAuto.Click += new System.EventHandler(this.btnAuto_Click);
            // 
            // pnlRevive
            // 
            this.pnlRevive.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRevive.Controls.Add(this.txtReviveCooldown);
            this.pnlRevive.Location = new System.Drawing.Point(164, 96);
            this.pnlRevive.Name = "pnlRevive";
            this.pnlRevive.Size = new System.Drawing.Size(109, 25);
            this.pnlRevive.TabIndex = 15;
            // 
            // txtReviveCooldown
            // 
            this.txtReviveCooldown.AutoSize = true;
            this.txtReviveCooldown.Location = new System.Drawing.Point(13, 5);
            this.txtReviveCooldown.Name = "txtReviveCooldown";
            this.txtReviveCooldown.Size = new System.Drawing.Size(81, 13);
            this.txtReviveCooldown.TabIndex = 0;
            this.txtReviveCooldown.Text = "Revive: 0:00";
            // 
            // pnlTeensy
            // 
            this.pnlTeensy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTeensy.Controls.Add(this.label8);
            this.pnlTeensy.Location = new System.Drawing.Point(14, 96);
            this.pnlTeensy.Name = "pnlTeensy";
            this.pnlTeensy.Size = new System.Drawing.Size(109, 25);
            this.pnlTeensy.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(5, 5);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(99, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "USB Connection";
            // 
            // LittleBattles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(286, 455);
            this.Controls.Add(this.pnlTeensy);
            this.Controls.Add(this.pnlRevive);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlPetTreat);
            this.Controls.Add(this.pnlLesserTreat);
            this.Controls.Add(this.pnlSafariHat);
            this.Controls.Add(this.pnlTurnReady);
            this.Controls.Add(this.pnlInBattle);
            this.Controls.Add(this.cbLesserTreat);
            this.Controls.Add(this.cbPetTreat);
            this.Controls.Add(this.cbSafariHat);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.pnlDataValid);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LittleBattles";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Micro Movie";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LittleBattles_FormClosing);
            this.Load += new System.EventHandler(this.LittleBattles_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LittleBattles_KeyPress);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LittleBattles_MouseDoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LittleBattles_MouseDown);
            this.MouseLeave += new System.EventHandler(this.LittleBattles_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LittleBattles_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LittleBattles_MouseUp);
            this.pnlDataValid.ResumeLayout(false);
            this.pnlDataValid.PerformLayout();
            this.pnlInBattle.ResumeLayout(false);
            this.pnlInBattle.PerformLayout();
            this.pnlTurnReady.ResumeLayout(false);
            this.pnlTurnReady.PerformLayout();
            this.pnlSafariHat.ResumeLayout(false);
            this.pnlSafariHat.PerformLayout();
            this.pnlLesserTreat.ResumeLayout(false);
            this.pnlLesserTreat.PerformLayout();
            this.pnlPetTreat.ResumeLayout(false);
            this.pnlPetTreat.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.pnlRevive.ResumeLayout(false);
            this.pnlRevive.PerformLayout();
            this.pnlTeensy.ResumeLayout(false);
            this.pnlTeensy.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer m_Timer;
        private System.Windows.Forms.ToolTip m_ToolTip;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.CheckBox cbSafariHat;
        private System.Windows.Forms.CheckBox cbPetTreat;
        private System.Windows.Forms.CheckBox cbLesserTreat;
        private System.Windows.Forms.Panel pnlDataValid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlInBattle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnlTurnReady;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pnlSafariHat;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel pnlLesserTreat;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel pnlPetTreat;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSingle;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnAuto;
        private System.Windows.Forms.Panel pnlRevive;
        private System.Windows.Forms.Label txtReviveCooldown;
        private System.Windows.Forms.Panel pnlTeensy;
        private System.Windows.Forms.Label label8;
    }
}

