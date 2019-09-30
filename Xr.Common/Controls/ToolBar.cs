using DevExpress.Utils;
using DevExpress.Utils.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Xr.Common.Controls
{
    /// <summary>
    /// 表示工具栏控件
    /// </summary>
    public class ToolBar : Panel
    {
        /// <summary>
        /// 其他按钮
        /// </summary>
        private ToolBarButton otherBtn = new ToolBarButton();


        /// <summary>
        /// 浮动显示框
        /// </summary>
        private FlyoutControl otherflyOutPanel = new FlyoutControl();

        /// <summary>
        /// 框框
        /// </summary>
        private BorderPanel polygonPanel = new BorderPanel();
      

        /// <summary>
        /// 最大的宽度
        /// </summary>
        private int maxWidth = 20;


        public ToolBar()
        {
            this.Dock = DockStyle.Top;
            this.Height = 30;
            this.BackColor = Color.FromArgb(241, 247, 248);
            otherBtn.Text = "其他";
            otherflyOutPanel.Options.AnchorType = DevExpress.Utils.Win.PopupToolWindowAnchor.TopLeft;
            otherflyOutPanel.Options.AnimationType = PopupToolWindowAnimation.Fade;
            otherflyOutPanel.Options.VertIndent = 36;
            otherflyOutPanel.BackColor = Color.Transparent;
            otherflyOutPanel.Options.CloseOnOuterClick = true;
            polygonPanel.Dock = DockStyle.Fill;
            polygonPanel.BackColor = BackColor;
            otherflyOutPanel.Controls.Add(polygonPanel);
            otherBtn.Click += otherBtn_Click;
        }

        void otherBtn_Click(object sender, EventArgs e)
        {
            if (otherflyOutPanel.OwnerControl == null)
            {
                otherflyOutPanel.HostControl = this.FindForm();
                otherflyOutPanel.OwnerControl = this.FindForm();
                otherflyOutPanel.ParentForm = this.FindForm();
            }
            otherflyOutPanel.Options.HorzIndent = PointToScreen(otherBtn.Location).X;
            otherflyOutPanel.Options.VertIndent = PointToScreen(otherBtn.Location).Y + this.Height;
            otherflyOutPanel.ShowPopup(true); 
            otherflyOutPanel.FlyoutPanelState.Form.TopMost = true;
        }

        [DefaultValue(DockStyle.Top)]
        public override DockStyle Dock
        {
            get { return base.Dock; }
            set { base.Dock = value; }
        }

        [DefaultValue(typeof(Color), "241, 247, 248")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }
        List<Control> scsControls = new List<Control>();
        List<Control> removeControls = new List<Control>();
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.Height = 30;
            if (this.IsDesignMode()) return;
            Control temp;
            retractButtons.Clear();           
            scsControls.Clear();
            removeControls.Clear();
            foreach (Control ctl in this.Controls)
            {
                scsControls.Add(ctl);
            }
            for (int i = 0; i < scsControls.Count; i++)
            {
                for (int j = i + 1; j < scsControls.Count; j++)
                {
                    if (scsControls[j].Right < scsControls[i].Right)
                    {
                        temp = scsControls[j];
                        scsControls[j] = scsControls[i];
                        scsControls[i] = temp;
                    }
                }
            }
            for (int i = 0; i < scsControls.Count; i++)
            {
                var ctl = scsControls[i];
                if (ctl.Right > this.Width)
                {
                    if (ctl.GetType() == typeof(ToolBarButton))
                    {
                        retractButtons.Add((ToolBarButton)ctl);
                        if (ctl.Width > maxWidth) maxWidth = ctl.Width;
                    }
                    if (ctl.GetType() != typeof(ToolBarButton) && ctl.GetType() != typeof(ToolBarSeparator))
                    {
                        return;
                    }
                    removeControls.Add(ctl);
                   
                }
                else if (ctl.Right + otherBtn.Width > this.Width && i != scsControls.Count - 1)
                {
                    if (ctl.GetType() == typeof(ToolBarButton))
                    {
                        if (ctl.Width > maxWidth) maxWidth = ctl.Width;
                        retractButtons.Add((ToolBarButton)ctl);
                    }
                    if (ctl.GetType() != typeof(ToolBarButton) && ctl.GetType() != typeof(ToolBarSeparator))
                    {
                        return;
                    }
                   removeControls.Add(ctl);
                }
            }

            if (retractButtons.Count > 0)
            {
                for(int m=this.Controls.Count-1;m>=0;m--)
                {
                    if (removeControls.Contains(this.Controls[m]))
                    {
                        this.Controls.RemoveAt(m);
                    }
                }
                this.Controls.Add(otherBtn);
                this.Controls.SetChildIndex(otherBtn, 0);
                retractButtons.Reverse();
                otherflyOutPanel.Width = maxWidth;
                otherflyOutPanel.Height = 30 * retractButtons.Count+2;
                foreach (var ctl in retractButtons)
                {
                    ctl.Dock = DockStyle.Top;
                    ctl.Height = 30;
                    polygonPanel.Controls.Add(ctl);
                }
            }
           
        }

        private List<ToolBarButton> retractButtons = new List<ToolBarButton>();

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
           
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
    }
}
