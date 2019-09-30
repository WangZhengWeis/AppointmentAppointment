using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Xr.Common.Controls
{
    public partial class PageControl : UserControl
    {
        public PageControl()
        {
            InitializeComponent();
            tePageSize.Text = pageSize.ToString();
        }

        public Form MainForm;

        private int record = 0;

        //事件处理函数形式，用delegate定义
        public delegate void QueryDelegate(int CurrentPage, int PageSize);
        /// <summary>
        /// 首页、尾页、下一页、上一页、跳转所要查询的事件
        /// </summary>
        public event QueryDelegate Query;

        private void ExecuteQuery(int CurrentPage)
        {
            teJumpPage.Text = currentPage.ToString();
            if (Query != null)
            {
                if (tePageSize.Text.Length > 0)
                {
                    this.pageSize = int.Parse(tePageSize.Text);
                }
                else
                {
                    this.pageSize = 10;
                }
                Query(currentPage, pageSize);
            }
        }

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

        private int pageSize = 10;

        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize
        {
            get 
            {
                pageSize = int.Parse(tePageSize.Text);
                return pageSize; 
            }
            set 
            { 
                pageSize = value;
                setPageSize();
            }
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
        /// 修改界面显示的每页最大行数
        /// </summary>
        private void setPageSize()
        {
            tePageSize.Text = pageSize.ToString();
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
                    MessageBoxUtils.Hint("当前已经是首页", HintMessageBoxIcon.Error, MainForm);
                    return;
                }
                else
                {
                    CurrentPage = 1;
                    ExecuteQuery(currentPage);
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
                    MessageBoxUtils.Hint("当前已经是首页", HintMessageBoxIcon.Error, MainForm);
                    return;
                }
                else
                {
                    CurrentPage = CurrentPage - 1;
                    ExecuteQuery(currentPage);
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
                    MessageBoxUtils.Hint("当前已经是末页", HintMessageBoxIcon.Error, MainForm);
                    return;
                }
                else
                {
                    CurrentPage = CurrentPage + 1;
                    ExecuteQuery(currentPage);
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
                    MessageBoxUtils.Hint("当前已经是末页", HintMessageBoxIcon.Error, MainForm);
                    return;
                }
                else
                {
                    CurrentPage = PageNum;
                    ExecuteQuery(currentPage);
                }
            }
        }

        private void InitPageInfo()
        {
            if (Record == 0 || (Record > 0 && CurrentPage > pageNum))
            {
                CurrentPage = 1;
            }
            labelControl1.Text = string.Format("共 {0} 页，共 {1} 条", PageNum, Record);
            //labelControl1.Text = CurrentPage.ToString();
        }

        /// <summary>
        /// 赋值总记录数、总页数、当前页
        /// </summary>
        /// <param name="Record">总记录数</param>
        /// <param name="PageSize">每页显示的行数</param>
        /// <param name="CurrentPage">当前页</param>
        public void setData(int Record, int PageSize, int CurrentPage)
        {
            this.Record = Record;
            this.PageSize = PageSize;
            this.CurrentPage = CurrentPage;
            InitPageInfo();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)13 && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void btnJump_Click(object sender, EventArgs e)
        {
            int count = 0;
            if (teJumpPage.Text.Trim().Length == 0) return;
            else
                count = int.Parse(teJumpPage.Text.Trim());
            if (count <= 0)
            {
                return;
            }
            if (count > PageNum)
            {
                MessageBoxUtils.Hint("跳转页码大于总页数", HintMessageBoxIcon.Error, MainForm);
                return;
            }

            CurrentPage = count;
            ExecuteQuery(currentPage);
        }
    }
}
