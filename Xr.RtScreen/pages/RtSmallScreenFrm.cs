using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Xr.RtScreen.VoiceCall;
using RestSharp;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Threading;
using Xr.RtScreen.Models;

namespace Xr.RtScreen.pages
{
    public partial class RtSmallScreenFrm : UserControl
    {
        public SynchronizationContext _context;
        public RtSmallScreenFrm()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
            _context = new SynchronizationContext();
            Control.CheckForIllegalCrossThreadCalls = false;
            GetSmallScreenInfo();
            if (Convert.ToBoolean(AppContext.AppConfig.IsOpenVioce))
            {
                var speakVoiceform = new SpeakVoicemainFrom();
                speakVoiceform.setFormTextValue += new Xr.RtScreen.VoiceCall.SpeakVoicemainFrom.setTextValue(form2_setFormTextValue);
                speakVoiceform.Show(this);
            }
            time();
        }
        private void form2_setFormTextValue(string textValue)
        {
            scrollingText1.ScrollText = textValue;
        }
        #region 画线条
        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, tableLayoutPanel2.ClientRectangle, Color.White,
                         1, ButtonBorderStyle.Solid, Color.Transparent,
                         1, ButtonBorderStyle.Solid, Color.White,
                         1, ButtonBorderStyle.Solid, Color.White,
                         1, ButtonBorderStyle.Solid);
        }
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
        #region 获取医生坐诊信息
        private string houzhenshuoming = string.Empty;
        public void GetSmallScreenInfo()
        {
            try
            {
                var prament = new Dictionary<string, string>();
                prament.Add("hospitalId", HelperClass.hospitalId);
                prament.Add("deptId", HelperClass.deptId);
                prament.Add("clinicId", HelperClass.clincId);
                Xr.RtScreen.Models.RestSharpHelper.ReturnResult<List<string>>(InterfaceAddress.findRoomScreenDataOne, prament, Method.POST, result =>
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
                                    var smallscreen = Newtonsoft.Json.JsonConvert.DeserializeObject<SmallScreenClass>(objT["result"].ToString());
                                    _context.Send((s) => label7.Text = smallscreen.clinicName + "\n" + smallscreen.doctorName, null);
                                    _context.Send((s) => label2.Text = smallscreen.visitPatient, null);
                                    _context.Send((s) => label8.Text = smallscreen.nextPatient, null);
                                    _context.Send((s) => scrollingText2.ScrollText = smallscreen.waitPatient, null);
                                    _context.Send((s) => label5.Text = smallscreen.waitNum, null);
                                    if (houzhenshuoming != smallscreen.waitingDesc)
                                    {
                                        houzhenshuoming = smallscreen.waitingDesc;
                                        _context.Send((s) => scrollingTexts1.ScrollText = smallscreen.waitingDesc, null);
                                    }
                                }
                                else
                                {
                                    _context.Send((s) => Xr.Common.MessageBoxUtils.Hint(objT["message"].ToString(), Form1.pCurrentWin), null);
                                }
                            }
                            break;
                    }
                });
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("获取诊室小屏错误信息：" + ex.Message);
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
            GetSmallScreenInfo();
        }
        #endregion 
        #region 设置字体
        public void SetFont()
        {
            try
            {
                label7.Font = new Font("微软雅黑", Convert.ToInt32(AppContext.AppConfig.FontSize), FontStyle.Bold);
                scrollingText1.Font = new Font("微软雅黑", Convert.ToInt32(AppContext.AppConfig.FontSize), FontStyle.Bold);
                label1.Font = new Font("微软雅黑", Convert.ToInt32(AppContext.AppConfig.FontSize), FontStyle.Bold);
                label6.Font = new Font("微软雅黑", Convert.ToInt32(AppContext.AppConfig.FontSize), FontStyle.Bold);
                label3.Font = new Font("微软雅黑", Convert.ToInt32(AppContext.AppConfig.FontSize), FontStyle.Bold);
                label4.Font = new Font("微软雅黑", Convert.ToInt32(AppContext.AppConfig.FontSize), FontStyle.Bold);
                label2.Font = new Font("微软雅黑", Convert.ToInt32(AppContext.AppConfig.FontSize), FontStyle.Bold);
                label8.Font = new Font("微软雅黑", Convert.ToInt32(AppContext.AppConfig.FontSize), FontStyle.Bold);
                scrollingText1.Font = new Font("微软雅黑", Convert.ToInt32(AppContext.AppConfig.FontSize), FontStyle.Bold);
                scrollingText2.Font = new Font("微软雅黑", Convert.ToInt32(AppContext.AppConfig.FontSize), FontStyle.Bold);
                label5.Font = new Font("微软雅黑", Convert.ToInt32(AppContext.AppConfig.FontSize), FontStyle.Bold);
            }
            catch
            {
            }
        }
        #endregion
    }
}
