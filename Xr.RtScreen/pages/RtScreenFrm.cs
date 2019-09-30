using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Xr.RtScreen.RtUserContronl;
using System.Threading;
using RestSharp;
using System.Net;
using Xr.RtScreen.VoiceCall;
using Newtonsoft.Json.Linq;
using Xr.RtScreen.Models;

namespace Xr.RtScreen.pages
{
    public partial class RtScreenFrm : UserControl
    {
        public SynchronizationContext _context;
        public String IP { get; set; }
        public RtScreenFrm()
        {
            InitializeComponent();
            _context = SynchronizationContext.Current;
            Control.CheckForIllegalCrossThreadCalls = false;
            #region 
            tableLayoutPanel1.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanel1, true, null);
            scrollingText1.Font = new Font("微软雅黑", Convert.ToInt32(AppContext.AppConfig.FontSize), FontStyle.Regular);
            scrollingTexts1.Font = new Font("微软雅黑", (Convert.ToInt32(AppContext.AppConfig.FontSize) + 5), FontStyle.Regular);
            label1.Font = new Font("微软雅黑", Convert.ToInt32(AppContext.AppConfig.FontSize), FontStyle.Regular);
            scrollingText2.Font = new Font("微软雅黑", Convert.ToInt32(AppContext.AppConfig.FontSize), FontStyle.Regular);
            tableLayoutPanel2.Height = 75;
            IsFirstGui = true;
            DoctorSittingConsultations();
            DepartmentWaiting();
            //if (Convert.ToBoolean(AppContext.AppConfig.IsOpenVioce))
            //{
            //    var speakVoiceform = new SpeakVoicemainFrom();
            //    speakVoiceform.setFormTextValue += new Xr.RtScreen.VoiceCall.SpeakVoicemainFrom.setTextValue(form2_setFormTextValue);
            //    speakVoiceform.Show(this);
            //}
            #endregion
            IP = GetIP();
            findCallListByScreen();
            time();
            RefulwaitingDesc();
            frequency = 0;
            SumCount = 0;
            VoiceNum = 0;
            queue = new Queue<string>();
            SetScrolling();
            times();
        }
        #region 获取本地IP
        protected string GetIP() //获取本地IP
        {
            IPHostEntry ipHost = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            Log4net.LogHelper.Info("获取到的本机IP地址为：" + ipAddr.ToString());
            return ipAddr.ToString();
        }
        #endregion
        #region 获取语音播放的记录
        private int VoiceSum = 0;
        public static List<string> list = new List<string>();
        Queue<string> queue = new Queue<string>();
        public void findCallListByScreen()
        {
            try
            {
                //VoiceSum++;
                var Url = string.Empty;
                if (Form1.ScreenType == "1")
                {
                    Url = AppContext.AppConfig.serverUrl + InterfaceAddress.findCallListByScreen + "?hospitalId=" + HelperClass.hospitalId + "&deptIds=" + HelperClass.deptId + "&remoteAddr=" + IP;
                }
                else
                {
                    Url = AppContext.AppConfig.serverUrl + InterfaceAddress.findCallListByScreen + "?hospitalId=" + HelperClass.hospitalId + "&deptIds=" + HelperClass.deptId + "&clinicId=" + HelperClass.clincId + "&remoteAddr=" + IP;
                }
                    var result = Xr.Http.HttpClass.httpPost(Url);
                    var objT = Newtonsoft.Json.Linq.JObject.Parse(result);
                    Log4net.LogHelper.Info("候诊大屏获取语音呼号记录请求返回结果：" + objT);
                    if (objT["state"].ToString().ToLower() == "true")
                    {
                        var jars = JArray.Parse(objT["result"].ToString());
                        var eName = String.Empty;
                        foreach (var jar in jars)
                        {
                            eName = jar.Value<string>("cellText") == null ? string.Empty : jar.Value<string>("cellText").Trim();
                            if (jar.Value<string>("cellText") != null && jar.Value<string>("cellText") != String.Empty)
                            {
                                if (!queue.Contains(eName))
                                {
                                    queue.Enqueue(eName);
                                }
                            }
                        }

                }
            }
            catch (Exception)
            {
            }
        }
        #endregion
        #region 开始一个延时任务
        /// <summary>
        /// 开始一个延时任务
        /// </summary>
        /// <param name="DelayTime">延时时长（秒）</param>
        /// <param name="taskEndAction">延时时间完毕之后执行的委托（会跳转回UI线程）</param>
        /// <param name="control">UI线程的控件</param>
        public void StartDelayTask(int DelayTime, Action taskEndAction, Control control)
        {
            if (control == null)
            {
                return;
            }
            System.Threading.Tasks.Task task = new System.Threading.Tasks.Task(() =>
            {
                try
                {
                    Thread.Sleep(DelayTime * 1000);
                    //返回UI线程
                    control.Invoke(new Action(() =>
                    {
                        taskEndAction();
                    }));
                }
                catch
                {
                }
            });
            task.Start();
        }
        #endregion
        #region 设置滚动的文本
        public void SetScrolling()
        {
            if (queue.Count <= 0)
            {
                scrollingText1.ScrollText = "请耐心等候叫号,下一位患者请到诊室前等候";
                this.timer3.Interval = Convert.ToInt32(AppContext.AppConfig.RefreshTime) * 1000;
            }
            else
            {
                scrollingText1.ScrollText = "";
                //scrollingText1.staticTextPos = Screen.PrimaryScreen.Bounds.Width;
                scrollingText1.staticTextPos = this.Width;
                scrollingText1.ScrollText = queue.Dequeue();
                this.timer3.Interval = Convert.ToInt32(AppContext.AppConfig.VoicePlayTime) * 1000;
            }
        }
        #endregion 
        #region 医生坐诊诊间列表
        private List<ScreenClass> clinicInfo;
        private List<ScreenClass> listScreenHelper = new List<ScreenClass>();
        private int frequency = 0;
        private bool IsMultiplePages = false;
        private int SumCount = 0;
        private int listCount = 0;
        public string overPatients = string.Empty;
        public static bool IsFirstGui { get; set; }
        /// <summary>
        /// 医生坐诊诊间列表
        /// </summary>
        public void DoctorSittingConsultations()
        {
            try
            {
                frequency++;
                SumCount++;
                listCount = 0;
                clinicInfo = new List<ScreenClass>();
                var prament = new Dictionary<string, string>();
                prament.Add("hospitalId", HelperClass.hospitalId);
                prament.Add("deptIds", HelperClass.deptId);
                Xr.RtScreen.Models.RestSharpHelper.ReturnResult<List<string>>(InterfaceAddress.findPublicScreenData, prament, Method.POST, result =>
                {
                    switch (result.ResponseStatus)
                    {
                        case ResponseStatus.Completed:
                            if (result.StatusCode == HttpStatusCode.OK)
                            {
                                Log4net.LogHelper.Info("请求结果：" + string.Join(",", result.Data.ToArray()));
                                var objT = JObject.Parse(string.Join(",", result.Data.ToArray()));
                                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                                {
                                    clinicInfo = objT["result"]["list"].ToObject<List<ScreenClass>>();
                                    listCount = clinicInfo.Count;
                                    if (clinicInfo.Count > Convert.ToInt32(AppContext.AppConfig.PageSize))
                                    {
                                        if (frequency > Convert.ToInt32(AppContext.AppConfig.RollingTime) && frequency <= Convert.ToInt32(AppContext.AppConfig.RollingTime) * 2)
                                        {
                                            if (IsMultiplePages)
                                            {
                                                clinicInfo = new PagingUtil<ScreenClass>(clinicInfo, Convert.ToInt32(AppContext.AppConfig.PageSize), 3);
                                                Log4net.LogHelper.Error("第三页的总数：" + clinicInfo.Count + "" + "时间：" + frequency);
                                                if (frequency == Convert.ToInt32(AppContext.AppConfig.RollingTime) * 2)
                                                {
                                                    frequency = 0;
                                                    IsMultiplePages = false;
                                                }
                                            }
                                            else
                                            {
                                                clinicInfo = new PagingUtil<ScreenClass>(clinicInfo, Convert.ToInt32(AppContext.AppConfig.PageSize), 2);
                                                Log4net.LogHelper.Error("第二页的总数：" + clinicInfo.Count + "" + "时间：" + frequency);
                                                if (frequency == Convert.ToInt32(AppContext.AppConfig.RollingTime) * 2)
                                                {
                                                    if (listCount > (Convert.ToInt32(AppContext.AppConfig.PageSize) * 2))
                                                    {
                                                        IsMultiplePages = true;
                                                        frequency = Convert.ToInt32(AppContext.AppConfig.RollingTime);
                                                    }
                                                    else
                                                    {
                                                        IsMultiplePages = false;
                                                        frequency = 0;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            clinicInfo = new PagingUtil<ScreenClass>(clinicInfo, Convert.ToInt32(AppContext.AppConfig.PageSize), 1);
                                            Log4net.LogHelper.Error("第一页的总数：" + clinicInfo.Count + "" + "时间：" + frequency);
                                            if (frequency > Convert.ToInt32(AppContext.AppConfig.RollingTime) * 2)
                                            {
                                                frequency = 0;
                                            }
                                        }
                                    }
                                    if (IsFirstGui)
                                    {
                                        _context.Send((s) => DynamicLayout(tableLayoutPanel1, (Convert.ToInt32(AppContext.AppConfig.PageSize) + 1), 6), null);
                                        _context.Send((s) => tableLayoutPanel1.ColumnStyles[0].Width = 60, null);
                                        _context.Send((s) => tableLayoutPanel1.ColumnStyles[1].Width = 70, null);
                                        _context.Send((s) => tableLayoutPanel1.ColumnStyles[2].Width = 70, null);
                                        _context.Send((s) => tableLayoutPanel1.ColumnStyles[3].Width = 70, null);
                                        _context.Send((s) => tableLayoutPanel1.ColumnStyles[4].Width = 40, null);
                                        _context.Send((s) => tableLayoutPanel1.ColumnStyles[5].Width = 40, null);
                                        _context.Send((s) => IsFirstGui = false, null);
                                    }
                                    if (!TwoListIsCotinet(clinicInfo, listScreenHelper))
                                    {
                                        if (listScreenHelper.Count != clinicInfo.Count)
                                        {
                                            _context.Send((s) => Clear(tableLayoutPanel1), null);
                                        }
                                        listScreenHelper = clinicInfo;
                                        _context.Send((s) => SetContronlText(), null);
                                    }
                                    if (overPatients != objT["result"]["overPatients"].ToString())
                                    {
                                        overPatients = objT["result"]["overPatients"].ToString();
                                        _context.Send((s) => scrollingText2.ScrollText = objT["result"]["overPatients"].ToString(), null);
                                    }
                                }
                                else
                                {
                                    Log4net.LogHelper.Error("科室大屏返回错误信息：" + objT["message"].ToString());
                                }
                            }
                            break;
                    }
                });
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("科室大屏查询错误信息：" + ex.Message);
            }
        }
        #endregion
        #region 清空数据
        protected void Clear(Control ctrl)
        {
            foreach (Control c in ctrl.Controls)
            {
                if (c is Label)
                {
                    if (c.Name != "label00" && c.Name != "label01" && c.Name != "label02" && c.Name != "label03" && c.Name != "label04" && c.Name != "label05" && c.Name != "label06")
                    {
                        ((Label)(c)).Text = string.Empty;
                    }
                }
                if (c is ScrollingText)
                {
                    ((ScrollingText)(c)).ScrollText = string.Empty;
                }
            }
        }
        #endregion
        #region 如果数据没有更新就不刷新界面
        /// <summary>
        /// 如果数据没有更新就不刷新界面
        /// </summary>
        /// <param name="list"></param>
        /// <param name="lists"></param>
        /// <returns></returns>
        public bool TwoListIsCotinet(List<ScreenClass> list, List<ScreenClass> lists)
        {
            var TrueOrFalse = false;
            if (lists.Count > 0)
            {
                for (var i = 0; i < list.Count; i++)
                {
                    if (list[i].nextPatient == lists[i].nextPatient && list[i].waitPatient == lists[i].waitPatient && list[i].isStop == lists[i].isStop && list[i].visitPatient == lists[i].visitPatient && list[i].waitNum == lists[i].waitNum && list[i].name == lists[i].name)
                    {
                        TrueOrFalse = true;
                    }
                    else
                    {
                        return TrueOrFalse = false;
                    }
                }
            }
            return TrueOrFalse;
        }
        #endregion
        #region 异步赋值
        public void SetScrollingSollText(string value)
        {
            scrollingText2.Invoke(new EventHandler(delegate
            {
                scrollingText2.ScrollText = value;
            }));
        }
        /// <summary>
        /// 异步给控件赋值
        /// </summary>
        public void SetContronlText()
        {
            try
            {
                var t1 = new Thread(new ThreadStart(Assignment));
                t1.IsBackground = true;
                t1.Start();
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("异步给控件赋值错误信息：" + ex.Message);
            }
        }
        /// <summary>
        /// 给控件赋值
        /// </summary>
        public void Assignment()
        {
            try
            {
                foreach (Control c in tableLayoutPanel1.Controls)
                {
                    if (c is Label)
                    {
                        for (var g = 0; g < clinicInfo.Count; g++)
                        {
                            if (c.Name != "label00" && c.Name != "label01" && c.Name != "label02" && c.Name != "label03" && c.Name != "label04" && c.Name != "label05" && c.Name != "label06")
                            {
                                if (c.Name == "label" + (g + 1) + 0)
                                {
                                    if (clinicInfo[g].isStop == "1")
                                    {
                                        c.Text = clinicInfo[g].name + "(暂停)";
                                        c.ForeColor = Color.Red;
                                    }
                                    else
                                    {
                                        c.Text = clinicInfo[g].name + "(正常)";
                                        c.ForeColor = Color.Yellow;
                                    }
                                    g = g + 1;
                                    break;
                                }
                                if (c.Name == "label" + (g + 1) + 1)
                                {
                                    c.Text = clinicInfo[g].visitPatient;
                                    g = g + 1;
                                    break;
                                }
                                if (c.Name == "label" + (g + 1) + 2)
                                {
                                    c.Text = clinicInfo[g].nextPatient;
                                    g = g + 1;
                                    break;
                                }
                                if (c.Name == "label" + (g + 1) + 4)
                                {
                                    c.Text = clinicInfo[g].bespeakNum;
                                    g = g + 1;
                                    break;
                                }
                                if (c.Name == "label" + (g + 1) + 5)
                                {
                                    c.Text = clinicInfo[g].waitNum;
                                    g = g + 1;
                                    break;
                                }
                            }
                        }
                    }
                    if (c is ScrollingText)
                    {
                        for (var g = 0; g < clinicInfo.Count; g++)
                        {
                            if (c.Name == "st" + (g + 1) + 3)
                            {
                                ((ScrollingText)(c)).ScrollText = clinicInfo[g].waitPatient;
                                g = g + 1;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("给控件赋值时的错误信息：" + ex.Message);
            }
        }
        /// <summary>
        /// 给控件赋值---测试
        /// </summary>
        public void Assignments()
        {
            try
            {
                foreach (Control c in tableLayoutPanel1.Controls)
                {
                    if (c is Label)
                    {
                        for (var g = 0; g < 13; g++)
                        {
                            if (c.Name != "label00" && c.Name != "label01" && c.Name != "label02" && c.Name != "label03" && c.Name != "label04" && c.Name != "label05" && c.Name != "label06")
                            {
                                if (c.Name == "label" + (g + 1) + 0)
                                {
                                    c.Text = "测试";
                                    c.ForeColor = Color.Yellow;
                                    g = g + 1;
                                    break;
                                }
                                if (c.Name == "label" + (g + 1) + 1)
                                {
                                    c.Text = "测试";
                                    g = g + 1;
                                    break;
                                }
                                if (c.Name == "label" + (g + 1) + 2)
                                {
                                    c.Text = "测试";
                                    g = g + 1;
                                    break;
                                }
                                if (c.Name == "label" + (g + 1) + 4)
                                {
                                    c.Text = "测试";
                                    g = g + 1;
                                    break;
                                }
                                if (c.Name == "label" + (g + 1) + 5)
                                {
                                    c.Text = "测试";
                                    g = g + 1;
                                    break;
                                }
                            }
                        }
                    }
                    if (c is ScrollingText)
                    {
                        for (var g = 0; g < 13; g++)
                        {
                            if (c.Name == "st" + (g + 1) + 3)
                            {
                                SetProperty(c, "测试");
                                g = g + 1;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("给控件赋值时的错误信息：" + ex.Message);
            }
        }
        /// <summary>
        /// 指定对象指定属性名的属性赋值
        /// </summary>
        /// <param name="control">所属控件</param>
        /// <param name="Value">设置的值</param>
        public void SetProperty(Control control, object Value)
        {
            try
            {
                var type = control.GetType();
                var proinfo = type.GetProperty("ScrollText");
                if (proinfo != null)
                {
                    proinfo.SetValue(control, Value, null);
                }
                else
                {
                    proinfo.SetValue(control, string.Empty, null);
                }
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("给滚动控件赋值时错误信息" + ex.Message);
            }
        }
        #endregion
        #region 刷新候诊说明
        public string waitingDesc = string.Empty;
        private void timer2_Tick(object sender, EventArgs e)
        {
            var t3 = new Thread(new ThreadStart(DepartmentWaiting));
            t3.IsBackground = true;
            t3.Start();
        }
        /// <summary>
        /// 刷新候诊说明
        /// </summary>
        public void RefulwaitingDesc()
        {
            if (!timer2.Enabled)
            {
                timer2.Interval = Convert.ToInt32(AppContext.AppConfig.RefreshTimeWaitingDesc) * 60 * 1000;
                timer2.Start();
            }
            else
            {
                timer2.Stop();
            }
        }
        /// <summary>
        /// 科室候诊说明
        /// </summary>
        public void DepartmentWaiting()
        {
            try
            {
                var prament = new Dictionary<string, string>();
                if (HelperClass.deptId == string.Empty || HelperClass.deptId == null)
                {
                    Xr.Common.MessageBoxUtils.Show("请检查配置的科室是否正确！", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, null);
                    System.Environment.Exit(0);
                }
                prament.Add("deptIds", HelperClass.deptId);
                Xr.RtScreen.Models.RestSharpHelper.ReturnResult<List<string>>(InterfaceAddress.findWaitingDesc, prament, Method.POST, result =>
                {
                    switch (result.ResponseStatus)
                    {
                        case ResponseStatus.Completed:
                            if (result.StatusCode == HttpStatusCode.OK)
                            {
                                Log4net.LogHelper.Info("请求结果：" + string.Join(",", result.Data.ToArray()));
                                var objT = JObject.Parse(string.Join(",", result.Data.ToArray()));
                                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                                {
                                    if (waitingDesc != objT["result"]["waitingDesc"].ToString())
                                    {
                                        waitingDesc = objT["result"]["waitingDesc"].ToString();
                                        if (waitingDesc.Length > 210)
                                        {
                                            waitingDesc = waitingDesc.Substring(0, 210);
                                        }
                                        _context.Send((s) => scrollingTexts1.ScrollText = waitingDesc, null);
                                    }
                                }
                                else
                                {
                                    _context.Send((s) => Xr.Common.MessageBoxUtils.Hint(objT["message"].ToString(), Form1.pCurrentWin), null);
                                    _context.Send((s) => System.Environment.Exit(0), null);
                                }
                            }
                            break;
                    }
                });
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("大屏获取科室候诊说明错误信息：" + ex.Message);
            }
        }
        public void SetVerticalScrolling(string value)
        {
            scrollingTexts1.Invoke(new EventHandler(delegate
            {
                scrollingTexts1.ScrollText = value;
            }));
        }
        #endregion 
        #region 
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0014)
            {
                return;
            }
            base.WndProc(ref m);
        }
        #endregion
        #region 动态布局
        /// <summary>
        /// 动态布局
        /// </summary>
        /// <param name="layoutPanel"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        private void DynamicLayout(TableLayoutPanel layoutPanel, int row, int col)
        {
            try
            {
                layoutPanel.Controls.Clear();
                layoutPanel.RowStyles.Clear();
                layoutPanel.ColumnStyles.Clear();
                layoutPanel.RowCount = row;
                panelControl2.Height = row * Convert.ToInt32(AppContext.AppConfig.Interval);
                panelControl2.Dock = DockStyle.Top;
                panelControl3.Dock = DockStyle.Fill;
                for (var i = 0; i < row; i++)
                {
                    layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                }
                layoutPanel.ColumnCount = col;
                for (var i = 0; i < col; i++)
                {
                    layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
                }
                for (var i = 0; i < row; i++)
                {
                    for (var j = 0; j < col; j++)
                    {
                        var label = new Label();
                        label.Dock = DockStyle.Fill;
                        label.TextAlign = ContentAlignment.MiddleCenter;
                        label.Margin = new Padding(1, 1, 1, 1);
                        label.Font = new Font("微软雅黑", Convert.ToInt32(AppContext.AppConfig.FontSize), FontStyle.Regular);
                        label.BackColor = Color.Transparent;
                        label.ForeColor = Color.Yellow;
                        label.Name = "label" + i + j;
                        var st = new ScrollingText();
                        st.Dock = DockStyle.Fill;
                        st.ScrollText = string.Empty;
                        st.Margin = new Padding(1, 1, 1, 1);
                        st.Font = new Font("微软雅黑", Convert.ToInt32(AppContext.AppConfig.FontSize), FontStyle.Regular);
                        st.ForeColor = Color.Yellow;
                        st.Name = "st" + i + j;
                        st.TextScrollSpeed = 10;
                        st.TextScrollDistance = 3;
                        switch (label.Name)
                        {
                            case "label00":
                                label.Text = "诊室";
                                break;
                            case "label01":
                                label.Text = "在诊患者";
                                break;
                            case "label02":
                                label.Text = "下一位";
                                break;
                            case "label03":
                                label.Text = "候诊患者";
                                break;
                            case "label04":
                                label.Text = "已约数";
                                break;
                            case "label05":
                                label.Text = "候诊数";
                                break;
                        }
                        if (j == 3 && label.Name != "label03")
                        {
                            layoutPanel.Controls.Add(st);
                            layoutPanel.SetRow(st, i);
                            layoutPanel.SetColumn(st, j);
                        }
                        else
                        {
                            layoutPanel.Controls.Add(label);
                            layoutPanel.SetRow(label, i);
                            layoutPanel.SetColumn(label, j);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("绘制控件时出现错误：" + ex.Message);
            }
        }
        #endregion
        #region 画线条
        private void panelControl2_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                ControlPaint.DrawBorder(e.Graphics, panelControl2.ClientRectangle, Color.White,
                             1, ButtonBorderStyle.Solid, Color.White,
                             1, ButtonBorderStyle.Solid, Color.White,
                             1, ButtonBorderStyle.Solid, Color.White,
                             1, ButtonBorderStyle.Solid);
            }
            catch
            {
            }
        }
        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                ControlPaint.DrawBorder(e.Graphics, panelControl1.ClientRectangle, Color.White,
                            1, ButtonBorderStyle.Solid, Color.White,
                            1, ButtonBorderStyle.Solid, Color.White,
                            1, ButtonBorderStyle.Solid, Color.Transparent,
                            1, ButtonBorderStyle.Solid);
            }
            catch
            {
            }
        }
        private void panelControl3_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                ControlPaint.DrawBorder(e.Graphics, panelControl3.ClientRectangle, Color.White,
                            1, ButtonBorderStyle.Solid, Color.White,
                            1, ButtonBorderStyle.Solid, Color.White,
                            1, ButtonBorderStyle.Solid, Color.Transparent,
                            1, ButtonBorderStyle.Solid);
            }
            catch
            {
            }
        }
        private void scrollingText1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                ControlPaint.DrawBorder(e.Graphics, panelControl1.ClientRectangle, Color.White,
                           1, ButtonBorderStyle.Solid, Color.White,
                           1, ButtonBorderStyle.Solid, Color.White,
                           1, ButtonBorderStyle.Solid, Color.Transparent,
                           1, ButtonBorderStyle.Solid);
            }
            catch
            {
            }
        }
        private void scrollingTexts1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                ControlPaint.DrawBorder(e.Graphics, scrollingTexts1.ClientRectangle, Color.Transparent,
                          1, ButtonBorderStyle.Solid, Color.Transparent,
                          1, ButtonBorderStyle.Solid, Color.Transparent,
                          1, ButtonBorderStyle.Solid, Color.Transparent,
                          1, ButtonBorderStyle.Solid);
            }
            catch
            {
            }
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                ControlPaint.DrawBorder(e.Graphics, panel1.ClientRectangle, Color.White,
                         1, ButtonBorderStyle.Solid, Color.Transparent,
                         1, ButtonBorderStyle.Solid, Color.White,
                         1, ButtonBorderStyle.Solid, Color.White,
                         1, ButtonBorderStyle.Solid);
            }
            catch
            {
            }
        }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                var pp = new Pen(Color.White);
                e.Graphics.DrawRectangle(pp, e.ClipRectangle.X - 1, e.ClipRectangle.Y - 1, e.ClipRectangle.X + e.ClipRectangle.Width - 0, e.ClipRectangle.Y + e.ClipRectangle.Height - 0);
            }
            catch
            {
            }
        }
        private void tableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            try
            {
                var pp = new Pen(Color.White);
                e.Graphics.DrawRectangle(pp, e.CellBounds.X, e.CellBounds.Y, e.CellBounds.X + Width - 1, e.CellBounds.Y + Height - 1);
            }
            catch
            {
            }
        }

        private void tableLayoutPanel2_CellPaint_1(object sender, TableLayoutCellPaintEventArgs e)
        {
            var pp = new Pen(Color.White);
            e.Graphics.DrawRectangle(pp, e.CellBounds.X, e.CellBounds.Y, e.CellBounds.X + Width - 1, e.CellBounds.Y + Height - 1);
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {
            var pp = new Pen(Color.White);
            e.Graphics.DrawRectangle(pp, e.ClipRectangle.X - 1, e.ClipRectangle.Y - 1, e.ClipRectangle.X + e.ClipRectangle.Width - 0, e.ClipRectangle.Y + e.ClipRectangle.Height - 0);
        }
        #endregion
        #region 时间控件
        public void time()
        {
            if (!timer1.Enabled)
            {
                timer1.Interval = Convert.ToInt32(AppContext.AppConfig.RefreshTime) * 1000;
                timer1.Start();
            }
            else
            {
                timer1.Stop();
            }
        }
        private int VoiceNum = 0;
        //private static Thread t3;
        private void timer1_Tick(object sender, EventArgs e)
        {
            //VoiceNum++;
            var t1 = new Thread(new ThreadStart(DoctorSittingConsultations));
            t1.IsBackground = true;
            t1.Start();
            var t2 = new Thread(new ThreadStart(findCallListByScreen));
            t2.IsBackground = true;
            t2.Start();
        }
        private Point downPoint;
        private void scrollingText1_MouseDown(object sender, MouseEventArgs e)
        {
            downPoint = new Point(e.X, e.Y);
        }

        private void scrollingText1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Form1.pCurrentWin.Location = new Point(Form1.pCurrentWin.Location.X + e.X - downPoint.X,
                     Form1.pCurrentWin.Location.Y + e.Y - downPoint.Y);
            }
        }
        #endregion
        #region 获取滚动的时间
        public void times()
        {
            if (!timer3.Enabled)
            {
                timer3.Interval = Convert.ToInt32(AppContext.AppConfig.RefreshTime) * 1000;
                timer3.Start();
            }
            else
            {
                timer3.Stop();
            }
        }
        private void timer3_Tick(object sender, EventArgs e)
        {
            SetScrolling();
        }
        #endregion
    }
}
