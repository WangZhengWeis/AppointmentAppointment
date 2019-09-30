using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using RestSharp;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Threading;
using Xr.RtScreen.Models;

///Auto:wzw
///Time:2019-01-07
namespace Xr.RtScreen.pages
{
    public partial class RtDoctorSmallScreenFrm : UserControl
    {
        public SynchronizationContext _context;
        public RtDoctorSmallScreenFrm()
        {
            InitializeComponent();
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
            Control.CheckForIllegalCrossThreadCalls = false;
            _context = new SynchronizationContext();
            pictureBox1.ImageLocation = "man.png";
            GetDoctorSmallScreenInfo();
            time();
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0014)
            {
                return;
            }
            base.WndProc(ref m);
        }
        #region 获取医生看诊信息
        private string doctorIntro = string.Empty;
        public void GetDoctorSmallScreenInfo()
        {
            try
            {
                var prament = new Dictionary<string, string>();
                prament.Add("hospitalId", HelperClass.hospitalId);
                prament.Add("deptId", HelperClass.deptId);
                prament.Add("clinicId", HelperClass.clincId);
                Xr.RtScreen.Models.RestSharpHelper.ReturnResult<List<string>>(InterfaceAddress.findRoomScreenDataTwo, prament, Method.POST, result =>
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
                                    var smallscreen = Newtonsoft.Json.JsonConvert.DeserializeObject<DoctorSmallScreenClass>(objT["result"].ToString());
                                    _context.Send((s) => label1.Text = smallscreen.clinicName, null);
                                    _context.Send((s) => label2.Text = smallscreen.doctorName, null);
                                    _context.Send((s) => label3.Text = smallscreen.doctorExcellence + smallscreen.doctorJob, null);
                                    _context.Send((s) => label6.Text = smallscreen.visitPatient, null);
                                    if (doctorIntro != smallscreen.doctorIntro)
                                    {
                                        doctorIntro = smallscreen.doctorIntro;
                                        _context.Send((s) => GetDoctorInfo(smallscreen.doctorIntro), null);
                                    }
                                    _context.Send((s) =>  scrollingText1.ScrollText = smallscreen.waitPatient, null);
                                    _context.Send((s) => label8.Text = smallscreen.nextPatient, null);
                                    _context.Send((s) => GetImage(smallscreen.doctorHeader), null);
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
        public void GetImage(dynamic value)
        {
            try
            {
                pictureBox1.Image = Image.FromStream(System.Net.WebRequest.Create(value).GetResponse().GetResponseStream());
            }
            catch
            {
            }
        }
        public void GetWasitPatrent(string wasitPatient)
        {
            try
            {
                scrollingText1.ScrollText = wasitPatient;
            }
            catch
            {
            }
        }
        public void GetDoctorInfo(string docotrinfo)
        {
            try
            {
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    scrollingTexts1.ScrollText = StripHTML(docotrinfo);
                });
            }
            catch
            {
            }
        }
        #endregion
        #region 去除HTML标记
        /// <summary>
        /// 去除HTML标记 
        /// </summary>
        /// <param name="strHtml">包括HTML的源码 </param>
        /// <returns>已经去除后的文字</returns>
        public static string StripHTML(string strHtml)
        {
            var aryReg = new string[] { @"<script[^>]*?>.*?</script>", @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>", @"([\r\n])[\s]+", @"&(quot|#34);", @"&(amp|#38);", @"&(lt|#60);", @"&(gt|#62);", @"&(nbsp|#160);", @"&(iexcl|#161);", @"&(cent|#162);", @"&(pound|#163);", @"&(copy|#169);", @"&#(\d+);", @"-->", @"<!--.*\n" };
            var aryRep = new string[] { string.Empty, string.Empty, string.Empty, "\"", "&", "<", ">", " ", "\xa1", "\xa2", "\xa3", "\xa9", string.Empty, "\r\n", string.Empty };
            var strOutput = strHtml;
            for (var i = 0; i < aryReg.Length; i++)
            {
                var regex = new System.Text.RegularExpressions.Regex(aryReg[i], System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                strOutput = regex.Replace(strOutput, aryRep[i]);
            }
            strOutput.Replace("<", string.Empty);
            strOutput.Replace(">", string.Empty);
            strOutput.Replace("\r\n", string.Empty);
            return strOutput;
        }
        #endregion
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void tableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
        }
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
            GetDoctorSmallScreenInfo();
        }
        #endregion
        #region 更随鼠标移动
        private Point downPoint;
        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            downPoint = new Point(e.X, e.Y);
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Form1.pCurrentWin.Location = new Point(Form1.pCurrentWin.Location.X + e.X - downPoint.X,
                    Form1.pCurrentWin.Location.Y + e.Y - downPoint.Y);
            }
        }
        #endregion
        #region 设置字体
        public void SetFont()
        {
            try
            {
                label2.Font = new Font("微软雅黑", Convert.ToInt32(AppContext.AppConfig.FontSize), FontStyle.Bold);
                label3.Font = new Font("微软雅黑", (Convert.ToInt32(AppContext.AppConfig.FontSize) - 9), FontStyle.Bold);
                scrollingTexts1.Font = new Font("微软雅黑", (Convert.ToInt32(AppContext.AppConfig.FontSize) - 9), FontStyle.Bold);
                label4.Font = new Font("微软雅黑", Convert.ToInt32(AppContext.AppConfig.FontSize), FontStyle.Bold);
                label6.Font = new Font("微软雅黑", Convert.ToInt32(AppContext.AppConfig.FontSize), FontStyle.Bold);
                label7.Font = new Font("微软雅黑", Convert.ToInt32(AppContext.AppConfig.FontSize), FontStyle.Bold);
                label8.Font = new Font("微软雅黑", Convert.ToInt32(AppContext.AppConfig.FontSize), FontStyle.Bold);
                label9.Font = new Font("微软雅黑", Convert.ToInt32(AppContext.AppConfig.FontSize), FontStyle.Bold);
                scrollingText1.Font = new Font("微软雅黑", Convert.ToInt32(AppContext.AppConfig.FontSize), FontStyle.Bold);
            }
            catch
            {
            }
        }
        #endregion
    }
}
