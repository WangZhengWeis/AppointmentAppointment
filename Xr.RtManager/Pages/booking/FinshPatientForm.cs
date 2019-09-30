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
using Xr.RtManager.Utils;
using Xr.RtManager.Pages.triage;
using DevExpress.XtraEditors;

namespace Xr.RtManager.Pages.booking
{
    public partial class FinshPatientForm : UserControl
    {
        private Form MainForm; //主窗体
        Xr.Common.Controls.OpaqueCommand cmd;
        public FinshPatientForm()
        {
            InitializeComponent();
            timer1.Interval = Int32.Parse(ConfigurationManager.AppSettings["AutoRefreshTimeSpan"]) * 1000;
            //cmd = new Xr.Common.Controls.OpaqueCommand(this);
            //cmd.ShowOpaqueLayer(225, true);
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            //cmd = new Xr.Common.Controls.OpaqueCommand(this);
            if (cmd == null)
                cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            cmd.rectDisplay = this.DisplayRectangle;
        }
        private void UserForm_Load(object sender, EventArgs e)
        {
            MainForm = (Form)this.Parent;
            getLuesInfo();
            setDateFomartDefult();
            性别.Caption = "性\r\n别";
            就诊状态.Caption = "就诊\r\n状态";
            cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            
            if (VerifyInfo())
            {
                //QueryInfo();
            }
        }
        /// <summary>
        /// 下拉框数据
        /// </summary>
        void getLuesInfo()
        {
            //查询科室下拉框数据
            String url = AppContext.AppConfig.serverUrl + "cms/dept/qureyOperateDept";
            String data = HttpClass.httpPost(url);
            JObject objT = JObject.Parse(data);
            if (string.Compare(objT["state"].ToString(), "true", true) == 0)
            {
                List<DeptEntity> deptList = objT["result"]["deptList"].ToObject<List<DeptEntity>>();
                treeDeptId.Properties.DataSource = deptList;
                treeDeptId.Properties.TreeList.KeyFieldName = "id";
                treeDeptId.Properties.TreeList.ParentFieldName = "parentId";
                treeDeptId.Properties.DisplayMember = "name";
                treeDeptId.Properties.ValueMember = "id";
                //默认选择选择第一个
                if(deptList.Count>0)
                    treeDeptId.EditValue = deptList[0].id;
            }
            else
            {
                MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                return;
            }
            /* //若没配置科室编码让其选择一个
            if (AppContext.AppConfig.deptCode == String.Empty)
            {
                treeDeptId.EditValue = " ";
            }
            else
            {

                treeDeptId.EditValue = AppContext.Session.deptId;
            }
             */
        }
        private void treeDeptId_EditValueChanged(object sender, EventArgs e)
        {
            GetDoctorLue();
        }
        /// <summary>
        /// 获取当前科室医生
        /// </summary>
        /// <param name="dept"></param>
        public void GetDoctorLue()
        {
            try
            {
                List<HospitalInfoEntity> doctorInfoEntity = new List<HospitalInfoEntity>();
                // 查询医生下拉框数据
                string url = AppContext.AppConfig.serverUrl + "cms/doctor/findAll?hospital.id=" + AppContext.Session.hospitalId + "&dept.id=" + treeDeptId.EditValue;
                String data = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    doctorInfoEntity = objT["result"].ToObject<List<HospitalInfoEntity>>();
                    doctorInfoEntity.Insert(0, new HospitalInfoEntity { id = "", name = "全部" });
                    lueDoctor.Properties.DataSource = doctorInfoEntity;
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
            catch (Exception ex)
            {
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                Log4net.LogHelper.Error("获取医生错误信息：" + ex.Message);
            }
        }
        /// <summary>
        /// 午别，0 上午，1下午，2晚上，""全天
        /// </summary>
        string Period = "0";
        private void rBtn_1_Click(object sender, EventArgs e)
        {
            if (rBtn_noon.IsCheck)
            {
                rBtn_noon.IsCheck = true;
                rBtn_noon.IsCheck = true;
                Period = "0";
            }
            else if (rBtn_afternoon.IsCheck)
            {
                rBtn_afternoon.IsCheck = true;
                rBtn_afternoon.IsCheck = true;
                Period = "1";
            }
            else if (rBtn_night.IsCheck)
            {
                rBtn_night.IsCheck = true;
                rBtn_night.IsCheck = true;
                Period = "2";
            }
            else
            {
                rBtn_allDay.IsCheck = true;
                rBtn_allDay.IsCheck = true;
                Period = "";
            }
        }
        /// <summary>
        /// //配置时间格式
        /// </summary>
        private void setDateFomartDefult()
        {
                this.deDate.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
                this.deDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                this.deDate.Properties.EditFormat.FormatString = "yyyy-MM-dd";
                this.deDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                this.deDate.Properties.Mask.EditMask = "yyyy-MM-dd";
                this.deDate.Properties.VistaCalendarInitialViewStyle = VistaCalendarInitialViewStyle.MonthView;
                this.deDate.Properties.VistaCalendarViewStyle = ((DevExpress.XtraEditors.VistaCalendarViewStyle)((DevExpress.XtraEditors.VistaCalendarViewStyle.MonthView | DevExpress.XtraEditors.VistaCalendarViewStyle.YearView)));
                this.deDate.EditValue = System.DateTime.Now;// new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, 1);
        }
        private void buttonControl3_Click(object sender, EventArgs e)
        {
            //cmd = new OpaqueCommand(AppContext.Session.waitControl);
            if (VerifyInfo())
            {
                QueryInfo();
            }
        }
        FinshpatientParam CurrentParam = new FinshpatientParam();
        private bool VerifyInfo()
        {
            if (treeDeptId.EditValue == " ")
            {
                MessageBoxUtils.Hint("请选择科室", HintMessageBoxIcon.Error, MainForm);
                return false;
            }
            //String dtStart = System.DateTime.Today.ToString("yyyy-MM-dd");

            CurrentParam.deptId = treeDeptId.EditValue.ToString();
            CurrentParam.visitDoctor = lueDoctor.EditValue.ToString();
            CurrentParam.period = Period;
            //CurrentParam.patientName = txt_nameQuery.Text;
            //CurrentParam.registerWay = lueRegisterWay.EditValue.ToString();
            CurrentParam.workDate = deDate.Text;

            return true;
        }
        private void QueryInfo()
        {
            // 弹出加载提示框
            //DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitingForm));
            cmd.ShowOpaqueLayer(225, true);

            // 开始异步
            BackgroundWorkerUtil.start_run(bw_DoWork, bw_RunWorkerCompleted, null, false);
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                List<String> Results = new List<String>();//lueDept.EditValue
                //String param = "deptId=2&registerWay=0&status=1&patientName=李鹏真&startDate=2019-01-05&endDate=2019-01-11";
                String param = "";//deptId={0}&registerWay={1}&status={2}&patientName={3}&startDate={4}&endDate={5}&pageSize=10000";
                /*param = String.Format(param,
                    CurrentParam.deptId,
                    CurrentParam.registerWay,
                    CurrentParam.status,
                    CurrentParam.patientName,
                    CurrentParam.startDate,
                    CurrentParam.endDate);
                 */

                //获取预约信息
                Dictionary<string, string> prament = new Dictionary<string, string>();
                //prament.Add("deptId", CurrentParam.deptId);
                /*
                 if (CurrentParam.registerWay != String.Empty)
                    prament.Add("registerWay", CurrentParam.registerWay);
                 */
                /*if (CurrentParam.patientName != String.Empty)
                    prament.Add("patientName", CurrentParam.patientName);
                 */
                //prament.Add("workDate", CurrentParam.workDate);
                prament.Add("deptId", CurrentParam.deptId);
                prament.Add("visitDoctor", CurrentParam.visitDoctor);
                prament.Add("workDate", CurrentParam.workDate);
                prament.Add("period", CurrentParam.period);

                String url = String.Empty;
                if (prament.Count != 0)
                {
                    param = string.Join("&", prament.Select(x => x.Key + "=" + x.Value).ToArray());
                }
                url = AppContext.AppConfig.serverUrl + "sch/registerTriage/findTriageList?" + param;
                string res = HttpClass.httpPost(url);
                Results.Add(HttpClass.httpPost(url));


                e.Result = Results;
            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
            }
        }
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                this.gvFinshPatient.ClearSorting();//清空排序
                List<String> datas = e.Result as List<String>;
                if (datas.Count == 0)
                {
                    return;
                }
                JObject objT = new JObject();
                objT = JObject.Parse(datas[0]);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    List<FinshPatientEntity> list = objT["result"].ToObject<List<FinshPatientEntity>>();
                    this.gcFinshPatient.DataSource = list;
                    this.lab_count.Text = list.Count.ToString();
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
            }
            finally
            {
                // 关闭加载提示框
                //DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
                cmd.HideOpaqueLayer();
            }
        }
        //自动刷新
        //private void cb_AutoRefresh_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (cb_AutoRefresh.Checked && !timer1.Enabled)
        //    {
        //        timer1.Start();
        //    }
        //    else
        //    {
        //        timer1.Stop();
        //    }
        //}

        private void timer1_Tick(object sender, EventArgs e)
        {
            QueryInfo();
        }


        private void gvFinshPatient_Click(object sender, EventArgs e)
        {
            //[DevExpress.Utils.DXMouseEventArgs] = {X = 24 Y = 17 Button = Right}
            //int i = 1;
        }

    }
    /// <summary>
    /// 查询参数实体
    /// </summary>
    public class FinshpatientParam
    {
        /*deptId=2&registerWay=0&status=1&patientName=李鹏真&startDate=2019-01-05&endDate=2019-01-11
         */
        /// <summary>
        /// 科室ID
        /// </summary>
        public String deptId { get; set; }
        /// <summary>
        /// 医生ID
        /// </summary>
        public String visitDoctor { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public String workDate { get; set; }
        /// <summary>
        /// 午别
        /// </summary>
        public String period { get; set; }
    }
    /// <summary>
    ///  已完成患者信息实体
    /// </summary>
    public class FinshPatientEntity
    {
        /// <summary>
        /// 患者姓名ID
        /// </summary>
        public String patientId { get; set; }
        /// <summary>
        /// 患者姓名
        /// </summary>
        public String patientName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public String sex { get; set; }
        /// <summary>
        /// 预约日期
        /// </summary>
        public String workDate { get; set; }
        /// <summary>
        /// 午别
        /// </summary>
        public String period { get; set; }
        /// <summary>
        /// 时间段
        /// </summary>
        public String time { get; set; }
        /// <summary>
        /// 医生姓名
        /// </summary>
        public String doctorName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public String status { get; set; }

    }
}
