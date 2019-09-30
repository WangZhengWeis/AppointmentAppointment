using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RestSharp;
using Xr.Http.RestSharp;
using System.Net;
using System.Threading;
using System.Configuration;
using Newtonsoft.Json.Linq;
using Xr.RtCall.Model;
using Xr.Common;

namespace Xr.RtCall.pages
{
    public partial class RtCallPeationFrm : UserControl
    {
        public SynchronizationContext _context;
        public static RtCallPeationFrm RTCallfrm = null;//初始化的时候窗体对象赋值
        public RtCallPeationFrm()
        {
            InitializeComponent();
            #region 
            this.SetStyle(ControlStyles.ResizeRedraw |
                  ControlStyles.OptimizedDoubleBuffer |
                  ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            _context = SynchronizationContext.Current;
            RTCallfrm = this;
            #endregion 
            PatientList();
        }
        #region 患者列表
        /// <summary>
        /// 患者列表
        /// </summary>
        public void PatientList()
        {
            try
            {
                Dictionary<string, string> prament = new Dictionary<string, string>();
                prament.Add("hospitalId", HelperClass.hospitalId);//医院ID
                prament.Add("deptId", HelperClass.deptId);//科室ID
                prament.Add("doctorId", HelperClass.doctorId);//医生ID
                prament.Add("status", Postoperative.EditValue.ToString());
                RestSharpHelper.ReturnResult<List<string>>(InterfaceAddress.findPatientListByDoctor, prament, Method.POST,
                 result =>
                {
                    switch (result.ResponseStatus)
                    {
                        case ResponseStatus.Completed:
                            if (result.StatusCode == HttpStatusCode.OK)
                            {
                                Log4net.LogHelper.Info("请求结果：" + string.Join(",", result.Data.ToArray()));
                                JObject objT = JObject.Parse(string.Join(",", result.Data.ToArray()));
                                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                                {
                                    List<Patient> a = objT["result"].ToObject<List<Patient>>();
                                    //for (int i = 0; i < a.Count; i++)
                                    //{
                                    //    if (a[i].showRegTime != true)
                                    //    {
                                    //        a[i].workTime = "";
                                    //    }
                                    //}
                                    _context.Send((s) => this.gc_Pateion.DataSource = a,null);
                                    _context.Send((s) => label1.Text=a.Count+"人", null);
                                }
                                else
                                {
                                    _context.Send((s) => MessageBoxUtils.Hint(objT["message"].ToString(), Form1.pCurrentWin), null);
                                }
                            }
                            break;
                    }
                });
            }
            catch (Exception ex)
            {
               Log4net.LogHelper.Error("获取患者列表错误信息：" + ex.Message);
            }
        }
        #endregion
        #region 右键菜单
        /// <summary>
        /// 复诊预约
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 复诊预约ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedRow = this.gv_Pateion.GetFocusedRow() as Patient;
                  if (selectedRow == null)
                    return;
                Form1.pCurrentWin.panel_MainFrm.Controls.Clear();
                RtIntersectionAppointmentFrm rtcpf = new RtIntersectionAppointmentFrm(selectedRow);
                rtcpf.Dock = DockStyle.Fill;
                Form1.pCurrentWin.panel_MainFrm.Controls.Add(rtcpf);
            }
            catch (Exception ex)
            {
               Log4net.LogHelper.Error("复诊预约错误信息："+ex.Message);
            }
        }
        /// <summary>
        /// 强制叫号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 叫号ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToBoolean(AppContext.AppConfig.IsStart))
                {
                    if (Form1.pCurrentWin.RecordHandle == IntPtr.Zero)
                    {
                        Form1.pCurrentWin.FindDoctorValue();
                    }
                    if (Form1.pCurrentWin.RecordHandle != IntPtr.Zero)
                    {
                        if (!Form1.pCurrentWin.IsTrueOrFalseVoice())
                        {
                            MessageBoxUtils.Hint("请先完成当前患者就诊或清空号码，才能呼叫下一病人！", null);
                            return;
                        }
                    }
                }
                 var selectedRow = this.gv_Pateion.GetFocusedRow() as Patient;
                  if (selectedRow == null)
                    return;
                  CallNewPatient(selectedRow.triageId,HelperClass.clinicId);
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("右键叫号时错误信息："+ex.Message);
            }
        }
        /// <summary>
        /// 当时间还没到时医生想呼叫下一位调用接口
        /// </summary>
        public void CallNewPatient(string triageId, string clinicId)
        {
            try
            {
                Dictionary<string, string> prament = new Dictionary<string, string>();
                prament.Add("triageId", triageId);
                prament.Add("clinicId", clinicId);
                RestSharpHelper.ReturnResult<List<string>>(InterfaceAddress.ForcedCall, prament, Method.POST, result =>
                {
                    if (result.ResponseStatus == ResponseStatus.Completed)
                    {
                        if (result.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            Log4net.LogHelper.Info("请求结果：" + string.Join(",", result.Data.ToArray()));
                            JObject objT = JObject.Parse(string.Join(",", result.Data.ToArray()));
                            if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                            {
                                _context.Send((s) => HelperClass.triageId = objT["result"][0]["triageId"].ToString(), null);
                                _context.Send((s) => Form1.pCurrentWin.label2.Text = objT["result"][0]["smallCellShow"].ToString() + objT["result"][0]["nextCellShow"].ToString(), null);
                                if (Convert.ToBoolean(AppContext.AppConfig.WhetherToAssign))
                                {
                                    string patientId = objT["result"][0]["patientId"].ToString();
                                    Form1.pCurrentWin.PatientId = patientId;
                                    if (Convert.ToBoolean(AppContext.AppConfig.IsStart))
                                    {
                                        _context.Send((s) => Form1.pCurrentWin.InputStr(patientId), null);
                                    }
                                    else
                                    {
                                        _context.Send((s) => Form1.pCurrentWin.Assignment(patientId), null);
                                    }
                                }
                                _context.Send((s) => PatientList(), null);
                            }
                            else
                            {
                                if (this.Size.Height == 28)
                                {
                                    _context.Send((s) => MessageBoxUtils.Hint(objT["message"].ToString(), null), null);
                                }
                                else
                                {
                                    _context.Send((s) => MessageBoxUtils.Hint(objT["message"].ToString(), Form1.pCurrentWin), null);
                                }
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("呼叫当前患者错误信息："+ex.Message);
            }
        }
        #endregion 
        #region 刷新按钮
        /// <summary>
        /// 刷新按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void skinbutNew_Click(object sender, EventArgs e)
        {
            this.skinbutNew.Text = "查询中";
            PatientList();
            //Thread.Sleep(1000);
            this.skinbutNew.Text = "刷新";
        }
        /// <summary>
        /// 单击选择刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Postoperative_Properties_SelectedIndexChanged(object sender, EventArgs e)
        {
            PatientList();
        }
        #endregion
        #region 设置右键菜单是否显示
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (Postoperative.EditValue.ToString() == "3")
            {
                this.contextMenuStrip1.Items[0].Visible = false;
            }
            else
            {
                this.contextMenuStrip1.Items[0].Visible = true;
            }
            if (Postoperative.EditValue.ToString() == "3" || Postoperative.EditValue.ToString() == "2")
            {
                this.contextMenuStrip1.Items[1].Visible = false;
            }
            else
            {
                this.contextMenuStrip1.Items[1].Visible = true;
            }
        }
        #endregion 
        #region 
        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingFrm ttf = new SettingFrm();
            Form1.pCurrentWin.Hide();
            ttf.Show();
            if (SettingFrm.SettingAppConfig)
            {
                Application.ExitThread();
                Application.Exit();
                Application.Restart();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }
        #endregion
        #region 设置右键完成按钮
        public static bool IsClick { get; set; }
        public static Patient patinet { get; set; }
        private void gv_Pateion_Click(object sender, EventArgs e)
        {
            if (Postoperative.EditValue.ToString() == "1" || Postoperative.EditValue.ToString() == "4")
            {
                IsClick = true;
                patinet = new Patient();
                patinet = this.gv_Pateion.GetFocusedRow() as Patient;
                if (patinet == null)
                    return;
            }
        }
        #endregion
        #region 完成
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                patinet = this.gv_Pateion.GetFocusedRow() as Patient;
                if (patinet == null)
                    return;
                String url = AppContext.AppConfig.serverUrl + InterfaceAddress.visitWin + "?triageId=" + patinet.triageId;
                String data = Xr.Http.HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    PatientList();
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, null);
                }
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Info("右键完成按钮错误信息："+ex.Message);
            }
        }
        #endregion
    }
}
