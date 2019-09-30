using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Configuration;
using Newtonsoft.Json.Linq;
using Xr.Common;
using Xr.Http;
using Xr.Common.Controls;
using DevExpress.Utils.Drawing;

namespace Xr.RtManager
{
    public partial class AppointmentReportForm : UserControl
    {
        Xr.Common.Controls.OpaqueCommand cmd;
        public AppointmentReportForm()
        {
            InitializeComponent();
            //cmd = new Xr.Common.Controls.OpaqueCommand(this);
            //cmd.ShowOpaqueLayer(225, true);
        }

        private JObject obj { get; set; }

        private void UserForm_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(243, 243, 243);
            SearchData(true, 1);

        }

        public void SearchData(bool flag, int pageNo)
        {
          
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            SearchData(false, 1);
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            var edit = new ClientVersionEdit();
            if (edit.ShowDialog() == DialogResult.OK)
            {
                MessageBoxUtils.Hint("保存成功!");
                SearchData(true, 1);
            }
        }
        
        private void btnDel_Click(object sender, EventArgs e)
        {


            //BorderPanel m_OpaqueLayer = new BorderPanel();
            //m_OpaqueLayer.Size = this.Size;
            //m_OpaqueLayer.BringToFront();
            //this.Controls.Add(m_OpaqueLayer);

        }

        private void btnUp_Click(object sender, EventArgs e)
        {
           
        }

        private void borderPanelButton8_Click(object sender, EventArgs e)
        {
            BorderPanelButton bpb = sender as BorderPanelButton;
            MessageBox.Show(bpb.BtnText);
        }

        private void buttonControl10_Click(object sender, EventArgs e)
        {
            ButtonControl bc = sender as ButtonControl;
            contextMenuStrip1.Show(bc,0, 0);
        }
        /// <summary>
        /// 自定义表头颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bandedGridView1_CustomDrawBandHeader(object sender, DevExpress.XtraGrid.Views.BandedGrid.BandHeaderCustomDrawEventArgs e)
        {
              //背景颜色没有设置且为空，则默认
            if (e.Band.AppearanceHeader == null || (e.Band.AppearanceHeader.BackColor == Color.Empty && !e.Band.AppearanceHeader.Options.UseBackColor))
               return;
           Rectangle rect = e.Bounds;
           rect.Inflate(-1, -1);
           // 填充标题颜色.
           e.Graphics.FillRectangle(new SolidBrush(e.Band.AppearanceHeader.BackColor), rect);
           e.Appearance.DrawString(e.Cache, e.Info.Caption, e.Info.CaptionRect);
           // 绘制过滤和排序按钮.
           foreach (DrawElementInfo info in e.Info.InnerElements)
           {
               if (!info.Visible) continue;
               ObjectPainter.DrawObject(e.Cache, info.ElementPainter, info.ElementInfo);
           }
           e.Handled = true;
        }


    }
}
