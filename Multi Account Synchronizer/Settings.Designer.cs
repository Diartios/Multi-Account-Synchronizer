﻿namespace Multi_Account_Synchronizer
{
    partial class Settings
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.MinilandInviteMax = new System.Windows.Forms.NumericUpDown();
            this.MinilandInviteMin = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.AttackLureMax = new System.Windows.Forms.NumericUpDown();
            this.AttackLureMin = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.ExitMinilandMax = new System.Windows.Forms.NumericUpDown();
            this.ExitMinilandMin = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.UseAmuletMax = new System.Windows.Forms.NumericUpDown();
            this.UseAmuletMin = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinilandInviteMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinilandInviteMin)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AttackLureMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AttackLureMin)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ExitMinilandMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExitMinilandMin)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UseAmuletMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UseAmuletMin)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.MinilandInviteMax, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.MinilandInviteMin, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 7);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(336, 30);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // MinilandInviteMax
            // 
            this.MinilandInviteMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.MinilandInviteMax.Location = new System.Drawing.Point(254, 5);
            this.MinilandInviteMax.Maximum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            0});
            this.MinilandInviteMax.Name = "MinilandInviteMax";
            this.MinilandInviteMax.Size = new System.Drawing.Size(79, 20);
            this.MinilandInviteMax.TabIndex = 48;
            this.MinilandInviteMax.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.MinilandInviteMax.ValueChanged += new System.EventHandler(this.MinilandInviteMax_ValueChanged);
            // 
            // MinilandInviteMin
            // 
            this.MinilandInviteMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.MinilandInviteMin.Location = new System.Drawing.Point(137, 5);
            this.MinilandInviteMin.Maximum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            0});
            this.MinilandInviteMin.Name = "MinilandInviteMin";
            this.MinilandInviteMin.Size = new System.Drawing.Size(78, 20);
            this.MinilandInviteMin.TabIndex = 47;
            this.MinilandInviteMin.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.MinilandInviteMin.ValueChanged += new System.EventHandler(this.MinilandInviteMin_ValueChanged);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Accept Invite Delay";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(227, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 48;
            this.label2.Text = "~";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Controls.Add(this.AttackLureMax, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.AttackLureMin, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label4, 2, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(9, 115);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(336, 30);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // AttackLureMax
            // 
            this.AttackLureMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.AttackLureMax.Location = new System.Drawing.Point(254, 5);
            this.AttackLureMax.Maximum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            0});
            this.AttackLureMax.Name = "AttackLureMax";
            this.AttackLureMax.Size = new System.Drawing.Size(79, 20);
            this.AttackLureMax.TabIndex = 48;
            this.AttackLureMax.Value = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.AttackLureMax.ValueChanged += new System.EventHandler(this.AttackLureMax_ValueChanged);
            // 
            // AttackLureMin
            // 
            this.AttackLureMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.AttackLureMin.Location = new System.Drawing.Point(137, 5);
            this.AttackLureMin.Maximum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            0});
            this.AttackLureMin.Name = "AttackLureMin";
            this.AttackLureMin.Size = new System.Drawing.Size(78, 20);
            this.AttackLureMin.TabIndex = 47;
            this.AttackLureMin.Value = new decimal(new int[] {
            1500,
            0,
            0,
            0});
            this.AttackLureMin.ValueChanged += new System.EventHandler(this.AttackLureMin_ValueChanged);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Attack Lure Delay";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(227, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 48;
            this.label4.Text = "~";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.Controls.Add(this.ExitMinilandMax, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.ExitMinilandMin, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label6, 2, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(9, 43);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(336, 30);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // ExitMinilandMax
            // 
            this.ExitMinilandMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ExitMinilandMax.Location = new System.Drawing.Point(254, 5);
            this.ExitMinilandMax.Maximum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            0});
            this.ExitMinilandMax.Name = "ExitMinilandMax";
            this.ExitMinilandMax.Size = new System.Drawing.Size(79, 20);
            this.ExitMinilandMax.TabIndex = 48;
            this.ExitMinilandMax.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.ExitMinilandMax.ValueChanged += new System.EventHandler(this.ExitMinilandMax_ValueChanged);
            // 
            // ExitMinilandMin
            // 
            this.ExitMinilandMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ExitMinilandMin.Location = new System.Drawing.Point(137, 5);
            this.ExitMinilandMin.Maximum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            0});
            this.ExitMinilandMin.Name = "ExitMinilandMin";
            this.ExitMinilandMin.Size = new System.Drawing.Size(78, 20);
            this.ExitMinilandMin.TabIndex = 47;
            this.ExitMinilandMin.Value = new decimal(new int[] {
            750,
            0,
            0,
            0});
            this.ExitMinilandMin.ValueChanged += new System.EventHandler(this.ExitMinilandMin_ValueChanged);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Exit Miniland Delay";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(227, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 13);
            this.label6.TabIndex = 48;
            this.label6.Text = "~";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.Controls.Add(this.UseAmuletMax, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.UseAmuletMin, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.label8, 2, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(9, 79);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(336, 30);
            this.tableLayoutPanel4.TabIndex = 3;
            // 
            // UseAmuletMax
            // 
            this.UseAmuletMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.UseAmuletMax.Location = new System.Drawing.Point(254, 5);
            this.UseAmuletMax.Maximum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            0});
            this.UseAmuletMax.Name = "UseAmuletMax";
            this.UseAmuletMax.Size = new System.Drawing.Size(79, 20);
            this.UseAmuletMax.TabIndex = 48;
            this.UseAmuletMax.Value = new decimal(new int[] {
            1450,
            0,
            0,
            0});
            this.UseAmuletMax.ValueChanged += new System.EventHandler(this.UseAmuletMax_ValueChanged);
            // 
            // UseAmuletMin
            // 
            this.UseAmuletMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.UseAmuletMin.Location = new System.Drawing.Point(137, 5);
            this.UseAmuletMin.Maximum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            0});
            this.UseAmuletMin.Name = "UseAmuletMin";
            this.UseAmuletMin.Size = new System.Drawing.Size(78, 20);
            this.UseAmuletMin.TabIndex = 47;
            this.UseAmuletMin.Value = new decimal(new int[] {
            750,
            0,
            0,
            0});
            this.UseAmuletMin.ValueChanged += new System.EventHandler(this.UseAmuletMin_ValueChanged);
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(91, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Use Amulet Delay";
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(227, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(14, 13);
            this.label8.TabIndex = 48;
            this.label8.Text = "~";
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.ClientSize = new System.Drawing.Size(359, 157);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel4);
            this.Controls.Add(this.tableLayoutPanel3);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "Settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinilandInviteMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinilandInviteMin)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AttackLureMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AttackLureMin)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ExitMinilandMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExitMinilandMin)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UseAmuletMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UseAmuletMin)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public System.Windows.Forms.NumericUpDown MinilandInviteMin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.NumericUpDown MinilandInviteMax;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        public System.Windows.Forms.NumericUpDown AttackLureMax;
        public System.Windows.Forms.NumericUpDown AttackLureMin;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        public System.Windows.Forms.NumericUpDown ExitMinilandMax;
        public System.Windows.Forms.NumericUpDown ExitMinilandMin;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        public System.Windows.Forms.NumericUpDown UseAmuletMax;
        public System.Windows.Forms.NumericUpDown UseAmuletMin;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
    }
}