using DevExpress.XtraEditors;
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
using Xr.Http;

namespace Xr.RtManager.Pages.triage
{
    public partial class TopShiftForm : Form
    {
        /// <summary>
        /// 出诊信息模板
        /// </summary>
        public DefaultVisitEntity defaultVisitTemplate { get; set; }

        Xr.Common.Controls.OpaqueCommand cmd;

        public TopShiftForm()
        {
            InitializeComponent();
        }

        private void TopShiftForm_Load(object sender, EventArgs e)
        {
            cmd = new Xr.Common.Controls.OpaqueCommand(this);
            GetKeShi();
            //午别下拉框数据
            List<DictEntity> dictList = new List<DictEntity>();
            DictEntity dict = new DictEntity();
            dict.value = "0";
            dict.label = "上午";
            dictList.Add(dict);
            dict = new DictEntity();
            dict.value = "1";
            dict.label = "下午";
            dictList.Add(dict);
            dict = new DictEntity();
            dict.value = "2";
            dict.label = "晚上";
            dictList.Add(dict);
            dict = new DictEntity();
            dict.value = "3";
            dict.label = "全天";
            dictList.Add(dict);
            treePeriod.Properties.DataSource = dictList;
            treePeriod.Properties.DisplayMember = "label";
            treePeriod.Properties.ValueMember = "value";
        }

        #region 关闭
        private void buttonControl2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion 

        #region 获取科室
        public void GetKeShi()
        {
            try
            {
                List<DeptEntity> deptList = AppContext.Session.deptList;
                List<Xr.Common.Controls.Item> itemList = new List<Xr.Common.Controls.Item>();
                foreach (DeptEntity dept in deptList)
                {
                    Xr.Common.Controls.Item item = new Xr.Common.Controls.Item();
                    item.name = dept.name;
                    item.value = dept.id;
                    item.tag = dept.hospitalId;
                    item.parentId = dept.parentId;
                    itemList.Add(item);
                }
                treeDeptStop.Properties.DataSource = itemList;
                treeDeptStop.Properties.TreeList.KeyFieldName = "value";
                treeDeptStop.Properties.TreeList.ParentFieldName = "parentId";
                treeDeptStop.Properties.DisplayMember = "name";
                treeDeptStop.Properties.ValueMember = "value";

                List<Xr.Common.Controls.Item> topItemList = new List<Xr.Common.Controls.Item>();
                foreach (DeptEntity dept in deptList)
                {
                    Xr.Common.Controls.Item item = new Xr.Common.Controls.Item();
                    item.name = dept.name;
                    item.value = dept.id;
                    item.tag = dept.hospitalId;
                    item.parentId = dept.parentId;
                    topItemList.Add(item);
                }
                treeDeptTop.Properties.DataSource = topItemList;
                treeDeptTop.Properties.TreeList.KeyFieldName = "value";
                treeDeptTop.Properties.TreeList.ParentFieldName = "parentId";
                treeDeptTop.Properties.DisplayMember = "name";
                treeDeptTop.Properties.ValueMember = "value";
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("顶班弹窗获取科室错误信息："+ex.Message);
                MessageBox.Show(ex.Message);
            }
        }
        #endregion 
        #region 获取医生
        private void treeKeshi_EditValueChanged(object sender, EventArgs e)
        {
            GetDoctorStop(treeDeptStop.EditValue.ToString());
        }
        public void GetDoctorStop(string dept)
        {
            try
            {
               List<HospitalInfoEntity>  doctorInfoEntity = new List<HospitalInfoEntity>();
                // 查询医生下拉框数据
                String url = AppContext.AppConfig.serverUrl + "cms/doctor/findAll?hospital.id=" + AppContext.Session.hospitalId + "&dept.id=" + dept;
                String data = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    doctorInfoEntity = objT["result"].ToObject<List<HospitalInfoEntity>>();
                    treeDoctorStop.Properties.DataSource = doctorInfoEntity;
                    treeDoctorStop.Properties.DisplayMember = "name";
                    treeDoctorStop.Properties.ValueMember = "id";
                }
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("顶班获取停诊医生错误信息："+ex.Message);
                MessageBox.Show(ex.Message);
            }
        }

        private void treeDeptTop_EditValueChanged(object sender, EventArgs e)
        {
            GetDoctorTop(treeDeptTop.EditValue.ToString());
        }

        public void GetDoctorTop(string dept)
        {
            try
            {
                List<HospitalInfoEntity> doctorInfoEntity = new List<HospitalInfoEntity>();
                // 查询医生下拉框数据
                String url = AppContext.AppConfig.serverUrl + "cms/doctor/findAll?hospital.id=" + AppContext.Session.hospitalId + "&dept.id=" + dept;
                String data = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    doctorInfoEntity = objT["result"].ToObject<List<HospitalInfoEntity>>();
                    treeDoctorTop.Properties.DataSource = doctorInfoEntity;
                    treeDoctorTop.Properties.DisplayMember = "name";
                    treeDoctorTop.Properties.ValueMember = "id";
                }
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("顶班获取停诊医生错误信息：" + ex.Message);
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 确认
        private void buttonControl1_Click(object sender, EventArgs e)
        {
            if (treeDeptStop.EditValue == null || treeDeptStop.EditValue.ToString().Length == 0)
            {
                MessageBoxUtils.Show("请选择停诊科室", MessageBoxButtons.OK, this);
                return;
            }
            if (treeDoctorStop.EditValue == null || treeDoctorStop.EditValue.ToString().Length == 0)
            {
                MessageBoxUtils.Show("请选择停诊医生", MessageBoxButtons.OK, this);
                return;
            }
            if (treeDeptTop.EditValue == null || treeDeptTop.EditValue.ToString().Length == 0)
            {
                MessageBoxUtils.Show("请选择顶班科室", MessageBoxButtons.OK, this);
                return;
            }
            if (treeDoctorTop.EditValue == null || treeDoctorTop.EditValue.ToString().Length == 0)
            {
                MessageBoxUtils.Show("请选择顶班医生", MessageBoxButtons.OK, this);
                return;
            }
            if (treePeriod.EditValue == null || treePeriod.EditValue.ToString().Length == 0)
            {
                MessageBoxUtils.Show("请选择午别", MessageBoxButtons.OK, this);
                return;
            }

            String url = AppContext.AppConfig.serverUrl + "sch/doctorStopRurn/stopDiagInstead?hospitalId=" + AppContext.Session.hospitalId
                + "&outDeptId=" + treeDeptStop.EditValue + "&outDoctorId=" + treeDoctorStop.EditValue
                + "&inDeptId=" + treeDeptTop.EditValue + "&inDoctorId=" + treeDoctorTop.EditValue
                + "&period=" + treePeriod.EditValue;
            cmd.ShowOpaqueLayer();
            this.DoWorkAsync(500, (o) =>
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) =>
            {
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Hint(objT["message"].ToString(), this);
                    this.Close();
                }
                else
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                    return;
                }
            });
        }
        #endregion 

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
