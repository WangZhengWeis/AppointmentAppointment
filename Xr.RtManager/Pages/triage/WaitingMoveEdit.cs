using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using Newtonsoft.Json.Linq;
using Xr.Http;
using System.IO;
using System.Net;
using Xr.Common;
using System.Threading;

namespace Xr.RtManager.Pages.triage
{
    public partial class WaitingMoveEdit : Form
    {
        public WaitingMoveEdit()
        {
            InitializeComponent();
        }

        Xr.Common.Controls.OpaqueCommand cmd;

        public bool flag { get; set; } //true：下移 false：上移

        public String triageId { get; set; }

        private void WaitingMoveEdit_Load(object sender, EventArgs e)
        {
            if (flag)
            {
                label1.Text = "下移";
            }
            else
            {
                label1.Text = "上移";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (textEdit1.Text.Length == 0 || int.Parse(textEdit1.Text)==0)
            {
                DialogResult = DialogResult.Cancel;
                return;
            }
            //cmd = new Xr.Common.Controls.OpaqueCommand(this);
            String param = "triageId=" + triageId + "&flag=" + flag + "&moveNum=" + textEdit1.Text;
            String url = AppContext.AppConfig.serverUrl + "sch/registerTriage/updateWaitingSort?" + param;
            String data = HttpClass.httpPost(url);
            JObject objT = JObject.Parse(data.ToString());
            if (string.Compare(objT["state"].ToString(), "true", true) == 0)
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                return;
            }
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


        
    }
}
