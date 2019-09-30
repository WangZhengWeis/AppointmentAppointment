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
using Xr.RtManager.Module.triage;

namespace Xr.RtManager.Pages.triage
{
    public partial class SpotBookingForm : UserControl
    {
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x02000000;////用双缓冲绘制窗口的所有子控件
        //        return cp;
        //    }
        //}
        private Form MainForm; //主窗体
        Xr.Common.Controls.OpaqueCommand cmd;
        public SpotBookingForm()
        {
            InitializeComponent();
            //cmd = new Xr.Common.Controls.OpaqueCommand(this);
            //cmd.ShowOpaqueLayer(225, true);
            //初始化T6 有线程延迟
            /*openT6 = HardwareInitialClass.OpenDevice();
            if (openT6 != 0)
            {
                LogClass.WriteLog("社保读卡器初始化失败:");
            }
            else
            {
                LogClass.WriteLog("社保读卡器初始化成功");
            }
             */
        }
        private void UserForm_Load(object sender, EventArgs e)
        {
            MainForm = (Form)this.Parent;
            getLuesInfo();
            cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            txt_cardNoQuery.Focus();
            Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.QueryDept});
        }
        /// <summary>
        /// 下拉框数据
        /// </summary>
        public List<DictEntity> ListCardType;
        void getLuesInfo()
        {
            //卡类型下拉框数据
            String param = "type={0}";
            param = String.Format(param, "card_type");

            String url = String.Empty;
            url = AppContext.AppConfig.serverUrl + "sys/sysDict/findByType?" + param;
            JObject objT = new JObject();
            objT = JObject.Parse(HttpClass.httpPost(url));
            if (string.Compare(objT["state"].ToString(), "true", true) == 0)
            {
                //List<Dic> list = objT["result"].ToObject<List<Dic>>();
                ListCardType = new List<DictEntity>();
                List<DictEntity> list = new List<DictEntity>();
                list.Add(new DictEntity { label = "", value = " " });
                list.AddRange(objT["result"].ToObject<List<DictEntity>>());
                ListCardType = list;
                lueCardType.Properties.DataSource = list;
                lueCardType.Properties.DisplayMember = "label";
                lueCardType.Properties.ValueMember = "value";
                lueCardType.ItemIndex = 0;

                lueCardTypeQuery.Properties.DataSource = objT["result"].ToObject<List<DictEntity>>();
                lueCardTypeQuery.Properties.DisplayMember = "label";
                lueCardTypeQuery.Properties.ValueMember = "value";
                lueCardTypeQuery.ItemIndex = 1;
            }
            else
            {
                MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                return;
            }
            String prament = "type={0}";
            prament = string.Format(prament, "booking_note_itme");
            url = AppContext.AppConfig.serverUrl + "sys/sysDict/findByType?" + prament;
            JObject objTs = new JObject();
            objTs = JObject.Parse(HttpClass.httpPost(url));
            if (string.Compare(objTs["state"].ToString(), "true", true) == 0)
            {
                List<dynamic> listNote = new List<dynamic>();
                listNote = objTs["result"].ToObject<List<dynamic>>();
                lueNote.Properties.Items.Clear();

                foreach (dynamic item in listNote)
                {
                    lueNote.Properties.Items.Add(item.label);
                }
                //lueNote.Properties.DisplayMember = "label";
                //lueNote.Properties.ValueMember = "value";
            }
            else
            {
                MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                return;
            }
        }
        #region 输入文本框限制
        private void txt_cardNoQuery_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            //if (e.KeyChar != '\b')//这是允许输入退格键  
            //{
            //    if ((e.KeyChar == 'X'))//这是允许输入"X"  
            //    {

            //    }
            //    else if ((e.KeyChar < '0') || (e.KeyChar > '9'))//这是允许输入0-9数字  
            //    {
            //        e.Handled = true;
            //    }
            //}
        }
        private void txt_cardNoQuery_Enter(object sender, EventArgs e)
        {
            BeginInvoke((Action)delegate
            {
                (sender as TextBox).SelectAll();
            });
        }
        private void txt_cardNoQuery_KeyUp(object sender, KeyEventArgs e)
        {
            //允许Ctrl+v粘贴数字
            //if (e.KeyData == (Keys.Control | Keys.V))
            //{
            //    if (Clipboard.ContainsText())
            //    {
            //        try
            //        {
            //            Convert.ToInt64(Clipboard.GetText());  //检查是否数字
            //            //((TextBox)sender).SelectedText = Clipboard.GetText().Trim(); //Ctrl+V 粘贴  
            //            //光标处插入
            //            string s = txt_cardNoQuery.Text;
            //            int idx = txt_cardNoQuery.SelectionStart;
            //            s = s.Insert(idx, Clipboard.GetText());

            //            txt_cardNoQuery.Text = s;
            //            txt_cardNoQuery.SelectionStart = idx + Clipboard.GetText().Length;
            //            txt_cardNoQuery.Focus();

            //        }
            //        catch (Exception)
            //        {
            //            e.Handled = true;
            //            //throw;
            //        }
            //    }
            //}
            ////允许Ctrl+A实现全选
            //else if (e.KeyData == (Keys.Control | Keys.A))
            //{
            //    ((TextBox)sender).SelectAll();
            //}
            ////允许Ctrl+C实现复制
            //else if (e.KeyData == (Keys.Control | Keys.C))
            //{
            //    if (txt_cardNoQuery.SelectedText != "")
            //        Clipboard.SetDataObject(txt_cardNoQuery.SelectedText);
            //}
            if (txt_cardNoQuery.Text != String.Empty)
            {
                if (e.KeyCode == Keys.Control || e.KeyCode == Keys.Enter)
                {
                    CardType = lueCardTypeQuery.EditValue.ToString();
                    CardID = txt_cardNoQuery.Text;
                    if (CardID != "\r\n")//回车字符也不请求
                    {
                        Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.ReadzlCard, Argument = new String[] { CardID, lueCardTypeQuery.EditValue.ToString() } });
                    }
                    txt_cardNoQuery.Text = String.Empty;
                    lueCardTypeQuery.ItemIndex = 1;
                    //gc_patient.Focus();
                    txt_cardNoQuery.Focus();
                }
            }
        }

        /// <summary>
        /// 双击实现全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_cardNoQuery_DoubleClick(object sender, EventArgs e)
        {
            txt_cardNoQuery.Select(0, txt_cardNoQuery.Text.Length);
        }
#endregion

        private void btn_zlk_Click(object sender, EventArgs e)
        {
            //ClearUIInfo();
            //WorkType = AsynchronousWorks.ReadzlCard;
            //cmd.IsShowCancelBtn = false;
            //cmd.ShowOpaqueLayer();
            
            lueCardTypeQuery.EditValue = "2";
            if (txt_cardNoQuery.Text != String.Empty)
            {
                CardID = txt_cardNoQuery.Text;
                CardType = "2";
                //CardID = "000675493100";
                Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.ReadzlCard, Argument = new String[] { CardID, "2" } });
                txt_cardNoQuery.Text = String.Empty;
            }
            txt_cardNoQuery.Focus();
            lueCardTypeQuery.ItemIndex = 1;
        }
        private void btn_readPatientid_Click(object sender, EventArgs e)
        {
            if (txt_cardNoQuery.Text != String.Empty)
            {
                CardID = txt_cardNoQuery.Text;
                CardType = "1";
                Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.ReadzlCard, Argument = new String[] { CardID, "1" } });
            }
            txt_cardNoQuery.Text = String.Empty;
            lueCardTypeQuery.ItemIndex = 0;
            //flowLayoutPanel1.Focus();
            txt_cardNoQuery.Focus();
        }
        private void btn_readSocialcard_Click(object sender, EventArgs e)
        {

            //WorkType = AsynchronousWorks.ReadSocialcard;
            timer1.Start();
            CardType = "3";
            Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.ReadSocialcard });
        }

        private void btn_readIdcard_Click(object sender, EventArgs e)
        {
            //WorkType = AsynchronousWorks.ReadIdCard;
            timer1.Start();
            CardType = "4";
            Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.ReadIdCard });
        }

        private void buttonControl1_Click(object sender, EventArgs e)
        {
            Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.Reservation, Argument = new String[] { SelectSchemaid, Patientid } });
        }
        private void reservationCalendar1_SelectDateTest(DateTime SelectedDate)
        {
            MessageBox.Show(SelectedDate.ToShortDateString() + "测试事件");
        }
        bool NeedWaitingFrm = true;
        public bool CancelFlag = false;
        /// <summary>
        /// 查询卡号
        /// </summary>
        private string CardID = String.Empty;
        /// <summary>
        /// 选中科室ID
        /// </summary>
        private string SelectDeptid = String.Empty;
        /// <summary>
        /// 医生ID
        /// </summary>
        private string Doctorid = String.Empty;
        /// <summary>
        /// 选中医生ID
        /// </summary>
        private string SelectDoctorid = String.Empty;
        /// <summary>
        /// 选中排班主键
        /// </summary>
        private string SelectSchemaid = String.Empty;
        /// <summary>
        /// 患者主键
        /// </summary>
        private string Patientid = String.Empty;
        /// <summary>
        /// 卡类型，1患者ID、2诊疗卡、3社保卡、4身份证、5健康卡、6健康虚拟卡
        /// </summary>
        private String CardType = " ";
        /// <summary>
        /// 午别
        /// </summary>
        //private String Period = "0";
        /// <summary>
        ///预约记录主键 (患者列表用)
        /// </summary>
        private String RegisterId = String.Empty;
        /// <summary>
        /// 患者列表状态：0预约、1候诊中、2已就诊、null全部
        /// </summary>
        //private String PatientListStatus = "null";
        private void Asynchronous(AsyncEntity ars)
        {
            //异步操作
            if (!this.backgroundWorker1.IsBusy)
            {
                backgroundWorker1 = new BackgroundWorker();
                var bw = backgroundWorker1;
                bw.WorkerReportsProgress = true;
                //需要异步的操作
                bw.DoWork += new DoWorkEventHandler(DoWork);
                //异步操作时报告前台状态变更
                //bw.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
                //异步操作完成后操作
                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
                bw.WorkerSupportsCancellation = true;
                //异步调用结束后释放操作
                bw.Disposed += new EventHandler(Disposed);
                //开始异步操作
                bw.RunWorkerAsync(ars);

                //打开等待框
                 if (ars.WorkType == AsynchronousWorks.ReadzlCard)
                {
                    cmd.IsShowCancelBtn = false;
                    cmd.ShowOpaqueLayer(0.56f, "请稍候...");
                    ClearPatientUIInfo();
                }

                else if (ars.WorkType == AsynchronousWorks.ReadIdCard || ars.WorkType == AsynchronousWorks.ReadSocialcard)
                {
                    cmd.IsShowCancelBtn = true;
                    cmd.ShowOpaqueLayer(0.56f, "正在读取...");
                    ClearPatientUIInfo();
                }
                /*
            else if (WorkType == AsynchronousWorks.SingInOrRegister)
            {
                //cmd.IsShowCancelBtn = false;
                //cmd.ShowOpaqueLayer();
            }
                 */
                else
                {
                    if (NeedWaitingFrm)
                    {
                        cmd.IsShowCancelBtn = false;
                        cmd.ShowOpaqueLayer(0.56f, "请稍候...");
                    }
                }
            }
        }
        void DoWork(object sender, DoWorkEventArgs e)
        {
            SycResult result = new SycResult();
            String[] Pras = new String[] { };
            AsyncEntity Arg = e.Argument as AsyncEntity;
            AsynchronousWorks workType = Arg.WorkType;
            result.WorkType = Arg.WorkType;
            if (Arg.Argument != null)
            {
                Pras = Arg.Argument;
            }
            // 异步操作1
            Thread.Sleep(100);
            #region 获取可挂号科室
            if (workType == AsynchronousWorks.QueryDept)
            {
                //报告前台状态变更
                backgroundWorker1.ReportProgress(50);
                // 异步操作2
                //Thread.Sleep(300);
                String param = "";
                //获取医生坐诊信息
                Dictionary<string, string> prament = new Dictionary<string, string>();
                //hospital.code=00000
                //prament.Add("hospital.code", AppContext.AppConfig.hospitalCode);
                //prament.Add("pageSize", "10000");
                //prament.Add("deptIds", AppContext.Session.deptIds);
                prament.Add("isOpen", "0");
                String url = String.Empty;
                if (prament.Count != 0)
                {
                    param = string.Join("&", prament.Select(x => x.Key + "=" + x.Value).ToArray());
                }
                url = AppContext.AppConfig.serverUrl + "cms/dept/findIsUse?" + param;
                String jsonStr = HttpClass.httpPost(url);
                //String jsonStr = @"{""code"":200,""message"":""操作成功"",""result"":[{""id"":2,""parentId"":"""",""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""急诊科1"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""code"":""1000011""},{""id"":9,""parentId"":2,""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""急诊科2"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""code"":""10000111""},{""id"":10,""parentId"":2,""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""急诊科3"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-30/=1315643547,2434956224"",""code"":""10000112""},{""id"":15,""parentId"":2,""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""测试科室二"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""code"":""001""},{""id"":14,""parentId"":2,""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""测试科室"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""code"":""1234566789""},{""id"":16,""parentId"":2,""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""测试科室3"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""code"":""003""},{""id"":17,""parentId"":2,""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""测试科室4444444444"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""code"":""004""},{""id"":11,""parentId"":"""",""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""儿科"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""code"":""1000012""}],""state"":true}";
                JObject objT = JObject.Parse(jsonStr);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    result.obj = objT;
                    result.result = true;
                    //result.msg = "成功";
                    result.msg = objT["message"].ToString();
                    e.Result = result;
                }
                else
                {
                    result.result = false;
                    result.msg = objT["message"].ToString();// PatientSearchInfoRef.Msg;
                    e.Result = result;
                }

            }
            #endregion
            #region 获取科室可挂号医生
            else if (workType == AsynchronousWorks.QueryDoctor)
            {
                //报告前台状态变更
                backgroundWorker1.ReportProgress(50);
                // 异步操作2
                //Thread.Sleep(300);
                String param = "";
                //获取获取科室可挂号医生信息
                Dictionary<string, string> prament = new Dictionary<string, string>();
                //hospital.id=12&dept.id=2
                prament.Add("hospital.id", AppContext.Session.hospitalId);
                //prament.Add("hospital.code", AppContext.Session.hospitalId);
                prament.Add("dept.id", Pras[0]);
                prament.Add("isOpen", "0");

                String url = String.Empty;
                if (prament.Count != 0)
                {
                    param = string.Join("&", prament.Select(x => x.Key + "=" + x.Value).ToArray());
                }
                url = AppContext.AppConfig.serverUrl + "cms/doctor/findAll?" + param;
                String jsonStr = HttpClass.httpPost(url);
                //jsonStr = @"{""code"":200,""message"":""操作成功"",""result"":{""count"":4,""list"":[{""code"":""00068"",""id"":1,""ignoreHoliday"":""0"",""ignoreYear"":""0"",""isShow"":""0"",""isUse"":""0"",""job"":""主任医师"",""name"":""张医生"",""sort"":1},{""code"":""12321321"",""id"":10,""ignoreHoliday"":""0"",""ignoreYear"":""0"",""isShow"":"" "",""isUse"":""0"",""job"":""123"",""name"":""1232"",""sort"":1},{""code"":""007"",""id"":15,""ignoreHoliday"":""0"",""ignoreYear"":""0"",""isShow"":""1"",""isUse"":""0"",""job"":""专家"",""name"":""杰大哥"",""sort"":1},{""code"":""12321"",""id"":13,""ignoreHoliday"":""0"",""ignoreYear"":""0"",""isShow"":"" "",""isUse"":""0"",""job"":""21321"",""name"":""21321"",""sort"":123213}],""pageNo"":1,""pageSize"":10,""sumPage"":1},""state"":true}";
                JObject objT = JObject.Parse(jsonStr);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    result.obj = objT;
                    result.result = true;
                    //result.msg = "成功";
                    result.msg = objT["message"].ToString();
                    e.Result = result;
                }
                else
                {
                    result.result = false;
                    result.msg = objT["message"].ToString();// PatientSearchInfoRef.Msg;
                    e.Result = result;
                }

            }
            #endregion
            #region 获取医生可预约日期
            else if (workType == AsynchronousWorks.QueryDoctorAvailableDate)
            {
                //报告前台状态变更
                backgroundWorker1.ReportProgress(50);
                // 异步操作2
                //Thread.Sleep(300);
                String param = "";
                //获取医生可预约日期
                Dictionary<string, string> prament = new Dictionary<string, string>();
                //hospitalId=12&deptId=2&doctorId=1&type=0
                prament.Add("hospitalId", AppContext.Session.hospitalId);
                prament.Add("deptId", Pras[0]);
                prament.Add("doctorId", Pras[1]);
                prament.Add("type", "0");//请求类型：0公开预约号源、1诊间预约号源
                //prament.Add("pageSize", "10000");

                String url = String.Empty;
                if (prament.Count != 0)
                {
                    param = string.Join("&", prament.Select(x => x.Key + "=" + x.Value).ToArray());
                }
                url = AppContext.AppConfig.serverUrl + "sch/doctorScheduPlan/findByDeptAndDoctor?" + param;
                String jsonStr = HttpClass.httpPost(url);
                //jsonStr = @"{""code"":200,""message"":""操作成功"",""result"":[{""workDate"":""2019-02-01""},{""workDate"":""2019-02-02""},{""workDate"":""2019-02-18""},{""workDate"":""2019-02-21""},{""workDate"":""2019-02-22""},{""workDate"":""2019-02-23""},{""workDate"":""2019-03-03""},{""workDate"":""2019-03-13""}],""state"":true}";
                JObject objT = JObject.Parse(jsonStr);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    result.obj = objT;
                    result.result = true;
                    //result.msg = "成功";
                    result.msg = objT["message"].ToString();
                    e.Result = result;
                }
                else
                {
                    result.result = false;
                    result.msg = objT["message"].ToString();// PatientSearchInfoRef.Msg;
                    e.Result = result;
                }

            }
            #endregion
            #region 获取医生可预约时间段
            else if (workType == AsynchronousWorks.QueryDoctorAvailableTimeSpan)
            {
                //报告前台状态变更
                backgroundWorker1.ReportProgress(50);


                //报告前台状态变更
                backgroundWorker1.ReportProgress(50);
                // 异步操作2
                //Thread.Sleep(300);
                String param = "";
                //获取候诊患者列表
                Dictionary<string, string> prament = new Dictionary<string, string>();
                //hospitalId=12&deptId=&doctorId=&beginDate=2019-02-16
                prament.Add("hospitalId", AppContext.Session.hospitalId);
                prament.Add("deptId", Pras[0]);
                prament.Add("doctorId", Pras[1]);
                prament.Add("beginDate", Pras[2]);
                prament.Add("endDate", Pras[2]);
                //prament.Add("pageSize", "10000");

                String url = String.Empty;
                if (prament.Count != 0)
                {
                    param = string.Join("&", prament.Select(x => x.Key + "=" + x.Value).ToArray());
                }
                url = AppContext.AppConfig.serverUrl + "sch/doctorScheduRegister/findRegister?" + param;
                String jsonStr = HttpClass.httpPost(url);
                //jsonStr = @"{""code"":200,""message"":""操作成功"",""result"":[{""address"":""广西临桂县四塘乡自信村委会鸟塘里村23-1号"",""addressType"":""1"",""age"":""31"",""beginTime"":""08:00"",""cardNo"":"""",""cardType"":""1 "",""createById"":1,""createByName"":""系统管理员"",""createDate"":""2019-02-15 16:52:57"",""deptId"":2,""deptName"":""急诊科1"",""doctorId"":1,""doctorName"":""张医生"",""endTime"":""08:30"",""hospitalId"":12,""id"":4,""idCard"":"""",""isCyfz"":""1"",""isMxb"":""1"",""isShfz"":""1"",""isUse"":""0"",""isYwzz"":""1"",""note"":"""",""patientId"":""000675493100"",""patientName"":""李鹏真"",""period"":""0"",""queueNum"":1,""registerTime"":""2019-02-15 16:52:57"",""registerWay"":""2"",""scheduPlanId"":34,""sex"":""男"",""status"":""4"",""statusTxt"":""取消预约"",""tempPhone"":""17666476268"",""time"":""08:00 - 08:30"",""updateById"":1,""updateByName"":""系统管理员"",""updateDate"":""2019-02-15 18:24:26"",""visitType"":""1"",""week"":""六"",""workDate"":""2019-02-16""},{""address"":""广西临桂县四塘乡自信村委会鸟塘里村23-1号"",""addressType"":""1"",""age"":""31"",""beginTime"":""08:30"",""cardNo"":""02096999"",""cardType"":""1 "",""createById"":1,""createByName"":""系统管理员"",""createDate"":""2019-02-15 16:57:24"",""deptId"":2,""deptName"":""急诊科1"",""doctorId"":1,""doctorName"":""张医生"",""endTime"":""09:00"",""hospitalId"":12,""id"":5,""idCard"":"""",""isCyfz"":""1"",""isMxb"":""1"",""isShfz"":""1"",""isUse"":""0"",""isYwzz"":""1"",""note"":"""",""patientId"":""000675493100"",""patientName"":""李鹏真"",""period"":""0"",""queueNum"":5,""registerTime"":""2019-02-15 16:57:24"",""registerWay"":""2"",""scheduPlanId"":123,""sex"":""男"",""status"":""7"",""statusTxt"":""爽约"",""tempPhone"":""17666476268"",""time"":""08:30 - 09:00"",""updateById"":1,""updateByName"":""系统管理员"",""updateDate"":""2019-02-15 19:09:01"",""visitType"":""1"",""week"":""日"",""workDate"":""2019-02-17""},{""address"":""广西临桂县四塘乡自信村委会鸟塘里村23-1号"",""addressType"":""1"",""age"":""31"",""beginTime"":""10:30"",""cardNo"":""02096999"",""cardType"":""1 "",""createById"":1,""createByName"":""系统管理员"",""createDate"":""2019-02-15 16:59:30"",""deptId"":2,""deptName"":""急诊科1"",""doctorId"":1,""doctorName"":""张医生"",""endTime"":""11:00"",""hospitalId"":12,""id"":6,""idCard"":"""",""isCyfz"":""1"",""isMxb"":""1"",""isShfz"":""1"",""isUse"":""0"",""isYwzz"":""1"",""note"":"""",""patientId"":""000675493100"",""patientName"":""李鹏真"",""period"":""0"",""queueNum"":21,""registerTime"":""2019-02-15 16:59:30"",""registerWay"":""2"",""scheduPlanId"":143,""sex"":""男"",""status"":""4"",""statusTxt"":""取消预约"",""tempPhone"":""17666476268"",""time"":""10:30 - 11:00"",""updateById"":1,""updateByName"":""系统管理员"",""updateDate"":""2019-02-18 11:05:12"",""visitType"":""1"",""week"":""二"",""workDate"":""2019-02-19""},{""address"":""广西临桂县四塘乡自信村委会鸟塘里村23-1号"",""addressType"":""1"",""age"":""31"",""beginTime"":""15:00"",""cardNo"":""02096999"",""cardType"":""1 "",""createById"":1,""createByName"":""系统管理员"",""createDate"":""2019-02-15 17:30:22"",""deptId"":2,""deptName"":""急诊科1"",""doctorId"":1,""doctorName"":""张医生"",""endTime"":""15:30"",""hospitalId"":12,""id"":7,""idCard"":"""",""isCyfz"":""1"",""isMxb"":""1"",""isShfz"":""1"",""isUse"":""0"",""isYwzz"":""1"",""note"":"""",""patientId"":""000675493100"",""patientName"":""李鹏真"",""period"":""1"",""queueNum"":9,""registerTime"":""2019-02-15 17:30:22"",""registerWay"":""2"",""scheduPlanId"":28,""sex"":""男"",""status"":""0"",""statusTxt"":""待签到"",""tempPhone"":""17666476268"",""time"":""15:00 - 15:30"",""updateById"":1,""updateByName"":""系统管理员"",""updateDate"":""2019-02-15 17:30:22"",""visitType"":""1"",""week"":""六"",""workDate"":""2019-02-16""},{""address"":""广西临桂县四塘乡自信村委会鸟塘里村23-1号"",""addressType"":""1"",""age"":""31"",""beginTime"":""08:30"",""cardNo"":""02096999"",""cardType"":""1 "",""createById"":1,""createByName"":""系统管理员"",""createDate"":""2019-02-15 17:35:44"",""deptId"":2,""deptName"":""急诊科1"",""doctorId"":1,""doctorName"":""张医生"",""endTime"":""09:00"",""hospitalId"":12,""id"":8,""idCard"":"""",""isCyfz"":""1"",""isMxb"":""1"",""isShfz"":""1"",""isUse"":""0"",""isYwzz"":""1"",""note"":"""",""patientId"":""000675493100"",""patientName"":""李鹏真"",""period"":""0"",""queueNum"":5,""registerTime"":""2019-02-15 17:35:44"",""registerWay"":""2"",""scheduPlanId"":91,""sex"":""男"",""status"":""7"",""statusTxt"":""爽约"",""tempPhone"":""17666476268"",""time"":""08:30 - 09:00"",""updateById"":1,""updateByName"":""系统管理员"",""updateDate"":""2019-02-18 11:05:19"",""visitType"":""1"",""week"":""一"",""workDate"":""2019-02-18""},{""address"":""广西临桂县四塘乡自信村委会鸟塘里村23-1号"",""addressType"":""1"",""age"":""31"",""beginTime"":""11:30"",""cardNo"":""02096999"",""cardType"":""1 "",""createById"":1,""createByName"":""系统管理员"",""createDate"":""2019-02-18 11:08:47"",""deptId"":2,""deptName"":""急诊科1"",""doctorId"":1,""doctorName"":""张医生"",""endTime"":""12:00"",""hospitalId"":12,""id"":9,""idCard"":"""",""isCyfz"":""1"",""isMxb"":""1"",""isShfz"":""1"",""isUse"":""0"",""isYwzz"":""1"",""note"":"""",""patientId"":""000675493100"",""patientName"":""李鹏真"",""period"":""0"",""queueNum"":29,""registerTime"":""2019-02-18 11:08:47"",""registerWay"":""2"",""scheduPlanId"":97,""sex"":""男"",""status"":""7"",""statusTxt"":""爽约"",""tempPhone"":""17666476268"",""time"":""11:30 - 12:00"",""updateById"":1,""updateByName"":""系统管理员"",""updateDate"":""2019-02-18 12:30:00"",""visitType"":""1"",""week"":""一"",""workDate"":""2019-02-18""},{""address"":""广西临桂县四塘乡自信村委会鸟塘里村23-1号"",""addressType"":""1"",""age"":""31"",""beginTime"":""17:30"",""cardNo"":""02096999"",""cardType"":""1 "",""createById"":1,""createByName"":""系统管理员"",""createDate"":""2019-02-18 11:09:04"",""deptId"":2,""deptName"":""急诊科1"",""doctorId"":1,""doctorName"":""张医生"",""endTime"":""18:00"",""hospitalId"":12,""id"":10,""idCard"":"""",""isCyfz"":""1"",""isMxb"":""1"",""isShfz"":""1"",""isUse"":""0"",""isYwzz"":""1"",""note"":"""",""patientId"":""000675493100"",""patientName"":""李鹏真"",""period"":""1"",""queueNum"":29,""registerTime"":""2019-02-18 11:09:04"",""registerWay"":""2"",""scheduPlanId"":105,""sex"":""男"",""status"":""7"",""statusTxt"":""爽约"",""tempPhone"":""17666476268"",""time"":""17:30 - 18:00"",""updateById"":1,""updateByName"":""系统管理员"",""updateDate"":""2019-02-18 18:30:00"",""visitType"":""1"",""week"":""一"",""workDate"":""2019-02-18""},{""address"":""广西临桂县四塘乡自信村委会鸟塘里村23-1号"",""addressType"":""1"",""age"":""31"",""beginTime"":""08:00"",""cardNo"":""02096999"",""cardType"":""1 "",""createById"":1,""createByName"":""系统管理员"",""createDate"":""2019-02-18 11:14:34"",""deptId"":2,""deptName"":""急诊科1"",""doctorId"":1,""doctorName"":""张医生"",""endTime"":""08:30"",""hospitalId"":12,""id"":11,""idCard"":"""",""isCyfz"":""1"",""isMxb"":""1"",""isShfz"":""1"",""isUse"":""0"",""isYwzz"":""1"",""note"":"""",""patientId"":""000675493100"",""patientName"":""李鹏真"",""period"":""0"",""queueNum"":1,""registerTime"":""2019-02-18 11:14:34"",""registerWay"":""2"",""scheduPlanId"":138,""sex"":""男"",""status"":""7"",""statusTxt"":""爽约"",""tempPhone"":""17666476268"",""time"":""08:00 - 08:30"",""updateById"":1,""updateByName"":""系统管理员"",""updateDate"":""2019-02-19 09:00:02"",""visitType"":""1"",""week"":""二"",""workDate"":""2019-02-19""},{""address"":""广西临桂县四塘乡自信村委会鸟塘里村23-1号"",""addressType"":""1"",""age"":""31"",""beginTime"":""15:00"",""cardNo"":""02096999"",""cardType"":""1 "",""createById"":1,""createByName"":""系统管理员"",""createDate"":""2019-02-18 11:15:30"",""deptId"":2,""deptName"":""急诊科1"",""doctorId"":1,""doctorName"":""张医生"",""endTime"":""15:30"",""hospitalId"":12,""id"":12,""idCard"":"""",""isCyfz"":""1"",""isMxb"":""1"",""isShfz"":""1"",""isUse"":""0"",""isYwzz"":""1"",""note"":"""",""patientId"":""000675493100"",""patientName"":""李鹏真"",""period"":""1"",""queueNum"":9,""registerTime"":""2019-02-18 11:15:30"",""registerWay"":""2"",""scheduPlanId"":132,""sex"":""男"",""status"":""7"",""statusTxt"":""爽约"",""tempPhone"":""17666476268"",""time"":""15:00 - 15:30"",""updateById"":1,""updateByName"":""系统管理员"",""updateDate"":""2019-02-19 16:00:00"",""visitType"":""1"",""week"":""二"",""workDate"":""2019-02-19""}],""state"":true}";

                JObject objT = JObject.Parse(jsonStr);
                List<JObject> objTs = new List<JObject>();
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    // 添加候诊患者列表
                    objTs.Add(objT);

                    //获取医生可预约时间段
                     prament = new Dictionary<string, string>();
                    //hospitalId=12&deptId=2&doctorId=1&type=0&workDate=2019-01-18
                    prament.Add("hospitalId", AppContext.Session.hospitalId);
                    prament.Add("deptId", Pras[0]);
                    prament.Add("doctorId", Pras[1]);
                    prament.Add("workDate", Pras[2]);
                    prament.Add("type", "0");//请求类型：0公开预约号源、1诊间预约号源
                    //prament.Add("pageSize", "10000");

                    if (prament.Count != 0)
                    {
                        param = string.Join("&", prament.Select(x => x.Key + "=" + x.Value).ToArray());
                    }
                    url = AppContext.AppConfig.serverUrl + "sch/doctorScheduPlan/findTimeNum?" + param;
                     jsonStr = HttpClass.httpPost(url);
                    //jsonStr = @"{""code"":200,""message"":""操作成功"",""result"":[{""workDate"":""2019-02-01""},{""workDate"":""2019-02-02""},{""workDate"":""2019-02-18""},{""workDate"":""2019-02-21""},{""workDate"":""2019-02-22""},{""workDate"":""2019-02-23""},{""workDate"":""2019-03-03""},{""workDate"":""2019-03-13""}],""state"":true}";
                     objT = JObject.Parse(jsonStr);
                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        //添加医生可预约时间段
                        objTs.Add(objT);

                        result.obj = objTs;
                        result.result = true;
                        //result.msg = "成功";
                        result.msg = objT["message"].ToString();
                        e.Result = result;
                    }
                    else
                    {
                        result.result = false;
                        result.msg = objT["message"].ToString();// PatientSearchInfoRef.Msg;
                        e.Result = result;
                    }
                }
                else
                {
                    result.result = false;
                    result.msg = objT["message"].ToString();// PatientSearchInfoRef.Msg;
                    e.Result = result;
                }

            }
            #endregion
            #region 查询患者信息
            else if (workType == AsynchronousWorks.QueryID || workType == AsynchronousWorks.ReadzlCard)
            {

                //报告前台状态变更
                backgroundWorker1.ReportProgress(50);
                // 异步操作2
                //Thread.Sleep(300);
                //提交异步操作结果供结束时操作
                if (CardID != String.Empty)
                {
                    //String serverUrl = ConfigurationManager.AppSettings["serverUrl"].ToString();
                    //String jsonStr = HttpClass.HRequest(serverUrl + "bedChargeSettle/queryHosRecords?numType=2&&num=" + SocialCardID + "&&queryType=1");

                    String param = "";
                    //获取患者信息
                    Dictionary<string, string> prament = new Dictionary<string, string>();
                    /*
                    //prament.Add("cardNo", CardID);
                    prament.Add("cardNo", Pras[0]);

                    //prament.Add("pageSize", "10000");

                    String url = String.Empty;
                    if (prament.Count != 0)
                    {
                        param = string.Join("&", prament.Select(x => x.Key + "=" + x.Value).ToArray());
                    }
                    url = AppContext.AppConfig.serverUrl + "patmi/findPatMiByCardNo?" + param;
                    String jsonStr = HttpClass.httpPost(url);
                    JObject objT = JObject.Parse(jsonStr);
                     */
                    prament.Add("cardNo", Pras[0].Trim());
                    prament.Add("cardType", Pras[1]);
                    if (prament.Count != 0)
                    {
                        param = string.Join("&", prament.Select(x => x.Key + "=" + x.Value).ToArray());
                    }
                    String url = AppContext.AppConfig.serverUrl + "patmi/findPatMiByTyptAndCardNo?" + param;
                    String jsonStr = HttpClass.httpPost(url);
                    JObject objT = JObject.Parse(jsonStr);

                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        Patientid = objT["result"]["patientId"].ToString();

                        result.obj = objT;
                        result.result = true;
                        //result.msg = "成功";
                        result.msg = objT["message"].ToString();
                        e.Result = result;
                    }
                    else
                    {
                        result.result = false;
                        if (objT["message"].ToString() == "未匹配到患者信息")
                        {
                            result.msg = "没有查询到基本信息，请去办卡";
                        }
                        else
                        {
                            result.msg = objT["message"].ToString();// PatientSearchInfoRef.Msg;
                        }
                        e.Result = result;
                    }
                    CardID = String.Empty;
                }
                else //输入为空时不查询
                {
                    result.obj = null;
                    result.result = true;
                    result.msg = "成功";
                    e.Result = result;
                }
                NeedWaitingFrm = true;
            }
            #endregion
            #region 读取身份证
            else if (workType == AsynchronousWorks.ReadIdCard)
            {
                //try
                //{
                //    JLIdCardInfoClass idCardInfo = JLIdCardInfoClass.getCardInfo();
                //    if (idCardInfo != null)
                //    {
                //        IDCardID = idCardInfo.Code.ToString();
                //    }
                //    if (IDCardID != String.Empty)
                //    {
                //        //patientId = carMes.user_id;
                //        LogClass.WriteLog("读取身份证成功，身份证号：" + IDCardID);
                //    }
                //    result.obj = null;
                //    result.result = true;
                //    result.msg = "成功";
                //    e.Result = result;
                //}
                //catch (Exception ee)
                //{
                //    result.obj = null;
                //    result.result = false;
                //    result.msg = "读取身份证失败:" + ee.Message;
                //    e.Result = result;
                //}

                try
                {
                    JLIdCardInfoClass.CancelFlag = false;
                    BackgroundWorker bgworker = sender as BackgroundWorker;
                    //绑定委托要执行的方法 
                    ReadCardDelegate work = new ReadCardDelegate(ReturnReadCardData);

                    //开始异步执行(ReturnDataTable)方法 
                    IAsyncResult ret = work.BeginInvoke("IdCard", null, null);

                    //(异步编程模式好久就是在执行一个很耗时的方法(ReturnDataTable)时,还能向下继续运行代码) 

                    //接着运行下面的while循环, 
                    //判断异步操作是否完成 
                    while (!ret.IsCompleted)
                    {
                        //没完成 
                        //判断是否取消了backgroundworker异步操作 
                        if (bgworker.CancellationPending)
                        {
                            //如果是  马上取消backgroundwork操作(这个地方才是真正取消) 
                            JLIdCardInfoClass.CancelFlag = true;
                            e.Cancel = true;
                            return;
                        }
                    }
                    e.Result = work.EndInvoke(ret); //返回查询结果 赋值给e.Result 
                }
                catch (Exception ex)
                {
                    e.Result = ex.Message;
                }

            }
            #endregion
            #region 读取社保卡
            else if (workType == AsynchronousWorks.ReadSocialcard)
            {
                CancelFlag = false;
                try
                {
                    //while (!CancelFlag)
                    //{

                    //    SocialCard carMes = new SocialCard();
                    //    carMes.readCard();
                    //    if (carMes.message_type == "1")
                    //    {
                    //        CancelFlag = true;
                    //        //patientId = carMes.user_id;
                    //        LogClass.WriteLog("读取社保卡成功，卡号：" + carMes.user_id);
                    //        SocialCardID = carMes.user_id;
                    //    }

                    //}
                    //result.obj = null;
                    //result.result = true;
                    //result.msg = "成功";
                    //e.Result = result;

                    BackgroundWorker bgworker = sender as BackgroundWorker;
                    //绑定委托要执行的方法 
                    ReadCardDelegate work = new ReadCardDelegate(ReturnReadCardData);

                    //开始异步执行(ReturnDataTable)方法 
                    IAsyncResult ret = work.BeginInvoke("", null, null);

                    //(异步编程模式好久就是在执行一个很耗时的方法(ReturnDataTable)时,还能向下继续运行代码) 

                    //接着运行下面的while循环, 
                    //判断异步操作是否完成 
                    while (!ret.IsCompleted)
                    {
                        //没完成 
                        //判断是否取消了backgroundworker异步操作 
                        if (bgworker.CancellationPending)
                        {
                            //如果是  马上取消backgroundwork操作(这个地方才是真正取消) 
                            SocialCard cardMes = new SocialCard();
                            cardMes.cancelReadCard();
                            CancelFlag = true;
                            e.Cancel = true;
                            return;
                        }
                    }
                    e.Result = work.EndInvoke(ret); //返回查询结果 赋值给e.Result 
                }
                catch (Exception ee)
                {
                    result.obj = null;
                    result.result = false;
                    result.msg = "读取社保卡失败:" + ee.Message;
                    CancelFlag = true;
                    e.Result = result;
                }
            }
            #endregion
            #region 现场预约
            else if (workType == AsynchronousWorks.Reservation)
            {
                //{"code":200,"message":"操作成功","result":{"registerId":9,"registerWay":"0","cardType":"1 ","cardNo":"02102337","status":"0","statusTxt":"待签到","triageId":""},"state":true}
                // 异步操作2
                //Thread.Sleep(300);
                //提交异步操作结果供结束时操作

                String param = "";
                //请求现场挂号
                Dictionary<string, string> prament = new Dictionary<string, string>();
                //scheduPlanId=10&patientId=000675493100&patientName=李鹏真&cardType=1&cardNo=02102337&tempPhone=17666476268&note=&visitType=1&addressType=1&isShfz=1&isYwzz=1&isCyfz=1&isMxb=1&registerWay=0
                prament.Add("scheduPlanId", Pras[0]);//排班记录主键
                prament.Add("patientId", Pras[1]);//患者主键
                prament.Add("patientName", lab_patientName.Text.Trim());//患者姓名
                prament.Add("cardType", lueCardType.EditValue.ToString());//卡类型
                prament.Add("cardNo", lab_cardID.Text.Trim());//卡号
                prament.Add("tempPhone", lab_tel.Text.Trim());//手机号
                prament.Add("note", lueNote.Text);//备注
                if (rBtn_visitType0.Checked)//就诊类别：0.初诊 ，1.复诊
                {
                    prament.Add("visitType", "0");
                }
                else
                {
                    prament.Add("visitType", "1");
                }
                if (rBtn_addressType0.Checked)//地址类别：0市内、1市外  
                {
                    prament.Add("addressType", "0");
                }
                else
                {
                    prament.Add("addressType", "1");
                }
                if (rBtn_isShfzF.Checked)//术后复诊：0是、1否
                {
                    prament.Add("isShfz", "1");
                }
                else
                {
                    prament.Add("isShfz", "0");
                }
                if (rBtn_isYwzzF.Checked)//外院转诊：0是、1否
                {
                    prament.Add("isYwzz", "1");
                }
                else
                {
                    prament.Add("isYwzz", "0");
                }
                if (rBtn_isCyfzF.Checked)//出院复诊：0是、1否
                {
                    prament.Add("isCyfz", "1");
                }
                else
                {
                    prament.Add("isCyfz", "0");
                }
                if (rBtn_isMxbF.Checked)//是否慢性病：0是、1否
                {
                    prament.Add("isMxb", "1");
                }
                else
                {
                    prament.Add("isMxb", "0");
                }
                //prament.Add("registerWay", "0");//预约途径：0分诊台、1诊间、2自助机、3公众号
                String url = String.Empty;
                if (prament.Count != 0)
                {
                    param = string.Join("&", prament.Select(x => x.Key + "=" + x.Value).ToArray());
                }
                url = AppContext.AppConfig.serverUrl + "sch/doctorScheduRegister/confirmBooking?" + param;
                String jsonStr = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(jsonStr);
                //{"code":200,"message":"预约成功","result":"【王小二】在【2018-01-25】预约了【内科】【王主任】【14:52】就诊，请提前10分诊到场签到就诊。","state":true}
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    result.obj = objT;
                    result.result = true;
                    //result.msg = "成功";
                    result.msg = objT["message"].ToString();
                    e.Result = result;
                }
                else
                {
                    result.result = false;
                    result.msg = objT["message"].ToString();// PatientSearchInfoRef.Msg;

                    e.Result = result;
                }
            }
            #endregion
            #region 取消预约
            //取消候诊
            else if (workType == AsynchronousWorks.CancelReservation)
            {
                //{"code":200,"message":"操作成功","result":{"registerId":9,"registerWay":"0","cardType":"1 ","cardNo":"02102337","status":"0","statusTxt":"待签到","triageId":""},"state":true}
                // 异步操作2
                //Thread.Sleep(300);
                //提交异步操作结果供结束时操作

                String param = "";
                //请求取消预约
                Dictionary<string, string> prament = new Dictionary<string, string>();
                prament.Add("registerId", Pras[0]);
                //prament.Add("triageId", TriageId);
                //prament.Add("pageSize", "10000");

                String url = String.Empty;
                if (prament.Count != 0)
                {
                    param = string.Join("&", prament.Select(x => x.Key + "=" + x.Value).ToArray());
                }
                url = AppContext.AppConfig.serverUrl + "sch/doctorScheduRegister/cancelBooking?" + param;
                String jsonStr = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(jsonStr);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    result.obj = objT;
                    result.result = true;
                    //result.msg = "成功";
                    result.msg = objT["message"].ToString();
                    e.Result = result;
                }
                else
                {
                    result.result = false;
                    result.msg = objT["message"].ToString();// PatientSearchInfoRef.Msg;
                    e.Result = result;
                }

            }
            #endregion
            #region 该医生当日预约名单
            else if (workType == AsynchronousWorks.ReservationPatientList)
            {
                //报告前台状态变更
                backgroundWorker1.ReportProgress(50);
                // 异步操作2
                //Thread.Sleep(300);
                String param = "";
                //获取候诊患者列表
                Dictionary<string, string> prament = new Dictionary<string, string>();
                //hospitalId=12&deptId=&doctorId=&beginDate=2019-02-16
                prament.Add("hospitalId", AppContext.Session.hospitalId);
                prament.Add("deptId", Pras[0]);
                prament.Add("doctorId", Pras[1]);
                prament.Add("beginDate", Pras[3]);
                prament.Add("endDate", Pras[2]);
                //prament.Add("pageSize", "10000");

                String url = String.Empty;
                if (prament.Count != 0)
                {
                    param = string.Join("&", prament.Select(x => x.Key + "=" + x.Value).ToArray());
                }
                url = AppContext.AppConfig.serverUrl + "sch/doctorScheduRegister/findRegister?" + param;
                String jsonStr = HttpClass.httpPost(url);
                //jsonStr = @"{""code"":200,""message"":""操作成功"",""result"":[{""patientName"":""李鹏真"",""registerWay"":""2"",""cradType"":""1 "",""cradNo"":"""",""regVisitTime"":""08:00 - 08:30"",""regTime"":""2019-02-15 16:52:57"",""status"":""待签到""},{""patientName"":""李鹏真"",""registerWay"":""2"",""cradType"":""1 "",""cradNo"":""02096999"",""regVisitTime"":""15:00 - 15:30"",""regTime"":""2019-02-15 17:30:22"",""status"":""待签到""}],""state"":true}";

                JObject objT = JObject.Parse(jsonStr);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    result.obj = objT;
                    result.result = true;
                    //result.msg = "成功";
                    result.msg = objT["message"].ToString();
                    e.Result = result;
                }
                else
                {
                    result.result = false;
                    result.msg = objT["message"].ToString();// PatientSearchInfoRef.Msg;
                    e.Result = result;
                }

            }
            #endregion
        }
        private delegate SycResult ReadCardDelegate(string parm);  //创建一个委托 
        /// <summary> 
        /// 异步委托的方法 
        /// </summary> 
        /// <param name="sql"></param> 
        /// <returns></returns> 
        private SycResult ReturnReadCardData(string parm)
        {

            SycResult Result = new SycResult() { result = true };
            NeedWaitingFrm = false;
            //System.Threading.Thread.Sleep(10000);
            if (parm == "IdCard")
            {
                //System.Threading.Thread.Sleep(3000);
                //if (backgroundWorker1.IsBusy)
                //{
                //    CardID = "45032219871222151X";
                //    Result.WorkType = AsynchronousWorks.ReadIdCard;
                //    Result.obj = CardID;
                //}

                JLIdCardInfoClass idCardInfo = JLIdCardInfoClass.getCardInfo();
                if (idCardInfo != null)
                {
                    if (backgroundWorker1.IsBusy)
                    {
                        CardID = idCardInfo.Code.ToString();
                        Result.WorkType = AsynchronousWorks.ReadIdCard;
                    }
                }
                if (CardID != String.Empty)
                {
                    //patientId = carMes.user_id;
                    Result.obj = CardID;
                    Xr.Log4net.LogHelper.Info("读取身份证成功，身份证号：" + CardID);
                }
                 
                //result.obj = null;
                //result.result = true;
                //result.msg = "成功
            }
            else
            {
                /*System.Threading.Thread.Sleep(3000);
               if (backgroundWorker1.IsBusy)
               {
                   CardID = "000675493100";
                   Result.WorkType = AsynchronousWorks.ReadSocialcard;
                   Result.obj = CardID;
               }
               */
                SocialCard carMes = new SocialCard();
                while (!CancelFlag)
                {
                    carMes.readCard();
                    System.Threading.Thread.Sleep(1000);//延时读取 以免刷新过快读卡器报错
                    if (carMes.message_type == "1")
                    {
                        CancelFlag = true;
                        //patientId = carMes.user_id;
                        Xr.Log4net.LogHelper.Info("读取社保卡成功，卡号：" + carMes.user_id);
                        if (backgroundWorker1.IsBusy)
                        {
                            CardID = carMes.user_id;
                            Result.WorkType = AsynchronousWorks.ReadSocialcard;
                            Result.obj = CardID;
                        }
                        break;
                    }
                }
                 
                //result.obj = null;
                //result.result = true;
                //result.msg = "成功";
                //e.Result = result;
            }


            return Result;
        }
        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //停止异步调用
            this.backgroundWorker1.CancelAsync();
            try
            {
                //Thread.Sleep(500);
                //通过异步操作完成结果判断后续提示
                if (e.Result == null)
                {
                    //MessageBoxUtils.Hint("操作失败，请稍候尝试。");
                    MessageBoxUtils.Show("操作失败，请稍候尝试。", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    return;
                }
                var result = (SycResult)e.Result;
                if (result.result)
                {
                    AsynchronousWorks workType = result.WorkType;
                    #region 可挂号科室
                    if (workType == AsynchronousWorks.QueryDept)
                    {
                        if (result.obj == null)
                        {
                            //_waitForm.DialogResult = DialogResult.OK;
                            //_waitForm.ChangeNoticeComplete(result.msg, Dialog.HintMessageBoxIcon.Error);
                            //_waitForm.Close();
                            cmd.HideOpaqueLayer();
                            return;
                        }
                        //jsonStr = @"{""code"":200,""message"":""操作成功"",""result"":[{""id"":2,""parentId"":"""",""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""急诊科1"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""code"":""1000011""},{""id"":9,""parentId"":2,""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""急诊科2"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""code"":""10000111""},{""id"":10,""parentId"":2,""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""急诊科3"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-30/=1315643547,2434956224"",""code"":""10000112""},{""id"":15,""parentId"":2,""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""测试科室二"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""code"":""001""},{""id"":14,""parentId"":2,""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""测试科室"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""code"":""1234566789""},{""id"":16,""parentId"":2,""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""测试科室3"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""code"":""003""},{""id"":17,""parentId"":2,""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""测试科室4444444444"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""code"":""004""},{""id"":11,""parentId"":"""",""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""儿科"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""code"":""1000012""}],""state"":true}";

                        JObject objT = result.obj as JObject;
                        List<DeptEntity> list = objT["result"].ToObject<List<DeptEntity>>();

                        //List<DeptEntity> list = AppContext.Session.deptList;
                        treeMenuControl1.KeyFieldName = "id";
                        treeMenuControl1.ParentFieldName = "parentId";
                        treeMenuControl1.DisplayMember = "name";
                        treeMenuControl1.ValueMember = "id";
                        treeMenuControl1.DataSource = list;
                        if (list.Count > 0)
                        {
                            treeMenuControl1.EditValue = list[0].id;
                            SelectDeptid = treeMenuControl1.EditValue;
                            NeedWaitingFrm = false;
                            Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.QueryDoctor, Argument = new String[] { SelectDeptid } });
                        }

                        /*
                        //panel_depts.Controls.Clear();
                        if (list.Count > 0)
                        {
                            list.Reverse();
                            int i = 0;
                            foreach (var item in list)
                            {
                                i++;
                                BorderPanelButton btn = new BorderPanelButton();

                                btn.BorderSides = Xr.Common.Controls.BorderSides.None;
                                btn.BorderColor = System.Drawing.Color.White;
                                btn.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
                                btn.BorderWidth = 0;
                                btn.BtnFont = new System.Drawing.Font("微软雅黑", 12F);
                                btn.BtnText = item.name;
                                btn.Dock = System.Windows.Forms.DockStyle.Top;
                                btn.FillColor1 = System.Drawing.Color.White;
                                btn.FillColor2 = System.Drawing.Color.White;
                                btn.Location = new System.Drawing.Point(0, 75);
                                btn.Margin = new System.Windows.Forms.Padding(0);
                                btn.Padding = new System.Windows.Forms.Padding(0);
                                btn.Tag = item.id;
                                btn.SelctedColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(195)))), ((int)(((byte)(245)))));
                                btn.Size = new System.Drawing.Size(170, 25);
                                btn.MouseClick += new System.Windows.Forms.MouseEventHandler(this.deptButtonWaiting_MouseClick);
                                if (i == list.Count)
                                {
                                    SelectDeptid = btn.Tag.ToString();
                                    btn.IsCheck = true;
                                }

                                this.panel_depts.Controls.Add(btn);
                            }

                            NeedWaitingFrm = false;
                            Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.QueryDoctor, Argument = new String[] { SelectDeptid } });
                        }
                         */
                        //cmd.HideOpaqueLayer();
                        //cmd = new Xr.Common.Controls.OpaqueCommand(this);
                        return;
                    }
                    #endregion
                    #region 获取科室可挂号医生
                    else if (workType == AsynchronousWorks.QueryDoctor)
                    {
                        ClearReservationUIInfo();
                        if (result.obj == null)
                        {
                            //_waitForm.DialogResult = DialogResult.OK;
                            //_waitForm.ChangeNoticeComplete(result.msg, Dialog.HintMessageBoxIcon.Error);
                            //_waitForm.Close();
                            cmd.HideOpaqueLayer();
                            return;
                        }
                        //jsonStr = @"{""code"":200,""message"":""操作成功"",""result"":[{""id"":2,""parentId"":"""",""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""急诊科1"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""code"":""1000011""},{""id"":9,""parentId"":2,""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""急诊科2"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""code"":""10000111""},{""id"":10,""parentId"":2,""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""急诊科3"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-30/=1315643547,2434956224"",""code"":""10000112""},{""id"":15,""parentId"":2,""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""测试科室二"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""code"":""001""},{""id"":14,""parentId"":2,""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""测试科室"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""code"":""1234566789""},{""id"":16,""parentId"":2,""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""测试科室3"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""code"":""003""},{""id"":17,""parentId"":2,""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""测试科室4444444444"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""code"":""004""},{""id"":11,""parentId"":"""",""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""儿科"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""code"":""1000012""}],""state"":true}";

                        JObject objT = result.obj as JObject;
                       // List<DoctorInfoEntity> doctorList = objT["result"]["list"].ToObject<List<DoctorInfoEntity>>();
                        List<DoctorInfoEntity> doctorList = objT["result"].ToObject<List<DoctorInfoEntity>>();
                        List<Item> itemList = new List<Item>();
                        if (doctorList.Count > 0)
                        {
                            //设置医生列表

                            foreach (DoctorInfoEntity doctor in doctorList)
                            {
                                Item item = new Item();
                                item.name = String.Format("{0,-6}[{1}]", doctor.name, doctor.job);//doctor.name + " [" + doctor.job + "]";
                                item.value = doctor.id;
                                item.tag = doctor.id + "§" + doctor.name + "§" + doctor.mobile;
                                itemList.Add(item);
                            }
                            mcDoctor.setDataSource(itemList);
                            mcDoctor.EditValue(itemList[0].value);
                            SelectDoctorid = itemList[0].value;

                            NeedWaitingFrm = false;
                            Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.QueryDoctorAvailableDate, Argument = new String[] { SelectDeptid, SelectDoctorid } });
                        }
                        else
                        {
                            mcDoctor.setDataSource(itemList);
                        }
                        /*
                        panel_doctors.Controls.Clear();
                        if (list.Count > 0)
                        {
                            list.Reverse();
                            int i = 0;
                            foreach (var item in list)
                            {
                                i++;
                                BorderPanelButton btn = new BorderPanelButton();

                                btn.BorderSides = Xr.Common.Controls.BorderSides.None;
                                btn.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
                                btn.BtnFont = new System.Drawing.Font("微软雅黑", 12F);
                                btn.BtnText = item.name + "\t[" + item.job + "]";
                                btn.CenterText = false;
                                btn.Dock = System.Windows.Forms.DockStyle.Top;
                                btn.FillColor1 = System.Drawing.Color.White;
                                btn.FillColor2 = System.Drawing.Color.White;
                                btn.Location = new System.Drawing.Point(0, 75);
                                btn.Margin = new System.Windows.Forms.Padding(0);
                                btn.Tag = item.id;
                                btn.SelctedColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(195)))), ((int)(((byte)(245)))));
                                btn.Size = new System.Drawing.Size(170, 25);
                                btn.MouseClick += new System.Windows.Forms.MouseEventHandler(this.doctorButtonWaiting_MouseClick);
                                if (i == list.Count)
                                {

                                    SelectDoctorid = btn.Tag.ToString();
                                    btn.IsCheck = true;
                                }

                                this.panel_doctors.Controls.Add(btn);
                            }
                            NeedWaitingFrm = false;
                            Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.QueryDoctorAvailableDate, Argument = new String[] { SelectDeptid, SelectDoctorid } });
                            //cmd.HideOpaqueLayer();
                            //cmd = new Xr.Common.Controls.OpaqueCommand(this);
                        }
                         */
                        return;
                    }
                    #endregion
                    #region 获取医生可预约日期
                    else if (workType == AsynchronousWorks.QueryDoctorAvailableDate)
                    {
                        if (result.obj == null)
                        {
                            //_waitForm.DialogResult = DialogResult.OK;
                            //_waitForm.ChangeNoticeComplete(result.msg, Dialog.HintMessageBoxIcon.Error);
                            //_waitForm.Close();
                            cmd.HideOpaqueLayer();
                            return;
                        }
                        //jsonStr = @"{""code"":200,""message"":""操作成功"",""result"":[{""workDate"":""2019-02-01""},{""workDate"":""2019-02-02""},{""workDate"":""2019-02-18""},{""workDate"":""2019-02-21""},{""workDate"":""2019-02-22""},{""workDate"":""2019-02-23""},{""workDate"":""2019-03-03""},{""workDate"":""2019-03-13""}],""state"":true}";

                        JObject objT = result.obj as JObject;

                        //更新日历
                        List<Dictionary<int, String>> dcs = new List<Dictionary<int, String>>();
                        List<AvaDateEntity> list = objT["result"].ToObject<List<AvaDateEntity>>();
                        if (list.Count > 0)
                        {
                            Dictionary<int, String> dc1 = new Dictionary<int, String>();
                            string Month = System.DateTime.Now.ToString("MM");//list[0].month;
                            foreach (var item in list)
                            {
                                if (item.month != Month)
                                {
                                    dcs.Add(dc1);
                                    dc1 = new Dictionary<int, String>();
                                    Month = item.month;

                                }
                                if (item.syNum > 0)
                                {
                                    dc1.Add(Int32.Parse(item.day), System.DateTime.Now.ToString());
                                }
                                else
                                { 
                                    dc1.Add(Int32.Parse(item.day), "约满");
                                }
                            }
                            dcs.Add(dc1);
                        }
                        reservationCalendar1.ChangeValidDate(dcs);

                        cmd.HideOpaqueLayer();
                        //cmd = new Xr.Common.Controls.OpaqueCommand(this);
                        return;
                    }
                    #endregion
                    #region 获取医生可预约时间段
                    else if (workType == AsynchronousWorks.QueryDoctorAvailableTimeSpan)
                    {
                        this.gv_patient.ClearSorting();//清空排序
                        if (result.obj == null)
                        {
                            //_waitForm.DialogResult = DialogResult.OK;
                            //_waitForm.ChangeNoticeComplete(result.msg, Dialog.HintMessageBoxIcon.Error);
                            //_waitForm.Close();
                            cmd.HideOpaqueLayer();
                            return;
                        }
                        List<JObject> objTs = result.obj as List<JObject>;
                        //更新该医生该日预约患者 
                        JObject objT = objTs[0];
                        // @"{""code"":200,""message"":""操作成功"",""result"":[{""address"":""广西临桂县四塘乡自信村委会鸟塘里村23-1号"",""addressType"":""1"",""age"":""31"",""beginTime"":""08:00"",""cardNo"":"""",""cardType"":""1 "",""createById"":1,""createByName"":""系统管理员"",""createDate"":""2019-02-15 16:52:57"",""deptId"":2,""deptName"":""急诊科1"",""doctorId"":1,""doctorName"":""张医生"",""endTime"":""08:30"",""hospitalId"":12,""id"":4,""idCard"":"""",""isCyfz"":""1"",""isMxb"":""1"",""isShfz"":""1"",""isUse"":""0"",""isYwzz"":""1"",""note"":"""",""patientId"":""000675493100"",""patientName"":""李鹏真"",""period"":""0"",""queueNum"":1,""registerTime"":""2019-02-15 16:52:57"",""registerWay"":""2"",""scheduPlanId"":34,""sex"":""男"",""status"":""4"",""statusTxt"":""取消预约"",""tempPhone"":""17666476268"",""time"":""08:00 - 08:30"",""updateById"":1,""updateByName"":""系统管理员"",""updateDate"":""2019-02-15 18:24:26"",""visitType"":""1"",""week"":""六"",""workDate"":""2019-02-16""},{""address"":""广西临桂县四塘乡自信村委会鸟塘里村23-1号"",""addressType"":""1"",""age"":""31"",""beginTime"":""08:30"",""cardNo"":""02096999"",""cardType"":""1 "",""createById"":1,""createByName"":""系统管理员"",""createDate"":""2019-02-15 16:57:24"",""deptId"":2,""deptName"":""急诊科1"",""doctorId"":1,""doctorName"":""张医生"",""endTime"":""09:00"",""hospitalId"":12,""id"":5,""idCard"":"""",""isCyfz"":""1"",""isMxb"":""1"",""isShfz"":""1"",""isUse"":""0"",""isYwzz"":""1"",""note"":"""",""patientId"":""000675493100"",""patientName"":""李鹏真"",""period"":""0"",""queueNum"":5,""registerTime"":""2019-02-15 16:57:24"",""registerWay"":""2"",""scheduPlanId"":123,""sex"":""男"",""status"":""7"",""statusTxt"":""爽约"",""tempPhone"":""17666476268"",""time"":""08:30 - 09:00"",""updateById"":1,""updateByName"":""系统管理员"",""updateDate"":""2019-02-15 19:09:01"",""visitType"":""1"",""week"":""日"",""workDate"":""2019-02-17""},{""address"":""广西临桂县四塘乡自信村委会鸟塘里村23-1号"",""addressType"":""1"",""age"":""31"",""beginTime"":""10:30"",""cardNo"":""02096999"",""cardType"":""1 "",""createById"":1,""createByName"":""系统管理员"",""createDate"":""2019-02-15 16:59:30"",""deptId"":2,""deptName"":""急诊科1"",""doctorId"":1,""doctorName"":""张医生"",""endTime"":""11:00"",""hospitalId"":12,""id"":6,""idCard"":"""",""isCyfz"":""1"",""isMxb"":""1"",""isShfz"":""1"",""isUse"":""0"",""isYwzz"":""1"",""note"":"""",""patientId"":""000675493100"",""patientName"":""李鹏真"",""period"":""0"",""queueNum"":21,""registerTime"":""2019-02-15 16:59:30"",""registerWay"":""2"",""scheduPlanId"":143,""sex"":""男"",""status"":""4"",""statusTxt"":""取消预约"",""tempPhone"":""17666476268"",""time"":""10:30 - 11:00"",""updateById"":1,""updateByName"":""系统管理员"",""updateDate"":""2019-02-18 11:05:12"",""visitType"":""1"",""week"":""二"",""workDate"":""2019-02-19""},{""address"":""广西临桂县四塘乡自信村委会鸟塘里村23-1号"",""addressType"":""1"",""age"":""31"",""beginTime"":""15:00"",""cardNo"":""02096999"",""cardType"":""1 "",""createById"":1,""createByName"":""系统管理员"",""createDate"":""2019-02-15 17:30:22"",""deptId"":2,""deptName"":""急诊科1"",""doctorId"":1,""doctorName"":""张医生"",""endTime"":""15:30"",""hospitalId"":12,""id"":7,""idCard"":"""",""isCyfz"":""1"",""isMxb"":""1"",""isShfz"":""1"",""isUse"":""0"",""isYwzz"":""1"",""note"":"""",""patientId"":""000675493100"",""patientName"":""李鹏真"",""period"":""1"",""queueNum"":9,""registerTime"":""2019-02-15 17:30:22"",""registerWay"":""2"",""scheduPlanId"":28,""sex"":""男"",""status"":""0"",""statusTxt"":""待签到"",""tempPhone"":""17666476268"",""time"":""15:00 - 15:30"",""updateById"":1,""updateByName"":""系统管理员"",""updateDate"":""2019-02-15 17:30:22"",""visitType"":""1"",""week"":""六"",""workDate"":""2019-02-16""},{""address"":""广西临桂县四塘乡自信村委会鸟塘里村23-1号"",""addressType"":""1"",""age"":""31"",""beginTime"":""08:30"",""cardNo"":""02096999"",""cardType"":""1 "",""createById"":1,""createByName"":""系统管理员"",""createDate"":""2019-02-15 17:35:44"",""deptId"":2,""deptName"":""急诊科1"",""doctorId"":1,""doctorName"":""张医生"",""endTime"":""09:00"",""hospitalId"":12,""id"":8,""idCard"":"""",""isCyfz"":""1"",""isMxb"":""1"",""isShfz"":""1"",""isUse"":""0"",""isYwzz"":""1"",""note"":"""",""patientId"":""000675493100"",""patientName"":""李鹏真"",""period"":""0"",""queueNum"":5,""registerTime"":""2019-02-15 17:35:44"",""registerWay"":""2"",""scheduPlanId"":91,""sex"":""男"",""status"":""7"",""statusTxt"":""爽约"",""tempPhone"":""17666476268"",""time"":""08:30 - 09:00"",""updateById"":1,""updateByName"":""系统管理员"",""updateDate"":""2019-02-18 11:05:19"",""visitType"":""1"",""week"":""一"",""workDate"":""2019-02-18""},{""address"":""广西临桂县四塘乡自信村委会鸟塘里村23-1号"",""addressType"":""1"",""age"":""31"",""beginTime"":""11:30"",""cardNo"":""02096999"",""cardType"":""1 "",""createById"":1,""createByName"":""系统管理员"",""createDate"":""2019-02-18 11:08:47"",""deptId"":2,""deptName"":""急诊科1"",""doctorId"":1,""doctorName"":""张医生"",""endTime"":""12:00"",""hospitalId"":12,""id"":9,""idCard"":"""",""isCyfz"":""1"",""isMxb"":""1"",""isShfz"":""1"",""isUse"":""0"",""isYwzz"":""1"",""note"":"""",""patientId"":""000675493100"",""patientName"":""李鹏真"",""period"":""0"",""queueNum"":29,""registerTime"":""2019-02-18 11:08:47"",""registerWay"":""2"",""scheduPlanId"":97,""sex"":""男"",""status"":""7"",""statusTxt"":""爽约"",""tempPhone"":""17666476268"",""time"":""11:30 - 12:00"",""updateById"":1,""updateByName"":""系统管理员"",""updateDate"":""2019-02-18 12:30:00"",""visitType"":""1"",""week"":""一"",""workDate"":""2019-02-18""},{""address"":""广西临桂县四塘乡自信村委会鸟塘里村23-1号"",""addressType"":""1"",""age"":""31"",""beginTime"":""17:30"",""cardNo"":""02096999"",""cardType"":""1 "",""createById"":1,""createByName"":""系统管理员"",""createDate"":""2019-02-18 11:09:04"",""deptId"":2,""deptName"":""急诊科1"",""doctorId"":1,""doctorName"":""张医生"",""endTime"":""18:00"",""hospitalId"":12,""id"":10,""idCard"":"""",""isCyfz"":""1"",""isMxb"":""1"",""isShfz"":""1"",""isUse"":""0"",""isYwzz"":""1"",""note"":"""",""patientId"":""000675493100"",""patientName"":""李鹏真"",""period"":""1"",""queueNum"":29,""registerTime"":""2019-02-18 11:09:04"",""registerWay"":""2"",""scheduPlanId"":105,""sex"":""男"",""status"":""7"",""statusTxt"":""爽约"",""tempPhone"":""17666476268"",""time"":""17:30 - 18:00"",""updateById"":1,""updateByName"":""系统管理员"",""updateDate"":""2019-02-18 18:30:00"",""visitType"":""1"",""week"":""一"",""workDate"":""2019-02-18""},{""address"":""广西临桂县四塘乡自信村委会鸟塘里村23-1号"",""addressType"":""1"",""age"":""31"",""beginTime"":""08:00"",""cardNo"":""02096999"",""cardType"":""1 "",""createById"":1,""createByName"":""系统管理员"",""createDate"":""2019-02-18 11:14:34"",""deptId"":2,""deptName"":""急诊科1"",""doctorId"":1,""doctorName"":""张医生"",""endTime"":""08:30"",""hospitalId"":12,""id"":11,""idCard"":"""",""isCyfz"":""1"",""isMxb"":""1"",""isShfz"":""1"",""isUse"":""0"",""isYwzz"":""1"",""note"":"""",""patientId"":""000675493100"",""patientName"":""李鹏真"",""period"":""0"",""queueNum"":1,""registerTime"":""2019-02-18 11:14:34"",""registerWay"":""2"",""scheduPlanId"":138,""sex"":""男"",""status"":""7"",""statusTxt"":""爽约"",""tempPhone"":""17666476268"",""time"":""08:00 - 08:30"",""updateById"":1,""updateByName"":""系统管理员"",""updateDate"":""2019-02-19 09:00:02"",""visitType"":""1"",""week"":""二"",""workDate"":""2019-02-19""},{""address"":""广西临桂县四塘乡自信村委会鸟塘里村23-1号"",""addressType"":""1"",""age"":""31"",""beginTime"":""15:00"",""cardNo"":""02096999"",""cardType"":""1 "",""createById"":1,""createByName"":""系统管理员"",""createDate"":""2019-02-18 11:15:30"",""deptId"":2,""deptName"":""急诊科1"",""doctorId"":1,""doctorName"":""张医生"",""endTime"":""15:30"",""hospitalId"":12,""id"":12,""idCard"":"""",""isCyfz"":""1"",""isMxb"":""1"",""isShfz"":""1"",""isUse"":""0"",""isYwzz"":""1"",""note"":"""",""patientId"":""000675493100"",""patientName"":""李鹏真"",""period"":""1"",""queueNum"":9,""registerTime"":""2019-02-18 11:15:30"",""registerWay"":""2"",""scheduPlanId"":132,""sex"":""男"",""status"":""7"",""statusTxt"":""爽约"",""tempPhone"":""17666476268"",""time"":""15:00 - 15:30"",""updateById"":1,""updateByName"":""系统管理员"",""updateDate"":""2019-02-19 16:00:00"",""visitType"":""1"",""week"":""二"",""workDate"":""2019-02-19""}],""state"":true}";
                        List<PatientListEntity> patientList = objT["result"].ToObject<List<PatientListEntity>>();
                        for (int i = 0; i < patientList.Count; i++)
                        {
                            if (patientList[i].cardType.ToString() != "")
                            {
                                patientList[i].cardType = ListCardType.FirstOrDefault(s => s.value == patientList[i].cardType).label;
                            }
                        }
                        gc_patient.DataSource = patientList;
                        //this.gv_patient.BestFitColumns();

                        //更新医生可预约时间段
                        //panel_timespan.Controls.Clear();
                        objT = objTs[1];
                        //jsonStr = @"{""code"":200,""message"":""操作成功"",""result"":[{""id"":2,""parentId"":"""",""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""急诊科1"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""code"":""1000011""},{""id"":9,""parentId"":2,""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""急诊科2"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""code"":""10000111""},{""id"":10,""parentId"":2,""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""急诊科3"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-30/=1315643547,2434956224"",""code"":""10000112""},{""id"":15,""parentId"":2,""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""测试科室二"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""code"":""001""},{""id"":14,""parentId"":2,""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""测试科室"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""code"":""1234566789""},{""id"":16,""parentId"":2,""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""测试科室3"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""code"":""003""},{""id"":17,""parentId"":2,""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""测试科室4444444444"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""code"":""004""},{""id"":11,""parentId"":"""",""pictureUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""name"":""儿科"",""hospitalId"":12,""logoUrl"":""http://192.168.11.43:8080/yyfz/uploadFileDir/user_1/2019-01-24/01bdf6a27f013a5060.jpg"",""code"":""1000012""}],""state"":true}";
                        List<TimeSpanEntity> list = objT["result"].ToObject<List<TimeSpanEntity>>();
                        List<Item> itemList = new List<Item>();
                        SelectSchemaid = String.Empty;
                        lab_timespan.Text = String.Empty;
                        btn_reservation.Enabled = false;
                        if (list.Count > 0)
                        {
                            //list.Reverse();
                            String Period = list[0].period;

                            foreach (TimeSpanEntity timeSpan in list)
                            {
                                if (Period != timeSpan.period)//按照午别分组 添加个空按钮分隔
                                {
                                    Item item1 = new Item();
                                    item1.Enable = false;
                                    item1.name = "";
                                    item1.tag = "";
                                    itemList.Add(item1);
                                    Period = timeSpan.period;
                                }
                                Item item = new Item();
                                item.name = timeSpan.beginTime + "-" + timeSpan.endTime + "    <" + timeSpan.num + ">";
                                item.value = timeSpan.id;
                                item.tag = timeSpan.beginTime + "-" + timeSpan.endTime + "#" + timeSpan.mzType;
                                if (timeSpan.mzType=="2")//特需门诊显红色
                                { 
                                    item.spcialBColor = "Red";
                                }
                                itemList.Add(item);
                            }
                            mcTimeSpan.setDataSource(itemList);


                            /*
                            foreach (var item in list)
                            {
                                if (Period != item.period)//按照午别分组 添加个空按钮分隔
                                {
                                    BorderPanelButton btn1 = new BorderPanelButton();

                                    btn1.BorderSides = Xr.Common.Controls.BorderSides.None;
                                    btn1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
                                    btn1.BtnFont = new System.Drawing.Font("微软雅黑", 12F);
                                    btn1.BtnText = "";
                                    btn1.CenterText = false;
                                    btn1.Enabled = false;
                                    btn1.Dock = System.Windows.Forms.DockStyle.Top;
                                    btn1.FillColor1 = System.Drawing.Color.White;
                                    btn1.FillColor2 = System.Drawing.Color.White;
                                    btn1.Location = new System.Drawing.Point(0, 75);
                                    btn1.Margin = new System.Windows.Forms.Padding(0);
                                    btn1.Tag = "";
                                    btn1.SelctedColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(195)))), ((int)(((byte)(245)))));
                                    btn1.Size = new System.Drawing.Size(170, 25);

                                    this.panel_timespan.Controls.Add(btn1);
                                    Period = item.period;
                                }
                                BorderPanelButton btn = new BorderPanelButton();

                                btn.BorderSides = Xr.Common.Controls.BorderSides.None;
                                btn.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
                                btn.BtnFont = new System.Drawing.Font("微软雅黑", 12F);
                                btn.BtnText = item.beginTime + "-" + item.endTime + "    <" + item.num + ">";
                                btn.CenterText = false;
                                btn.Dock = System.Windows.Forms.DockStyle.Top;
                                btn.FillColor1 = System.Drawing.Color.White;
                                btn.FillColor2 = System.Drawing.Color.White;
                                btn.Location = new System.Drawing.Point(0, 75);
                                btn.Margin = new System.Windows.Forms.Padding(0);
                                btn.Tag = new String[] { item.id, item.beginTime + "-" + item.endTime };
                                btn.SelctedColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(195)))), ((int)(((byte)(245)))));
                                btn.Size = new System.Drawing.Size(170, 25);
                                btn.MouseClick += new System.Windows.Forms.MouseEventHandler(this.timeSpanButton_MouseClick);

                                this.panel_timespan.Controls.Add(btn);

                            }
                             */
                        }
                        else
                        {
                            mcTimeSpan.setDataSource(itemList);
                        }
                        cmd.HideOpaqueLayer();
                        return;
                    }
                    #endregion
                    #region 患者信息
                    else if (workType == AsynchronousWorks.QueryID || workType == AsynchronousWorks.ReadzlCard)
                    {
                        if (result.obj == null)
                        {
                            //_waitForm.DialogResult = DialogResult.OK;
                            //_waitForm.ChangeNoticeComplete(result.msg, Dialog.HintMessageBoxIcon.Error);
                            //_waitForm.Close();
                            cmd.HideOpaqueLayer();
                            return;
                        }
                        JObject objT = result.obj as JObject;
                        lab_patientName.Text = objT["result"]["patientName"].ToString();
                        //lab_name.Text = objT["result"]["age"].ToString();
                        lab_tel.Text = objT["result"]["phone"].ToString();

                        if (objT["result"]["sex"].ToString() == "男")
                        {
                            rBtn_male.Checked = true;
                        }
                        else
                        {
                            rBtn_female.Checked = true;
                        }
                        lueCardType.EditValue = CardType;
                        if (CardType == "1")//患者ID
                        {
                            lab_cardID.Text = objT["result"]["patientId"].ToString();
                        }
                        else if (CardType == "2")//诊疗卡
                        {
                            lab_cardID.Text = objT["result"]["zlk"].ToString();
                        }
                        else if (CardType == "3")//社保卡
                        {
                            lab_cardID.Text = objT["result"]["sbk"].ToString();
                        }
                        else if (CardType == "4")//身份证
                        {
                            lab_cardID.Text = objT["result"]["sfz"].ToString();
                        }
                        else if (CardType == "5")//健康卡
                        {
                            lab_cardID.Text = objT["result"]["jkt"].ToString();
                        }
                        else if (CardType == "6")//健康虚拟卡
                        {
                            lab_cardID.Text = objT["result"]["jktXnk"].ToString();
                        }

                        /*lab_zlk.Text = objT["result"]["zlk"].ToString();
                        lab_jkt.Text = objT["result"]["jkt"].ToString();
                        lab_sbk.Text = objT["result"]["sbk"].ToString();
                         */
                        if (SelectSchemaid != String.Empty)
                        {
                            btn_reservation.Enabled = true;
                        }
                        //MessageBoxUtils.Hint("请选择医生为该患者现场挂号");
                    }
                    #endregion
                    #region 取消现场预约
                    else if (workType == AsynchronousWorks.CancelReservation)
                    {
                        if (result.obj == null)
                        {
                            //_waitForm.DialogResult = DialogResult.OK;
                            //_waitForm.ChangeNoticeComplete(result.msg, Dialog.HintMessageBoxIcon.Error);
                            //_waitForm.Close();
                            return;
                        }

                        JObject objT = result.obj as JObject;
                        if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                        {
                            //重新查询更新状态
                            //workType = AsynchronousWorks.QueryID;
                            //CardID = lab_cardNo.Text;
                            NeedWaitingFrm = false;
                            Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.QueryDoctorAvailableTimeSpan, Argument = new String[] { SelectDeptid, SelectDoctorid, lab_reservationDate.Text } });

                            MessageBoxUtils.Hint(result.msg, MainForm);
                        }
                        else
                        {
                            //MessageBoxUtils.Hint(objT["message"].ToString());
                            MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                        }
                    }
                    #endregion
                    #region 现场预约
                    else if (workType == AsynchronousWorks.Reservation)
                    {
                        if (result.obj == null)
                        {
                            //_waitForm.DialogResult = DialogResult.OK;
                            //_waitForm.ChangeNoticeComplete(result.msg, Dialog.HintMessageBoxIcon.Error);
                            //_waitForm.Close();
                            return;
                        }

                        JObject objT = result.obj as JObject;
                        if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                        {
                            /*
                            lab_name.Text = objT["result"]["hospitalName"].ToString();
                            //lab_name.Text = objT["result"]["age"].ToString();
                            lab_birthday.Text = objT["result"]["deptName"].ToString();
                            lab_tel.Text = objT["result"]["clinicName"].ToString();
                            lab_sex.Text = objT["result"]["queueNum"].ToString();

                            lab_sfz.Text = objT["result"]["waitingNum"].ToString();
                            lab_zlk.Text = objT["result"]["currentTime"].ToString();
                            lab_jkt.Text = objT["result"]["tipMsg"].ToString();
                             */
                            //重新查询更新状态
                            //workType = AsynchronousWorks.QueryID;
                            //CardID = lab_cardNo.Text;
                            NeedWaitingFrm = false;
                            Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.QueryDoctorAvailableTimeSpan, Argument = new String[] { SelectDeptid, SelectDoctorid, lab_reservationDate.Text } });
                            ClearPatientUIInfo();
                            //打印小票
                            /*PrintNote print = new PrintNote(objT["result"]["hospitalName"].ToString(), objT["result"]["deptName"].ToString(), objT["result"]["clinicName"].ToString(), objT["result"]["queueNum"].ToString(), objT["result"]["waitingNum"].ToString(), lab_zlk.Text = objT["result"]["currentTime"].ToString());
                            string message = "";
                            if (!print.Print(ref message))
                            {
                                MessageBoxUtils.Show("打印小票失败：" + message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            }
                            else
                            {
                                MessageBoxUtils.Hint("打印小票完成");
                            }
                             */
                            MessageBoxUtils.Hint(result.msg, MainForm);
                        }
                        else
                        {
                            //MessageBoxUtils.Hint(objT["message"].ToString());
                            MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                        }
                    }
                    #endregion
                    #region 读取身份证
                    else if (workType == AsynchronousWorks.ReadIdCard)
                    {
                        /*
                        _waitForm.DialogResult = DialogResult.OK;
                        //_waitForm.ChangeNoticeComplete(result.msg, Dialog.HintMessageBoxIcon.Error);
                        _waitForm.Close();
                         */
                        //workType = AsynchronousWorks.QueryID;
                        if (!JLIdCardInfoClass.CancelFlag)
                        {
                            Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.QueryID, Argument = new String[] { result.obj.ToString(),"4" } });
                        }
                        else
                        {
                            cmd.HideOpaqueLayer();
                        }
                        //Asynchronous();
                    }
                    #endregion
                    #region 读取社保卡
                    else if (workType == AsynchronousWorks.ReadSocialcard)
                    {
                        //workType = AsynchronousWorks.QueryID;
                        Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.QueryID, Argument = new String[] { result.obj.ToString(),"3" } });
                        //Asynchronous();
                    }
                    #endregion
                    #region 该医生当日预约名单
                    else if (workType == AsynchronousWorks.ReservationPatientList)
                    {
                        this.gv_patient.ClearSorting();//清空排序
                        if (result.obj == null)
                        {
                            cmd.HideOpaqueLayer();
                            return;
                        }

                        JObject objT = result.obj as JObject;
                        List<PatientListEntity> list = objT["result"].ToObject<List<PatientListEntity>>();
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (list[i].cardType.ToString() != "")
                            {
                                list[i].cardType = ListCardType.FirstOrDefault(s => s.value == list[i].cardType).label;
                            }
                        }
                        //if (list.Count > 0)
                        // {
                        gc_patient.DataSource = list;
                        // }
                        cmd.HideOpaqueLayer();
                        return;
                    }
                    #endregion
                }
                else
                {
                    MessageBoxUtils.Show(result.msg, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    cmd.HideOpaqueLayer();
                }
                //MessageBoxUtils.Hint(result.msg);

            }
            catch (InvalidOperationException ex)
            {
                Log4net.LogHelper.Info(ex.Message);
                cmd.HideOpaqueLayer();
                txt_cardNoQuery.Focus();
                //MessageBoxUtils.Hint("操作被取消。", HintMessageBoxIcon.Error, MainForm);
            }
            catch (Exception ex)
            {
                cmd.HideOpaqueLayer();
                //if (ex.Message == "操作被取消。")
                //    MessageBoxUtils.Hint(ex.Message, MainForm);
                //else
                    MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                cmd.HideOpaqueLayer();
            }
            finally
            {
                // 关闭加载提示框
                //DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
                //cmd.HideOpaqueLayer();
                //关闭等待框
                if (NeedWaitingFrm)
                {
                    cmd.HideOpaqueLayer();
                }
                else
                {
                    NeedWaitingFrm = true;
                }
            }
        }
        new void Disposed(object sender, object e)
        {

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!cmd.waitingBox.Visible)
            {
                //开始取消 
                if (backgroundWorker1.IsBusy) //是否在运行异步操作 
                {
                    backgroundWorker1.CancelAsync(); //(是)提交取消命令 
                    if (timer1.Enabled)
                    {
                        timer1.Stop();
                    }
                }
            }
        }
        /// <summary>
        /// 科室点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deptButtonWaiting_MouseClick(object sender, MouseEventArgs e)
        {
            BorderPanelButton btn = sender as BorderPanelButton;
            if (e.Button == MouseButtons.Left)
            {
                //pram = btn.Tag as String;
                SelectDeptid = btn.Tag as String;

                //更新候诊列表
                //WorkType = AsynchronousWorks.WaitingPatientList;
                Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.QueryDoctor, Argument = new String[] { SelectDeptid } });
            }
        }
        /// <summary>
        /// 科室点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeMenuControl1_MenuItemClick(object sender, EventArgs e, object selectItem)
        {
            DeptEntity dept = selectItem as DeptEntity;
            SelectDeptid = dept.id;
            //更新候诊列表
            //WorkType = AsynchronousWorks.WaitingPatientList;
            Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.QueryDoctor, Argument = new String[] { SelectDeptid } });
        }
        /// <summary>
        /// 医生点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcDoctor_MenuItemClick(object sender, EventArgs e)
        {
            ClearReservationUIInfo();
            Label label = null;
            if (typeof(Label).IsInstanceOfType(sender))
            {
                label = (Label)sender;
            }
            else
            {
                PanelEx panelEx = (PanelEx)sender;
                label = (Label)panelEx.Controls[0];
            }
            String[] args = label.Tag.ToString().Split('§');
            SelectDoctorid = args[0];
            lab_docName.Text = "【"+args[1]+"】电话：";
            lab_docMobile.Text = args[2];
            //更新日历
            Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.QueryDoctorAvailableDate, Argument = new String[] { SelectDeptid, SelectDoctorid } });
        }

        /// <summary>
        /// 医生点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void doctorButtonWaiting_MouseClick(object sender, MouseEventArgs e)
        {
            BorderPanelButton btn = sender as BorderPanelButton;
            //pram = btn.Tag as String;
            if (e.Button == MouseButtons.Left)
            {
                ClearReservationUIInfo();
                SelectDoctorid = btn.Tag as String;

                //更新候诊列表
                //WorkType = AsynchronousWorks.WaitingPatientList;
                //Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.WaitingPatientList });

                //更新日历
                Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.QueryDoctorAvailableDate, Argument = new String[] { SelectDeptid, SelectDoctorid } });
                /*List<Dictionary<int, DateTime>> dcs = new List<Dictionary<int, DateTime>>();
                JObject objT = JObject.Parse(@"{""code"":200,""message"":""操作成功"",""result"":[{""workDate"":""2019-02-01""},{""workDate"":""2019-02-02""},{""workDate"":""2019-02-18""},{""workDate"":""2019-02-21""},{""workDate"":""2019-02-22""},{""workDate"":""2019-02-23""},{""workDate"":""2019-03-03""},{""workDate"":""2019-03-13""}],""state"":true}");
                List<AvaDateEntity> list = objT["result"].ToObject<List<AvaDateEntity>>();
                Dictionary<int, DateTime> dc1 = new Dictionary<int, DateTime>();
                string Month = list[0].month;
                foreach (var item in list)
                {
                    if (item.month != Month)
                    {
                        dcs.Add(dc1);
                        dc1 = new Dictionary<int, DateTime>();
                        Month = item.month;

                    }
                    dc1.Add(Int32.Parse(item.day), System.DateTime.Now);
                }
                dcs.Add(dc1);
                reservationCalendar1.ChangeValidDate(dcs);
                //reservationCalendar1.ValidDateLists = dcs;
                //reservationCalendar1.SetGridClanderValue();
                 */

            }
        }
        /// <summary>
        /// 获取预约日历选中日期
        /// </summary>
        /// <param name="SelectedDate"></param>
        private void reservationCalendar1_SelectDate(DateTime SelectedDate)
        {
            //MessageBox.Show(SelectedDate.ToString("yyyy-MM-dd"));
            lab_reservationDate.Text = SelectedDate.ToString("yyyy-MM-dd");
            //更新时间段
            Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.QueryDoctorAvailableTimeSpan, Argument = new String[] { SelectDeptid, SelectDoctorid, SelectedDate.ToString("yyyy-MM-dd") } });
            //更新该医生该日预约患者                prament.Add("deptId", Pras[0]);prament.Add("doctorId", Pras[1]);prament.Add("beginDate", Pras[3]);

        }
        /// <summary>
        /// 时间段点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timeSpanButton_MouseClick(object sender, MouseEventArgs e)
        {
            BorderPanelButton btn = sender as BorderPanelButton;
            //pram = btn.Tag as String;
            if (e.Button == MouseButtons.Left)
            {
                String[] args = btn.Tag as String[];
                SelectSchemaid = args[0];
                lab_timespan.Text = args[1];
                if (Patientid != String.Empty)
                {
                    btn_reservation.Enabled = true;
                }
            }
        }
        /// <summary>
        /// 时间段点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcTimeSpan_MenuItemClick(object sender, EventArgs e)
        {
            Label label = null;
            if (typeof(Label).IsInstanceOfType(sender))
            {
                label = (Label)sender;
            }
            else
            {
                PanelEx panelEx = (PanelEx)sender;
                label = (Label)panelEx.Controls[0];
            }
            SelectSchemaid = label.Name;
            lab_timespan.Text = label.Tag.ToString().Split('#')[0];
            if (label.Tag.ToString().Split('#')[1] == "2")
            {
                lab_mz_typeStr.Text = "特需门诊"; 
            }
            else
            {
                lab_mz_typeStr.Text = "普通门诊";
            }
            if (Patientid != String.Empty)
            {
                btn_reservation.Enabled = true;
            }
        }
        //患者列表右键点击事件
        private void gv_patients_MouseDown(object sender, MouseEventArgs e)
        {
            //鼠标右键点击
            System.Threading.Thread.Sleep(10);
            if (e.Button == MouseButtons.Right)
            {

                //GridHitInfo gridHitInfo = gridView.CalcHitInfo(e.X, e.Y);
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo gridHitInfo = gv_patient.CalcHitInfo(e.X, e.Y);

                if (gridHitInfo.Column != null)
                {
                    if (!gridHitInfo.InColumnPanel)//判断是否在单元格内
                    {

                        int i = gridHitInfo.RowHandle;
                        PatientListEntity obj = gv_patient.GetRow(i) as PatientListEntity;
                        if (obj.status == "0")//只有待签到可取消预约//(obj.status != "4")//判断该记录不为取消状态
                        {
                            RegisterId = obj.id;
                            contextMenuStrip1.Show(gc_patient, e.Location);
                        }
                    }
                }
            }
        }
        //取消预约
        private void CancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBoxUtils.Show("确定为该患者取消预约吗?", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MainForm) == DialogResult.OK)
            {
                //WorkType = AsynchronousWorks.CancelWaiting;
                if (RegisterId != null && RegisterId != String.Empty)
                    Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.CancelReservation, Argument = new String[] { RegisterId } });
                else
                    MessageBoxUtils.Hint("该患者尚未分诊", HintMessageBoxIcon.Error, MainForm);
            }

        }
        private void SpotBookingForm_Resize(object sender, EventArgs e)
        {
            if (cmd == null)
                cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            cmd.rectDisplay = this.DisplayRectangle;
        }
        void ClearPatientUIInfo()
        {
            /*prament.Add("scheduPlanId", Pras[0]);//排班记录主键
            prament.Add("patientId", Pras[1]);//患者主键
            prament.Add("patientName", lab_patientName.Text.Trim());//患者姓名
            prament.Add("cradType", lueCardType.EditValue.ToString());//卡类型
            prament.Add("cradNo", lab_cardID.Text.Trim());//卡号
            prament.Add("tempPhone", lab_tel.Text.Trim());//手机号
             */
            btn_reservation.Enabled = false;

            lab_patientName.Text = String.Empty;
            //lab_name.Text = objT["result"]["age"].ToString();
            lab_cardID.Text = String.Empty;

            lueCardType.ItemIndex = 0;
            lueCardType.Enabled = false;
            lab_tel.Text = String.Empty;
            

            Patientid = String.Empty;
            lueNote.Text = "";
        }
        void ClearReservationUIInfo()
        {
            btn_reservation.Enabled = false;
            //SelectDeptid = String.Empty;
            SelectDoctorid = String.Empty;
            lab_docName.Text = " 电话：";
            lab_docMobile.Text = String.Empty;
            SelectSchemaid = String.Empty;
            lab_reservationDate.Text = String.Empty;
            lab_mz_typeStr.Text = String.Empty;
            lab_timespan.Text = String.Empty;
            reservationCalendar1.ChangeValidDate(new List<Dictionary<int, String>>() );
            this.gv_patient.ClearSorting();//清空排序
            gc_patient.DataSource = null;
            mcTimeSpan.setDataSource( new List<Item>());
            //panel_timespan.Controls.Clear();
        }
        /// <summary>
        ///  科室实体
        /// </summary>
        public class DeptInfoEntity
        {    /// <summary>
            /// 科室ID
            /// </summary>
            public String id { get; set; }
            /// <summary>
            /// 上级科室ID
            /// </summary>
            public String parentId { get; set; }
            /// <summary>
            /// 图片地址
            /// </summary>
            public String pictureUrl { get; set; }
            /// <summary>
            /// 科室名称
            /// </summary>
            public String name { get; set; }
            /// <summary>
            /// 科室所属医院ID
            /// </summary>
            public String hospitalId { get; set; }
            /// <summary>
            /// 图标地址 
            /// </summary>
            public String logoUrl { get; set; }
            /// <summary>
            /// 科室代码
            /// </summary>
            public String code { get; set; }

        }
        /// <summary>
        ///  医生实体
        /// </summary>
        public class DoctorInfoEntity
        {    /// <summary>
            /// 医生编码
            /// </summary>
            public String code { get; set; }
            /// <summary>
            /// 医生主键
            /// </summary>
            public String id { get; set; }
            /// <summary>
            /// 医生姓名
            /// </summary>
            public String name { get; set; }
            /// <summary>
            /// 节日标志
            /// </summary>
            public String ignoreHoliday { get; set; }
            /// <summary>
            /// 
            /// </summary>
            //public String ignoreYear { get; set; }
            /// <summary>
            /// 是否显示
            /// </summary>
            public String isShow { get; set; }
            /// <summary>
            /// 启用标志
            /// </summary>
            public String isUse { get; set; }
            /// <summary>
            /// 职称
            /// </summary>
            public String job { get; set; }
            /// <summary>
            /// 手机号码
            /// </summary>
            public String mobile { get; set; }

        }
        /// <summary>
        ///  预约时间段实体
        /// </summary>
        public class TimeSpanEntity
        {    /// <summary>
            /// 排班主键
            /// </summary>
            public String id { get; set; }
            /// <summary>
            /// 时段码：0、1、2、3
            /// </summary>
            public String period { get; set; }
            /// <summary>
            /// 时段名称：上午、下午、晚上、全天
            /// </summary>
            public String periodName { get; set; }
            /// <summary>
            /// 开始时间
            /// </summary>
            public String beginTime { get; set; }
            /// <summary>
            /// 结束时间
            /// </summary>
            public String endTime { get; set; }
            /// <summary>
            /// 剩余号源数量
            /// </summary>
            public String num { get; set; }
            /// <summary>
            /// 门诊类型
            /// </summary>
            public String mzType { get; set; }

        }
        /// <summary>
        ///  患者列表实体
        /// </summary>
        public class PatientListEntity
        {
            /// <summary>
            /// 预约主键
            /// </summary>
            public String id { get; set; }
            public String cardNo { get; set; }
            public String cardType { get; set; }
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
            /// 预约就诊日期
            /// </summary>
            public String workDate { get; set; }
            /// <summary>
            /// 周几：周一、周二...
            /// </summary>
            public String week { get; set; }
            /// <summary>
            /// 时间，号源所在的时间段beginTime-endTime,如：08:00-08:30
            /// </summary>
            public String time { get; set; }
            /// <summary>
            /// 科室名称
            /// </summary>
            public String deptName { get; set; }
            /// <summary>
            /// 医生姓名
            /// </summary>
            public String doctorName { get; set; }
            /// <summary>
            /// 预约状态
            /// </summary>
            public String status { get; set; }


            /// <summary>
            /// 预约途径
            /// </summary>
            public String registerWay { get; set; }
            /// <summary>
            /// 就诊类别,初诊、复诊
            /// </summary>
            public String visitType { get; set; }
            /// <summary>
            /// 术后复诊
            /// </summary>
            public String isShfz { get; set; }
            /// <summary>
            /// 出院复诊
            /// </summary>
            public String isCyfz { get; set; }
            /// <summary>
            /// 院外转诊
            /// </summary>
            public String isYwzz { get; set; }
            /// <summary>
            /// 就诊时间
            /// </summary>
            public String registerTime { get; set; }
            /// <summary>
            /// 备注
            /// </summary>
            public String note { get; set; }
            /// <summary>
            /// 年龄：30岁1月
            /// </summary>
            public String age { get; set; }
            /// <summary>
            /// 联系电话
            /// </summary>
            public String tempPhone { get; set; }
            /// <summary>
            /// 联系地址
            /// </summary>
            public String address { get; set; }
            /// <summary>
            /// 预约状态Txt
            /// </summary>
            public String statusTxt { get; set; }
            /// <summary>
            /// 预约途径TxT
            /// </summary>
            public String registerWayTxt { get; set; }
            private String _visitTypeTxt;
            /// <summary>
            /// 就诊类别TxT,就诊类别：0.初诊 ，1.复诊
            /// </summary>
            public String visitTypeTxt
            {
                get
                {
                    if (_visitTypeTxt == "0")
                    {
                        return "初诊";
                    }
                    else 
                    {
                        return "复诊";
                    }
                }

                set
                {
                    _visitTypeTxt = value;
                }
            }
        }
        /// <summary>
        ///  可约日期实体
        /// </summary>
        public class AvaDateEntity
        {    /// <summary>
            /// 可约日期
            /// </summary>
            public String workDate { get; set; }
            /// <summary>
            /// 周
            /// </summary>
            public String weekName { get; set; }
            /// <summary>
            /// 开放总数
            /// </summary>
            public int sumNum { get; set; }
            /// <summary>
            /// 已预约总数
            /// </summary>
            public int regNum { get; set; }
            /// <summary>
            /// 剩余数量
            /// </summary>
            public int syNum { get; set; }
            /// <summary>
            ///  年
            /// </summary>
            public String year
            {
                get 
                {
                    return workDate.Substring(0,4); 
                }
            }
            /// <summary>
            /// 月 
            /// </summary>
            public String month {
                get
                {
                    return workDate.Substring(5, 2);
                }
            }
            /// <summary>
            /// 日
            /// </summary>
            public String day {
                get
                {
                    return workDate.Substring(8, 2);
                }
            }

        }

        private void reservationCalendar1_ChangeMonth(DateTime SelectedMonth)
        {
            //changeMonth 置空
        }

        private void lueCardTypeQuery_QueryCloseUp(object sender, CancelEventArgs e)
        {
            txt_cardNoQuery.Focus();
        }









        /*
        #region 获取卡类型
        /// <summary>
        /// 获取卡类型
        /// </summary>
        public void GetCardType()
        {
            try
            {
                Dictionary<string, string> prament = new Dictionary<string, string>();
                prament.Add("type", "card_type");
                Xr.RtCall.Model.RestSharpHelper.ReturnResult<List<string>>("api/sys/sysDict/findByType", prament, Method.POST,
               result =>
               {
                   if (result.Data != null)
                   {
                       LogClass.WriteLog("请求结果：" + string.Join(",", result.Data.ToArray()));
                   }
                   #region
                   switch (result.ResponseStatus)
                   {
                       case ResponseStatus.Completed:
                           if (result.StatusCode == System.Net.HttpStatusCode.OK)
                           {
                               JObject objT = JObject.Parse(string.Join(",", result.Data.ToArray()));
                               if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                               {
                                   List<CardType> lsit = objT["result"].ToObject<List<CardType>>();
                                   _context.Send((s) => lueIsUse.Properties.DataSource = lsit, null);
                                   _context.Send((s) => lueIsUse.Properties.DisplayMember = "label", null);
                                   _context.Send((s) => lueIsUse.Properties.ValueMember = "value", null);
                                   _context.Send((s) => lueIsUse.ItemIndex = selectLue, null);
                               }
                               else
                               {
                                   MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                               }
                           }
                           break;
                   }
                   #endregion
               });
            }
            catch (Exception ex)
            {
                LogClass.WriteLog("获取卡类型错误信息：" + ex.Message);
            }
        }
        #endregion
         */
    }
}
