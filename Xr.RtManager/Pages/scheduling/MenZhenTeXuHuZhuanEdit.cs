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
using System.IO;
using System.Net;
using Xr.Common;
using System.Threading;
using System.Text.RegularExpressions;
using DevExpress.XtraEditors;

namespace Xr.RtManager.Pages.scheduling
{
    public partial class MenZhenTeXuHuZhuanEdit : Form
    {
        public MenZhenTeXuHuZhuanEdit()
        {
            InitializeComponent();
        }

        Xr.Common.Controls.OpaqueCommand cmd;

        public ScheduledEntity scheduled { get; set; }

        public DefaultVisitEntity defaultVisit { get; set; }
        /// <summary>
        /// 出诊信息模板
        /// </summary>
        public DefaultVisitEntity defaultVisitTemplate { get; set; }

        private void ModifyNumberSourceEdit_Load(object sender, EventArgs e)
        {
            cmd = new Xr.Common.Controls.OpaqueCommand(this);
            if (scheduled.mzType.Equals("2"))
                cbTx.CheckState = CheckState.Checked;
            String sdName = "";
            if (scheduled.period.Equals("0")) sdName = "上午";
            if (scheduled.period.Equals("1")) sdName = "下午";
            if (scheduled.period.Equals("2")) sdName = "晚上";
            if (scheduled.period.Equals("3")) sdName = "全天";
            label1.Text = "科室：" + scheduled.deptName + "        医生：" + scheduled.doctorName + "        日期：" + scheduled.workDate + sdName;
            String param = "hospitalId=" + AppContext.Session.hospitalId + "&deptId=" + scheduled.deptId
                + "&doctorId=" + scheduled.doctorId + "&workDate=" + scheduled.workDate
                + "&period=" + scheduled.period + "&isOpen=1";
            String url = AppContext.AppConfig.serverUrl + "sch/doctorScheduPlan/findScheduList?" + param;
            cmd.ShowOpaqueLayer(0f);
            this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    List<SourceDataEntity> sourceDataList = objT["result"].ToObject<List<SourceDataEntity>>();
                    gcSourceData.DataSource = sourceDataList;

                    //获取默认出诊时间字典配置
                    url = AppContext.AppConfig.serverUrl + "cms/doctor/findDoctorVisitingDict";
                    this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                    {
                        String data2 = HttpClass.httpPost(url);
                        return data2;

                    }, null, (data3) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                    {
                        objT = JObject.Parse(data3.ToString());
                        if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                        {
                            defaultVisitTemplate = objT["result"].ToObject<DefaultVisitEntity>();
                            defaultVisit = new DefaultVisitEntity();
                            String[] hyArr = defaultVisitTemplate.defaultSourceNumber.Split(new char[] { '-' });
                            defaultVisitTemplate.mStart = defaultVisitTemplate.defaultVisitTimeAm.Substring(0, 5);
                            defaultVisitTemplate.mEnd = defaultVisitTemplate.defaultVisitTimeAm.Substring(6, 5);
                            defaultVisitTemplate.mSubsection = defaultVisitTemplate.segmentalDuration;
                            defaultVisitTemplate.mScene = hyArr[0];
                            defaultVisitTemplate.mOpen = hyArr[1];
                            defaultVisitTemplate.mRoom = hyArr[2];
                            defaultVisitTemplate.mEmergency = hyArr[3];

                            defaultVisitTemplate.aStart = defaultVisitTemplate.defaultVisitTimePm.Substring(0, 5);
                            defaultVisitTemplate.aEnd = defaultVisitTemplate.defaultVisitTimePm.Substring(6, 5);
                            defaultVisitTemplate.aSubsection = defaultVisitTemplate.segmentalDuration;
                            defaultVisitTemplate.aScene = hyArr[0];
                            defaultVisitTemplate.aOpen = hyArr[1];
                            defaultVisitTemplate.aRoom = hyArr[2];
                            defaultVisitTemplate.aEmergency = hyArr[3];

                            defaultVisitTemplate.nStart = defaultVisitTemplate.defaultVisitTimeNight.Substring(0, 5);
                            defaultVisitTemplate.nEnd = defaultVisitTemplate.defaultVisitTimeNight.Substring(6, 5);
                            defaultVisitTemplate.nSubsection = defaultVisitTemplate.segmentalDuration;
                            defaultVisitTemplate.nScene = hyArr[0];
                            defaultVisitTemplate.nOpen = hyArr[1];
                            defaultVisitTemplate.nRoom = hyArr[2];
                            defaultVisitTemplate.nEmergency = hyArr[3];

                            defaultVisitTemplate.allStart = defaultVisitTemplate.defaultVisitTimeAllDay.Substring(0, 5);
                            defaultVisitTemplate.allEnd = defaultVisitTemplate.defaultVisitTimeAllDay.Substring(6, 5);
                            defaultVisitTemplate.allSubsection = defaultVisitTemplate.segmentalDuration;
                            defaultVisitTemplate.allScene = hyArr[0];
                            defaultVisitTemplate.allOpen = hyArr[1];
                            defaultVisitTemplate.allRoom = hyArr[2];
                            defaultVisitTemplate.allEmergency = hyArr[3];
                            
                            defaultVisit.mStart = defaultVisitTemplate.defaultVisitTimeAm.Substring(0, 5);
                            defaultVisit.mEnd = defaultVisitTemplate.defaultVisitTimeAm.Substring(6, 5);
                            defaultVisit.mSubsection = defaultVisitTemplate.segmentalDuration;
                            defaultVisit.mScene = hyArr[0];
                            defaultVisit.mOpen = hyArr[1];
                            defaultVisit.mRoom = hyArr[2];
                            defaultVisit.mEmergency = hyArr[3];

                            defaultVisit.aStart = defaultVisitTemplate.defaultVisitTimePm.Substring(0, 5);
                            defaultVisit.aEnd = defaultVisitTemplate.defaultVisitTimePm.Substring(6, 5);
                            defaultVisit.aSubsection = defaultVisitTemplate.segmentalDuration;
                            defaultVisit.aScene = hyArr[0];
                            defaultVisit.aOpen = hyArr[1];
                            defaultVisit.aRoom = hyArr[2];
                            defaultVisit.aEmergency = hyArr[3];

                            defaultVisit.nStart = defaultVisitTemplate.defaultVisitTimeNight.Substring(0, 5);
                            defaultVisit.nEnd = defaultVisitTemplate.defaultVisitTimeNight.Substring(6, 5);
                            defaultVisit.nSubsection = defaultVisitTemplate.segmentalDuration;
                            defaultVisit.nScene = hyArr[0];
                            defaultVisit.nOpen = hyArr[1];
                            defaultVisit.nRoom = hyArr[2];
                            defaultVisit.nEmergency = hyArr[3];

                            defaultVisit.allStart = defaultVisitTemplate.defaultVisitTimeAllDay.Substring(0, 5);
                            defaultVisit.allEnd = defaultVisitTemplate.defaultVisitTimeAllDay.Substring(6, 5);
                            defaultVisit.allSubsection = defaultVisitTemplate.segmentalDuration;
                            defaultVisit.allScene = hyArr[0];
                            defaultVisit.allOpen = hyArr[1];
                            defaultVisit.allRoom = hyArr[2];
                            defaultVisit.allEmergency = hyArr[3];
                            String startTop = "";
                            String endTop = "";
                            String subsectionTop = "";
                            String sceneTop = "";
                            String openTop = "";
                            String roomTop = "";

                            param = "deptId=" + scheduled.deptId + "&doctorId=" + scheduled.doctorId
                            + "&hospitalId=" + AppContext.Session.hospitalId + "&period=" + scheduled.period;
                            url = AppContext.AppConfig.serverUrl + "cms/doctor/findVisitingTime?" + param;
                            this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                            {
                                data = HttpClass.httpPost(url);
                                return data;

                            }, null, (data2) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                            {
                                objT = JObject.Parse(data.ToString());
                                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                                {
                                    List<WorkingDayEntity> workingDayList = objT["result"].ToObject<List<WorkingDayEntity>>();
                                    if (workingDayList.Count > 0)
                                    {
                                        startTop = workingDayList[0].beginTime;
                                        endTop = workingDayList[0].endTime;
                                        subsectionTop = workingDayList[0].segmentalDuration;
                                        sceneTop = workingDayList[0].numSite;
                                        openTop = workingDayList[0].numOpen;
                                        roomTop = workingDayList[0].numClinic;
                                    }
                                    else
                                    {
                                        if (scheduled.period.Equals("0"))
                                        {
                                            startTop = defaultVisit.mStart;
                                            endTop = defaultVisit.mEnd;
                                            subsectionTop = defaultVisit.mSubsection;
                                            sceneTop = defaultVisit.mScene;
                                            openTop = defaultVisit.mOpen;
                                            roomTop = defaultVisit.mRoom;
                                        }
                                        else if (scheduled.period.Equals("1"))
                                        {
                                            startTop = defaultVisit.aStart;
                                            endTop = defaultVisit.aEnd;
                                            subsectionTop = defaultVisit.aSubsection;
                                            sceneTop = defaultVisit.aScene;
                                            openTop = defaultVisit.aOpen;
                                            roomTop = defaultVisit.aRoom;
                                        }
                                        else if (scheduled.period.Equals("2"))
                                        {
                                            startTop = defaultVisit.nStart;
                                            endTop = defaultVisit.nEnd;
                                            subsectionTop = defaultVisit.nSubsection;
                                            sceneTop = defaultVisit.nScene;
                                            openTop = defaultVisit.nOpen;
                                            roomTop = defaultVisit.nRoom;
                                        }
                                        else if (scheduled.period.Equals("3"))
                                        {
                                            startTop = defaultVisit.allStart;
                                            endTop = defaultVisit.allEnd;
                                            subsectionTop = defaultVisit.allSubsection;
                                            sceneTop = defaultVisit.allScene;
                                            openTop = defaultVisit.allOpen;
                                            roomTop = defaultVisit.allRoom;
                                        }
                                    }
                                    teStart.EditValue = startTop;
                                    teEnd.EditValue = endTop;
                                    teSubsection.EditValue = subsectionTop;
                                    teScene.EditValue = sceneTop;
                                    teOpen.EditValue = openTop;
                                    teRoom.EditValue = roomTop;

                                    cmd.HideOpaqueLayer();
                                }
                            });
                        }
                        else
                        {
                            cmd.HideOpaqueLayer();
                            MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                            return;
                        }
                    });
                }
                else
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                    return;
                }
            });
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<SourceDataEntity> sourceDataList = gcSourceData.DataSource as List<SourceDataEntity>;
            String mzType = "1";
            if (cbTx.CheckState == CheckState.Checked)
            {
                mzType = "2";
            }
            foreach (SourceDataEntity sd in sourceDataList)
            {
                sd.mzType = mzType;
            }
            String scheduSets = Newtonsoft.Json.JsonConvert.SerializeObject(sourceDataList);
            String param = "hospitalId=" + AppContext.Session.hospitalId + "&deptId=" + scheduled.deptId
                + "&doctorId=" + scheduled.doctorId + "&workDate=" + scheduled.workDate
                + "&period=" + scheduled.period + "&scheduSets=" + scheduSets;
            String url = AppContext.AppConfig.serverUrl + "sch/doctorScheduPlan/addScheduList?" + param;
            cmd.ShowOpaqueLayer();
            this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    cmd.HideOpaqueLayer();
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                    return;
                }
            });
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
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
                    cmd.HideOpaqueLayer();
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

        private void gridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            //正整数验证 @"^[1-9]\d*$"
            var regex = new Regex(@"^[0-9]*[0-9][0-9]*$", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            if (!regex.IsMatch(e.Value.ToString()))
            {
                e.ErrorText = "只能输入正整数！";
                e.Valid = false;
                return;
            }
        }

        private void buttonControl2_Click(object sender, EventArgs e)
        {
            String start = teStart.Text;
            String end = teEnd.Text;
            String subsection = teSubsection.Text;
            String scene = teScene.Text;
            String open = teOpen.Text;
            String room = teRoom.Text;
            String emergency = "0";

            List<SourceDataEntity> sourceDataList = new List<SourceDataEntity>();

            if (start.Length == 0 && end.Length == 0 && subsection.Length == 0)
            {
                sourceDataList = gcSourceData.DataSource as List<SourceDataEntity>;
                foreach (SourceDataEntity sd in sourceDataList)
                {
                    sd.numSite = scene;
                    sd.numOpen = open;
                    sd.numClinic = room;
                    sd.numYj = emergency;
                }
                gcSourceData.DataSource = sourceDataList;
                gcSourceData.RefreshDataSource();
                return;
            }

            String sdName = "";
            if (scheduled.period.Equals("0")) sdName = "上午";
            if (scheduled.period.Equals("1")) sdName = "下午";
            if (scheduled.period.Equals("2")) sdName = "晚上";
            if (scheduled.period.Equals("3")) sdName = "全天";

            //分段数量
            int rowNum = 0;

            #region 计算分段数量
            if (start.Trim().Length == 0 || end.Trim().Length == 0
                || subsection.Trim().Length == 0)
            {
                cmd.HideOpaqueLayer();
                MessageBoxUtils.Hint(sdName + "的设置不能为空", this);
                return;
            }
            String[] startArr = start.Split(new char[] { ':', '：' });
            String[] endArr = end.Split(new char[] { ':', '：' });
            if (startArr.Length != 2)
            {
                cmd.HideOpaqueLayer();
                MessageBoxUtils.Hint(sdName + "的开始时间设置有误", this);
                return;
            }
            if (endArr.Length != 2)
            {
                cmd.HideOpaqueLayer();
                MessageBoxUtils.Hint(sdName + "的结束时间设置有误", this);
                return;
            }

            //计算早上的分段数量
            if (sdName.Equals("上午"))
            {

                DateTime d1 = new DateTime(2004, 1, 1, int.Parse(startArr[0]), int.Parse(startArr[1]), 00);
                DateTime d2 = new DateTime(2004, 1, 1, int.Parse(endArr[0]), int.Parse(endArr[1]), 00);
                TimeSpan d3 = d2.Subtract(d1);
                int minute = d3.Hours * 60 + d3.Minutes;
                if (minute <= 0)
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Hint("上午结束时间不能小于或等于开始时间", this);
                    return;
                }
                if (minute < int.Parse(subsection))
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Hint("上午分段时间大于总时间", this);
                    return;
                }
                if (minute % int.Parse(subsection) == 0)
                    rowNum = minute / int.Parse(subsection);
                else
                    rowNum = (minute / int.Parse(subsection)) + 1;
            }

            //计算下午的分段数量
            if (sdName.Equals("下午"))
            {
                DateTime d1 = new DateTime(2004, 1, 1, int.Parse(startArr[0]), int.Parse(startArr[1]), 00);
                DateTime d2 = new DateTime(2004, 1, 1, int.Parse(endArr[0]), int.Parse(endArr[1]), 00);
                TimeSpan d3 = d2.Subtract(d1);
                int minute = d3.Hours * 60 + d3.Minutes;
                if (minute <= 0)
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Hint("下午结束时间不能小于或等于开始时间", this);
                    return;
                }
                if (minute < int.Parse(subsection))
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Hint("下午分段时间大于总时间", this);
                    return;
                }
                if (minute % int.Parse(subsection) == 0)
                    rowNum = minute / int.Parse(subsection);
                else
                    rowNum = (minute / int.Parse(subsection)) + 1;
            }

            //计算晚上的分段数量
            if (sdName.Equals("晚上"))
            {
                DateTime d1 = new DateTime(2004, 1, 1, int.Parse(startArr[0]), int.Parse(startArr[1]), 00);
                DateTime d2 = new DateTime();
                if (endArr[0].Equals("24"))
                    d2 = new DateTime(2004, 1, 2, 00, int.Parse(endArr[1]), 00);
                if (int.Parse(endArr[0]) < int.Parse(startArr[0]))
                    d2 = new DateTime(2004, 1, 2, int.Parse(endArr[0]), int.Parse(endArr[1]), 00);
                else
                    d2 = new DateTime(2004, 1, 1, int.Parse(endArr[0]), int.Parse(endArr[1]), 00);
                TimeSpan d3 = d2.Subtract(d1);
                int minute = d3.Hours * 60 + d3.Minutes;
                if (minute <= 0)
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Hint("晚上结束时间不能小于或等于开始时间", this);
                    return;
                }
                if (minute < int.Parse(subsection))
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Hint("晚上分段时间大于总时间", this);
                    return;
                }
                if (minute % int.Parse(subsection) == 0)
                    rowNum = minute / int.Parse(subsection);
                else
                    rowNum = (minute / int.Parse(subsection)) + 1;
            }

            //计算全天的分段数量
            if (sdName.Equals("全天"))
            {
                DateTime d1 = new DateTime(2004, 1, 1, int.Parse(startArr[0]), int.Parse(startArr[1]), 00);
                DateTime d2 = new DateTime();
                if (endArr[0].Equals("24"))
                    d2 = new DateTime(2004, 1, 2, 00, int.Parse(endArr[1]), 00);
                else
                    d2 = new DateTime(2004, 1, 1, int.Parse(endArr[0]), int.Parse(endArr[1]), 00);
                TimeSpan d3 = d2.Subtract(d1);
                int minute = d3.Hours * 60 + d3.Minutes;
                if (minute <= 0)
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Hint("全天结束时间不能小于或等于开始时间", this);
                    return;
                }
                if (minute < int.Parse(subsection))
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Hint("全天分段时间大于总时间", this);
                    return;
                }
                if (minute % int.Parse(subsection) == 0)
                    rowNum = minute / int.Parse(subsection);
                else
                    rowNum = (minute / int.Parse(subsection)) + 1;
            }
            #endregion

            

            DateTime dt1 = new DateTime();//开始时间
            DateTime dt2 = new DateTime();//结束时间

            dt1 = DateTime.Parse("2008-08-08 " + start + ":00");
            dt2 = dt1.AddMinutes(int.Parse(subsection));

            for (int i = 0; i < rowNum; i++)
            {
                SourceDataEntity sourceData = new SourceDataEntity();
                start = dt1.ToString("HH:mm");
                if (i == rowNum-1)
                    end = teEnd.Text;
                else
                    end = dt2.ToString("HH:mm");
                dt1 = dt2;
                dt2 = dt1.AddMinutes(int.Parse(subsection));
                sourceData.beginTime = start;
                sourceData.endTime = end;
                sourceData.numSite = scene;
                sourceData.numOpen = open;
                sourceData.numClinic = room;
                sourceData.numYj = emergency;
                sourceDataList.Add(sourceData);
            }
            gcSourceData.DataSource = sourceDataList;
        }

        /// <summary>
        /// 限制时间
        /// </summary>
        /// <param name="sender">时间控件</param>
        /// <param name="wb">午别 0:上午 1：下午 2：晚上</param>
        /// <param name="flag">true：开始时间 false：结束时间</param>
        private void ifTime(object sender, String wb, Boolean flag)
        {
            TimeEdit te = (TimeEdit)sender;
            if (te.Text.Length > 0)
            {
                if (wb.Equals("0"))
                {
                    if (te.Text.CompareTo(defaultVisitTemplate.mStart) == -1)
                    {
                        te.EditValue = defaultVisitTemplate.mStart;
                    }
                    if (te.Text.CompareTo(defaultVisitTemplate.mEnd) == 1)
                    {
                        te.EditValue = defaultVisitTemplate.mEnd;
                    }
                }
                else if (wb.Equals("1"))
                {
                    if (te.Text.CompareTo(defaultVisitTemplate.aStart) == -1)
                    {
                        te.EditValue = defaultVisitTemplate.aStart;
                    }
                    if (te.Text.CompareTo(defaultVisitTemplate.aEnd) == 1)
                    {
                        te.EditValue = defaultVisitTemplate.aEnd;
                    }
                }
                else if (wb.Equals("2"))
                {
                    String text = "";
                    if (te.Text.CompareTo("12:00") == -1)
                    {
                        text = "2019/07/30 " + te.Text;
                    }
                    else
                    {
                        text = "2019/07/29 " + te.Text;
                    }
                    String nStart = "2019/07/29 " + defaultVisitTemplate.nStart;
                    String nEnd = "2019/07/30 " + defaultVisitTemplate.nEnd;
                    if (text.CompareTo(nStart) == -1)
                    {
                        te.EditValue = defaultVisitTemplate.nStart;
                    }
                    if (text.CompareTo(nEnd) == 1)
                    {
                        te.EditValue = defaultVisitTemplate.nEnd;
                    }
                }
            }
        }

        private void teStart_EditValueChanged(object sender, EventArgs e)
        {
            ifTime(sender, scheduled.period, true);
        }

        private void teEnd_EditValueChanged(object sender, EventArgs e)
        {
            ifTime(sender, scheduled.period, false);
        }

    }
}
