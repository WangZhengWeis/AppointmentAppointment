using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Xr.Common.Controls
{
    public partial class frmPreview : Form
    {
        public frmPreview(string HTMLContent)
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.Text = "内容预览";
            this.StartPosition = FormStartPosition.CenterScreen;
            webBrowser1.DocumentText = HTMLContent;
            webBrowser1.Show();
        }
    }
}
