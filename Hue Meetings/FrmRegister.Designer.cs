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
    partial class FrmRegister
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
            this.btnFind = new System.Windows.Forms.Button();
            this.btnRegister = new System.Windows.Forms.Button();
            this.chkRemote = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblBridgeUser = new System.Windows.Forms.Label();
            this.cmbBridges = new System.Windows.Forms.ComboBox();
            this.btnGrantAccess = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtCallback = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtClientSecret = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtClientId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAppId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbUsers = new System.Windows.Forms.ComboBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lnkGenerateAccess = new System.Windows.Forms.LinkLabel();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnFind
            // 
            this.btnFind.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFind.Location = new System.Drawing.Point(168, 112);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(112, 32);
            this.btnFind.TabIndex = 0;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // btnRegister
            // 
            this.btnRegister.Enabled = false;
            this.btnRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegister.Location = new System.Drawing.Point(320, 112);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(112, 32);
            this.btnRegister.TabIndex = 1;
            this.btnRegister.Text = "Register";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // chkRemote
            // 
            this.chkRemote.AutoSize = true;
            this.chkRemote.Location = new System.Drawing.Point(88, 152);
            this.chkRemote.Name = "chkRemote";
            this.chkRemote.Size = new System.Drawing.Size(96, 19);
            this.chkRemote.TabIndex = 2;
            this.chkRemote.Text = "Using a VPN?";
            this.chkRemote.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(64, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(496, 24);
            this.label2.TabIndex = 4;
            this.label2.Text = "Press the button on the selected Philips Hue Bridge, and within 30 seconds, click" +
    " \'Register\'.";
            // 
            // lblBridgeUser
            // 
            this.lblBridgeUser.Location = new System.Drawing.Point(64, 32);
            this.lblBridgeUser.Name = "lblBridgeUser";
            this.lblBridgeUser.Size = new System.Drawing.Size(408, 16);
            this.lblBridgeUser.TabIndex = 5;
            this.lblBridgeUser.Text = "This will add a user called \'hue-meetings#xxxxxx\' to your bridge.";
            // 
            // cmbBridges
            // 
            this.cmbBridges.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBridges.FormattingEnabled = true;
            this.cmbBridges.Location = new System.Drawing.Point(168, 56);
            this.cmbBridges.Name = "cmbBridges";
            this.cmbBridges.Size = new System.Drawing.Size(264, 23);
            this.cmbBridges.TabIndex = 6;
            this.cmbBridges.SelectedIndexChanged += new System.EventHandler(this.cmbBridges_SelectedIndexChanged);
            // 
            // btnGrantAccess
            // 
            this.btnGrantAccess.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGrantAccess.Location = new System.Drawing.Point(248, 312);
            this.btnGrantAccess.Name = "btnGrantAccess";
            this.btnGrantAccess.Size = new System.Drawing.Size(112, 32);
            this.btnGrantAccess.TabIndex = 7;
            this.btnGrantAccess.Text = "Grant Access";
            this.btnGrantAccess.UseVisualStyleBackColor = true;
            this.btnGrantAccess.Click += new System.EventHandler(this.btnGrantAccess_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtCallback);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtClientSecret);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtClientId);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtAppId);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(88, 168);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(424, 136);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Hue Remote";
            // 
            // txtCallback
            // 
            this.txtCallback.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtCallback.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCallback.ForeColor = System.Drawing.Color.White;
            this.txtCallback.Location = new System.Drawing.Point(104, 104);
            this.txtCallback.Name = "txtCallback";
            this.txtCallback.ReadOnly = true;
            this.txtCallback.Size = new System.Drawing.Size(288, 23);
            this.txtCallback.TabIndex = 7;
            this.txtCallback.Text = "https://localhost:xxxx/callback/oauth";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(16, 104);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 24);
            this.label5.TabIndex = 6;
            this.label5.Text = "Callback URL:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtClientSecret
            // 
            this.txtClientSecret.BackColor = System.Drawing.Color.Black;
            this.txtClientSecret.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtClientSecret.ForeColor = System.Drawing.Color.White;
            this.txtClientSecret.Location = new System.Drawing.Point(104, 64);
            this.txtClientSecret.Name = "txtClientSecret";
            this.txtClientSecret.PasswordChar = '*';
            this.txtClientSecret.Size = new System.Drawing.Size(288, 23);
            this.txtClientSecret.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(16, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 24);
            this.label4.TabIndex = 4;
            this.label4.Text = "Client Secret:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtClientId
            // 
            this.txtClientId.BackColor = System.Drawing.Color.Black;
            this.txtClientId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtClientId.ForeColor = System.Drawing.Color.White;
            this.txtClientId.Location = new System.Drawing.Point(104, 40);
            this.txtClientId.Name = "txtClientId";
            this.txtClientId.Size = new System.Drawing.Size(288, 23);
            this.txtClientId.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(40, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 24);
            this.label3.TabIndex = 2;
            this.label3.Text = "Client Id:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAppId
            // 
            this.txtAppId.BackColor = System.Drawing.Color.Black;
            this.txtAppId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAppId.ForeColor = System.Drawing.Color.White;
            this.txtAppId.Location = new System.Drawing.Point(104, 16);
            this.txtAppId.Name = "txtAppId";
            this.txtAppId.Size = new System.Drawing.Size(288, 23);
            this.txtAppId.TabIndex = 1;
            this.txtAppId.Text = "hue_meetings";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(40, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "App Id:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbUsers
            // 
            this.cmbUsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUsers.FormattingEnabled = true;
            this.cmbUsers.Location = new System.Drawing.Point(168, 80);
            this.cmbUsers.Name = "cmbUsers";
            this.cmbUsers.Size = new System.Drawing.Size(264, 23);
            this.cmbUsers.Sorted = true;
            this.cmbUsers.TabIndex = 9;
            this.cmbUsers.SelectedIndexChanged += new System.EventHandler(this.cmbUsers_SelectedIndexChanged);
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Location = new System.Drawing.Point(440, 80);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(80, 24);
            this.btnDelete.TabIndex = 10;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lnkGenerateAccess
            // 
            this.lnkGenerateAccess.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lnkGenerateAccess.Location = new System.Drawing.Point(416, 304);
            this.lnkGenerateAccess.Name = "lnkGenerateAccess";
            this.lnkGenerateAccess.Size = new System.Drawing.Size(96, 16);
            this.lnkGenerateAccess.TabIndex = 11;
            this.lnkGenerateAccess.TabStop = true;
            this.lnkGenerateAccess.Text = "Generate Access";
            this.lnkGenerateAccess.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lnkGenerateAccess.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkGenerateAccess_LinkClicked);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(88, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 24);
            this.label6.TabIndex = 8;
            this.label6.Text = "Bridges:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(88, 80);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 24);
            this.label7.TabIndex = 12;
            this.label7.Text = "Users:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FrmRegister
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 353);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lnkGenerateAccess);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.cmbUsers);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnGrantAccess);
            this.Controls.Add(this.cmbBridges);
            this.Controls.Add(this.lblBridgeUser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkRemote);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.btnFind);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmRegister";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Hue Meetings - Connect Bridge";
            this.Load += new System.EventHandler(this.FrmRegister_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnFind;
        private Button btnRegister;
        private CheckBox chkRemote;
        private Label label2;
        private Label lblBridgeUser;
        private ComboBox cmbBridges;
        private Button btnGrantAccess;
        private GroupBox groupBox1;
        private TextBox txtAppId;
        private Label label1;
        private TextBox txtClientId;
        private Label label3;
        private TextBox txtClientSecret;
        private Label label4;
        private TextBox txtCallback;
        private Label label5;
        private ComboBox cmbUsers;
        private Button btnDelete;
        private LinkLabel lnkGenerateAccess;
        private Label label6;
        private Label label7;
    }
}