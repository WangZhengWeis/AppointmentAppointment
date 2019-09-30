using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace Xr.Common.Controls
{
    public partial class RichEditorForm : Form
    {
        public RichEditorForm()
        {
            InitializeComponent();
        }

        public String text { get; set; }
        public String ImagUploadUrl { get; set; }

        private void RichEditorForm_Load(object sender, EventArgs e)
        {
            richEditor1.ImagUploadUrl = ImagUploadUrl;
            richEditor1.LoadText(text);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            text = richEditor1.InnerHtml;
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        private String HtmlTransform2Str(String Html)
        {
            return HttpUtility.UrlEncode(Html, Encoding.UTF8); // url编码
        }
    }
}
