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

namespace Xr.RtManager.Pages.booking
{
    public partial class TodayAppointmentForm : UserControl
    {
        private Form MainForm; //主窗体
        Xr.Common.Controls.OpaqueCommand cmd;
        public TodayAppointmentForm()
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
            性别.Caption = "性\r\n别";
            年龄.Caption = "年\r\n龄";
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

            //预约状态下拉框数据
            String param = "type={0}";
            param = String.Format(param, "register_status_type");

            url = AppContext.AppConfig.serverUrl + "sys/sysDict/findByType?" + param;
            objT = JObject.Parse(HttpClass.httpPost(url));
            if (string.Compare(objT["state"].ToString(), "true", true) == 0)
            {
                //List<Dic> list = objT["result"].ToObject<List<Dic>>();
                List<DictEntity> list = new List<DictEntity>();
                list.Add(new DictEntity { label = "全部", value = "" });
                list.AddRange(objT["result"].ToObject<List<DictEntity>>());
                lueState.Properties.DataSource = list;
                lueState.Properties.DisplayMember = "label";
                lueState.Properties.ValueMember = "value";
                lueState.ItemIndex = 0;
            }
            else
            {
                MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                return;
            }
          
        }
        private void buttonControl3_Click(object sender, EventArgs e)
        {
            //cmd = new OpaqueCommand(AppContext.Session.waitControl);
            if (VerifyInfo())
            {
                QueryInfo();
            }
        }
        AppointmentQueryParam CurrentParam = new AppointmentQueryParam();
        private bool VerifyInfo()
        {
            if (treeDeptId.EditValue== " ")
            {
                MessageBoxUtils.Hint("请选择科室", HintMessageBoxIcon.Error, MainForm);
                return false;
            }
            String dtStart = System.DateTime.Today.ToString("yyyy-MM-dd");
            String dtEnd = System.DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");

            CurrentParam.deptId = treeDeptId.EditValue.ToString();
            //CurrentParam.patientName = txt_nameQuery.Text;
            //CurrentParam.registerWay = lueRegisterWay.EditValue.ToString();
            CurrentParam.status = lueState.EditValue.ToString();
            CurrentParam.startDate = dtStart;
            CurrentParam.endDate = dtEnd;

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
                prament.Add("deptId", CurrentParam.deptId);
                /*
                 if (CurrentParam.registerWay != String.Empty)
                    prament.Add("registerWay", CurrentParam.registerWay);
                 */
                if (CurrentParam.status != String.Empty)
                    prament.Add("status", CurrentParam.status);
                /*if (CurrentParam.patientName != String.Empty)
                    prament.Add("patientName", CurrentParam.patientName);
                 */
                prament.Add("startDate", CurrentParam.startDate);
                prament.Add("endDate", CurrentParam.endDate);
                prament.Add("pageSize", "10000");

                String url = String.Empty;
                if (prament.Count != 0)
                {
                    param = string.Join("&", prament.Select(x => x.Key + "=" + x.Value).ToArray());
                }
                url = AppContext.AppConfig.serverUrl + "sch/doctorScheduRegister/list?" + param;
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
                this.gvAppointmentInfo.ClearSorting();//清空排序
                List<String> datas = e.Result as List<String>;
                if (datas.Count == 0)
                {
                    return;
                }
                JObject objT = new JObject();
                objT = JObject.Parse(datas[0]);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    List<AppointmentEntity> list = objT["result"]["list"].ToObject<List<AppointmentEntity>>();
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].beginTime = list[i].beginTime + "-" + list[i].endTime;
                    }
                    this.gcAppointmentInfo.DataSource = list;
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
        private void cb_AutoRefresh_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_AutoRefresh.Checked && !timer1.Enabled)
            {
                timer1.Start();
            }
            else
            {
                timer1.Stop();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            QueryInfo();
        }

    }
}
