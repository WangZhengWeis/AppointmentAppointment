using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Xr.RtCall.pages
{
    public partial class RtCallFrm : UserControl
    {
        public RtCallFrm()
        {
            InitializeComponent();
           this.Height = this.Height - this.panelControl2.Height;
            //this.panelControl2.Visible = false;
            //this.Size=new Size(615,40);
        }
        string name = "";
        private void skinButNext_Click(object sender, EventArgs e)
        {
            RtCallMessageFrm rcf = new RtCallMessageFrm(name);
            Xr.RtCall.Form1 f = new Form1();
            f.panelControl1.Controls.Clear();
            f.Height = rcf.Height + 30;
            f.Width = rcf.Width;
            //f.labBoxInfor.Text = "";
            f.panelControl1.Controls.Add(rcf);
            f.Show();
        }

        private void skinbutBig_Click(object sender, EventArgs e)
        {
            this.Size = new Size(615,500);
            this.panelControl2.Visible = true;
        }
        //private static Size lastFromSize = new Size();
        //private bool minSizeflag = false;
        //private void RtCallFrm_Resize(object sender, EventArgs e)
        //{
        //    this.panelControl2.Location = new Point(3,3+this.panelControl1.Height);
        //    this.panelControl2.Size = new Size(this.Size.Width-6,this.Size.Height-6-this.panelControl1.Height);
        //    if (!minSizeflag)
        //    {
        //        lastFromSize = this.Size;
        //    }
        //}
    }
}
