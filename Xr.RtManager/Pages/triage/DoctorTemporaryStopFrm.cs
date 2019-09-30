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

namespace Xr.RtManager.Pages.triage
{
    public partial class DoctorTemporaryStopFrm : Form
    {
        private String Period = "";
        private Form MainForm; //主窗体
        Xr.Common.Controls.OpaqueCommand cmd;
        public DoctorTemporaryStopFrm(string deptId, string Periods)
        {
            InitializeComponent();
            MainForm = (Form)this.Parent;
            cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            cmd.ShowOpaqueLayer(225, false);
            Period = Periods;
            List<DictEntity> dictList = new List<DictEntity>();
            DictEntity dict = new DictEntity();
            dict.value = "0";
            dict.label = "开诊";
            dictList.Add(dict);
            dict = new DictEntity();
            dict.value = "1";
            dict.label = "停诊";
            dictList.Add(dict);
            repositoryItemLookUpEdit1.DataSource = dictList;
            repositoryItemLookUpEdit1.DisplayMember = "label";
            repositoryItemLookUpEdit1.ValueMember = "value";
            repositoryItemLookUpEdit1.ShowHeader = false;
            repositoryItemLookUpEdit1.ShowFooter = false;
            cmd.HideOpaqueLayer();
            GetDoctorSetting();
        }
        #region 查询医生列表
        public List<DoctorHelper> listDoctor;
        public void GetDoctorSetting()
        {
            try
            {
                String param = "";
                //获取医生坐诊信息
                Dictionary<string, string> prament = new Dictionary<string, string>();
                //hospitalId=12&deptId=2&period=3
                prament.Add("hospitalId", AppContext.Session.hospitalId);
                prament.Add("deptIds", AppContext.Session.deptIds);
                prament.Add("period", Period);
                //prament.Add("pageSize", "10000");

                String url = String.Empty;
                if (prament.Count != 0)
                {
                    param = string.Join("&", prament.Select(x => x.Key + "=" + x.Value).ToArray());
                }
                url = AppContext.AppConfig.serverUrl + "sch/doctorSitting/findDoctorSitting?" + param;
                String jsonStr = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(jsonStr);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    List<DoctorHelper> list = objT["result"].ToObject<List<DoctorHelper>>();
                    listDoctor = new List<DoctorHelper>();
                    listDoctor = list;
                    this.gridControl1.DataSource = list;
                    cmd.HideOpaqueLayer();
                }
            }
            catch (Exception ex)
            {
                cmd.HideOpaqueLayer();
                Log4net.LogHelper.Error("获取医生坐诊列表错误信息：" + ex.Message);
            }
        }
        #endregion
        #region 帮助类
        public class DoctorHelper
        {
            public String hospitalId { get; set; }
            public String deptId { get; set; }
            public String doctorId { get; set; }
            public String doctorName { get; set; }
            public String period { get; set; }
            public String periodTxt { get; set; }
            public String clinicName { get; set; }
            public String isStop { get; set; }
        }
        public class DictEntity
        {
            public String label { get; set; }
            public String value { get; set; }
        }
        #endregion
        #region 查询按钮
        private void buttonControl1_Click(object sender, EventArgs e)
        {
            if (txtDoctorName.Text.Trim()!="")
            {
                this.gridControl1.DataSource = listDoctor.Where(s => s.doctorName == txtDoctorName.Text.Trim()).ToList();
            }
            else
            {
                GetDoctorSetting();
            }
        }
        #endregion
        #region 按钮
        private void repositoryItemLookUpEdit1_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            cmd.ShowOpaqueLayer(225, false);
            int selectRow = gridView1.GetSelectedRows()[0];
            //停诊要显示特定的界面
            var selectedRow = gridView1.GetFocusedRow() as DoctorHelper;
            if (selectedRow == null)
                return;
            GetClinicList(selectedRow.hospitalId,selectedRow.deptId);
            string clinicId = clinicInfo.FirstOrDefault(s => s.name == (selectedRow.clinicName).Substring(0, selectedRow.clinicName.Length - 1)).id;
            string isStop=e.NewValue.ToString();
            if (StopDoctor(selectedRow.hospitalId, selectedRow.deptId, clinicId, selectedRow.doctorId, isStop, selectedRow.period))
            {
                MessageBoxUtils.Hint("操作成功!", MainForm);
                GetDoctorSetting();
            }
        }
        List<ClinicInfoEntity> clinicInfo;
        public void GetClinicList(string hospitalId, string deptId)
        {
            try
            {
                clinicInfo = new List<ClinicInfoEntity>();
                String url = AppContext.AppConfig.serverUrl + "cms/clinic/list?hospital.id=" + hospitalId + "&dept.id=" + deptId + "&pageNo=1" + "&pageSize=1000";
                String data = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    clinicInfo = new List<ClinicInfoEntity>();
                    clinicInfo = objT["result"]["list"].ToObject<List<ClinicInfoEntity>>();
                }
            }
            catch (Exception ex)
            {
                //MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                Log4net.LogHelper.Error("获取诊室列表错误信息：" + ex.Message);
            }
        }
        public bool StopDoctor(string hospitalId, string deptId, string clinicId, string doctorId, string isStop, string period)
        {
            bool IsStop = false;
            try
            {
                String url = AppContext.AppConfig.serverUrl + "sch/doctorSitting/openStop?hospitalId=" + hospitalId + "&deptId=" + deptId + "&clinicId=" + clinicId + "&doctorId=" + doctorId + "&isStop=" + isStop + "&period=" + period;
                String data = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    IsStop = true;
                }
                else
                {
                    IsStop = false;
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, null);
                }
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Info("叫号退出时设置当前医生停诊状态错误信息：" + ex.Message);
            }
            return IsStop;
        }
        #endregion
    }
}
