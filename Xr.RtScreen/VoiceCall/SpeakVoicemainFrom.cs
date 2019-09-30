using Newtonsoft.Json.Linq;
using SpeechLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Xr.Http;
using Xr.RtScreen.Models;

namespace Xr.RtScreen.VoiceCall
{
    public partial class SpeakVoicemainFrom : Form
    {
        public delegate void setTextValue(string textValue);
        public event setTextValue setFormTextValue;
        private SpeechVoiceSpeakFlags _spFlags;
        private SpVoice _voice;
        public SpeakVoicemainFrom()
        {
            InitializeComponent();
            MaximizeBox = false;///禁用最大化
            ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            timer1.Interval = Int32.Parse(ConfigurationManager.AppSettings["CallNextSpan"]) * 1000;
            WindowState = FormWindowState.Minimized;
            Size = new Size(739, 150);
            time();
        }
        public static Func<List<CallPrint>> GetDataUpdate = new Func<List<CallPrint>>(delegate()
        {
            return GetData();
        });
        private int succeedCount = 0;
        private static int failedCount = 0;
        public void UpdateForeach(List<CallPrint> list)
        {
            FuZhi = 0;
            foreach (var callpatient in list)
            {
                try
                {
                    _voice.Speak(callpatient.CallVoiceString(), _spFlags);
                    _voice.Speak(callpatient.CallVoiceString(), _spFlags);
                }
                catch
                {
                }
                setFormTextValue(callpatient.LogString());
                LogPrint(callpatient.LogString());
                do
                {
                    FuZhi++;
                }
                while (FuZhi != 6000);
                succeedCount++;
            }
            lab_succeedCount.Text = "正常次数：" + succeedCount;
            lab_failedCount.Text = "失败次数：" + failedCount;
            lab_lasttime.Text = "最后时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        private static List<CallPrint> listPrint;
        private static List<CallPrint> GetData()
        {
            var cpList = new List<CallPrint>();
            listPrint = new List<CallPrint>();
            var Url = string.Empty;
            if (Form1.ScreenType == "1")
            {
                Url = AppContext.AppConfig.serverUrl + InterfaceAddress.findCallList + "?hospitalId=" + HelperClass.hospitalId + "&deptIds=" + HelperClass.deptId;
            }
            else
            {
                Url = AppContext.AppConfig.serverUrl + InterfaceAddress.findCallList + "?hospitalId=" + HelperClass.hospitalId + "&deptIds=" + HelperClass.deptId + "&clinicId=" + HelperClass.clincId;
            }
            var result = HttpClass.httpPost(Url);
            Log4net.LogHelper.Info("呼号请求地址：" + Url);
            try
            {
                var objT = Newtonsoft.Json.Linq.JObject.Parse(result);
                Log4net.LogHelper.Info("呼号请求返回结果：" + objT);
                if (objT["state"].ToString().ToLower() != "true")
                {
                    return new List<CallPrint>();
                }
                else
                {
                    var jars = JArray.Parse(objT["result"].ToString());
                    foreach (var jar in jars)
                    {
                        var eName = String.Empty;
                        eName = jar.Value<string>("cellText") == null ? string.Empty : jar.Value<string>("cellText").Trim();
                        if (jar.Value<string>("cellText") != null && jar.Value<string>("cellText") != String.Empty)
                        {
                            eName = string.Empty + eName;
                        }
                        var cp2 = new CallPrint(jar.Value<string>("cellText"));
                        cpList.Add(cp2);
                    }
                }
            }
            catch (Exception rx)
            {
                Log4net.LogHelper.Error("查询呼号错误信息：" + rx.Message);
                failedCount++;
            }
            listPrint = cpList;
            return cpList;
        }
        public void PlayVoice()
        {
            try
            {
                GetDataUpdate.BeginInvoke(ar =>
                {
                    var objT = GetDataUpdate.EndInvoke(ar);

                    Invoke(new Action(() => UpdateForeach(objT)));
                }, null);
            }
            catch
            {
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "开始播放（暂停中）")
            {
                button1.Text = "暂停播放（播放中）";
                lab_startime.Text = "开始时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                PlayVoice();
                timer1.Start();
                _voice.Resume();
            }
            else
            {
                button1.Text = "开始播放（暂停中）";
                timer1.Stop();
                _voice.Pause();
            }
        }
        private void SpeakVoicemainFrom_Load(object sender, EventArgs e)
        {
            _voice = new SpVoice();
            _spFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
            _voice.Rate = Int16.Parse(ConfigurationManager.AppSettings["VoiceRate"]);
            _voice.Volume = 100;
            try
            {
                _voice.Voice = _voice.GetVoices(string.Empty, string.Empty).Item(Int16.Parse(ConfigurationManager.AppSettings["VoicePackage"]));
            }
            catch
            {
                _voice.Voice = _voice.GetVoices(string.Empty, string.Empty).Item(0);
            }
            finally
            {
                button1.PerformClick();
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            PlayVoice();
        }
        private int FuZhi = 0;
        private delegate void LogPrintDelegate(string log);
        public void LogPrint(string log)
        {
            FuZhi = 0;
            if (txt_log.InvokeRequired)
            {
                txt_log.Invoke(new LogPrintDelegate(LogPrint), log);
            }
            else
            {
                var newLine = String.Format("{0:yyyy-MM-dd HH:mm:ss}==>{1}\n{2}", DateTime.Now, log, Environment.NewLine);
                txt_log.Text = txt_log.Text.Insert(0, newLine);
            }
        }
        private void SpeakVoicemainFrom_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
        public void time()
        {
            if (!timer2.Enabled)
            {
                timer2.Interval = 1 * 60 * 1000;
                timer2.Start();
            }
            else
            {
                timer2.Stop();
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (listPrint.Count == 0)
            {
                setFormTextValue("请耐心等候叫号");
            }
        }
    }
}
