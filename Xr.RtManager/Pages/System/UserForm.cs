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
using Xr.RtManager.Utils;
using Xr.Http;
using Xr.Common.Controls;

namespace Xr.RtManager
{
    public partial class UserForm : UserControl
    {
        private Form MainForm; //主窗体
        OpaqueCommand cmd;
        private JObject obj { get; set; }

        public UserForm()
        {
            InitializeComponent();
        }


        private void UserForm_Load(object sender, EventArgs e)
        {
            MainForm = (Form)this.Parent;
            pageControl1.MainForm = MainForm;
            //this.BackColor = Color.FromArgb(243, 243, 243);
            cmd = new OpaqueCommand(AppContext.Session.waitControl);
            pageControl1.PageSize = Convert.ToInt32(AppContext.AppConfig.pagesize);
            cmd.ShowOpaqueLayer(0f);
            // 开始异步
            BackgroundWorkerUtil.start_run(bw_DoWork, bw_RunWorkerCompleted, null, false);

           
        }
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                String url = AppContext.AppConfig.serverUrl + "sys/sysOffice/findAll";
                e.Result = HttpClass.httpPost(url);
            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
            }
        }
       
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                String data = e.Result as String;
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    //ModelHandler<OfficeEntity> m = new ModelHandler<OfficeEntity>();
                    //DataTable dt = m.FillDataTable(objT["result"].ToObject<List<OfficeEntity>>());
                    //treeCompany.Properties.DataSource = dt; //绑定数据           
                    //treeCompany.Properties.TreeList.KeyFieldName = "id";//设置ID  
                    //treeCompany.Properties.TreeList.ParentFieldName = "parentId";//设置PreID   
                    //treeCompany.Properties.DisplayMember = "name";  //要在树里展示的
                    //treeCompany.Properties.ValueMember = "id";    //对应的value

                    //treeOffice.Properties.DataSource = dt; //绑定数据           
                    //treeOffice.Properties.TreeList.KeyFieldName = "id";//设置ID  
                    //treeOffice.Properties.TreeList.ParentFieldName = "parentId";//设置PreID   
                    //treeOffice.Properties.DisplayMember = "name";  //要在树里展示的
                    //treeOffice.Properties.ValueMember = "id";    //对应的value

                    SearchData(1, 10);
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            }
            catch (Exception ex)
            {
                Xr.Log4net.LogHelper.Error(ex.Message);
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1, MainForm);
            }
        }


        int currPageNo = 1;
        int pageSize = 10;
        public void SearchData(int pageNo, int pageSize)
        {
            currPageNo = pageNo;
            this.pageSize = Convert.ToInt32(AppContext.AppConfig.pagesize);
            BackgroundWorkerUtil.start_run(grid_bw_DoWork, grid_bw_RunWorkerCompleted, null, false);
        }

        private void grid_bw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                System.Threading.Thread.Sleep(500);
                String param = "?loginName=" + tbLoginName.Text + "&&name=" + tbName.Text
                + "&&pageNo=" + currPageNo + "&&pageSize=" + pageSize + "&deptIds=" + AppContext.Session.deptIds;
                String url = AppContext.AppConfig.serverUrl + "sys/sysUser/findAll" + param;
                e.Result = HttpClass.httpPost(url);
            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
            }
        }

        private void grid_bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                String data = e.Result as String;
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    gcUser.DataSource = objT["result"]["list"].ToObject<List<UserEntity>>();
                    pageControl1.setData(int.Parse(objT["result"]["count"].ToString()),
                        int.Parse(objT["result"]["pageSize"].ToString()),
                        int.Parse(objT["result"]["pageNo"].ToString()));
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            }
            catch (Exception ex )
            {
                throw new Exception(ex.InnerException.Message);
            }
            finally
            {
                // 关闭加载提示框
                //DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
                cmd.HideOpaqueLayer();
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            // 弹出加载提示框
            //DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitingForm));
            cmd.ShowOpaqueLayer();
            SearchData(1, pageControl1.PageSize);
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            var edit = new UserEdit();
            if (edit.ShowDialog() == DialogResult.OK)
            {
                Thread.Sleep(300);
                cmd.ShowOpaqueLayer();
                SearchData(1, pageControl1.PageSize);
                MessageBoxUtils.Hint("保存成功!", MainForm);
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            var selectedRow = gridView1.GetFocusedRow() as UserEntity;
            if (selectedRow == null)
                return;
            var edit = new UserEdit();
            edit.user = selectedRow;
            edit.Text = "用户修改";
            if (edit.ShowDialog() == DialogResult.OK)
            {
                Thread.Sleep(300);
                cmd.ShowOpaqueLayer();
                SearchData(1, pageControl1.PageSize);
                MessageBoxUtils.Hint("修改用户成功!", MainForm);
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            var selectedRow = gridView1.GetFocusedRow() as UserEntity;
            if (selectedRow == null)
                return;

            if (MessageBoxUtils.Show("确定要删除吗?", MessageBoxButtons.OKCancel,
                 MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MainForm) == DialogResult.OK)
            {
                String url = AppContext.AppConfig.serverUrl + "sys/sysUser/delete?id=" + selectedRow.id;
                String data = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    SearchData(1, pageControl1.PageSize);
                    MessageBoxUtils.Hint("删除用户成功!", MainForm);
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            }
        }

        /// <summary>
        /// 用户解锁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemButtonEdit1_Click(object sender, EventArgs e)
        {
            var selectedRow = gridView1.GetFocusedRow() as UserEntity;
            if (selectedRow == null)
                return;
            if (selectedRow.isLocked.Equals("0"))
            {
                MessageBoxUtils.Hint("该用户没有被锁定，无需解锁!", MainForm);
                return;
            }
            if (!AppContext.Session.userType.Equals("1"))
            {
                MessageBoxUtils.Hint("当前用户不是管理员用户，没有解锁的权限!",
                    HintMessageBoxIcon.Error, MainForm);
                return;
            }
            MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
            DialogResult dr = MessageBox.Show("确定要解锁该用户吗?", "解锁用户", messButton);

            if (dr == DialogResult.OK)
            {
                String url = AppContext.AppConfig.serverUrl + "sys/sysUser/userUnlock?id=" + selectedRow.id;
                String data = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    MessageBoxUtils.Hint("解锁用户成功!", MainForm);
                    SearchData(1, pageControl1.PageSize);
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            }
        }

        private void pageControl1_Query(int CurrentPage, int PageSize)
        {
            cmd.ShowOpaqueLayer();
            SearchData(CurrentPage, PageSize);
        }

        private void UserForm_Resize(object sender, EventArgs e)
        {
            if (cmd == null)
                cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            cmd.rectDisplay = this.DisplayRectangle;
        }

        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.Column.Caption != "操作")
            {
                var selectedRow = gridView1.GetFocusedRow() as UserEntity;
                if (selectedRow == null)
                    return;
                var edit = new UserEdit();
                edit.user = selectedRow;
                edit.Text = "用户修改";
                if (edit.ShowDialog() == DialogResult.OK)
                {
                    Thread.Sleep(300);
                    cmd.ShowOpaqueLayer();
                    SearchData(1, pageControl1.PageSize);
                    MessageBoxUtils.Hint("修改用户成功!", MainForm);
                }
            }
        }

        private void repositoryItemButtonEdit2_Click(object sender, EventArgs e)
        {
            var selectedRow = gridView1.GetFocusedRow() as UserEntity;
            if (selectedRow == null)
                return;

            if (MessageBoxUtils.Show("确定要删除吗?", MessageBoxButtons.OKCancel,
                 MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MainForm) == DialogResult.OK)
            {
                String url = AppContext.AppConfig.serverUrl + "sys/sysUser/delete?id=" + selectedRow.id;
                String data = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    SearchData(1, pageControl1.PageSize);
                    MessageBoxUtils.Hint("删除用户成功!", MainForm);
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            }
        }
    }
}
