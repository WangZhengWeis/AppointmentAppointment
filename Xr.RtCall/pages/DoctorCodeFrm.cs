using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Xr.RtCall.Model;

namespace Xr.RtCall.pages
{
    /// <summary>
    /// 如果打开第二个叫号需要输入医生工号来启动
    /// </summary>
    public partial class DoctorCodeFrm : Form
    {
        public DoctorCodeFrm()
        {
            InitializeComponent();
        }
        #region 确定按钮
        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonControl1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text.Trim() == "")
            {
                MessageBox.Show("医生工号不能为空");
                return;
            }
            Xr.RtCall.Model.IsOpen.IsOpenOrClose = true;
            HelperClass.Code = this.textBox1.Text.Trim();
            Xr.RtCall.Model.AppContext.Load();
            Form1 ttf = new Form1();
            this.Hide();
            ttf.Show();
        }
        #endregion
        #region 取消按钮
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonControl2_Click(object sender, EventArgs e)
        {
            Xr.RtCall.Model.IsOpen.IsOpenOrClose = false;
            this.Close();
        }
        #endregion
    }
}
