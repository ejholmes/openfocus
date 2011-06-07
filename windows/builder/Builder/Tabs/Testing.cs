using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Builder
{
    public partial class MainWindow
    {
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (this.btnConnect.Text == "Connect" && dev.Connect())
            {
                this.btnTemperatureTestStart.Enabled = true;

                this.btnConnect.Text = "Disconnect";
            }
            else if (this.btnConnect.Text == "Disconnect")
            {
                dev.Disconnect();
                this.btnConnect.Text = "Connect";
            }
        }

        private void btnTemperatureTestStart_Click(object sender, EventArgs e)
        {
            this.btnTemperatureTestStart.Enabled = false;
            this.btnTemperatureTestStop.Enabled = true;

            tempTimer.Interval = 200;
            tempTimer.Tick += new EventHandler(tempTimer_Tick);

            tempTimer.Start();
        }

        void tempTimer_Tick(object sender, EventArgs e)
        {
            double temperature = dev.Temperature;

            this.lbTemperatureLog.Items.Add("Temp: " + temperature.ToString());

            this.lbTemperatureLog.SelectedIndex = this.lbTemperatureLog.Items.Count - 1;
        }

        private void btnTemperatureTestStop_Click(object sender, EventArgs e)
        {
            this.btnTemperatureTestStop.Enabled = false;
            this.btnTemperatureTestStart.Enabled = true;

            tempTimer.Stop();
        }
    }
}
