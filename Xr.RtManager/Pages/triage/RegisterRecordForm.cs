using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Xr.Common;
using Xr.Common.Controls;
using Xr.Http;
using Xr.RtManager.Utils;

namespace Xr.RtManager.Pages.triage
{

    public partial class RegisterRecordForm : Form
    {
        public List<PatientReservationInfoEntity> list{get; set;}
        public Dictionary<String, PatientReservationInfoEntity> dicDeptIDs = new Dictionary<string, PatientReservationInfoEntity>();

        public String registerId;//预约id，医生停诊用的

        public String triageId;//分诊id，复诊医生停诊用的

        public String patientid;//患者主键

        public RegisterRecordForm()
        {
            InitializeComponent();
        }

        private void RegisterRecordForm_Load(object sender, EventArgs e)
        {
            RefreshData();
        }

        /// <summary>
        /// 刷新界面数据
        /// </summary>
        private void RefreshData()
        {
            panel1.Controls.Clear();
            foreach (var item in list)
            {
                ReservationInfoPanel pan = new ReservationInfoPanel();
                pan.Dock = DockStyle.Top;
                pan.lab_state.Text = item.statusTxt;
                pan.lab_dept.Text = item.deptName;
                pan.lab_doc.Text = item.doctorName;
                pan.lab_name.Text = item.patientName;
                pan.lab_reservationTime.Text = item.registerTime;
                pan.obj = item;
                if (item.status.Equals("0"))//待签到
                {
                    pan.btn_Operation.Text = "预约签到";
                }
                else if (item.status.Equals("1"))//已签到(候诊中)
                {
                    pan.btn_Operation.Text = "取消候诊";
                    pan.btn_buDa.Visible = true;
                }
                else if (item.status.Equals("2"))//就诊中
                {
                    pan.btn_Operation.Visible = false;
                }
                else if (item.status.Equals("3"))//已就诊(需要显示补打按钮)
                {
                    pan.btn_Operation.Text = "复诊签到";
                    pan.btn_buDa.Visible = true;
                    pan.btn_buDa.Tag = item.registerId;
                }
                else if (item.status.Equals("6"))//医生停诊需要转诊
                {
                    pan.btn_Operation.Text = "选择医生";
                }
                else if (item.status.Equals("7"))//复诊医生停诊需要转诊
                {
                    pan.btn_Operation.Text = "选择医生";
                }
                pan.BtnOperationClick += new Xr.Common.Controls.ReservationInfoPanel.OperationClick(this.btn_Operation_Click);
                pan.BtnBuDaClick += new Xr.Common.Controls.ReservationInfoPanel.BuDaClick(this.btn_BuDa_Click);
                panel1.Controls.Add(pan);
                try
                {
                    dicDeptIDs.Add(item.deptId+item.period, item);
                }
                catch 
                { 

                }
            }
        }

        private void btn_Operation_Click(object sender, EventArgs e, Object obj)
        {
            PatientReservationInfoEntity pr = obj as PatientReservationInfoEntity;
            if (pr.status == "0") //未分诊，调用签到接口
            {
                String param = "registerId=" + pr.registerId;
                String url = AppContext.AppConfig.serverUrl + "sch/registerTriage/signIn?" + param;
                this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                {
                    String data = HttpClass.httpPost(url);
                    return data;

                }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                {
                    //cmd.HideOpaqueLayer();
                    JObject objT = JObject.Parse(data.ToString());
                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        String tipMsg = "";
                        if (objT["result"]["tipMsg"] != null && objT["result"]["tipMsg"].ToString().Length != 0)
                        {
                            tipMsg = objT["result"]["tipMsg"].ToString() + ",";
                        }
                        String regVisitTime = null;
                        if (objT["result"]["regVisitTime"] != null)
                        {
                            regVisitTime = objT["result"]["regVisitTime"].ToString();
                        }
                        PrintNoteEntity printData = new PrintNoteEntity()
                        {
                            HospitalName = objT["result"]["hospitalName"].ToString(),
                            DeptName = objT["result"]["deptName"].ToString(),
                            ClinicName = objT["result"]["clinicName"].ToString(),

                            MzType = objT["result"]["mzType"].ToString(),
                            DoctorPrice = objT["result"]["doctorPrice"].ToString(),
                            DoctorName = objT["result"]["doctorName"].ToString(),
                            PatientId = objT["result"]["patientId"].ToString(),

                            QueueNum = objT["result"]["queueNum"].ToString(),
                            Name = objT["result"]["patientName"].ToString(),
                            WaitingNum = objT["result"]["waitingNum"].ToString(),
                            Time = objT["result"]["currentTime"].ToString(),
                            TipMsg = objT["result"]["tipMsg"].ToString(),
                            DoctorTip = objT["result"]["doctorTip"].ToString(),
                            RegVisitTime = regVisitTime
                        };
                        //打印小票
                        PrintNote print = new PrintNote(printData);
                        string message = "";
                        if (!print.Print(ref message))
                        {
                            MessageBoxUtils.Show(tipMsg + "打印小票失败：" + message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                        }
                        else
                        {
                            MessageBoxUtils.Hint(tipMsg + "打印小票完成", this);
                        }
                        getData();
                    }
                    else
                    {
                        MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                    }
                });
            }
            else if (pr.status == "1") //已签到取消候诊
            {
                String param = "triageId=" + pr.triageId;
                String url = AppContext.AppConfig.serverUrl + "sch/registerTriage/cancelWaiting?" + param;
                this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                {
                    String data = HttpClass.httpPost(url);
                    return data;

                }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                {
                    //cmd.HideOpaqueLayer();
                    JObject objT = JObject.Parse(data.ToString());
                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        getData();
                    }
                    else
                    {
                        MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                    }
                });
            }
            else if (pr.status == "2") //就诊中不需要操作
            {
                return;
            }
            else if (pr.status == "3") //已就诊，调用复诊签到接口
            {
                String param = "patientId=" + patientid;
                String url = AppContext.AppConfig.serverUrl + "itf/clab/checkLisReport?" + param;
                this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                {
                    String data = HttpClass.httpPost(url);
                    return data;

                }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                {
                    //cmd.HideOpaqueLayer();
                    JObject objT = JObject.Parse(data.ToString());
                    if (string.Compare(objT["state"].ToString(), "false", true) == 0)
                    {
                        ReSign(pr.triageId);
                    }
                    else
                    {
                        if (MessageBoxUtils.Show("该患者还有未出结果报告：\r\n" + objT["result"].ToString() + "是否继续为他复诊签到？", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, this) == DialogResult.OK)
                        {
                            ReSign(pr.triageId);
                        }
                    }
                });
                
            }

            else if (pr.status == "6") //该患者预约的医生已停诊，请选择其他医生签到
            {
                this.registerId = pr.registerId;
                DialogResult = DialogResult.No;
            }
            else if (pr.status == "7") //该复诊患者预约的医生已停诊，请选择其他医生签到
            {
                this.triageId = pr.triageId;
                DialogResult = DialogResult.Retry;
            }
        }
        private void ReSign(String triageId)
        {
            String param = "triageId=" + triageId;
            String url = AppContext.AppConfig.serverUrl + "sch/registerTriage/reviewSignIn?" + param;
            this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                //cmd.HideOpaqueLayer();
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    String tipMsg = "";
                    if (objT["result"]["tipMsg"] != null && objT["result"]["tipMsg"].ToString().Length != 0)
                    {
                        tipMsg = objT["result"]["tipMsg"].ToString() + ",";
                    }
                    String regVisitTime = null;
                    if (objT["result"]["regVisitTime"] != null)
                    {
                        regVisitTime = objT["result"]["regVisitTime"].ToString();
                    }
                    PrintNoteEntity printData = new PrintNoteEntity()
                    {
                        HospitalName = objT["result"]["hospitalName"].ToString(),
                        DeptName = objT["result"]["deptName"].ToString(),
                        ClinicName = objT["result"]["clinicName"].ToString(),

                        MzType = objT["result"]["mzType"].ToString(),
                        DoctorPrice = objT["result"]["doctorPrice"].ToString(),
                        DoctorName = objT["result"]["doctorName"].ToString(),
                        PatientId = objT["result"]["patientId"].ToString(),

                        QueueNum = objT["result"]["queueNum"].ToString(),
                        Name = objT["result"]["patientName"].ToString(),
                        WaitingNum = objT["result"]["waitingNum"].ToString(),
                        Time = objT["result"]["currentTime"].ToString(),
                        TipMsg = objT["result"]["tipMsg"].ToString(),
                        DoctorTip = objT["result"]["doctorTip"].ToString(),
                        RegVisitTime = regVisitTime
                    };
                    //打印小票
                    PrintNote print = new PrintNote(printData);
                    string message = "";
                    if (!print.Print(ref message))
                    {
                        MessageBoxUtils.Show(tipMsg + "打印小票失败：" + message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                    }
                    else
                    {
                        MessageBoxUtils.Hint(tipMsg + "打印小票完成", this);
                    }
                    getData();
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                }
            });
        }
        /// <summary>
        /// 补打指引单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_BuDa_Click(object sender, EventArgs e, Object obj)
        {
            PatientReservationInfoEntity pr = obj as PatientReservationInfoEntity;
            String param = "triageId=" + pr.triageId;
            String url = AppContext.AppConfig.serverUrl + "sch/registerTriage/waitingList?" + param;
            this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                //cmd.HideOpaqueLayer();
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    String tipMsg = "";
                    if (objT["result"]["tipMsg"] != null && objT["result"]["tipMsg"].ToString().Length != 0)
                    {
                        tipMsg = objT["result"]["tipMsg"].ToString() + ",";
                    }
                    String regVisitTime = null;
                    if (objT["result"]["regVisitTime"] != null)
                    {
                        regVisitTime = objT["result"]["regVisitTime"].ToString();
                    }
                    PrintNoteEntity printData = new PrintNoteEntity()
                    {
                        HospitalName = objT["result"]["hospitalName"].ToString(),
                        DeptName = objT["result"]["deptName"].ToString(),
                        ClinicName = objT["result"]["clinicName"].ToString(),

                        MzType = objT["result"]["mzType"].ToString(),
                        DoctorPrice = objT["result"]["doctorPrice"].ToString(),
                        DoctorName = objT["result"]["doctorName"].ToString(),
                        PatientId = objT["result"]["patientId"].ToString(),

                        QueueNum = objT["result"]["queueNum"].ToString(),
                        Name = objT["result"]["patientName"].ToString(),
                        WaitingNum = objT["result"]["waitingNum"].ToString(),
                        Time = objT["result"]["currentTime"].ToString(),
                        TipMsg = objT["result"]["tipMsg"].ToString(),
                        DoctorTip = objT["result"]["doctorTip"].ToString(),
                        RegVisitTime = regVisitTime
                    };
                    PrintNote print = new PrintNote(printData);
                    string message = "";
                    if (!print.Print(ref message))
                    {
                        MessageBoxUtils.Show(tipMsg + "打印小票失败：" + message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                    }
                    else
                    {
                        MessageBoxUtils.Hint(tipMsg + "打印小票完成", this);
                    }
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                }
            });
        }

        private void getData()
        {
            //获取患者预约信息
            String jsonStr = String.Empty;

            String param = "hospitalId=" +AppContext.Session.hospitalId + "&deptIds="+AppContext.Session.deptIds
                    +"&patientId="+patientid;
            String url = AppContext.AppConfig.serverUrl + "sch/doctorScheduRegister/patCurrentRegsiter?" + param;
            this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                //cmd.HideOpaqueLayer();
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    list = objT["result"].ToObject<List<PatientReservationInfoEntity>>();
                    RefreshData();
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                }
            });
        }

        /// <summary>
        /// 多线程异步后台处理某些耗时的数据，不会卡死界面
        /// </summary>
        /// <param name="time">线程延迟多少</param>
        /// <param name="workFunc">Func委托，包装耗时处理（不含UI界面处理），示例：(o)=>{ 具体耗时逻辑; return 处理的结果数据 }</param>
        /// <param name="funcArg">Func委托参数，用于跨线程传递给耗时处理逻辑所需要的对象，示例：String对象、JObject对象或DataTable等任何一个值</param>
        /// <param name="workCompleted">Action委托，包装耗时处理完成后，下步操作（一般是更新界面的数据或UI控件），示列：(r)=>{ datagirdview1.DataSource=r; }</param>
        protected void DoWorkAsync(int time, Func<object, object> workFunc, object funcArg = null, Action<object> workCompleted = null)
        {
            var bgWorkder = new BackgroundWorker();


            //Form loadingForm = null;
            //System.Windows.Forms.Control loadingPan = null;
            bgWorkder.WorkerReportsProgress = true;
            bgWorkder.ProgressChanged += (s, arg) =>
            {
                if (arg.ProgressPercentage > 1) return;

            };

            bgWorkder.RunWorkerCompleted += (s, arg) =>
            {

                try
                {
                    bgWorkder.Dispose();

                    if (workCompleted != null)
                    {
                        workCompleted(arg.Result);
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                        throw new Exception(ex.InnerException.Message);
                    else
                        
                        throw new Exception(ex.Message);
                }
            };

            bgWorkder.DoWork += (s, arg) =>
            {
                bgWorkder.ReportProgress(1);
                var result = workFunc(arg.Argument);
                arg.Result = result;
                bgWorkder.ReportProgress(100);
                Thread.Sleep(time);
            };

            bgWorkder.RunWorkerAsync(funcArg);
        }

    }
}
