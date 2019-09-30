using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Xr.Common;
using Xr.Common.Controls;
using Xr.Http;

namespace Xr.RtManager
{
    public partial class SetUpForm : Form
    {
        public SetUpForm()
        {
            InitializeComponent();
        }

        private AppConfigEntity config = new AppConfigEntity();
        private OpaqueCommand cmd;

        private void SetUpForm_Load(object sender, EventArgs e)
        {
            dcConfig.DataType = typeof(AppConfigEntity);
            cmd = new Xr.Common.Controls.OpaqueCommand(this);
            cmd.ShowOpaqueLayer(0f);
            //查询医院下拉框数据
            String url = AppContext.AppConfig.serverUrl + "itf/intro/hospital/findAll";
            this.DoWorkAsync(500, (o) =>
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) =>
            {
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    lueHospital.Properties.DataSource = objT["result"].ToObject<List<HospitalInfoEntity>>();
                    lueHospital.Properties.DisplayMember = "name";
                    lueHospital.Properties.ValueMember = "code";
                    config = AppContext.AppConfig;
                    dcConfig.SetValue(config);
                    cmd.HideOpaqueLayer();
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                    return;
                }
            });
        }

        /// <summary>
        /// 修改配置文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!dcConfig.Validate())
            {
                return;
            }
            dcConfig.GetValue(config);
            config.firstStart = "0";
            AppContext.updateAppConfig(config);
            DialogResult = DialogResult.OK;
        }

        private void buttonControl1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
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
                    if(ex.InnerException!=null)
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
