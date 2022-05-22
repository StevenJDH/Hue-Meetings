/*
 * This file is part of Hue Meetings <https://github.com/StevenJDH/Hue-Meetings>.
 * Copyright (C) 2022 Steven Jenkins De Haro.
 *
 * Hue Meetings is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Hue Meetings is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Hue Meetings.  If not, see <http://www.gnu.org/licenses/>.
 */

namespace Hue_Meetings
{
    partial class FrmConfig
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
            this.cmbLights = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblVpnNotice = new System.Windows.Forms.Label();
            this.chkRemote = new System.Windows.Forms.CheckBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.btnBusyColorPicker = new System.Windows.Forms.Button();
            this.pbBusyColor = new System.Windows.Forms.PictureBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.btnAvailableColorPicker = new System.Windows.Forms.Button();
            this.pbAvailableColor = new System.Windows.Forms.PictureBox();
            this.lblAvailable = new System.Windows.Forms.Label();
            this.lblBusy = new System.Windows.Forms.Label();
            this.btnAvailableCapture = new System.Windows.Forms.Button();
            this.btnBusyCapture = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbBusyColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAvailableColor)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbLights
            // 
            this.cmbLights.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLights.FormattingEnabled = true;
            this.cmbLights.Location = new System.Drawing.Point(136, 64);
            this.cmbLights.Name = "cmbLights";
            this.cmbLights.Size = new System.Drawing.Size(264, 23);
            this.cmbLights.Sorted = true;
            this.cmbLights.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(56, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 24);
            this.label6.TabIndex = 9;
            this.label6.Text = "Light:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(480, 312);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(112, 32);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(360, 312);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(112, 32);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblVpnNotice
            // 
            this.lblVpnNotice.Location = new System.Drawing.Point(112, 288);
            this.lblVpnNotice.Name = "lblVpnNotice";
            this.lblVpnNotice.Size = new System.Drawing.Size(480, 16);
            this.lblVpnNotice.TabIndex = 12;
            this.lblVpnNotice.Text = "VPN mode is disabled. Please grant access to Hue Meetings to enable the remote AP" +
    "Is.";
            this.lblVpnNotice.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblVpnNotice.Visible = false;
            // 
            // chkRemote
            // 
            this.chkRemote.AutoSize = true;
            this.chkRemote.Location = new System.Drawing.Point(104, 104);
            this.chkRemote.Name = "chkRemote";
            this.chkRemote.Size = new System.Drawing.Size(96, 19);
            this.chkRemote.TabIndex = 13;
            this.chkRemote.Text = "Using a VPN?";
            this.chkRemote.UseVisualStyleBackColor = true;
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(288, 96);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(112, 32);
            this.btnFind.TabIndex = 14;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // btnBusyColorPicker
            // 
            this.btnBusyColorPicker.Location = new System.Drawing.Point(296, 224);
            this.btnBusyColorPicker.Name = "btnBusyColorPicker";
            this.btnBusyColorPicker.Size = new System.Drawing.Size(104, 23);
            this.btnBusyColorPicker.TabIndex = 16;
            this.btnBusyColorPicker.Text = "Set Color...";
            this.btnBusyColorPicker.UseVisualStyleBackColor = true;
            this.btnBusyColorPicker.Click += new System.EventHandler(this.btnBusyColorPicker_Click);
            // 
            // pbBusyColor
            // 
            this.pbBusyColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbBusyColor.Location = new System.Drawing.Point(296, 152);
            this.pbBusyColor.Name = "pbBusyColor";
            this.pbBusyColor.Size = new System.Drawing.Size(104, 64);
            this.pbBusyColor.TabIndex = 15;
            this.pbBusyColor.TabStop = false;
            // 
            // btnAvailableColorPicker
            // 
            this.btnAvailableColorPicker.Location = new System.Drawing.Point(168, 224);
            this.btnAvailableColorPicker.Name = "btnAvailableColorPicker";
            this.btnAvailableColorPicker.Size = new System.Drawing.Size(104, 23);
            this.btnAvailableColorPicker.TabIndex = 18;
            this.btnAvailableColorPicker.Text = "Set Color...";
            this.btnAvailableColorPicker.UseVisualStyleBackColor = true;
            this.btnAvailableColorPicker.Click += new System.EventHandler(this.btnAvailableColorPicker_Click);
            // 
            // pbAvailableColor
            // 
            this.pbAvailableColor.BackColor = System.Drawing.SystemColors.Control;
            this.pbAvailableColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbAvailableColor.Location = new System.Drawing.Point(168, 152);
            this.pbAvailableColor.Name = "pbAvailableColor";
            this.pbAvailableColor.Size = new System.Drawing.Size(104, 64);
            this.pbAvailableColor.TabIndex = 17;
            this.pbAvailableColor.TabStop = false;
            // 
            // lblAvailable
            // 
            this.lblAvailable.BackColor = System.Drawing.Color.Transparent;
            this.lblAvailable.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblAvailable.ForeColor = System.Drawing.Color.White;
            this.lblAvailable.Location = new System.Drawing.Point(176, 176);
            this.lblAvailable.Name = "lblAvailable";
            this.lblAvailable.Size = new System.Drawing.Size(88, 16);
            this.lblAvailable.TabIndex = 19;
            this.lblAvailable.Text = "Available";
            this.lblAvailable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBusy
            // 
            this.lblBusy.BackColor = System.Drawing.Color.Transparent;
            this.lblBusy.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblBusy.ForeColor = System.Drawing.Color.White;
            this.lblBusy.Location = new System.Drawing.Point(304, 176);
            this.lblBusy.Name = "lblBusy";
            this.lblBusy.Size = new System.Drawing.Size(88, 16);
            this.lblBusy.TabIndex = 20;
            this.lblBusy.Text = "In Meeting";
            this.lblBusy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnAvailableCapture
            // 
            this.btnAvailableCapture.Location = new System.Drawing.Point(168, 248);
            this.btnAvailableCapture.Name = "btnAvailableCapture";
            this.btnAvailableCapture.Size = new System.Drawing.Size(104, 23);
            this.btnAvailableCapture.TabIndex = 21;
            this.btnAvailableCapture.Text = "Capture Current";
            this.btnAvailableCapture.UseVisualStyleBackColor = true;
            // 
            // btnBusyCapture
            // 
            this.btnBusyCapture.Location = new System.Drawing.Point(296, 248);
            this.btnBusyCapture.Name = "btnBusyCapture";
            this.btnBusyCapture.Size = new System.Drawing.Size(104, 23);
            this.btnBusyCapture.TabIndex = 22;
            this.btnBusyCapture.Text = "Capture Current";
            this.btnBusyCapture.UseVisualStyleBackColor = true;
            // 
            // FrmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 353);
            this.Controls.Add(this.btnBusyCapture);
            this.Controls.Add(this.btnAvailableCapture);
            this.Controls.Add(this.lblBusy);
            this.Controls.Add(this.lblAvailable);
            this.Controls.Add(this.btnAvailableColorPicker);
            this.Controls.Add(this.pbAvailableColor);
            this.Controls.Add(this.btnBusyColorPicker);
            this.Controls.Add(this.pbBusyColor);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.chkRemote);
            this.Controls.Add(this.lblVpnNotice);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbLights);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmConfig";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configuration - Hue Meetings";
            this.Load += new System.EventHandler(this.FrmConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbBusyColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAvailableColor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComboBox cmbLights;
        private Label label6;
        private Button btnSave;
        private Button btnCancel;
        private Label lblVpnNotice;
        private CheckBox chkRemote;
        private Button btnFind;
        private Button btnBusyColorPicker;
        private PictureBox pbBusyColor;
        private ColorDialog colorDialog1;
        private Button btnAvailableColorPicker;
        private PictureBox pbAvailableColor;
        private Label lblAvailable;
        private Label lblBusy;
        private Button btnAvailableCapture;
        private Button btnBusyCapture;
    }
}