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
    public partial class LogMsgForm : BaseForm
    {
        public LogMsgForm()
        {
            InitializeComponent();
        }

        public String text { get; set; }

        private void buttonControl1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void LogMsgForm_Load(object sender, EventArgs e)
        {
            memoEdit1.Text = text;
        }
    }
}
