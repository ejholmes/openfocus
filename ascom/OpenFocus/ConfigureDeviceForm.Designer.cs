namespace ASCOM.OpenFocus
{
    partial class ConfigureDeviceForm
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
            this.lblName = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.gbPositionSettings = new System.Windows.Forms.GroupBox();
            this.btnSetPosition = new System.Windows.Forms.Button();
            this.lblMaxPosition = new System.Windows.Forms.Label();
            this.tbMaxPosition = new System.Windows.Forms.TextBox();
            this.gbTemperatureCompensation = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbTemperatureCoefficient = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.gbDeviceInfo = new System.Windows.Forms.GroupBox();
            this.FirmwareVersion = new System.Windows.Forms.Label();
            this.lblFirmwareVersion = new System.Windows.Forms.Label();
            this.Product = new System.Windows.Forms.Label();
            this.lblProduct = new System.Windows.Forms.Label();
            this.SerialNumber = new System.Windows.Forms.Label();
            this.lblSerial = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.gbPositionSettings.SuspendLayout();
            this.gbTemperatureCompensation.SuspendLayout();
            this.gbDeviceInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(26, 9);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name";
            // 
            // tbName
            // 
            this.tbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbName.Location = new System.Drawing.Point(67, 6);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(202, 20);
            this.tbName.TabIndex = 1;
            // 
            // gbPositionSettings
            // 
            this.gbPositionSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbPositionSettings.Controls.Add(this.btnSetPosition);
            this.gbPositionSettings.Controls.Add(this.lblMaxPosition);
            this.gbPositionSettings.Controls.Add(this.tbMaxPosition);
            this.gbPositionSettings.Location = new System.Drawing.Point(12, 116);
            this.gbPositionSettings.Name = "gbPositionSettings";
            this.gbPositionSettings.Size = new System.Drawing.Size(257, 78);
            this.gbPositionSettings.TabIndex = 2;
            this.gbPositionSettings.TabStop = false;
            this.gbPositionSettings.Text = "Position Settings";
            // 
            // btnSetPosition
            // 
            this.btnSetPosition.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSetPosition.Location = new System.Drawing.Point(63, 45);
            this.btnSetPosition.Name = "btnSetPosition";
            this.btnSetPosition.Size = new System.Drawing.Size(127, 23);
            this.btnSetPosition.TabIndex = 2;
            this.btnSetPosition.Text = "Set Current Position";
            this.btnSetPosition.UseVisualStyleBackColor = true;
            this.btnSetPosition.Click += new System.EventHandler(this.btnSetPosition_Click);
            // 
            // lblMaxPosition
            // 
            this.lblMaxPosition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMaxPosition.AutoSize = true;
            this.lblMaxPosition.Location = new System.Drawing.Point(6, 22);
            this.lblMaxPosition.Name = "lblMaxPosition";
            this.lblMaxPosition.Size = new System.Drawing.Size(67, 13);
            this.lblMaxPosition.TabIndex = 1;
            this.lblMaxPosition.Text = "Max Position";
            // 
            // tbMaxPosition
            // 
            this.tbMaxPosition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMaxPosition.Location = new System.Drawing.Point(79, 19);
            this.tbMaxPosition.Name = "tbMaxPosition";
            this.tbMaxPosition.Size = new System.Drawing.Size(172, 20);
            this.tbMaxPosition.TabIndex = 0;
            this.tbMaxPosition.Validating += new System.ComponentModel.CancelEventHandler(this.tbMaxPosition_Validating);
            // 
            // gbTemperatureCompensation
            // 
            this.gbTemperatureCompensation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbTemperatureCompensation.Controls.Add(this.label1);
            this.gbTemperatureCompensation.Controls.Add(this.tbTemperatureCoefficient);
            this.gbTemperatureCompensation.Location = new System.Drawing.Point(12, 200);
            this.gbTemperatureCompensation.Name = "gbTemperatureCompensation";
            this.gbTemperatureCompensation.Size = new System.Drawing.Size(257, 49);
            this.gbTemperatureCompensation.TabIndex = 3;
            this.gbTemperatureCompensation.TabStop = false;
            this.gbTemperatureCompensation.Text = "Temperature Compensation";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Temp. Coefficient";
            this.toolTip.SetToolTip(this.label1, "Value should be steps per unit temperature");
            // 
            // tbTemperatureCoefficient
            // 
            this.tbTemperatureCoefficient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTemperatureCoefficient.Location = new System.Drawing.Point(110, 19);
            this.tbTemperatureCoefficient.Name = "tbTemperatureCoefficient";
            this.tbTemperatureCoefficient.Size = new System.Drawing.Size(141, 20);
            this.tbTemperatureCoefficient.TabIndex = 1;
            this.toolTip.SetToolTip(this.tbTemperatureCoefficient, "Value should be steps per unit temperature");
            this.tbTemperatureCoefficient.Validating += new System.ComponentModel.CancelEventHandler(this.tbTemperatureCoefficient_Validating);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(194, 255);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(113, 255);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // gbDeviceInfo
            // 
            this.gbDeviceInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbDeviceInfo.Controls.Add(this.FirmwareVersion);
            this.gbDeviceInfo.Controls.Add(this.lblFirmwareVersion);
            this.gbDeviceInfo.Controls.Add(this.Product);
            this.gbDeviceInfo.Controls.Add(this.lblProduct);
            this.gbDeviceInfo.Controls.Add(this.SerialNumber);
            this.gbDeviceInfo.Controls.Add(this.lblSerial);
            this.gbDeviceInfo.Location = new System.Drawing.Point(12, 32);
            this.gbDeviceInfo.Name = "gbDeviceInfo";
            this.gbDeviceInfo.Size = new System.Drawing.Size(257, 78);
            this.gbDeviceInfo.TabIndex = 6;
            this.gbDeviceInfo.TabStop = false;
            this.gbDeviceInfo.Text = "Device Information";
            // 
            // FirmwareVersion
            // 
            this.FirmwareVersion.Location = new System.Drawing.Point(102, 56);
            this.FirmwareVersion.Name = "FirmwareVersion";
            this.FirmwareVersion.Size = new System.Drawing.Size(149, 13);
            this.FirmwareVersion.TabIndex = 5;
            // 
            // lblFirmwareVersion
            // 
            this.lblFirmwareVersion.AutoSize = true;
            this.lblFirmwareVersion.Location = new System.Drawing.Point(6, 56);
            this.lblFirmwareVersion.Name = "lblFirmwareVersion";
            this.lblFirmwareVersion.Size = new System.Drawing.Size(90, 13);
            this.lblFirmwareVersion.TabIndex = 4;
            this.lblFirmwareVersion.Text = "Firmware Version:";
            // 
            // Product
            // 
            this.Product.Location = new System.Drawing.Point(56, 36);
            this.Product.Name = "Product";
            this.Product.Size = new System.Drawing.Size(195, 13);
            this.Product.TabIndex = 3;
            // 
            // lblProduct
            // 
            this.lblProduct.AutoSize = true;
            this.lblProduct.Location = new System.Drawing.Point(6, 36);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(47, 13);
            this.lblProduct.TabIndex = 2;
            this.lblProduct.Text = "Product:";
            // 
            // SerialNumber
            // 
            this.SerialNumber.Location = new System.Drawing.Point(58, 16);
            this.SerialNumber.Name = "SerialNumber";
            this.SerialNumber.Size = new System.Drawing.Size(193, 13);
            this.SerialNumber.TabIndex = 1;
            // 
            // lblSerial
            // 
            this.lblSerial.AutoSize = true;
            this.lblSerial.Location = new System.Drawing.Point(6, 16);
            this.lblSerial.Name = "lblSerial";
            this.lblSerial.Size = new System.Drawing.Size(46, 13);
            this.lblSerial.TabIndex = 0;
            this.lblSerial.Text = "Serial #:";
            // 
            // ConfigureDeviceForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(282, 285);
            this.Controls.Add(this.gbDeviceInfo);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.gbTemperatureCompensation);
            this.Controls.Add(this.gbPositionSettings);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.lblName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ConfigureDeviceForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configure Device";
            this.gbPositionSettings.ResumeLayout(false);
            this.gbPositionSettings.PerformLayout();
            this.gbTemperatureCompensation.ResumeLayout(false);
            this.gbTemperatureCompensation.PerformLayout();
            this.gbDeviceInfo.ResumeLayout(false);
            this.gbDeviceInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region Data Validation

        void tbMaxPosition_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try { System.UInt16.Parse(((System.Windows.Forms.TextBox)sender).Text); }
            catch { System.Windows.Forms.MessageBox.Show("Please enter a valid integer between 0 and 32767"); ((System.Windows.Forms.TextBox)sender).Focus(); ((System.Windows.Forms.TextBox)sender).SelectAll(); }
        }

        void tbTemperatureCoefficient_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try { double.Parse(((System.Windows.Forms.TextBox)sender).Text); }
            catch { System.Windows.Forms.MessageBox.Show("Please enter a valid temperature coefficient"); ((System.Windows.Forms.TextBox)sender).Focus(); ((System.Windows.Forms.TextBox)sender).SelectAll(); }
        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.GroupBox gbPositionSettings;
        private System.Windows.Forms.Button btnSetPosition;
        private System.Windows.Forms.Label lblMaxPosition;
        private System.Windows.Forms.TextBox tbMaxPosition;
        private System.Windows.Forms.GroupBox gbTemperatureCompensation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbTemperatureCoefficient;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.GroupBox gbDeviceInfo;
        private System.Windows.Forms.Label lblSerial;
        private System.Windows.Forms.Label SerialNumber;
        private System.Windows.Forms.Label Product;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.Label FirmwareVersion;
        private System.Windows.Forms.Label lblFirmwareVersion;
    }
}