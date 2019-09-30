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
using System.Runtime.InteropServices;
using Xr.Http;

namespace Xr.RtManager
{
    public partial class LogForm : UserControl
    {
        //[DllImport("user32.dll")]
        //public static extern IntPtr GetActiveWindow();

        //[DllImport("user32.dll")]
        //public static extern IntPtr SetActiveWindow(IntPtr hwnd);

        public LogForm()
        {
            InitializeComponent();
        }

        private Form MainForm; //主窗体
        Xr.Common.Controls.OpaqueCommand cmd;
        private JObject obj { get; set; }

        private void LogForm_Load(object sender, EventArgs e)
        {
            MainForm = (Form)this.Parent;
            pageControl1.MainForm = MainForm;
            cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            cmd.ShowOpaqueLayer(0f);
            pageControl1.PageSize = Convert.ToInt32(AppContext.AppConfig.pagesize);
            String url = AppContext.AppConfig.serverUrl + "sys/sysOffice/findAll";
            this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    List<OfficeEntity> officeList = objT["result"].ToObject<List<OfficeEntity>>();
                    treeCompany.Properties.DataSource = officeList; //绑定数据           
                    treeCompany.Properties.TreeList.KeyFieldName = "id";//设置ID  
                    treeCompany.Properties.TreeList.ParentFieldName = "parentId";//设置PreID   
                    treeCompany.Properties.DisplayMember = "name";  //要在树里展示的
                    treeCompany.Properties.ValueMember = "id";    //对应的value

                    treeOffice.Properties.DataSource = officeList; //绑定数据           
                    treeOffice.Properties.TreeList.KeyFieldName = "id";//设置ID  
                    treeOffice.Properties.TreeList.ParentFieldName = "parentId";//设置PreID   
                    treeOffice.Properties.DisplayMember = "name";  //要在树里展示的
                    treeOffice.Properties.ValueMember = "id";    //对应的value

                    SearchData(true, 1, pageControl1.PageSize);
                }
                else
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            });
        }

        public void SearchData(bool flag, int pageNo, int pageSize)
        {
            String exception = "";
            if (checkEdit1.Checked) exception = "true"; //不为空白字符串就行
            String param = "?companyId=" + treeCompany.EditValue
                + "&&officeId=" + treeOffice.EditValue + "&&createByName=" + tbUserName.Text
                + "&&requestUri=" + tbURL.Text + "&&exception=" + exception
                + "&&beginDate=" + deStart.EditValue + "&&endDate=" + deEnd.EditValue
                + "&&pageNo=" + pageNo + "&&pageSize=" + pageSize;
            String url = AppContext.AppConfig.serverUrl + "sys/sysLog/findAll" + param;
            this.DoWorkAsync(500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    gcDict.DataSource = objT["result"]["list"].ToObject<List<LogEntity>>();
                    pageControl1.setData(int.Parse(objT["result"]["count"].ToString()),
                    int.Parse(objT["result"]["pageSize"].ToString()),
                    int.Parse(objT["result"]["pageNo"].ToString()));
                    cmd.HideOpaqueLayer();
                }
                else
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            });
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            cmd.ShowOpaqueLayer();
            SearchData(false, 1, pageControl1.PageSize);
        }

        private void repositoryItemButtonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            var selectedRow = gridView1.GetFocusedRow() as LogEntity;
            if (selectedRow == null)
                return;
            String text = "";
            text += "用户代理:"+selectedRow.userAgent + "\r\n";
            text += "提交参数:"+selectedRow.param;
            if (checkEdit1.CheckState == CheckState.Checked)
            {
                text += "\r\n" + "异常信息" + selectedRow.exception;
            }
            //int i = gridView1.GetFocusedDataSourceRowIndex();
            ////IntPtr activeForm = GetActiveWindow(); // 先得到当前的活动窗体 
            //MainForm mainForm = (MainForm)this.Parent.Parent.Parent.Parent.Parent;
            //MessageBoxUtils.HintTextEdit(text, mainForm.Left + 177, mainForm.Top+195 + ((i + 1) * 29), gcDict.Width);
            //SetActiveWindow(activeForm); // 在把焦点还给之前的活动窗体
            var edit = new LogMsgForm();
            edit.text = text;
            edit.ShowDialog();
        }

        private void pageControl1_Query(int CurrentPage, int pageSize)
        {
            cmd.ShowOpaqueLayer();
            SearchData(false, CurrentPage, pageSize);
        }

        /// <summary>
        /// 多线程异步后台处理某些耗时的数据，不会卡死界面
        /// </summary>
        /// <param name="time">线程延迟多少</param>
        /// <param name="workFunc">Func委托，包装耗时处理（不含UI界面处理），示例：(o)=>{ 具体耗时逻辑; return 处理的结果数据 }</param>
        /// <param name="funcArg">Func委托参数，用于跨线程传递给耗时处理逻辑所需要的对象，示例：String对象、JObject对象或DataTable等任何一个值</param>
        /// <param name="workCompleted">Action委托，包装耗时处理完成后，下步操作（一般是更新界面的数据或UI控件），示列：(r)=>{ datagirdview1.DataSource=r; }</param>
        protected void DoWorkAsync(int time, Func<object, object> workFunc, object funcArg = null, Action<object> workCompleted = null)
        {
            var bgWorkder = new BackgroundWorker();


            //Form loadingForm = null;
            //System.Windows.Forms.Control loadingPan = null;
            bgWorkder.WorkerReportsProgress = true;
            bgWorkder.ProgressChanged += (s, arg) =>
            {
                if (arg.ProgressPercentage > 1) return;

            };

            bgWorkder.RunWorkerCompleted += (s, arg) =>
            {

                try
                {
                    bgWorkder.Dispose();

                    if (workCompleted != null)
                    {
                        workCompleted(arg.Result);
                    }
                }
                catch (Exception ex)
                {
                    cmd.HideOpaqueLayer();
                    if (ex.InnerException != null)
                        throw new Exception(ex.InnerException.Message);
                    else
                        throw new Exception(ex.Message);
                }
            };

            bgWorkder.DoWork += (s, arg) =>
            {
                bgWorkder.ReportProgress(1);
                var result = workFunc(arg.Argument);
                arg.Result = result;
                bgWorkder.ReportProgress(100);
                Thread.Sleep(time);
            };

            bgWorkder.RunWorkerAsync(funcArg);
        }

        private void LogForm_Resize(object sender, EventArgs e)
        {
            if (cmd == null)
                cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            cmd.rectDisplay = this.DisplayRectangle;
        }
        
    }
}
