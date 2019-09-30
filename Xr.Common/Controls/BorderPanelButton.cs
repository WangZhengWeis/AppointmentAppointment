using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.Utils.Editors;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace Xr.Common.Controls
{
    /// <summary>
    /// 带边框和圆角的Panel按钮
    /// </summary>
    [ToolboxItem(true)]
    public class BorderPanelButton : BorderPanel
    {
        public BorderPanelButton()
        {
            InitializeComponent();
        }

        private string _Text;
         [Description("按钮文字")]
        public  string BtnText
        {
            get { return _Text; }
            set { _Text = value; }
        }
         private Font _BtnFont=new Font("微软雅黑",12f);
        [DefaultValue(typeof(Font), "微软雅黑,12px")]
         [Description("按钮文字样式")]
         public Font BtnFont
         {
             get { return _BtnFont; }
             set { _BtnFont = value; }
         }
        private bool _CenterText = true;
        [DefaultValue(typeof(Font), "微软雅黑,12px")]
        [Description("按钮文字居中")]
        public bool CenterText
        {
            get { return _CenterText; }
            set { _CenterText = value; }
        }

        private Color _BackColor=Color.White;
        [Description("按钮背景颜色")]
        public override Color BackColor
        {
            get
            {
                return _BackColor;
            }
            set
            {
                _BackColor = value;
            }
        }
        private Color _SelctedColor = Color.FromArgb(60, 195, 245);
        [Description("按钮选中颜色")]
        public Color SelctedColor
        {
            get { return _SelctedColor; }
            set { _SelctedColor = value; }
        }
        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // BorderPanelButton
            // 
            this.Click += new System.EventHandler(this.BorderPanelButton_Click);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.BorderPanelButton_Paint);
            this.MouseEnter += new System.EventHandler(this.BorderPanelButton_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.BorderPanelButton_MouseLeave);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }
        private bool _EnableCheck=true;
        [Description("能否选择")]
        [DefaultValue(true)]
        public bool EnableCheck
        {
            get { return _EnableCheck; }
            set { _EnableCheck = value; }
        }
        private bool isCheck ;
        [DefaultValue(false)]
        [Description("是否选中")]
        public bool IsCheck
        {
            get { return isCheck; }
            set
            {
                if (this.Enabled)
                {
                    if (!this.EnableCheck) 
                    {
                        isCheck = false;
                    }
                    else
                    {
                        if (value != isCheck)
                        {
                            ChangeBackColor();
                        }
                        if (value == true)
                        {
                            if (this.Parent != null)
                            {
                                foreach (Control c in this.Parent.Controls)
                                {
                                    if (c is BorderPanelButton && c != this)
                                    {
                                        (c as BorderPanelButton).IsCheck = false;
                                    }
                                }

                            }
                        }
                        isCheck = value;
                    }
                }
                else
                {
                    isCheck = false;
                }
            }
        }
        protected void ChangeBackColor()
        {
            if (isCheck)
            {
                this.FillColor1 = _BackColor;
                this.FillColor2 = _BackColor;
                //RefreshFillBrush();
            }
            else//选中颜色
            {
                this.FillColor1 = _SelctedColor;
                this.FillColor2 = _SelctedColor;
                //RefreshFillBrush();
            }
        }
        private void BorderPanelButton_MouseEnter(object sender, EventArgs e)
        {
            if (!isCheck)
            {
                this.FillColor1 = Color.WhiteSmoke;
                this.FillColor2 = Color.WhiteSmoke;
            }
            else
            {
                this.FillColor1 = _SelctedColor;
                this.FillColor2 = _SelctedColor;
            }
        }

        private void BorderPanelButton_MouseLeave(object sender, EventArgs e)
        {
            if (!isCheck)
            {
                this.FillColor1 = _BackColor;
                this.FillColor2 = _BackColor;
            }
            else
            {
                this.FillColor1 = _SelctedColor;
                this.FillColor2 = _SelctedColor;
            }
        }

        private void BorderPanelButton_Click(object sender, EventArgs e)
        {
            if (isCheck)
            {
                if (this.Parent != null)
                {
                    foreach (Control c in this.Parent.Controls)
                    {
                        if (c is BorderPanelButton && c != this)
                        {
                            (c as BorderPanelButton).IsCheck = false;
                        }
                    }
                }
                //IsCheck = false;
            }
            else
            {
                IsCheck = true;
            }

        }

        private void BorderPanelButton_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var textSize = e.Graphics.MeasureString(this._Text, this._BtnFont);
            Brush br;
            if (IsCheck)
            {
                br = Brushes.White;
            }
            else
            {
                br = Brushes.Black; 
            }
            if (_CenterText)
            {
                e.Graphics.DrawString(this._Text, _BtnFont, br, (this.Width - (int)textSize.Width) / 2, (this.Height - (int)textSize.Height) / 2);
            }
            else
            { 
                e.Graphics.DrawString(this._Text, _BtnFont, br, 0,0);
            }
        }

    }
}
