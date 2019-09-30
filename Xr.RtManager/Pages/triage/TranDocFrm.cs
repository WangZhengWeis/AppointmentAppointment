using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using Newtonsoft.Json.Linq;
using Xr.Http;
using Xr.Common;
using Xr.RtManager.Module.triage;
using System.Threading;

namespace Xr.RtManager.Pages.triage
{
    public partial class TranDocFrm : Form
    {
        String Dept = String.Empty;
        Xr.Common.Controls.OpaqueCommand cmd;
        public TranDocFrm(String deptID)
        {
            InitializeComponent();
            Dept = deptID;
            Getperiod();
        }

        private void OfficeEdit_Load(object sender, EventArgs e)
        {
            cmd = new Xr.Common.Controls.OpaqueCommand(this);
              //获取科室下拉框数据
            treeStopDeptId.Properties.DataSource = AppContext.Session.deptList;
            treeStopDeptId.Properties.TreeList.KeyFieldName = "id";
            treeStopDeptId.Properties.TreeList.ParentFieldName = "parentId";
            treeStopDeptId.Properties.DisplayMember = "name";
            treeStopDeptId.Properties.ValueMember = "id";

            //获取科室下拉框数据
            treeIntoDeptId.Properties.DataSource = AppContext.Session.deptList;
            treeIntoDeptId.Properties.TreeList.KeyFieldName = "id";
            treeIntoDeptId.Properties.TreeList.ParentFieldName = "parentId";
            treeIntoDeptId.Properties.DisplayMember = "name";
            treeIntoDeptId.Properties.ValueMember = "id";
            //默认选择选择第一个
            treeStopDeptId.EditValue = AppContext.Session.deptList[0].id;
            
        }

        void Getperiod()
        {
            List<DeptEntity> list = new List<DeptEntity>();
            list.Add(new DeptEntity() { name = "上午", id = "0" });
            list.Add(new DeptEntity() { name = "下午", id = "1" });
            list.Add(new DeptEntity() { name = "晚上", id = "2" });
            list.Add(new DeptEntity() { name = "全天", id = "3" });
            lueperiod.Properties.DataSource = list;
            lueperiod.Properties.DisplayMember = "name";
            lueperiod.Properties.ValueMember = "id";
            lueperiod.EditValue = SetNoonCode();
        }
        #region 根据系统时间设置当前午别
        /// <summary>
        /// 根据系统时间设置当前午别
        /// </summary>
        private String SetNoonCode()
        {
            string Period = String.Empty;
            string url = AppContext.AppConfig.serverUrl + "sch/registerTriage/getCurrentPeriod";
            //{"code": 200,"message": "操作成功","result": {"period": "2"},"state": true}
            string jsonStr = HttpClass.httpPost(url);
            JObject objT = JObject.Parse(jsonStr);
            if (string.Compare(objT["state"].ToString(), "true", true) == 0)
            {
                  return objT["result"]["period"].ToString();
            }
            else
            {
                int hour = System.DateTime.Now.Hour;
                if (hour <= 7)
                {
                    return "2";
                }
                else if (hour > 7 && hour <= 13)
                {
                    return "0";
                }
                else if (hour > 13 && hour <= 18)
                {
                    return "1";
                }
                else
                {
                    return "2";
                }
            }
        }
        #endregion
        private void lueperiod_EditValueChanged(object sender, EventArgs e)
        {
            setStopDocLue();
            setIntoDocLue();
        }
        private void treeDeptId_EditValueChanged(object sender, EventArgs e)
        {
            setStopDocLue();
        }
        private void treeIntoDeptId_EditValueChanged(object sender, EventArgs e)
        {
            setIntoDocLue();
        }
        private void setStopDocLue()
        {     //获取获取科室可挂号医生信息
            Dictionary<string, string> prament = new Dictionary<string, string>();
            String param = "";
            prament.Add("hospitalId", AppContext.Session.hospitalId);
            prament.Add("deptId", treeStopDeptId.EditValue.ToString());
            prament.Add("period", lueperiod.EditValue.ToString());
            String url = String.Empty;
            if (prament.Count != 0)
            {
                param = string.Join("&", prament.Select(x => x.Key + "=" + x.Value).ToArray());
            }
            url = AppContext.AppConfig.serverUrl + "sch/doctorSitting/findSittingDoctor?" + param;
            JObject objT = new JObject();
            objT = JObject.Parse(HttpClass.httpPost(url));
            //{""code"":200,""message"":""操作成功"",""result"":[{""hospitalId"":12,""deptId"":2,""doctorId"":22,""doctorName"":""梁医生"",""isStop"":""0""},{""hospitalId"":12,""deptId"":2,""doctorId"":20,""doctorName"":""测试"",""isStop"":""0""},{""hospitalId"":12,""deptId"":2,""doctorId"":19,""doctorName"":""1"",""isStop"":""0""},{""hospitalId"":12,""deptId"":2,""doctorId"":13,""doctorName"":""21321"",""isStop"":""0""}],""state"":true}
            if (string.Compare(objT["state"].ToString(), "true", true) == 0)
            {
                //List<Dic> list = objT["result"].ToObject<List<Dic>>();
                List<RoomInfoEntity> list = objT["result"].ToObject<List<RoomInfoEntity>>();
                lueStopDoctor.Properties.DataSource = list;
                lueStopDoctor.Properties.DisplayMember = "doctorName";
                lueStopDoctor.Properties.ValueMember = "doctorId";
                lueStopDoctor.EditValue = null;

            }
            else
            {
                MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                return;
            }
        }
        private void setIntoDocLue()
        {     //获取获取科室可挂号医生信息
            Dictionary<string, string> prament = new Dictionary<string, string>();
            String param = "";
            prament.Add("hospitalId", AppContext.Session.hospitalId);
            prament.Add("deptId", treeIntoDeptId.EditValue.ToString());
            prament.Add("period", lueperiod.EditValue.ToString());
            String url = String.Empty;
            if (prament.Count != 0)
            {
                param = string.Join("&", prament.Select(x => x.Key + "=" + x.Value).ToArray());
            }
            url = AppContext.AppConfig.serverUrl + "sch/doctorSitting/findSittingDoctor?" + param;
            JObject objT = new JObject();
            objT = JObject.Parse(HttpClass.httpPost(url));
            //{""code"":200,""message"":""操作成功"",""result"":[{""hospitalId"":12,""deptId"":2,""doctorId"":22,""doctorName"":""梁医生"",""isStop"":""0""},{""hospitalId"":12,""deptId"":2,""doctorId"":20,""doctorName"":""测试"",""isStop"":""0""},{""hospitalId"":12,""deptId"":2,""doctorId"":19,""doctorName"":""1"",""isStop"":""0""},{""hospitalId"":12,""deptId"":2,""doctorId"":13,""doctorName"":""21321"",""isStop"":""0""}],""state"":true}
            if (string.Compare(objT["state"].ToString(), "true", true) == 0)
            {
                //List<Dic> list = objT["result"].ToObject<List<Dic>>();
                List<RoomInfoEntity> list = objT["result"].ToObject<List<RoomInfoEntity>>();
                ////转入医生需判断医生的isStop是否为“0”，为“0”，才显示在转入医生下拉框中；
                //List<RoomInfoEntity> nonStopDoclist = new List<RoomInfoEntity>();
                //foreach (var item in list)
                //{
                //    if (item.isStop == "0")
                //    {
                //        nonStopDoclist.Add(item);
                //    }
                //}
                ////转入医生
                //lueIntoDoctor.Properties.DataSource = nonStopDoclist;
                //转入医生
                lueIntoDoctor.Properties.DataSource = list;
                lueIntoDoctor.Properties.DisplayMember = "doctorName";
                lueIntoDoctor.Properties.ValueMember = "doctorIdANDperiod";
                lueIntoDoctor.EditValue = null;
            }
            else
            {
                MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                return;
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (lueStopDoctor.EditValue == null)
            {
                MessageBoxUtils.Hint("请选择停诊医生", HintMessageBoxIcon.Error, this);
                return;
            }
            if (lueIntoDoctor.EditValue == null)
            {
                MessageBoxUtils.Hint("请选择转入医生", HintMessageBoxIcon.Error, this);
                return;
            }
            if (getTriageIds().Length==0)
            {
                MessageBoxUtils.Hint("请选择转诊患者", HintMessageBoxIcon.Error, this);
                return;
            }

            //转诊
            Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.PatientTranDoc });
        }
        
        bool NeedCloseWaitingFrm = true;
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

                    if (NeedCloseWaitingFrm)
                    {
                        cmd.IsShowCancelBtn = false;
                        cmd.ShowOpaqueLayer(0.56f, "请稍候...");
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
            try
            {
                #region 候诊中列表
                if (workType == AsynchronousWorks.WaitingPatientList)
                {
                    //报告前台状态变更
                    backgroundWorker1.ReportProgress(50);
                    // 异步操作2
                    //Thread.Sleep(300);

                    

                    String param = "";
                    //获取医生坐诊信息
                    Dictionary<string, string> prament = new Dictionary<string, string>();
                    //hospitalId=12&deptId=2&period=3
                    prament.Add("hospitalId", AppContext.Session.hospitalId);
                    prament.Add("deptId", treeStopDeptId.EditValue.ToString());
                    prament.Add("doctorId", lueStopDoctor.EditValue.ToString());
                    //lueIntoDoctor
                    //prament.Add("pageSize", "10000");

                    String url = String.Empty;
                    if (prament.Count != 0)
                    {
                        param = string.Join("&", prament.Select(x => x.Key + "=" + x.Value).ToArray());
                    }
                    url = AppContext.AppConfig.serverUrl + "sch/registerTriage/getWaitingListByDoctorId?" + param;
                    String jsonStr = HttpClass.httpPost(url);
                    //jsonStr=@"{""code"":200,""message"":""操作成功"",""result"":[{""doctorId"":1,""doctorName"":""张医生"",""period"":""下午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""晚上"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""上午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":80,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""下午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""晚上"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""上午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":80,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""下午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""上午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":80,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""上午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":80,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""下午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""晚上"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""上午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":80,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""下午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""上午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":80,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""上午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":80,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""下午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""晚上"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""上午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":80,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""下午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""上午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":80,""isStop"":""0""},{""doctorId"":15,""doctorName"":""杰大哥"",""period"":""全天"",""clinicPrefix"":""B"",""clinicName"":""02诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":15,""doctorName"":""杰大哥"",""period"":""上午"",""clinicPrefix"":""B"",""clinicName"":""02诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":15,""doctorName"":""杰大哥"",""period"":""下午"",""clinicPrefix"":""B"",""clinicName"":""02诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":15,""doctorName"":""杰大哥"",""period"":""晚上"",""clinicPrefix"":""B"",""clinicName"":""02诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":15,""doctorName"":""杰大哥"",""period"":""上午"",""clinicPrefix"":""B"",""clinicName"":""02诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":15,""doctorName"":""杰大哥"",""period"":""下午"",""clinicPrefix"":""B"",""clinicName"":""02诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":15,""doctorName"":""杰大哥"",""period"":""晚上"",""clinicPrefix"":""B"",""clinicName"":""02诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""}],""state"":true}";

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
                #region 患者转诊
                if (workType == AsynchronousWorks.PatientTranDoc)
                {
                    //报告前台状态变更
                    backgroundWorker1.ReportProgress(50);
                    // 异步操作2
                    //Thread.Sleep(300);
                    String param = "";
                    //获取医生坐诊信息
                    Dictionary<string, string> prament = new Dictionary<string, string>();
                    //hospitalId=12&deptId=2&period=3
                    prament.Add("hospitalId", AppContext.Session.hospitalId);
                    prament.Add("outDeptId", treeStopDeptId.EditValue.ToString());
                    prament.Add("outDoctorId", lueStopDoctor.EditValue.ToString());
                    prament.Add("inDeptId", treeIntoDeptId.EditValue.ToString());
                    prament.Add("inDoctorId", lueIntoDoctor.EditValue.ToString().Split('&')[0]);
                    prament.Add("inPeriod", lueIntoDoctor.EditValue.ToString().Split('&')[1]);
                    prament.Add("triageIds", getTriageIds());
                    //prament.Add("pageSize", "10000");

                    String url = String.Empty;
                    if (prament.Count != 0)
                    {
                        param = string.Join("&", prament.Select(x => x.Key + "=" + x.Value).ToArray());
                    }
                    url = AppContext.AppConfig.serverUrl + "sch/doctorStopRurn/updateWaitingListByDoctorId?" + param;
                    String jsonStr = HttpClass.httpPost(url);
                    //jsonStr=@"{""code"":200,""message"":""操作成功"",""result"":[{""doctorId"":1,""doctorName"":""张医生"",""period"":""下午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""晚上"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""上午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":80,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""下午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""晚上"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""上午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":80,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""下午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""上午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":80,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""上午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":80,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""下午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""晚上"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""上午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":80,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""下午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""上午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":80,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""上午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":80,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""下午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""晚上"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""上午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":80,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""下午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":1,""doctorName"":""张医生"",""period"":""上午"",""clinicPrefix"":""A"",""clinicName"":""01诊室"",""waitingNum"":0,""siteSyNum"":80,""isStop"":""0""},{""doctorId"":15,""doctorName"":""杰大哥"",""period"":""全天"",""clinicPrefix"":""B"",""clinicName"":""02诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":15,""doctorName"":""杰大哥"",""period"":""上午"",""clinicPrefix"":""B"",""clinicName"":""02诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":15,""doctorName"":""杰大哥"",""period"":""下午"",""clinicPrefix"":""B"",""clinicName"":""02诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":15,""doctorName"":""杰大哥"",""period"":""晚上"",""clinicPrefix"":""B"",""clinicName"":""02诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":15,""doctorName"":""杰大哥"",""period"":""上午"",""clinicPrefix"":""B"",""clinicName"":""02诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":15,""doctorName"":""杰大哥"",""period"":""下午"",""clinicPrefix"":""B"",""clinicName"":""02诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""},{""doctorId"":15,""doctorName"":""杰大哥"",""period"":""晚上"",""clinicPrefix"":""B"",""clinicName"":""02诊室"",""waitingNum"":0,""siteSyNum"":0,""isStop"":""0""}],""state"":true}";

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
            catch (Exception ex)
            {
                result.result = false;
                result.msg = ex.Message;// PatientSearchInfoRef.Msg;
                e.Result = result;
            }
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
                    MessageBoxUtils.Show("操作失败，请稍候尝试。", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                    return;
                }
                var result = (SycResult)e.Result;
                if (result.result)
                {
                    AsynchronousWorks workType = result.WorkType;
                    #region 候诊中列表
                    if (workType == AsynchronousWorks.WaitingPatientList)
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
                        List<WaitingPatientsEntity> list = objT["result"].ToObject<List<WaitingPatientsEntity>>();
                        Gc_patients.DataSource = list;

                        cmd.HideOpaqueLayer();
                        //cmd = new Xr.Common.Controls.OpaqueCommand(this);
                        return;
                    }
                    #endregion
                    #region 患者转诊
                    if (workType == AsynchronousWorks.PatientTranDoc)
                    {
                        if (result.obj == null)
                        {
                            //_waitForm.DialogResult = DialogResult.OK;
                            //_waitForm.ChangeNoticeComplete(result.msg, Dialog.HintMessageBoxIcon.Error);
                            //_waitForm.Close();
                            cmd.HideOpaqueLayer();
                            return;
                        }
                        
                        NeedCloseWaitingFrm = false;
                        Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.WaitingPatientList });
                        MessageBoxUtils.Hint("操作成功",this);

                        
                        //cmd = new Xr.Common.Controls.OpaqueCommand(this);
                        return;
                    }
                    #endregion
                }
                else
                {
                    Gc_patients.DataSource = null;
                    MessageBoxUtils.Show(result.msg, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                    cmd.HideOpaqueLayer();
                    //MessageBoxUtils.Hint(result.msg);
                }
            }
            catch (Exception ex)
            {
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                cmd.HideOpaqueLayer();
            }
            finally
            {
                // 关闭加载提示框
                //DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
                //cmd.HideOpaqueLayer();
                //关闭等待框
                if (NeedCloseWaitingFrm)
                {
                    cmd.HideOpaqueLayer();
                }
                else
                {
                    NeedCloseWaitingFrm = true;
                }
            }
        }
        new void Disposed(object sender, object e)
        {

        }

        private void gv_patients_MouseDown(object sender, MouseEventArgs e)
        {
            //鼠标左键点击
            System.Threading.Thread.Sleep(10);
            if (e.Button == MouseButtons.Left)
            {

                //GridHitInfo gridHitInfo = gridView.CalcHitInfo(e.X, e.Y);
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo gridHitInfo = gv_patients.CalcHitInfo(e.X, e.Y);

                //在列标题栏内且列标题name是"colName"
                if (gridHitInfo.Column != null)
                {
                    if (gridHitInfo.InColumnPanel && gridHitInfo.Column.Name == "select")
                    {

                        //获取该列右边线的x坐标

                        DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo gridViewInfo = (DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo)this.gv_patients.GetViewInfo();

                        int x = gridViewInfo.GetColumnLeftCoord(gridHitInfo.Column) + gridHitInfo.Column.Width;

                        //右边线向左移动3个像素位置不弹出对话框（实验证明3个像素是正好的）

                        if (e.X < x - 3)
                        {

                            //XtraMessageBox.Show("点击select列标题！");
                            //bool flag = false;
                            for (int i = 0; i < gv_patients.RowCount; i++)
                            {
                                //鼠标的那个按钮按下 
                                string dr = gv_patients.GetRowCellValue(i, "check").ToString();
                                if (dr == "1"){
                                    //flag = false;
                                    gv_patients.SetRowCellValue(i, gv_patients.Columns["check"], "0");
                                }
                                else{
                                    //flag = true;
                                    gv_patients.SetRowCellValue(i, gv_patients.Columns["check"], "1");
                                }
                                    
                            }
                            //if(flag){
                            //    select.Caption = "☑";
                            //}
                            //else
                            //{
                            //    select.Caption = "□";
                            //}
                        }
                    }
                    else if (!gridHitInfo.InColumnPanel)
                    {

                        int i = gridHitInfo.RowHandle;
                        string dr = gv_patients.GetRowCellValue(i, "check").ToString();
                        if (dr == "1")
                            gv_patients.SetRowCellValue(i, gv_patients.Columns["check"], "0");
                        else//(dr == "0")
                            gv_patients.SetRowCellValue(i, gv_patients.Columns["check"], "1");

                    }
                }

            }

        }

        private String getTriageIds()
        {
            //string triageIds = "";
            //List<WaitingPatientsEntity> wpList = gv_patients.DataSource as List<WaitingPatientsEntity>;
            //foreach (WaitingPatientsEntity wp in wpList)
            //{
            //    if (wp.check)
            //    {
            //        triageIds += wp.triageId + ",";
            //    }
            //}

            //if (triageIds.Length > 0)
            //    triageIds = triageIds.Substring(0, triageIds.Length - 1);
            //return triageIds;

            string triageIds = "";
            for (int i = 0; i < gv_patients.RowCount; i++)
            {   //   获取选中行的check的值   
                string dr = gv_patients.GetRowCellValue(i, "check").ToString();
                if (dr != String.Empty)
                {
                    if (dr == "1")
                    {
                        WaitingPatientsEntity rowItem = gv_patients.GetRow(i) as WaitingPatientsEntity;
                        triageIds += rowItem.triageId + ",";
                        //bandedGvList.Columns["euDrugtype"].FilterInfo = new ColumnFilterInfo("[euDrugtype] LIKE '0'");
                    }
                }
            }
            if (triageIds.Length > 0)
                triageIds = triageIds.Substring(0, triageIds.Length - 1);
            return triageIds;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        private void lueStopDoctor_EditValueChanged(object sender, EventArgs e)
        {
            if (lueStopDoctor.EditValue != null)
            {
                string intoDoc=lueIntoDoctor.EditValue == null ? "" : lueIntoDoctor.EditValue.ToString();
                if (lueStopDoctor.EditValue.ToString() == intoDoc)
                {
                    lueStopDoctor.EditValue = null;
                    MessageBoxUtils.Hint("转入医生不能与停诊医生相同", HintMessageBoxIcon.Error, this);
                }
                else
                {
                    //读取该医生候诊信息
                    Asynchronous(new AsyncEntity() { WorkType = AsynchronousWorks.WaitingPatientList });
                }
            }
            else
            {
                Gc_patients.DataSource = null;
            }
        }
        private void lueIntoDoctor_EditValueChanged(object sender, EventArgs e)
        {
            if (lueIntoDoctor.EditValue != null)
            {
                string intoDoc = lueIntoDoctor.EditValue == null ? "" : lueIntoDoctor.EditValue.ToString();
                if (lueStopDoctor.EditValue.ToString() == intoDoc)
                {
                    lueIntoDoctor.EditValue = null;
                    MessageBoxUtils.Hint("转入医生不能与停诊医生相同", HintMessageBoxIcon.Error, this);
                }
            }
        }





     
    }
}
