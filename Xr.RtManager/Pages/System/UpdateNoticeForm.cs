using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Xr.Common;

namespace Xr.RtManager
{
    public partial class UpdateNoticeForm : BaseForm
    {
        public UpdateNoticeForm()
        {
            InitializeComponent();
        }


        public ClientVersionEntity cv { get; set; }

        private void UpdateNoticeForm_Load(object sender, EventArgs e)
        {
            if (cv != null)
            {
                this.Text = "更新公告V" + cv.version;
                label2.Text = cv.createDate;
                meUpdateDesc.Text = cv.updateDesc;
                meUpdateDesc.SelectionStart = meUpdateDesc.Text.Length;
                meUpdateDesc.ScrollToCaret();
                btnClose.Focus();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSeeMore_Click(object sender, EventArgs e)
        {
            //DialogResult = DialogResult.OK;
            MainForm form = (MainForm)this.Parent.TopLevelControl;
            form.JumpInterface("ClientVersionQueryForm", "更新公告", null);
        }
    }
}
