using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Xr.RtManager.Pages.triage
{
    public partial class CheckInAgain : Form
    {
        public CheckInAgain()
        {
            InitializeComponent();
            IsCheckAgain = "";
        }
        /// <summary>
        /// 是否直接复诊签到(0--抽血取号，1--复诊取号)
        /// </summary>
        public static String IsCheckAgain { get; set; }
        /// <summary>
        /// 抽血取号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonControl1_Click(object sender, EventArgs e)
        {
            IsCheckAgain = "0";
            this.Close();
        }
        /// <summary>
        /// 复诊签到
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonControl2_Click(object sender, EventArgs e)
        {
            IsCheckAgain = "1";
            this.Close();
        }
    }
}
