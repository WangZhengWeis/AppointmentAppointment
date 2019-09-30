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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors;
using Xr.RtManager.Module.triage;

namespace Xr.RtManager.Pages.booking
{
    public partial class AppointmentQueryForm : UserControl
    {
        private Form MainForm; //主窗体
        Xr.Common.Controls.OpaqueCommand cmd;
        public AppointmentQueryForm()
        {
            InitializeComponent();
            MainForm = (Form)this.Parent;
            //cmd = new Xr.Common.Controls.OpaqueCommand(this);
            //cmd.ShowOpaqueLayer(225, true);
        }
        /// <summary>
        /// 查询卡号
        /// </summary>
        private string CardID = String.Empty;
        /// <summary>
        /// 患者主键
        /// </summary>
        private string Patientid = String.Empty;
        /// <summary>
        ///预约记录主键 
        /// </summary>
        private String RegisterId = String.Empty; //用于列表鼠标右键取消候诊的
        /// <summary>
        ///预约记录主键 
        /// </summary>
        private String registerId = String.Empty; //用于取消待签到的
        /// <summary>
        ///状态
        /// </summary>
        private String status = String.Empty; 
        bool NeedWaitingFrm = true;
        bool isFirstload = true;
        private void UserForm_Load(object sender, EventArgs e)
        {
            cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            //下拉框数据
            getLuesInfo();
            //配置时间格式
            setDateFomartDefult();
            性别.Caption = "性\r\n别";
            状态.Caption = "状\r\n态";
            就诊类别.Caption = "就诊\r\n类别";
            术后复诊.Caption = "术后\r\n复诊";
            出院复诊.Caption = "出院\r\n复诊";
            外院转诊.Caption = "外院\r\n转诊";
            登记时间.Caption = "登记\r\n时间";
            isFirstload = false;
        }
        /// <summary>
        /// 下拉框数据
        /// </summary>
        void getLuesInfo()
        {
            //查询科室下拉框数据
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
                if (deptList.Count > 0)
                    treeDeptId.EditValue = deptList[0].id;
            }
            else
            {
                MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                return;
            }
            /*//若没配置科室编码让其选择一个
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
                List<Dic> list = new List<Dic>();
                list.Add(new Dic { label = "全部", value = "" });
                list.AddRange(objT["result"].ToObject<List<Dic>>());
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

            //卡类型下拉框数据
            param = "type={0}";
            param = String.Format(param, "card_type");

            url = String.Empty;
            url = AppContext.AppConfig.serverUrl + "sys/sysDict/findByType?" + param;
            objT = new JObject();
            objT = JObject.Parse(HttpClass.httpPost(url));
            if (string.Compare(objT["state"].ToString(), "true", true) == 0)
            {
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

            //预约途径下拉框数据
            param = "type={0}";
            param = String.Format(param, "register_way_type");

            url = AppContext.AppConfig.serverUrl + "sys/sysDict/findByType?" + param;
            objT = JObject.Parse(HttpClass.httpPost(url));
            if (string.Compare(objT["state"].ToString(), "true", true) == 0)
            {
                List<Dic> list = new List<Dic>();
                list.Add(new Dic { label = "全部", value = "" });
                list.AddRange(objT["result"].ToObject<List<Dic>>());
                lueRegisterWay.Properties.DataSource = list;
                lueRegisterWay.Properties.DisplayMember = "label";
                lueRegisterWay.Properties.ValueMember = "value";
                lueRegisterWay.ItemIndex = 0;
            }
            else
            {
                MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
            }
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
                    luDoctords.Properties.DataSource = doctorInfoEntity;
                    luDoctords.Properties.DisplayMember = "name";
                    luDoctords.Properties.ValueMember = "id";
                    luDoctords.ItemIndex = 0;
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
                (sender as TextEdit).SelectAll();
            });
        }
        private void txt_cardNoQuery_KeyUp(object sender, KeyEventArgs e)
        {
            //允许Ctrl+v粘贴数字
            if (e.KeyData == (Keys.Control | Keys.V))
            {
                if (Clipboard.ContainsText())
                {
                    try
                    {
                        Convert.ToInt64(Clipboard.GetText());  //检查是否数字
                        //((TextEdit)sender).SelectedText = Clipboard.GetText().Trim(); //Ctrl+V 粘贴  
                        ((TextEdit)sender).Text = Clipboard.GetText().Trim();

                    }
                    catch (Exception)
                    {
                        e.Handled = true;
                        //throw;
                    }
                }
            }

            if (txt_cardNoQuery.Text != String.Empty)
            {
                if (e.KeyCode == Keys.Control || e.KeyCode == Keys.Enter)
                {
                    CardID = txt_cardNoQuery.Text;
                    Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.ReadzlCard, Argument = new String[] { CardID, lueCardTypeQuery.EditValue.ToString() } });
                    txt_cardNoQuery.Text = String.Empty;
                    gcAppointmentInfo.Focus();
                }
            }
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
                //CardID = "000675493100";
                Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.ReadzlCard, Argument = new String[] { CardID, "2" } });
            }
            lueCardTypeQuery.ItemIndex = 1;
            txt_cardNoQuery.Focus();
        }
        private void btn_readSocialcard_Click(object sender, EventArgs e)
        {

            //WorkType = AsynchronousWorks.ReadSocialcard;
            timer1.Start();
            Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.ReadSocialcard });
        }

        private void btn_readIdcard_Click(object sender, EventArgs e)
        {
            //WorkType = AsynchronousWorks.ReadIdCard;
            timer1.Start();
            Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.ReadIdCard });
        }
        public bool CancelFlag = false;
        private void buttonControl1_Click(object sender, EventArgs e)
        {
            //cmd = new OpaqueCommand(AppContext.Session.waitControl);
            if (VerifyInfo())
            {
                QueryInfo();
            }
            checkEdit1.Checked = true;
            panel7.Enabled = false;
        }
        AppointmentQueryParam CurrentParam = new AppointmentQueryParam();
        private bool VerifyInfo()
        { //deStart.Text,
            //deEnd.Text
            if (treeDeptId.EditValue == " ")
            {
                MessageBoxUtils.Hint("请选择科室", HintMessageBoxIcon.Error, MainForm);
                return false;
            }
            if (deStart.EditValue == null)
            {
                MessageBoxUtils.Hint("请选择开始日期", HintMessageBoxIcon.Error, MainForm);
                return false;
            }
            if (deEnd.EditValue == null)
            {
                MessageBoxUtils.Hint("请选择结束日期", HintMessageBoxIcon.Error, MainForm);
                return false;
            }

            DateTime dtStart = new DateTime();
            DateTime dtEnd = new DateTime();
                dtStart = DateTime.ParseExact(deStart.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
                dtEnd = DateTime.ParseExact(deEnd.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
            
            if (dtEnd < dtStart)
            {
                MessageBoxUtils.Hint("结束日期需大于开始日期", HintMessageBoxIcon.Error, MainForm);
                return false;
            }

            CurrentParam.deptId = treeDeptId.EditValue.ToString();
            CurrentParam.docId = luDoctords.EditValue.ToString();//""为全部
            CurrentParam.patientName = txt_nameQuery.Text;
            CurrentParam.registerWay = lueRegisterWay.EditValue.ToString();
            CurrentParam.status = lueState.EditValue.ToString();
            CurrentParam.startDate = deStart.Text;
            CurrentParam.endDate = deEnd.Text;

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
                prament.Add("doctorId", CurrentParam.docId);
                if (CurrentParam.registerWay != String.Empty)
                    prament.Add("registerWay", CurrentParam.registerWay);
                if (CurrentParam.status != String.Empty)
                    prament.Add("status", CurrentParam.status);
                if (CurrentParam.patientName != String.Empty)
                    prament.Add("patientName", CurrentParam.patientName);
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
                this.gv_AppointmentInfo.ClearSorting();//清空排序
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
                    if (list.Count == 0)
                    {
                        ClearUIInfo();
                        MessageBoxUtils.Hint("没有查询到相应信息", HintMessageBoxIcon.Error, MainForm);
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].beginTime=list[i].beginTime+"-"+list[i].endTime;
                    }
                    this.gcAppointmentInfo.DataSource = list;
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
                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(RunWorkerCompleted);
                bw.WorkerSupportsCancellation = true;
                //异步调用结束后释放操作
                //bw.Disposed += new EventHandler(Disposed);
                //开始异步操作
                bw.RunWorkerAsync(ars);

                //打开等待框
                 if (ars.WorkType == AsynchronousWorks.ReadzlCard)
                {
                    cmd.IsShowCancelBtn = false;
                    cmd.ShowOpaqueLayer(0.56f, "请稍候...");
                }

                else if (ars.WorkType == AsynchronousWorks.ReadIdCard || ars.WorkType == AsynchronousWorks.ReadSocialcard)
                {
                    cmd.IsShowCancelBtn = true;
                    cmd.ShowOpaqueLayer(0.56f, "正在读取...");
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
            #region 查询患者信息
            if (workType == AsynchronousWorks.QueryID || workType == AsynchronousWorks.ReadzlCard)
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

                    prament.Add("cardNo", Pras[0]);
                    prament.Add("cardType", Pras[1]);
                    if (prament.Count != 0)
                    {
                        param = string.Join("&", prament.Select(x => x.Key + "=" + x.Value).ToArray());
                    }
                    String url = AppContext.AppConfig.serverUrl + "patmi/findPatMiByTyptAndCardNo?" + param;
                    String jsonStr = HttpClass.httpPost(url);
                    JObject objT = JObject.Parse(jsonStr);


                    List<JObject> objTs = new List<JObject>();
                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
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
            #region 查询预约信息
            else if (workType == AsynchronousWorks.ReservationPatientList)
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
                //prament.Add("doctorId", CurrentParam.docId);
                if (CurrentParam.registerWay != String.Empty)
                    prament.Add("registerWay", CurrentParam.registerWay);
                if (CurrentParam.status != String.Empty)
                    prament.Add("status", CurrentParam.status);
                if (!checkEdit1.Checked)
                {
                    if (CurrentParam.patientName != String.Empty)
                        prament.Add("patientName", CurrentParam.patientName);
                }
                prament.Add("startDate", CurrentParam.startDate);
                prament.Add("endDate", CurrentParam.endDate);

                prament.Add("patientId", Pras[0]);
                prament.Add("pageSize", "10000");

                String url = String.Empty;
                if (prament.Count != 0)
                {
                    param = string.Join("&", prament.Select(x => x.Key + "=" + x.Value).ToArray());
                }
                url = AppContext.AppConfig.serverUrl + "sch/doctorScheduRegister/list?" + param;
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
                 if(backgroundWorker1.IsBusy)
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
        void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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
                    #region 患者信息
                    if (workType == AsynchronousWorks.QueryID || workType == AsynchronousWorks.ReadzlCard)
                    {
                        if (result.obj == null)
                        {
                            //_waitForm.DialogResult = DialogResult.OK;
                            //_waitForm.ChangeNoticeComplete(result.msg, Dialog.HintMessageBoxIcon.Error);
                            //_waitForm.Close();
                            cmd.HideOpaqueLayer();
                            return;
                        }
                        List<JObject> objTs = result.obj as List<JObject>;//{"code":200,"message":"操作成功","result":{"age":"32","birthday":"1987-12-22","jkt":"003000005010","patientId":"000675493100","patientName":"李鹏真","phone":"17666476268","sbk":"11111111","sex":"男","sfz":"45032219871222151X","zlk":"02071196"},"state":true}
                        JObject objT = objTs[0];
                        Patientid = objT["result"]["patientId"].ToString();
                        NeedWaitingFrm = false;
                        Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.ReservationPatientList, Argument = new String[] { Patientid } });

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
                    #region 患者预约信息
                    else if (workType == AsynchronousWorks.ReservationPatientList)
                    {
                        if (result.obj == null)
                        {
                            //_waitForm.DialogResult = DialogResult.OK;
                            //_waitForm.ChangeNoticeComplete(result.msg, Dialog.HintMessageBoxIcon.Error);
                            //_waitForm.Close();
                            cmd.HideOpaqueLayer();
                            ClearUIInfo();
                            return;
                        }
                        JObject objT = result.obj as JObject;
                        if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                        {
                            List<AppointmentEntity> list = objT["result"]["list"].ToObject<List<AppointmentEntity>>();
                            if (list.Count == 0)
                            {
                                ClearUIInfo();
                                MessageBoxUtils.Hint("没有查询到相应信息", HintMessageBoxIcon.Error, MainForm); 
                            }
                            for (int i = 0; i < list.Count; i++)
                            {
                                list[i].beginTime = list[i].beginTime + "-" + list[i].endTime;   
                            }
                            this.gv_AppointmentInfo.ClearSorting();//清空排序
                            this.gcAppointmentInfo.DataSource = list;
                        }
                        else
                        {
                            ClearUIInfo();
                            MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                            return;
                        }

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
                            if (VerifyInfo())
                            {
                                Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.ReservationPatientList, Argument = new String[] { Patientid } });
                            }
                            MessageBoxUtils.Hint(result.msg, MainForm);
                        }
                        else
                        {
                            //MessageBoxUtils.Hint(objT["message"].ToString());
                            MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                        }
                    }
                    #endregion
                }
                else
                {
                    ClearUIInfo();
                    //Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.ReservationPatientList, Argument = new String[] { Patientid } });
                    MessageBoxUtils.Show(result.msg, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    cmd.HideOpaqueLayer();
                }
                //MessageBoxUtils.Hint(result.msg);

            }
            catch (InvalidOperationException ex)
            {
                Log4net.LogHelper.Info(ex.Message);
                ClearUIInfo();
                cmd.HideOpaqueLayer();
                txt_cardNoQuery.Focus();
                //MessageBoxUtils.Hint("操作被取消。", HintMessageBoxIcon.Error, MainForm);
            }
            catch (Exception ex)
            {
                ClearUIInfo();
                cmd.HideOpaqueLayer();
                //if (ex.Message == "操作被取消。")
                //    MessageBoxUtils.Hint(ex.Message, HintMessageBoxIcon.Error, MainForm);
                //else
                    MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
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
        private void gv_AppointmentInfo_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            GridView gv = sender as GridView;

            AppointmentEntity CurrentrowItem = gv.GetRow(e.RowHandle) as AppointmentEntity;
            lab_deptName.Text = CurrentrowItem.deptName;
            lab_patientName.Text = CurrentrowItem.patientName;
            lab_visitType.Text = CurrentrowItem.visitType;
            lab_doctorName.Text = CurrentrowItem.doctorName;
            lab_sex.Text = CurrentrowItem.sex;
            lab_cardType.Text = CurrentrowItem.cardType;
            lab_beginTime.Text = CurrentrowItem.beginTime;
            lab_age.Text = CurrentrowItem.age;
            lab_cardNo.Text = CurrentrowItem.cardNo;
            lab_statusTxt.Text = CurrentrowItem.statusTxt;
            lab_tempPhone.Text = CurrentrowItem.tempPhone;
            lab_registerTime.Text = CurrentrowItem.registerTime;
            lab_address.Text = CurrentrowItem.address;
            lab_note.Text = CurrentrowItem.note;

            lab_cancelOperaName.Text = CurrentrowItem.cancelOperaName;
            lab_cancelTime.Text = CurrentrowItem.cancelTime;
            lab_cancelWayTxt.Text = CurrentrowItem.cancelWayTxt;

            registerId = CurrentrowItem.id;
            status = CurrentrowItem.status;
            if (CurrentrowItem.status == "0")//只有待签到可取消预约
            {
                buttonControl2.Enabled = true;
            }
            else
            {
                buttonControl2.Enabled = false;
            }
            //鼠标右键点击
            System.Threading.Thread.Sleep(10);
            if (e.Button == MouseButtons.Right)
            {
                if (CurrentrowItem.status == "0")//只有待签到可取消预约
                {
                    RegisterId = CurrentrowItem.id;
                    contextMenuStrip1.Show(gcAppointmentInfo, e.Location);
                }
                else
                { 

                }
            }
        }
        private void gv_AppointmentInfo_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            GridView gv = sender as GridView;

            AppointmentEntity CurrentrowItem = gv.GetRow(e.FocusedRowHandle) as AppointmentEntity;
            if (CurrentrowItem != null)
            {
                lab_deptName.Text = CurrentrowItem.deptName;
                lab_patientName.Text = CurrentrowItem.patientName;
                lab_visitType.Text = CurrentrowItem.visitType;
                lab_doctorName.Text = CurrentrowItem.doctorName;
                lab_sex.Text = CurrentrowItem.sex;
                lab_cardType.Text = CurrentrowItem.cardType;
                lab_beginTime.Text = CurrentrowItem.beginTime;
                lab_age.Text = CurrentrowItem.age;
                lab_cardNo.Text = CurrentrowItem.cardNo;
                lab_statusTxt.Text = CurrentrowItem.statusTxt;
                lab_tempPhone.Text = CurrentrowItem.tempPhone;
                lab_registerTime.Text = CurrentrowItem.registerTime;
                lab_address.Text = CurrentrowItem.address;
                lab_note.Text = CurrentrowItem.note;

                lab_cancelOperaName.Text = CurrentrowItem.cancelOperaName;
                lab_cancelTime.Text = CurrentrowItem.cancelTime;
                lab_cancelWayTxt.Text = CurrentrowItem.cancelWayTxt;

                registerId = CurrentrowItem.id;
                status = CurrentrowItem.status;
                if (CurrentrowItem.status == "0")//只有待签到可取消预约
                {
                    buttonControl2.Enabled = true;
                }
                else
                {
                    buttonControl2.Enabled = false;
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
        public void setDateFomartDefult()
        {
            this.deStart.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.deStart.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deStart.Properties.EditFormat.FormatString = "yyyy-MM-dd";
            this.deStart.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deStart.Properties.Mask.EditMask = "yyyy-MM-dd";
            this.deStart.Properties.VistaCalendarInitialViewStyle = VistaCalendarInitialViewStyle.MonthView;
            //this.deStart.EditValue = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.AddMonths(-1).Month, 1);
            this.deStart.EditValue = System.DateTime.Now;//new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, 1);

            this.deEnd.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.deEnd.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deEnd.Properties.EditFormat.FormatString = "yyyy-MM-dd";
            this.deEnd.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deEnd.Properties.Mask.EditMask = "yyyy-MM-dd";
            this.deEnd.Properties.VistaCalendarInitialViewStyle = VistaCalendarInitialViewStyle.MonthView;
            this.deEnd.Properties.VistaCalendarViewStyle = ((DevExpress.XtraEditors.VistaCalendarViewStyle)((DevExpress.XtraEditors.VistaCalendarViewStyle.MonthView | DevExpress.XtraEditors.VistaCalendarViewStyle.YearView)));
            //this.deEnd.EditValue = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.AddMonths(1).Month, System.DateTime.Now.Day);
            this.deEnd.EditValue = System.DateTime.Now.AddDays(7); //new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day);
        }

        private void AppointmentQueryForm_Resize(object sender, EventArgs e)
        {
            if(cmd==null)
                cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            cmd.rectDisplay = this.DisplayRectangle;
        }
        private void treeDeptId_EditValueChanged(object sender, EventArgs e)
        {
            GetDoctorLue();
            //if (!isFirstload)
            //{
            //    if (VerifyInfo())
            //    {
            //        Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.ReservationPatientList, Argument = new String[] { Patientid } });
            //    }
            //}
        }

        private void lueState_EditValueChanged(object sender, EventArgs e)
        {
            //if (!isFirstload)
            //{
            //    if (VerifyInfo())
            //    {
            //        Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.ReservationPatientList, Argument = new String[] { Patientid } });
            //    }
            //}
        }

        private void lueRegisterWay_EditValueChanged(object sender, EventArgs e)
        {
            //if (!isFirstload)
            //{
            //    if (VerifyInfo())
            //    {
            //        Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.ReservationPatientList, Argument = new String[] { Patientid } });
            //    }
            //}
        }
        private void deStart_EditValueChanged(object sender, EventArgs e)
        {
            //if (!isFirstload)
            //{
            //    if (VerifyInfo())
            //    {
            //        Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.ReservationPatientList, Argument = new String[] { Patientid } });
            //    }
            //}
            
        }

        private void deEnd_EditValueChanged(object sender, EventArgs e)
        {
            //if (VerifyInfo())
            //{
            //    Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.ReservationPatientList, Argument = new String[] { Patientid } });
            //}
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            Patientid = String.Empty;
            txt_cardNoQuery.Text = String.Empty;
            
            if (checkEdit1.Checked)
            {
                panel7.Enabled = false;
                ClearUIInfo();
            }
            else
            {
                panel7.Enabled = true;
                txt_nameQuery.Text = String.Empty;
                ClearUIInfo();
            }
            /*if (VerifyInfo())
            {
                Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.ReservationPatientList, Argument = new String[] { Patientid,"True" } });
            }
             */
        }
        private void ClearUIInfo()
        {
            this.gv_AppointmentInfo.ClearSorting();//清空排序
            gcAppointmentInfo.DataSource = null;

            lab_deptName.Text = String.Empty;
            lab_patientName.Text = String.Empty;
            lab_visitType.Text = String.Empty;
            lab_doctorName.Text = String.Empty;
            lab_sex.Text = String.Empty;
            lab_cardType.Text = String.Empty;
            lab_beginTime.Text = String.Empty;
            lab_age.Text = String.Empty;
            lab_cardNo.Text = String.Empty;
            lab_statusTxt.Text = String.Empty;
            lab_tempPhone.Text = String.Empty;
            lab_registerTime.Text = String.Empty;
            lab_address.Text = String.Empty;
            lab_note.Text = String.Empty;

            lab_cancelOperaName.Text = String.Empty;
            lab_cancelTime.Text = String.Empty;
            lab_cancelWayTxt.Text = String.Empty;
        }

        private void buttonControl2_Click(object sender, EventArgs e)
        {
            if (registerId == null) return;
            if (!status.Equals("0"))
            {
                MessageBoxUtils.Hint("只能取消待签到的预约记录", HintMessageBoxIcon.Error, MainForm);
                return;
            }
            if (MessageBoxUtils.Show("确定为该患者取消预约吗?", MessageBoxButtons.OKCancel,
                       MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MainForm) == DialogResult.OK)
            {
                Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.CancelReservation, Argument = new String[] { registerId } });
            }
        }

        private void lueCardTypeQuery_QueryCloseUp(object sender, CancelEventArgs e)
        {
            txt_cardNoQuery.Focus();
        }
    }
    /// <summary>
    /// 下拉框数据
    /// </summary>
    public class Dic
    {
        public string value { get; set; }
        public string label { get; set; }
    }
    /// <summary>
    /// 查询参数实体
    /// </summary>
    public class AppointmentQueryParam
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
        public String docId { get; set; }
        /// <summary>
        /// 预约途径
        /// </summary>
        public String registerWay { get; set; }
        /// <summary>
        /// 预约状态
        /// </summary>
        public String status { get; set; }
        /// <summary>
        /// 患者姓名
        /// </summary>
        public String patientName { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public String startDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public String endDate { get; set; }
    }
    /// <summary>
    ///  预约信息实体
    /// </summary>
    public class AppointmentEntity
    {
        /// <summary>
        /// 预约主键
        /// </summary>
        public String id { get; set; }
        /// <summary>
        /// 患者姓名
        /// </summary>
        public String patientName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public String sex { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public String age { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public String tempPhone { get; set; }
        /// <summary>
        /// 预约卡类型 
        /// </summary>
        public String cardType { get; set; }
        /// <summary>
        /// 预约卡号 
        /// </summary>
        public String cardNo { get; set; }
        /// <summary>
        /// 预约日期
        /// </summary>
        public String workDate { get; set; }
        /// <summary>
        /// 预约周
        /// </summary>
        public String week { get; set; }
        /// <summary>
        /// 预约时间
        /// </summary>
        public String beginTime { get; set; }
        public String endTime { get; set; }
        /// <summary>
        /// 科室名称
        /// </summary>
        public String deptName { get; set; }
        /// <summary>
        /// 医生姓名
        /// </summary>
        public String doctorName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public String status { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public String statusTxt { get; set; }
        /// <summary>
        /// 途径 
        /// </summary>
        public String registerWay { get; set; }
        /// <summary>
        /// 就诊类别 
        /// </summary>
        public String visitType { get; set; }
        /// <summary>
        /// 途径描述 
        /// </summary>
        public String registerWayTxt { get; set; }
        /// <summary>
        /// 就诊类别描述 
        /// </summary>
        public String visitTypeTxt { get; set; }
        /// <summary>
        /// 术后复诊
        /// </summary>
        public String isShfz { get; set; }
        /// <summary>
        /// 出院复诊
        /// </summary>
        public String isCyfz { get; set; }

        /// <summary>
        /// 外院转诊
        /// </summary>
        public String isYwzz { get; set; }
        /// <summary>
        /// 登记时间
        /// </summary>
        public String registerTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String note { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public String address { get; set; }
        /// <summary>
        /// 取消操作者
        /// </summary>
        public String cancelOperaName { get; set; }
        /// <summary>
        /// 取消时间
        /// </summary>
        public String cancelTime { get; set; }
        /// <summary>
        /// 取消方式
        /// </summary>
        public String cancelWayTxt { get; set; }
        
    }
}
