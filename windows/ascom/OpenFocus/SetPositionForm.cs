using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.OpenFocus
{
    public partial class SetPositionForm : Form
    {
        public string Position = String.Empty;

        public SetPositionForm()
        {
            InitializeComponent();
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            Position = this.tbPosition.Text;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        }
    }
}
