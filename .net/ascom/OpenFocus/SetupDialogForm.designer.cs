/*
 * File: SetupDialogForm.designer.cs
 * Package: OpenFocus ASCOM
 *
 * Copyright (c) 2010 Eric J. Holmes
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General
 * Public License along with this library; if not, write to the
 * Free Software Foundation, Inc., 59 Temple Place, Suite 330,
 * Boston, MA  02111-1307  USA
 *
 */

namespace ASCOM.OpenFocus
{
    partial class SetupDialogForm
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
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.cbDevices = new System.Windows.Forms.ComboBox();
            this.lblDevices = new System.Windows.Forms.Label();
            this.lblTip = new System.Windows.Forms.Label();
            this.gbTemperatureUnits = new System.Windows.Forms.GroupBox();
            this.rbUnitsFahrenheit = new System.Windows.Forms.RadioButton();
            this.rbUnitsCelsius = new System.Windows.Forms.RadioButton();
            this.btnConfigureDevice = new System.Windows.Forms.Button();
            this.lblDriverVersion = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.gbTemperatureUnits.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(248, 145);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(59, 24);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(248, 175);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.OpenFocus.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(259, 9);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            // 
            // cbDevices
            // 
            this.cbDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDevices.FormattingEnabled = true;
            this.cbDevices.Location = new System.Drawing.Point(12, 29);
            this.cbDevices.Name = "cbDevices";
            this.cbDevices.Size = new System.Drawing.Size(214, 21);
            this.cbDevices.TabIndex = 4;
            // 
            // lblDevices
            // 
            this.lblDevices.AutoSize = true;
            this.lblDevices.Location = new System.Drawing.Point(13, 13);
            this.lblDevices.Name = "lblDevices";
            this.lblDevices.Size = new System.Drawing.Size(74, 13);
            this.lblDevices.TabIndex = 5;
            this.lblDevices.Text = "Select Device";
            // 
            // lblTip
            // 
            this.lblTip.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblTip.Location = new System.Drawing.Point(13, 52);
            this.lblTip.Name = "lblTip";
            this.lblTip.Size = new System.Drawing.Size(213, 35);
            this.lblTip.TabIndex = 7;
            this.lblTip.Text = "If you have multiple devices you can select which device to connect to.";
            // 
            // gbTemperatureUnits
            // 
            this.gbTemperatureUnits.Controls.Add(this.rbUnitsFahrenheit);
            this.gbTemperatureUnits.Controls.Add(this.rbUnitsCelsius);
            this.gbTemperatureUnits.Location = new System.Drawing.Point(12, 135);
            this.gbTemperatureUnits.Name = "gbTemperatureUnits";
            this.gbTemperatureUnits.Size = new System.Drawing.Size(214, 46);
            this.gbTemperatureUnits.TabIndex = 8;
            this.gbTemperatureUnits.TabStop = false;
            this.gbTemperatureUnits.Text = "Temperature Units";
            // 
            // rbUnitsFahrenheit
            // 
            this.rbUnitsFahrenheit.AutoSize = true;
            this.rbUnitsFahrenheit.Location = new System.Drawing.Point(71, 20);
            this.rbUnitsFahrenheit.Name = "rbUnitsFahrenheit";
            this.rbUnitsFahrenheit.Size = new System.Drawing.Size(75, 17);
            this.rbUnitsFahrenheit.TabIndex = 1;
            this.rbUnitsFahrenheit.TabStop = true;
            this.rbUnitsFahrenheit.Text = "Fahrenheit";
            this.rbUnitsFahrenheit.UseVisualStyleBackColor = true;
            // 
            // rbUnitsCelsius
            // 
            this.rbUnitsCelsius.AutoSize = true;
            this.rbUnitsCelsius.Location = new System.Drawing.Point(6, 19);
            this.rbUnitsCelsius.Name = "rbUnitsCelsius";
            this.rbUnitsCelsius.Size = new System.Drawing.Size(58, 17);
            this.rbUnitsCelsius.TabIndex = 0;
            this.rbUnitsCelsius.TabStop = true;
            this.rbUnitsCelsius.Text = "Celsius";
            this.rbUnitsCelsius.UseVisualStyleBackColor = true;
            // 
            // btnConfigureDevice
            // 
            this.btnConfigureDevice.Location = new System.Drawing.Point(12, 90);
            this.btnConfigureDevice.Name = "btnConfigureDevice";
            this.btnConfigureDevice.Size = new System.Drawing.Size(214, 23);
            this.btnConfigureDevice.TabIndex = 9;
            this.btnConfigureDevice.Text = "Configure Selected Device";
            this.btnConfigureDevice.UseVisualStyleBackColor = true;
            this.btnConfigureDevice.Click += new System.EventHandler(this.btnConfigureDevice_Click);
            // 
            // lblDriverVersion
            // 
            this.lblDriverVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblDriverVersion.AutoSize = true;
            this.lblDriverVersion.Location = new System.Drawing.Point(13, 187);
            this.lblDriverVersion.Name = "lblDriverVersion";
            this.lblDriverVersion.Size = new System.Drawing.Size(0, 13);
            this.lblDriverVersion.TabIndex = 10;
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 208);
            this.Controls.Add(this.lblDriverVersion);
            this.Controls.Add(this.btnConfigureDevice);
            this.Controls.Add(this.gbTemperatureUnits);
            this.Controls.Add(this.lblTip);
            this.Controls.Add(this.lblDevices);
            this.Controls.Add(this.cbDevices);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OpenFocus Setup";
            this.Shown += new System.EventHandler(this.SetupDialogForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.gbTemperatureUnits.ResumeLayout(false);
            this.gbTemperatureUnits.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.ComboBox cbDevices;
        private System.Windows.Forms.Label lblDevices;
        private System.Windows.Forms.Label lblTip;
        private System.Windows.Forms.GroupBox gbTemperatureUnits;
        private System.Windows.Forms.RadioButton rbUnitsFahrenheit;
        private System.Windows.Forms.RadioButton rbUnitsCelsius;
        private System.Windows.Forms.Button btnConfigureDevice;
        private System.Windows.Forms.Label lblDriverVersion;
    }
}