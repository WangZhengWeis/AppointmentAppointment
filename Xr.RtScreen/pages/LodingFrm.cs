using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Xr.RtScreen.pages
{
    public partial class LodingFrm : Form
    {
        private Form partentForm = null;
        public LodingFrm(Form partentForm)
        {
            InitializeComponent();
            this.partentForm = partentForm;
        }
        private void LodingFrm_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None;
            ControlBox = false;
            StartPosition = FormStartPosition.CenterParent;
        }
    }
}
