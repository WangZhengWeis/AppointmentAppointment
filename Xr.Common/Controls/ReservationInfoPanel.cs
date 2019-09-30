using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Xr.Common.Controls
{
    public partial class ReservationInfoPanel : UserControl
    {
        public ReservationInfoPanel()
        {
            InitializeComponent();
        }

        public Object obj { get; set; }

        //事件处理函数形式，用delegate定义
        public delegate void OperationClick(object sender, EventArgs e, Object obj);
        public  event OperationClick BtnOperationClick;

        public delegate void BuDaClick(object sender, EventArgs e, Object obj);
        public event BuDaClick BtnBuDaClick;

        private void btn_Operation_Click(object sender, EventArgs e)
        {
            BtnOperationClick(sender, new EventArgs(), obj);
        }

        private void btn_BuDa_Click(object sender, EventArgs e)
        {
            BtnBuDaClick(sender, new EventArgs(), obj);
        }
    }
}
