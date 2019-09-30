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
using System.Text.RegularExpressions;

namespace Xr.RtManager.Pages.scheduling
{
    public partial class AddRowNumberEdit : Form
    {
        public AddRowNumberEdit()
        {
            InitializeComponent();
        }

        Xr.Common.Controls.OpaqueCommand cmd;

        public ScheduledEntity scheduled { get; set; }

        private void AddRowNumberEdit_Load(object sender, EventArgs e)
        {
            cmd = new Xr.Common.Controls.OpaqueCommand(this);
            String sdName = "";
            if (scheduled.period.Equals("0")) sdName = "上午";
            if (scheduled.period.Equals("1")) sdName = "下午";
            if (scheduled.period.Equals("2")) sdName = "晚上";
            if (scheduled.period.Equals("3")) sdName = "全天";
            label1.Text = "科室：" + scheduled.deptName + "        医生：" + scheduled.doctorName + "        日期：" + scheduled.workDate + sdName;
            String param = "hospitalId=" + AppContext.Session.hospitalId + "&deptId=" + scheduled.deptId
                + "&doctorId=" + scheduled.doctorId + "&workDate=" + scheduled.workDate
                + "&period=" + scheduled.period;
            String url = AppContext.AppConfig.serverUrl + "sch/doctorScheduPlan/getLaveTime?" + param;
            cmd.ShowOpaqueLayer(0f);
            this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    JArray dyList = JArray.Parse(objT["result"].ToString());
                    List<SourceDataEntity> sdList = new List<SourceDataEntity>();
                    for (int i = 0; i < dyList.Count; i++)
                    {
                        JArray arr = JArray.Parse(dyList[i].ToString());
                        SourceDataEntity sd = new SourceDataEntity();
                        sd.workDate = arr[0].ToString();
                        sd.beginTime = arr[1].ToString();
                        sd.endTime = arr[2].ToString();
                        sd.mzType = "1";
                        sd.numSite = "0";
                        sd.numOpen = "0";
                        sd.numClinic = "0";
                        sd.numYj = "0";
                        sdList.Add(sd);
                    }
                    gcSourceData.DataSource = sdList;
                    cmd.HideOpaqueLayer();
                }
                else
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                    if (objT["message"].ToString().Equals("只能对当前时段进行增加排班号源"))
                    {
                        DialogResult = DialogResult.Cancel;
                    }
                    return;
                }
            });
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            List<SourceDataEntity> sourceDataList = gcSourceData.DataSource as List<SourceDataEntity>;
            List<SourceDataEntity> selectSourceDataList = new List<SourceDataEntity>();
            foreach (SourceDataEntity sd in sourceDataList)
            {
                if (!sd.numSite.Equals("0") || !sd.numOpen.Equals("0") || !sd.numClinic.Equals("0"))
                {
                    sd.mzType = "1";
                    selectSourceDataList.Add(sd);
                }
            }
            String scheduSets = Newtonsoft.Json.JsonConvert.SerializeObject(selectSourceDataList);
            String param = "hospitalId=" + AppContext.Session.hospitalId + "&deptId=" + scheduled.deptId
                + "&doctorId=" + scheduled.doctorId// + "&workDate=" + scheduled.workDate
                + "&period=" + scheduled.period + "&scheduSets=" + scheduSets;
            String url = AppContext.AppConfig.serverUrl + "sch/doctorScheduPlan/addLaveScheduList?" + param;
            cmd.ShowOpaqueLayer();
            this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    cmd.HideOpaqueLayer();
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                    return;
                }
            });
        }

        private void btnCancel_Click(object sender, EventArgs e)
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

        private void gridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            //正整数验证 @"^[1-9]\d*$"
            var regex = new Regex(@"^[0-9]*[0-9][0-9]*$", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            if (!regex.IsMatch(e.Value.ToString()))
            {
                e.ErrorText = "只能输入正整数！";
                e.Valid = false;
                return;
            }
        }
        bool firstfootcell = true;
        private void gcSourceData_CustomDrawFooter(object sender, DevExpress.XtraGrid.Views.Base.RowObjectCustomDrawEventArgs e)
        {
            Rectangle r = e.Bounds;
            Brush brush = e.Cache.GetGradientBrush(e.Bounds, Color.Transparent, Color.Transparent, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
            e.Graphics.FillRectangle(brush, r);
            e.Handled = true;
        }
        /// <summary>
        /// 自定义表格尾部数据统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            int dx = e.Bounds.Height;
            //Brush brush = e.Cache.GetGradientBrush(e.Bounds, Color.Transparent, Color.Transparent, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
            Rectangle r = e.Bounds;
            r.Inflate(1, -1);
            //r.Inflate(0, -1);
            Pen p = new Pen(Color.LightGray);
            Brush brush = e.Cache.GetGradientBrush(e.Bounds, Color.WhiteSmoke, Color.WhiteSmoke, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
            e.Graphics.FillRectangle(brush, r);
            e.Graphics.DrawRectangle(p, r.X - 1, r.Y, r.Width, r.Height);
            //e.Graphics.DrawRectangle(p, r.X - 1, r.Y, r.Width+1, r.Height);
            //ControlPaint.DrawBorder(e.Graphics, r, Color.Gray, ButtonBorderStyle.Solid);
            //r.Inflate(-1, -1);
            //e.Graphics.FillRectangle(brush, r);
            //r.Inflate(-2, 0);
            e.Appearance.DrawString(e.Cache, e.Info.DisplayText, r);
            if (firstfootcell)
            {
                //e.Graphics.DrawRectangle(p, r.X-1, r.Y, r.Width, r.Height);
                e.Appearance.DrawString(e.Cache, "总计", r);

                firstfootcell = false;
            }
            e.Handled = true;
        }
        private void gcSourceData_Paint(object sender, PaintEventArgs e)
        {
            firstfootcell = true;
        }
    }
}
