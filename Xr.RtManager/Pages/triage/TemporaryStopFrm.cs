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
    public partial class TemporaryStopFrm : Form
    {
        /// <summary>
        /// 出诊信息模板
        /// </summary>
        public DefaultVisitEntity defaultVisitTemplate { get; set; }

        Xr.Common.Controls.OpaqueCommand cmd;

        public TemporaryStopFrm()
        {
            InitializeComponent();
            GetKeShi();
        }

        private void TemporaryStopFrm_Load(object sender, EventArgs e)
        {
            cmd = new Xr.Common.Controls.OpaqueCommand(this);

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

            cmd.ShowOpaqueLayer();
            //获取默认出诊时间字典配置
            String url = AppContext.AppConfig.serverUrl + "cms/doctor/findDoctorVisitingDict";
            this.DoWorkAsync(500, (o) =>
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) =>
            {
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    defaultVisitTemplate = objT["result"].ToObject<DefaultVisitEntity>();

                    String[] hyArr = defaultVisitTemplate.defaultSourceNumber.Split(new char[] { '-' });
                    defaultVisitTemplate.mStart = defaultVisitTemplate.defaultVisitTimeAm.Substring(0, 5);
                    defaultVisitTemplate.mEnd = defaultVisitTemplate.defaultVisitTimeAm.Substring(6, 5);
                    defaultVisitTemplate.mSubsection = defaultVisitTemplate.segmentalDuration;
                    defaultVisitTemplate.mScene = hyArr[0];
                    defaultVisitTemplate.mOpen = hyArr[1];
                    defaultVisitTemplate.mRoom = hyArr[2];
                    defaultVisitTemplate.mEmergency = hyArr[3];

                    defaultVisitTemplate.aStart = defaultVisitTemplate.defaultVisitTimePm.Substring(0, 5);
                    defaultVisitTemplate.aEnd = defaultVisitTemplate.defaultVisitTimePm.Substring(6, 5);
                    defaultVisitTemplate.aSubsection = defaultVisitTemplate.segmentalDuration;
                    defaultVisitTemplate.aScene = hyArr[0];
                    defaultVisitTemplate.aOpen = hyArr[1];
                    defaultVisitTemplate.aRoom = hyArr[2];
                    defaultVisitTemplate.aEmergency = hyArr[3];

                    defaultVisitTemplate.nStart = defaultVisitTemplate.defaultVisitTimeNight.Substring(0, 5);
                    defaultVisitTemplate.nEnd = defaultVisitTemplate.defaultVisitTimeNight.Substring(6, 5);
                    defaultVisitTemplate.nSubsection = defaultVisitTemplate.segmentalDuration;
                    defaultVisitTemplate.nScene = hyArr[0];
                    defaultVisitTemplate.nOpen = hyArr[1];
                    defaultVisitTemplate.nRoom = hyArr[2];
                    defaultVisitTemplate.nEmergency = hyArr[3];

                    defaultVisitTemplate.allStart = defaultVisitTemplate.defaultVisitTimeAllDay.Substring(0, 5);
                    defaultVisitTemplate.allEnd = defaultVisitTemplate.defaultVisitTimeAllDay.Substring(6, 5);
                    defaultVisitTemplate.allSubsection = defaultVisitTemplate.segmentalDuration;
                    defaultVisitTemplate.allScene = hyArr[0];
                    defaultVisitTemplate.allOpen = hyArr[1];
                    defaultVisitTemplate.allRoom = hyArr[2];
                    defaultVisitTemplate.allEmergency = hyArr[3];
                    cmd.HideOpaqueLayer();
                }
                else
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                    return;
                }
            });
        }

        #region 关闭
        private void buttonControl2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion 
        #region 验证诊室是否被占用
        private void treeClinc_EditValueChanged(object sender, EventArgs e)
        {
            if (treeClinc.Text=="选择诊室"||treeClinc.Text=="")
            {
                return;
            }
            if (!CheckIsTrue(AppContext.Session.hospitalId,treeDoctor.EditValue.ToString(),treeKeshi.EditValue.ToString(),treeClinc.EditValue.ToString(),DateTime.Now.ToString("yyyy-MM-dd"), treePeriod.EditValue.ToString()))
            {
                treeClinc.Text = "选择诊室";
            }
        }
        public bool CheckIsTrue(string hospitalId, string doctorID, string deptId, string clinicId, string workDate, string period)
        {
            bool Check = false;
            try
            {
                if (clinicId != "")
                {
                    String url = AppContext.AppConfig.serverUrl + "sch/doctorSitting/checkIsExist?hospitalId=" + hospitalId + "&doctorId=" + doctorID + "&deptId=" + deptId + "&clinicId=" + clinicId + "&workDate=" + workDate + "&period=" + period;
                    String data = HttpClass.httpPost(url);
                    JObject objT = JObject.Parse(data);
                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        Check = true;
                    }
                    else
                    {
                        MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                        Check = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                Log4net.LogHelper.Error("检查当前科室+日期+诊室是否已经存在错误信息:" + ex.Message);
            }
            return Check;
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
                itemList.Insert(0, new Xr.Common.Controls.Item { value = "", tag = "", name = "请选择", parentId = "" });
                treeKeshi.Properties.DataSource = itemList;
                treeKeshi.Properties.TreeList.KeyFieldName = "value";
                treeKeshi.Properties.TreeList.ParentFieldName = "parentId";
                treeKeshi.Properties.DisplayMember = "name";
                treeKeshi.Properties.ValueMember = "value";
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("临时停诊获取科室错误信息："+ex.Message);
            }
        }
        #endregion 
        #region 获取医生
        private void treeKeshi_EditValueChanged(object sender, EventArgs e)
        {
            GetDoctor(treeKeshi.EditValue.ToString());
        }
        public void GetDoctor(string dept)
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
                    doctorInfoEntity.Insert(0, new HospitalInfoEntity { id = "", name = "请选择" });
                    treeDoctor.Properties.DataSource = doctorInfoEntity;
                    treeDoctor.Properties.DisplayMember = "name";
                    treeDoctor.Properties.ValueMember = "id";
                }
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("临时停诊获取医生错误信息："+ex.Message);
            }
        }
        #endregion
        #region 获取诊室
        private void treeDoctor_EditValueChanged(object sender, EventArgs e)
        {
            //GetClinc(AppContext.Session.hospitalId,treeKeshi.EditValue.ToString());
            setVisitingTime();
        }
        public void GetClinc(string hospitalId, string deptId)
        {
            try
            {
                if (treePeriod.EditValue != null && treePeriod.EditValue.ToString().Length > 0)
                {
                    List<ClinicInfoEntity> clinicInfo = new List<ClinicInfoEntity>();
                    String url = AppContext.AppConfig.serverUrl + "cms/clinic/findClinicList?hospitalId=" + hospitalId 
                        + "&deptId=" + deptId + "&workDate=" + DateTime.Now.ToString("yyyy-MM-dd")
                        + "&period=" + treePeriod.EditValue;
                    String data = HttpClass.httpPost(url);
                    JObject objT = JObject.Parse(data);
                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        clinicInfo = new List<ClinicInfoEntity>();
                        clinicInfo = objT["result"].ToObject<List<ClinicInfoEntity>>();
                        ClinicInfoEntity dept = new ClinicInfoEntity();
                        dept.id = "";
                        dept.name = "选择诊室";
                        clinicInfo.Insert(0, dept);
                        treeClinc.Properties.DataSource = clinicInfo;
                        treeClinc.Properties.DisplayMember = "name";
                        treeClinc.Properties.ValueMember = "id";
                    }
                    else
                    {
                        MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("临时停诊获取诊室错误信息："+ex.Message);
            }
        }
        #endregion 
        #region 确认
        private void buttonControl1_Click(object sender, EventArgs e)
        {
            try
            {

                if (treePeriod.EditValue == null || treePeriod.EditValue.ToString().Length == 0)
                {
                    MessageBoxUtils.Show("请选择午别", MessageBoxButtons.OK, this);
                    return;
                }
                if (teSubsection.Text.Length == 0)
                {
                    MessageBoxUtils.Show("分段时间不能为空", MessageBoxButtons.OK, this);
                    return;
                }
                if (teNumSite.Text.Length == 0)
                {
                    MessageBoxUtils.Show("现场号数不能为空", MessageBoxButtons.OK, this);
                    return;
                }

                String url = AppContext.AppConfig.serverUrl + "sch/doctorScheduPlan/temporaryScene?hospitalId=" + AppContext.Session.hospitalId 
                    + "&deptId=" + treeKeshi.EditValue + "&doctorId="+treeDoctor.EditValue
                    + "&clinicId=" + treeClinc.EditValue + "&period=" + treePeriod.EditValue
                    + "&beginTime=" + teStart.Text + "&endTime=" + teEnd.Text
                    + "&segmentalDuration=" + teSubsection.Text + "&numSite=" + teNumSite.Text;
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
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("临时坐诊保存错误信息：" + ex.Message);
            }
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


        private void setVisitingTime(){
            if (treeDoctor.EditValue != null && treeDoctor.EditValue.ToString().Length > 0
                && treePeriod.EditValue != null && treePeriod.EditValue.ToString().Length > 0)
            {
                String url = AppContext.AppConfig.serverUrl + "cms/doctor/findVisitingTime?"
                + "hospitalId=" + AppContext.Session.hospitalId + "&deptId=" + treeKeshi.EditValue
                + "&doctorId=" + treeDoctor.EditValue + "&period=" + treePeriod.EditValue;
                String data = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    List<WorkingDayEntity> workingDayList = objT["result"].ToObject<List<WorkingDayEntity>>();
                    if (workingDayList.Count > 0)
                    {
                        teStart.EditValue = workingDayList[0].beginTime;
                        teEnd.EditValue = workingDayList[0].endTime;
                        teSubsection.Text = workingDayList[0].segmentalDuration;
                        teNumSite.Text = workingDayList[0].numSite;
                    }
                    else
                    {
                        if (treePeriod.EditValue.Equals("0"))
                        {
                            teStart.EditValue = defaultVisitTemplate.mStart;
                            teEnd.EditValue = defaultVisitTemplate.mEnd;
                        }
                        else if (treePeriod.EditValue.Equals("1"))
                        {
                            teStart.EditValue = defaultVisitTemplate.aStart;
                            teEnd.EditValue = defaultVisitTemplate.aEnd;
                        }
                        else if (treePeriod.EditValue.Equals("2"))
                        {
                            teStart.EditValue = defaultVisitTemplate.nStart;
                            teEnd.EditValue = defaultVisitTemplate.nEnd;
                        }
                        else if (treePeriod.EditValue.Equals("3"))
                        {
                            teStart.EditValue = defaultVisitTemplate.allStart;
                            teEnd.EditValue = defaultVisitTemplate.allEnd;
                        }
                    }
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                }
            }
        }

        private void treePeriod_EditValueChanged(object sender, EventArgs e)
        {
            GetClinc(AppContext.Session.hospitalId, treeKeshi.EditValue.ToString());
            setVisitingTime();
        }

        /// <summary>
        /// 限制时间
        /// </summary>
        /// <param name="sender">时间控件</param>
        /// <param name="wb">午别 0:上午 1：下午 2：晚上</param>
        /// <param name="flag">true：开始时间 false：结束时间</param>
        private void ifTime(object sender, String wb, Boolean flag)
        {
            TimeEdit te = (TimeEdit)sender;
            if (te.Text.Length > 0)
            {
                if (wb.Equals("0"))
                {
                    if (te.Text.CompareTo(defaultVisitTemplate.mStart) == -1)
                    {
                        te.EditValue = defaultVisitTemplate.mStart;
                    }
                    if (te.Text.CompareTo(defaultVisitTemplate.mEnd) == 1)
                    {
                        te.EditValue = defaultVisitTemplate.mEnd;
                    }
                }
                else if (wb.Equals("1"))
                {
                    if (te.Text.CompareTo(defaultVisitTemplate.aStart) == -1)
                    {
                        te.EditValue = defaultVisitTemplate.aStart;
                    }
                    if (te.Text.CompareTo(defaultVisitTemplate.aEnd) == 1)
                    {
                        te.EditValue = defaultVisitTemplate.aEnd;
                    }
                }
                else if (wb.Equals("2"))
                {
                    String text = "";
                    if (te.Text.CompareTo("12:00") == -1)
                    {
                        text = "2019/07/30 " + te.Text;
                    }
                    else
                    {
                        text = "2019/07/29 " + te.Text;
                    }
                    String nStart = "2019/07/29 " + defaultVisitTemplate.nStart;
                    String nEnd = "2019/07/30 " + defaultVisitTemplate.nEnd;
                    if (text.CompareTo(nStart) == -1)
                    {
                        te.EditValue = defaultVisitTemplate.nStart;
                    }
                    if (text.CompareTo(nEnd) == 1)
                    {
                        te.EditValue = defaultVisitTemplate.nEnd;
                    }
                }
            }
        }

        private void teStart_EditValueChanged(object sender, EventArgs e)
        {
            if (treePeriod.EditValue != null && treePeriod.EditValue.ToString().Length > 0)
            {
                ifTime(sender, treePeriod.EditValue.ToString(), true);
            }
        }

        private void teEnd_EditValueChanged(object sender, EventArgs e)
        {
            if (treePeriod.EditValue != null && treePeriod.EditValue.ToString().Length > 0)
            {
                ifTime(sender, treePeriod.EditValue.ToString(), false);

            }
        }

    }
}
