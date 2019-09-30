using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraWaitForm;

namespace Xr.RtManager
{
    internal partial class WaitingForm : WaitForm
    {
        

        public WaitingForm()
        {
            InitializeComponent();
            this.progressPanel1.AutoHeight = true;
        }

       
    }
}
