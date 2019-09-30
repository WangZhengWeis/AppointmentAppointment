using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Xr.RtScreen.Models;
using RestSharp;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Threading;
using Xr.RtScreen.RtUserContronl;

namespace Xr.RtScreen.pages
{
    public partial class DepartmentDoctorDescription : UserControl
    {
        public SynchronizationContext _context;
        public DepartmentDoctorDescription()
        {
            InitializeComponent();
            _context = new SynchronizationContext();
            Control.CheckForIllegalCrossThreadCalls = false;
            tableLayoutPanel1.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanel1, true, null);
            IsFirstGuis = true;
            DoctorSittingConsultations();
            time();
        }
        private List<doctorScheduPlan> clinicInfo;
        private int a = -1;
        private int frequency = 0;
        private bool IsMultiplePages = false;
        private int SumCount = 0;
        private int listCount = 0;
        public string overPatients = string.Empty;
        private List<doctorScheduPlan> listScreenHelper = new List<doctorScheduPlan>();
        public static bool IsFirstGuis { get; set; }
        #region 医生坐诊诊间列表
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
                clinicInfo = new List<doctorScheduPlan>();
                var prament = new Dictionary<string, string>();
                prament.Add("hospitalId", HelperClass.hospitalId);
                prament.Add("deptIds", HelperClass.deptId);
                Xr.RtScreen.Models.RestSharpHelper.ReturnResult<List<string>>(InterfaceAddress.doctorScheduPlanList, prament, Method.POST, result =>
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
                                    clinicInfo = objT["result"].ToObject<List<doctorScheduPlan>>();
                                    listCount = clinicInfo.Count;
                                    if (clinicInfo.Count > (Convert.ToInt32(AppContext.AppConfig.PageSize) + 8))
                                    {
                                        if (frequency > Convert.ToInt32(AppContext.AppConfig.RollingTime) && frequency <= Convert.ToInt32(AppContext.AppConfig.RollingTime) * 2)
                                        {
                                            if (IsMultiplePages)
                                            {
                                                clinicInfo = new PagingUtil<doctorScheduPlan>(clinicInfo, (Convert.ToInt32(AppContext.AppConfig.PageSize) + 8), 3);
                                                if (frequency == Convert.ToInt32(AppContext.AppConfig.RollingTime) * 2)
                                                {
                                                    frequency = 0;
                                                    IsMultiplePages = false;
                                                }
                                            }
                                            else
                                            {
                                                clinicInfo = new PagingUtil<doctorScheduPlan>(clinicInfo, (Convert.ToInt32(AppContext.AppConfig.PageSize) + 8), 2);
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
                                            clinicInfo = new PagingUtil<doctorScheduPlan>(clinicInfo, (Convert.ToInt32(AppContext.AppConfig.PageSize) + 8), 1);
                                            if (frequency > Convert.ToInt32(AppContext.AppConfig.RollingTime) * 2)
                                            {
                                                frequency = 0;
                                            }
                                        }
                                    }
                                    if (IsFirstGuis)
                                    {
                                        _context.Send((s) => DynamicLayout(tableLayoutPanel1, 17, 6), null);
                                        _context.Send((s) => tableLayoutPanel1.ColumnStyles[0].Width = 70, null);
                                        _context.Send((s) => tableLayoutPanel1.ColumnStyles[1].Width = 30, null);
                                        _context.Send((s) => tableLayoutPanel1.ColumnStyles[2].Width = 50, null);
                                        _context.Send((s) => tableLayoutPanel1.ColumnStyles[3].Width = 20, null);
                                        _context.Send((s) => tableLayoutPanel1.ColumnStyles[4].Width = 20, null);
                                        _context.Send((s) => tableLayoutPanel1.ColumnStyles[5].Width = 70, null);
                                        IsFirstGuis = false;
                                    }
                                    a = clinicInfo.Count;
                                    if (!TwoListIsCotinet(clinicInfo, listScreenHelper))
                                    {
                                        if (clinicInfo.Count != listScreenHelper.Count)
                                        {
                                            _context.Send((s) => Clear(tableLayoutPanel1), null);
                                        }
                                        listScreenHelper = clinicInfo;
                                        _context.Send((s) => SetContronlText(), null);
                                    }
                                }
                                else
                                {
                                    Log4net.LogHelper.Error("科室医生大屏返回错误信息：" + objT["message"].ToString());
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
        public bool TwoListIsCotinet(List<doctorScheduPlan> list, List<doctorScheduPlan> lists)
        {
            var TrueOrFalse = false;
            if (lists.Count > 0)
            {
                for (var i = 0; i < list.Count; i++)
                {
                    if (list[i].deptName == lists[i].deptName && list[i].excellence == lists[i].excellence && list[i].clinicName == lists[i].clinicName && list[i].visitTime == lists[i].visitTime && list[i].doctorName == lists[i].doctorName && list[i].doctorJob == lists[i].doctorJob)
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
        #region 帮助类
        public class doctorScheduPlan
        {
            public string deptName { get; set; }
            public string  excellence { get; set; }
            public string clinicName { get; set; }
            public string  visitTime { get; set; }
            public string  doctorName { get; set; }
            public string  doctorJob { get; set; }
        }
        #endregion
        #region 异步给控件赋值
        /// <summary>
        /// 异步给控件赋值
        /// </summary>
        public void SetContronlText()
        {
            try
            {
                var t2 = new Thread(new ThreadStart(Assignment));
                t2.IsBackground = true;
                t2.Start();
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
                                    c.Text = clinicInfo[g].deptName;
                                    c.ForeColor = Color.Yellow;
                                    g = g + 1;
                                    break;
                                }
                                if (c.Name == "label" + (g + 1) + 1)
                                {
                                    c.Text = clinicInfo[g].doctorName;
                                    g = g + 1;
                                    break;
                                }
                                if (c.Name == "label" + (g + 1) + 2)
                                {
                                    c.Text = clinicInfo[g].doctorJob;
                                    g = g + 1;
                                    break;
                                }
                                if (c.Name == "label" + (g + 1) + 4)
                                {
                                    c.Text = clinicInfo[g].clinicName;
                                    g = g + 1;
                                    break;
                                }
                                if (c.Name == "label" + (g + 1) + 3)
                                {
                                    c.Text = clinicInfo[g].visitTime;
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
                            if (c.Name == "st" + (g + 1) + 5)
                            {
                                SetProperty(c, clinicInfo[g].excellence);
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
                if (InvokeRequired)
                {
                    Invoke(new MethodInvoker(delegate
                    {
                        DynamicLayout(layoutPanel, row, col);
                    }));
                    return;
                }
                layoutPanel.Controls.Clear();
                layoutPanel.RowStyles.Clear();
                layoutPanel.ColumnStyles.Clear();
                layoutPanel.RowCount = row;
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
                        st.TextScrollDistance = 1;
                        switch (label.Name)
                        {
                            case "label00":
                                label.Text = "科室";
                                label.ForeColor = Color.GreenYellow;
                                break;
                            case "label01":
                                label.Text = "医生姓名";
                                label.ForeColor = Color.GreenYellow;
                                break;
                            case "label02":
                                label.Text = "职称";
                                label.ForeColor = Color.GreenYellow;
                                break;
                            case "label03":
                                label.Text = "诊时";
                                label.ForeColor = Color.GreenYellow;
                                break;
                            case "label04":
                                label.Text = "诊室";
                                label.ForeColor = Color.GreenYellow;
                                break;
                            case "label05":
                                label.Text = "专长";
                                label.ForeColor = Color.GreenYellow;
                                break;
                        }
                        if (j == 5 && label.Name != "label05")
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
        private void timer1_Tick(object sender, EventArgs e)
        {
            var t1 = new Thread(new ThreadStart(DoctorSittingConsultations));
            t1.IsBackground = true;
            t1.Start();
            //AutoUp();
        }
        private static bool IsAutoUp { get; set; }
        /// <summary>
        /// 每天六点自动重启
        /// </summary>
        private void AutoUp()
        {
            var checkTime = "10";
            var time = DateTime.Now.Hour.ToString();
            if (checkTime == time)
            {
                if (!IsAutoUp)
                {
                    Application.Exit();
                    var Info = new System.Diagnostics.ProcessStartInfo();
                    Info.FileName = "Xr.RtScreen.exe";
                    Info.WorkingDirectory = @"Xr.RtScreen.exe";
                    Info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                    System.Diagnostics.Process Proc;
                    try
                    {
                        Proc = System.Diagnostics.Process.Start(Info);
                        System.Threading.Thread.Sleep(500);
                        Console.WriteLine();
                        IsAutoUp = true;
                    }
                    catch (System.ComponentModel.Win32Exception x)
                    {
                        MessageBox.Show(x.ToString());
                    }
                }
            }
        }
        #endregion
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0014)
            {
                return;
            }
            base.WndProc(ref m);
        }
        #region 画线条
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            var pp = new Pen(Color.White);
            e.Graphics.DrawRectangle(pp, e.ClipRectangle.X - 1, e.ClipRectangle.Y - 1, e.ClipRectangle.X + e.ClipRectangle.Width - 0, e.ClipRectangle.Y + e.ClipRectangle.Height - 0);
        }

        private void tableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            var pp = new Pen(Color.White);
            e.Graphics.DrawRectangle(pp, e.CellBounds.X, e.CellBounds.Y, e.CellBounds.X + Width - 1, e.CellBounds.Y + Height - 1);
        }
        #endregion
    }
}
