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
    public partial class SchedulingSuspensionLogForm : UserControl
    {
        public SchedulingSuspensionLogForm()
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
            pageControl1.PageSize = Convert.ToInt32(AppContext.AppConfig.pagesize);
            cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            deBegin.EditValue = DateTime.Now.ToString("yyyy-MM-dd");
            deEnd.EditValue = DateTime.Now.ToString("yyyy-MM-dd");
            //获取可操作科室
            String url = AppContext.AppConfig.serverUrl + "cms/dept/qureyOperateDept";
            this.DoWorkAsync(500, (o) =>
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) =>
            {
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    List<DeptEntity> deptList = objT["result"]["deptList"].ToObject<List<DeptEntity>>();
                    //DeptEntity dept = new DeptEntity();
                    //dept.id = "`";
                    //dept.name = "全部";
                    //deptList.Insert(0, dept);
                    treeDept.Properties.DataSource = deptList;
                    treeDept.Properties.TreeList.KeyFieldName = "id";
                    treeDept.Properties.TreeList.ParentFieldName = "parentId";
                    treeDept.Properties.DisplayMember = "name";
                    treeDept.Properties.ValueMember = "id";
                    treeDept.EditValue = deptList[0].id;

                    SearchData(true, 1, pageControl1.PageSize);
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                    return;
                }
            });
        }

        public void SearchData(bool flag, int pageNo, int pageSize)
        {
            Object deptId = treeDept.EditValue;
            if (deptId.Equals("`"))
                deptId = "";
            String param = "?deptId=" + deptId
                + "&&doctorId=" + lueDoctor.EditValue + "&&beginTime=" + deBegin.Text
                + "&&endTime=" + deEnd.Text
                + "&&pageNo=" + pageNo + "&&pageSize=" + pageSize;
            String url = AppContext.AppConfig.serverUrl + "/sch/SchPlanModify"+param;
            this.DoWorkAsync(500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    gcDict.DataSource = objT["result"]["list"].ToObject<List<SchedulingSuspensionLogEntity>>();
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

        private void treeDept_EditValueChanged(object sender, EventArgs e)
        {
            Object deptId = treeDept.EditValue;
            if (deptId.Equals("`"))
                deptId = "";
            String param = "pageNo=1&pageSize=10000&hospital.id=" + AppContext.Session.hospitalId + "&dept.id=" + deptId;
            String url = AppContext.AppConfig.serverUrl + "cms/doctor/list?" + param;
            String data = HttpClass.httpPost(url);
            JObject objT = JObject.Parse(data);
            if (string.Compare(objT["state"].ToString(), "true", true) == 0)
            {
                List<DoctorInfoEntity> doctorList = objT["result"]["list"].ToObject<List<DoctorInfoEntity>>();
                DoctorInfoEntity doctor = new DoctorInfoEntity();
                doctor.id = "";
                doctor.name = "全部医生";
                doctorList.Insert(0, doctor);
                lueDoctor.Properties.DataSource = doctorList;
                lueDoctor.Properties.DisplayMember = "name";
                lueDoctor.Properties.ValueMember = "id";
                lueDoctor.ItemIndex = 0;
            }
            else
            {
                MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                return;
            }
        }
        
    }
}
