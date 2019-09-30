using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Xr.Http.RestSharp;
using RestSharp;
using System.Net;
using Newtonsoft.Json.Linq;
using Xr.Common;
using Xr.RtCall.Model;
using System.Runtime.InteropServices;

namespace Xr.RtCall.pages
{
    public partial class RtCallMessageFrm : UserControl
    {
        public SynchronizationContext _context;
        public RtCallMessageFrm(string name)
        {
            InitializeComponent();
            _context = SynchronizationContext.Current;
            label1.Text = "当前患者/"+name;
        }
        #region 完成下一位/过号下一位
        /// <summary>
        /// 呼号到诊/过号重排
        /// </summary>
        /// <param name="triageId">分诊记录主键，第一次可空</param>
        /// <param name="type">类型：0(呼号到诊)、1(过号重排)</param>
        public void PatientOkAndNext(string triageId, string type)
        {
            try
            {
                Dictionary<string, string> prament = new Dictionary<string, string>();
                prament.Add("triageId", triageId);
                string Url = "";
                if (type == "0")
                {
                    Url = InterfaceAddress.inPlace;//呼号到诊接口
                }
                else
                {
                    Url = InterfaceAddress.passNum;//过号重排接口
                }
                RestSharpHelper.ReturnResult<List<string>>(Url, prament, Method.POST,
                 result =>
                 {
                     switch (result.ResponseStatus)
                     {
                         case ResponseStatus.Completed:
                             if (result.StatusCode == System.Net.HttpStatusCode.OK)
                             {
                                 Log4net.LogHelper.Info("请求结果：" + string.Join(",", result.Data.ToArray()));
                                 JObject objT = JObject.Parse(string.Join(",", result.Data.ToArray()));
                                 if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                                 {
                                     if (type == "0")
                                     {
                                         if (Convert.ToBoolean(AppContext.AppConfig.WhetherToAssign))
                                         {
                                             string patientId=objT["result"][0]["patientId"].ToString();
                                             _context.Send((s) => Assignment(patientId), null);//把患者ID传给医生工作站并让医生工作站回车
                                         }
                                     }
                                     else
                                     {
                                         _context.Send((s) => Form1.pCurrentWin.label2.Text = "等待呼叫病人 [请稍候...]",null);
                                     }
                                     _context.Send((s) => CloseForm(), null);
                                     _context.Send((s) => ShuaXin(), null);
                                 }
                                 else
                                 {
                                     _context.Send((s) => MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1,this), null);
                                 }
                             }
                             break;
                     }
                 });
            }
            catch (Exception ex)
            {
               Log4net.LogHelper.Error("获取完成下一位/过号下一位错误信息：" + ex.Message);
            }
        }
        /// <summary>
        /// 是否刷新患者列表
        /// </summary>
        public void ShuaXin()
        {
            if (Form1.IsMax)
            {
                RtCallPeationFrm.RTCallfrm.PatientList();
            }
        }
        /// <summary>
        /// 关闭窗口
        /// </summary>
        public void CloseForm()
        {
            Form f1 = this.Parent as HostingForm;
            if (f1 != null)
                f1.Close();
            this.Dispose();
        }
        #endregion
        #region 把ID传给医生工作站
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        private readonly int MOUSEEVENTF_LEFTDOWN = 0x0002;//模拟鼠标移动
        private readonly int MOUSEEVENTF_MOVE = 0x0001;//模拟鼠标左键按下
        private readonly int MOUSEEVENTF_LEFTUP = 0x0004;//模拟鼠标左键抬起
        private readonly int MOUSEEVENTF_ABSOLUTE = 0x8000;//鼠标绝对位置
        //private readonly int MOUSEEVENTF_RIGHTDOWN = 0x0008; //模拟鼠标右键按下 
        //private readonly int MOUSEEVENTF_RIGHTUP = 0x0010; //模拟鼠标右键抬起 
        //private readonly int MOUSEEVENTF_MIDDLEDOWN = 0x0020; //模拟鼠标中键按下 
        //private readonly int MOUSEEVENTF_MIDDLEUP = 0x0040;// 模拟鼠标中键抬起 

        [DllImport("user32")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        /// <summary>
        /// 把患者ID传给医生工作站(利用鼠标的复制和粘贴)
        /// </summary>
        /// <param name="id"></param>
        public void Assignment(string id)
        {
            string printid = id;
            int cx = Cursor.Position.X + 1;
            int cy = Cursor.Position.Y + 1;
            if (AppContext.AppConfig.OutPutLocationX !="" || AppContext.AppConfig.OutPutLocationY != "")
            {
                int x = Int32.Parse(AppContext.AppConfig.OutPutLocationX);
                int y = Int32.Parse(AppContext.AppConfig.OutPutLocationY);
                Clipboard.SetDataObject(printid);
                //绝对位置
                mouse_event(MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE, x * 65535 / Screen.PrimaryScreen.Bounds.Width, y * 65535 / Screen.PrimaryScreen.Bounds.Height, 0, 0);//移动到需要点击的位置
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_ABSOLUTE, x * 65535 / Screen.PrimaryScreen.Bounds.Width, y * 65535 / Screen.PrimaryScreen.Bounds.Height, 0, 0);//点击
                mouse_event(MOUSEEVENTF_LEFTUP | MOUSEEVENTF_ABSOLUTE, x * 65535 / Screen.PrimaryScreen.Bounds.Width, y * 65535 / Screen.PrimaryScreen.Bounds.Height, 0, 0);//抬起
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_ABSOLUTE, x * 65535 / Screen.PrimaryScreen.Bounds.Width, y * 65535 / Screen.PrimaryScreen.Bounds.Height, 0, 0);//点击
                mouse_event(MOUSEEVENTF_LEFTUP | MOUSEEVENTF_ABSOLUTE, x * 65535 / Screen.PrimaryScreen.Bounds.Width, y * 65535 / Screen.PrimaryScreen.Bounds.Height, 0, 0);//抬起
                mouse_event(MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE, cx * 65535 / Screen.PrimaryScreen.Bounds.Width, cy * 65535 / Screen.PrimaryScreen.Bounds.Height, 0, 0);//移回到点击前的位置
                System.Threading.Thread.Sleep(Int32.Parse(AppContext.AppConfig.sleepOutPutTime));
                keybd_event((byte)Keys.ControlKey, 0, 0, 0);//按下
                keybd_event((byte)Keys.V, 0, 0, 0);
                keybd_event((byte)Keys.ControlKey, 0, 0x2, 0);//松开
                keybd_event((byte)Keys.V, 0, 0x2, 0);
                keybd_event((byte)Keys.Enter, 0, 0, 0);//按下
                keybd_event((byte)Keys.Enter, 0, 0x2, 0);//松开
            }
        }
        #endregion 
        #region 过号下一位
        /// <summary>
        /// 过号下一位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void skinbutNew_Click(object sender, EventArgs e)
        {
            PatientOkAndNext(HelperClass.triageId, "1");
        }
        #endregion 
        #region 患者到诊
        /// <summary>
        /// 患者到诊
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void skinButton1_Click(object sender, EventArgs e)
        {
            PatientOkAndNext(HelperClass.triageId, "0");
        }
        #endregion 
        #region 发送windows消息
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(
        int hWnd,                  // handle to destination window
        int Msg,                   // message
        int wParam,                // first message parameter
        ref COPYDATASTRUCT lParam  // second message parameter
        );
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern int FindWindow(string lpClassName, string lpWindowName);
        const int WM_COPYDATA = 0x004A;
        public void SendSess(string id)
        { 
          int WINDOW_HANDLER = FindWindow(null, "receiveForm"); //根据窗口名称，查找窗口
            if (WINDOW_HANDLER != 0)
            {
               // Console.WriteLine("YES 找到receiveForm!!!");
                byte[] sarr =
                System.Text.Encoding.Default.GetBytes(id);//消息内容
                int len = sarr.Length;
                COPYDATASTRUCT cds;
                cds.dwData = (IntPtr)100;
                cds.lpData = id;//消息内容
                cds.cbData = len + 1;
                SendMessage(WINDOW_HANDLER, WM_COPYDATA, 0, ref cds);
            }
        }
        #endregion 
    }
}
