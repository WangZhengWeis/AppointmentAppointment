using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Xr.Common;
using Xr.Http;
using Xr.RtManager.Module.triage;

namespace Xr.RtManager.Pages.triage
{
    /// <summary>
    /// 医生坐诊诊室选择
    /// Author:wzw
    /// CreationTime:2019-08-30
    /// CreationContent:修改以前选择诊室的方式,改为点击诊室那一列弹出诊室选择框来选择诊室
    /// UpdateTime:2019-09-02
    /// UpdateContent:完善选择诊室的判断,同一个科室下的同一个时间同一个时段不能选择同一个诊室
    /// </summary>
    public partial class DoctorsOfficeSettingFrm : Form
    {
        #region
        private Form MainForm; //主窗体
        Xr.Common.Controls.OpaqueCommand cmd;
        private DoctorSrtting ds;
        //private int Sum = 0;
        private bool OneFirster { get; set; }
        #endregion
        public DoctorsOfficeSettingFrm(DoctorSrtting doctorSetting,bool OneFirst)
        {
            InitializeComponent();
            #region 程序初始设置
            MainForm = (Form)this.Parent;
            ds = doctorSetting;
            OneFirster = OneFirst;
            cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            cmd.ShowOpaqueLayer(225, false);
            this.txtDepartment.Text = doctorSetting.deptName;
            this.txtDoctor.Text = doctorSetting.doctorName;
            this.txtDate.Text = doctorSetting.workDate;
            this.txtTimeSlot.Text = doctorSetting.periodTxt;
            treeList2.OptionsBehavior.Editable = false;
            treeList2.KeyFieldName = "id";//设置ID 
            treeList2.DataSource = DoctorSittingSettingForm.clinicInfo;
            cmd.HideOpaqueLayer();
            #endregion
        }
        #region 检查当前科室+日期+诊室是否已经存在
        /// <summary>
        /// 检查当前科室+日期+诊室是否已经存在
        /// </summary>
        /// <param name="hospitalId">医院主键</param>
        /// <param name="deptId">科室主键</param>
        /// <param name="clinicId">诊间ID</param>
        /// <param name="workDate">坐诊日期</param>
        /// <param name="period">时间段代码(0 上午，1下午，2晚上，3全天)</param>
        public bool CheckInfo(string hospitalId, string doctorID, string deptId, string clinicId, string workDate, string period)
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
                        MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                        Check = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                Log4net.LogHelper.Error("检查当前科室+日期+诊室是否已经存在错误信息:" + ex.Message);
            }
            return Check;
        }
        #endregion 
        #region 选择诊室的时候去检查诊室是否被占用
        private void treeList2_Click(object sender, EventArgs e)
        {
            if (treeList2.Nodes.Count != 0)
            {
                if (treeList2.FocusedNode == null) return;
                string clinicId = treeList2.FocusedNode.GetValue("id").ToString();
                string ClincName = treeList2.FocusedNode.GetDisplayText(0).ToString();
                if (clinicId == "") return;
                if (DoctorSittingSettingForm.listhelpers.Count > 0)
                {
                    Xr.RtManager.Pages.triage.DoctorSittingSettingForm.SoltHelper sh = new Xr.RtManager.Pages.triage.DoctorSittingSettingForm.SoltHelper();
                    sh.ClincNames = ClincName;
                    sh.Prioxs = ds.period;
                    sh.Times = ds.workDate;
                    sh.depetIds = ds.deptId;
                    var isPrioxs = DoctorSittingSettingForm.listhelpers.Exists(o => o.Prioxs == sh.Prioxs && o.ClincNames == sh.ClincNames && o.Times == sh.Times && o.depetIds == sh.depetIds);
                    if (isPrioxs)
                    {
                        MessageBoxUtils.Show("当前诊室已被坐诊,请选择其他的诊室", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                        return;
                    }
                    //选了全天不能上下午晚上，选了上下午晚上不能选全天的限制
                    //else
                    //{
                    //    if (ds.period.Equals("3"))
                    //    {
                    //        isPrioxs = DoctorSittingSettingForm.listhelpers.Exists(o => o.Prioxs == "0" && o.ClincNames == sh.ClincNames && o.Times == sh.Times && o.depetIds == sh.depetIds);
                    //        if (isPrioxs)
                    //        {
                    //            MessageBoxUtils.Show("当前诊室已被坐诊,请选择其他的诊室", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    //            return;
                    //        }
                    //        isPrioxs = DoctorSittingSettingForm.listhelpers.Exists(o => o.Prioxs == "1" && o.ClincNames == sh.ClincNames && o.Times == sh.Times && o.depetIds == sh.depetIds);
                    //        if (isPrioxs)
                    //        {
                    //            MessageBoxUtils.Show("当前诊室已被坐诊,请选择其他的诊室", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    //            return;
                    //        }
                    //        isPrioxs = DoctorSittingSettingForm.listhelpers.Exists(o => o.Prioxs == "2" && o.ClincNames == sh.ClincNames && o.Times == sh.Times && o.depetIds == sh.depetIds);
                    //        if (isPrioxs)
                    //        {
                    //            MessageBoxUtils.Show("当前诊室已被坐诊,请选择其他的诊室", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    //            return;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        isPrioxs = DoctorSittingSettingForm.listhelpers.Exists(o => o.Prioxs == "3" && o.ClincNames == sh.ClincNames && o.Times == sh.Times && o.depetIds == sh.depetIds);
                    //        if (isPrioxs)
                    //        {
                    //            MessageBoxUtils.Show("当前诊室已被坐诊,请选择其他的诊室", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    //            return;
                    //        }
                    //    }
                    //}
                }
                if (CheckInfo(AppContext.Session.hospitalId, ds.doctorId, ds.deptId, clinicId, ds.workDate, ds.period))
                {
                    DoctorSittingSettingForm.ClincId = clinicId;
                    DoctorSittingSettingForm.ClincName = ClincName;
                    DoctorSittingSettingForm.Priox = ds.period;
                    DoctorSittingSettingForm.Time = ds.workDate;
                    DoctorSittingSettingForm.deptIds = ds.deptId;
                    this.Close();
                }
            }
        }
        #endregion
    }
}
