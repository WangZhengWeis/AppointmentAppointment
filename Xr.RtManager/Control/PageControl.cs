using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Xr.Common;

namespace Xr.RtManager.Control
{
    public partial class PageControl : UserControl
    {
        public PageControl()
        {
            InitializeComponent();
        }

        public string parentName = null;

        private int record = 0;

        /// <summary>
        /// 总记录数
        /// </summary>
        public int Record
        {
            get { return record; }
            set
            {
                record = value;
                //InitPageInfo();
            }
        }

        private int pageSize = 20;

        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        private int currentPage = 1;

        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage
        {
            get { return currentPage; }
            set { currentPage = value; }
        }

        public int pageNum = 0;

        /// <summary>
        /// 总页码
        /// </summary>
        public int PageNum
        {
            get
            {
                if (Record == 0)
                {
                    pageNum = 0;
                }
                else
                {
                    if (Record % PageSize > 0)
                    {
                        pageNum = Record / PageSize + 1;
                    }
                    else
                    {
                        pageNum = Record / PageSize;
                    }
                }
                return pageNum;
            }

        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFirst_Click(object sender, EventArgs e)
        {
            if (Record > 0)
            {
                if (CurrentPage == 1)
                {
                    MessageBoxUtils.Hint("当前已经是首页", HintMessageBoxIcon.Error);
                    return;
                }
                else
                {
                    CurrentPage = 1;
                    buttonEvent();
                }
            }

        }
        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPre_Click(object sender, EventArgs e)
        {
            if (Record > 0)
            {
                if (CurrentPage == 1)
                {
                    MessageBoxUtils.Hint("当前已经是首页", HintMessageBoxIcon.Error);
                    return;
                }
                else
                {
                    CurrentPage = CurrentPage - 1;
                    buttonEvent();
                }
            }
        }
        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (Record > 0)
            {
                if (CurrentPage == PageNum)
                {
                    MessageBoxUtils.Hint("当前已经是末页", HintMessageBoxIcon.Error);
                    return;
                }
                else
                {
                    CurrentPage = CurrentPage + 1;
                    buttonEvent();
                }
            }
        }
        /// <summary>
        /// 末页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLast_Click(object sender, EventArgs e)
        {
            if (Record > 0)
            {
                if (CurrentPage == PageNum)
                {
                    MessageBoxUtils.Hint("当前已经是末页", HintMessageBoxIcon.Error);
                    return;
                }
                else
                {
                    CurrentPage = PageNum;
                    buttonEvent();
                }
            }
        }

        private void InitPageInfo()
        {
            if (Record == 0 || (Record > 0 && CurrentPage > pageNum))
            {
                CurrentPage = 1;
            }
            labelControl1.Text = string.Format("共 {0} 条记录  共 {1} 页  当前第 {2} 页", Record, PageNum, CurrentPage);
            //labelControl1.Text = CurrentPage.ToString();
        }

        /// <summary>
        /// 赋值总记录数、总页数、当前页
        /// </summary>
        /// <param name="Record">总记录数</param>
        /// <param name="PageNum">总页数</param>
        /// <param name="CurrentPage">当前页</param>
        public void setData(int Record, int PageSize, int CurrentPage)
        {
            this.Record = Record;
            this.PageSize = PageSize;
            this.CurrentPage = CurrentPage;
            InitPageInfo();
        }

        private void buttonEvent()
        {
            if (parentName != null)
            {
                // 弹出加载提示框
                //DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitingForm));

                if (parentName.Equals("UserForm"))
                {
                    UserForm form = this.Parent as UserForm;
                    form.SearchData(CurrentPage);
                }
                else if (parentName.Equals("DictForm"))
                {
                    DictForm form = this.Parent as DictForm;
                    form.SearchData(false, CurrentPage);
                }
                else if (parentName.Equals("LogForm"))
                {
                    LogForm form = this.Parent as LogForm;
                    form.SearchData(false, CurrentPage);
                }
                else if (parentName.Equals("ClientVersionForm"))
                {
                    ClientVersionForm form = this.Parent as ClientVersionForm;
                    form.SearchData(false, CurrentPage);
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)13 && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int count = 0;
            if (textBox1.Text.Trim().Length == 0) return;
            else
                count = int.Parse(textBox1.Text.Trim());
            if (count <= 0)
            {
                return;
            }
            if (count > PageNum)
            {
                MessageBoxUtils.Hint("跳转页码大于总页数", HintMessageBoxIcon.Error);
                return;
            }

            CurrentPage = count;
            buttonEvent();
        }
    }
}
