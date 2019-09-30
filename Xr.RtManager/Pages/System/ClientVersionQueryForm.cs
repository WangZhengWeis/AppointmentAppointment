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

namespace Xr.RtManager
{
    public partial class ClientVersionQueryForm : UserControl
    {
        public ClientVersionQueryForm()
        {
            InitializeComponent();
        }

        private Form MainForm; //主窗体
        private OpaqueCommand cmd;
        private JObject obj { get; set; }

        private void ClientVersionQueryForm_Load(object sender, EventArgs e)
        {
            MainForm = (Form)this.Parent;
            pageControl1.MainForm = MainForm;
            cmd = new OpaqueCommand(AppContext.Session.waitControl);
            //cmd.ShowOpaqueLayer(0f);
            pageControl1.PageSize = Convert.ToInt32(AppContext.AppConfig.pagesize);
            String url = AppContext.AppConfig.serverUrl + "sys/sysDict/findByType?type=client_version_type";

            this.DoWorkAsync(500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (r) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                JObject objT = JObject.Parse(r.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    List<DictEntity> dictList = objT["result"].ToObject<List<DictEntity>>();
                    DictEntity dict = new DictEntity();
                    dict.label = "全部";
                    dict.value = "`";
                    dictList.Insert(0, dict);
                    lueType.Properties.DataSource = dictList;
                    lueType.Properties.DisplayMember = "label";
                    lueType.Properties.ValueMember = "value";
                    lueType.ItemIndex = 0;
                    SearchData(true, 1, pageControl1.PageSize);
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            });
        }

        public void SearchData(bool flag, int pageNo, int pageSize)
        {
            String type = "";
            if (!lueType.EditValue.ToString().Equals("`"))
                type = lueType.EditValue.ToString();
            String param = "?title=" + tbTitle.Text
                + "&&version=" + tbVersion.Text + "&&pageNo=" + pageNo
                + "&&pageSize=" + pageSize + "&&type=" + type;
            String url = AppContext.AppConfig.serverUrl + "sys/clientVersion/listTwo" + param;

            this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (r) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                cmd.HideOpaqueLayer();
                JObject objT = JObject.Parse(r.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    gcDict.DataSource = objT["result"][0]["list"].ToObject<List<ClientVersionEntity>>();
                    pageControl1.setData(int.Parse(objT["result"][0]["count"].ToString()),
                    int.Parse(objT["result"][0]["pageSize"].ToString()),
                    int.Parse(objT["result"][0]["pageNo"].ToString()));
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, 
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            });
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            cmd.ShowOpaqueLayer(255, true);
            SearchData(false, 1, pageControl1.PageSize);
        }

        private void pageControl1_Query(int CurrentPage, int pageSize)
        {
            cmd.ShowOpaqueLayer(255, true);
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

        private void ClientVersionForm_Resize(object sender, EventArgs e)
        {
            if (cmd == null)
                cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            cmd.rectDisplay = this.DisplayRectangle;
        }

        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            var selectedRow = gridView1.GetFocusedRow() as ClientVersionEntity;
            if (selectedRow == null)
                return;
            var edit = new ClientVersionQueryEdit();
            edit.clientVersion = selectedRow;
            edit.Text = "更新公告内容";
            edit.ShowDialog();
        }

    }
}
