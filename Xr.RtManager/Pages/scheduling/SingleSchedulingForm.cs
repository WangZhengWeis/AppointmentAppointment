using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Xr.Http;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using Xr.Common;
using Xr.Common.Controls;
using DevExpress.XtraEditors;
using System.Threading;
using System.Text.RegularExpressions;

namespace Xr.RtManager.Pages.scheduling
{
    public partial class SingleSchedulingForm : UserControl
    {
        public SingleSchedulingForm()
        {
            InitializeComponent();
        }

        private Form MainForm; //主窗体
        Xr.Common.Controls.OpaqueCommand cmd;
        private bool ifQuery = true;//用于控制保存成功后清除时段不出发查询事件
        public DefaultVisitEntity defaultVisit { get; set; }
        /// <summary>
        /// 出诊信息模板
        /// </summary>
        public DefaultVisitEntity defaultVisitTemplate { get; set; }

        private void SingleSchedulingForm_Load(object sender, EventArgs e)
        {
            MainForm = (Form)this.Parent;
            dateEdit1.Properties.MinValue = DateTime.Now;
            dateEdit1.Properties.MaxValue = DateTime.Now.AddDays(90);

            cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            cmd.ShowOpaqueLayer(0f);
            //获取可操作科室
            String url = AppContext.AppConfig.serverUrl + "cms/dept/qureyOperateDept";
            this.DoWorkAsync(0, (o) =>
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) =>
            {
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    List<DeptEntity> deptList = objT["result"]["deptList"].ToObject<List<DeptEntity>>();
                    //List<DeptEntity> deptList = AppContext.Session.deptList;
                    treeMenuControl1.KeyFieldName = "id";
                    treeMenuControl1.ParentFieldName = "parentId";
                    treeMenuControl1.DisplayMember = "name";
                    treeMenuControl1.ValueMember = "id";
                    treeMenuControl1.DataSource = deptList;
                    if (deptList.Count > 0)
                    {
                        treeMenuControl1.EditValue = deptList[0].id;
                        treeMenuControl1_MenuItemClick(null, null, deptList[0]);
                    }

                    //查询状态下拉框数据
                    url = AppContext.AppConfig.serverUrl + "sys/sysDict/findByType?type=is_use";
                    this.DoWorkAsync( 0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                    {
                        data = HttpClass.httpPost(url);
                        return data;

                    }, null, (data2) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                    {
                        objT = JObject.Parse(data2.ToString());
                        if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                        {
                            List<DictEntity> dictList = objT["result"].ToObject<List<DictEntity>>();
                            lueIsUse.Properties.DataSource = dictList;
                            lueIsUse.Properties.DisplayMember = "label";
                            lueIsUse.Properties.ValueMember = "value";
                            lueIsUse.EditValue = dictList[0].value;

                            //获取默认出诊时间字典配置
                            url = AppContext.AppConfig.serverUrl + "cms/doctor/findDoctorVisitingDict";
                            this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                            {
                                data = HttpClass.httpPost(url);
                                return data;

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
                                    cmd.HideOpaqueLayer();
                                }
                                else
                                {
                                    cmd.HideOpaqueLayer();
                                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                                    return;
                                }
                            });
                        }
                        else
                        {
                            cmd.HideOpaqueLayer();
                            MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                            return;
                        }
                    });
                }
                else
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    return;
                }
            });
        }

        private void treeMenuControl1_MenuItemClick(object sender, EventArgs e, object selectItem)
        {
            DeptEntity dept = selectItem as DeptEntity;
            //String param = "pageNo=1&pageSize=10000&hospital.id=" + dept.hospitalId + "&dept.id=" + dept.id;
            String param = "hospital.id=" + dept.hospitalId + "&dept.id=" + dept.id;
            String url = AppContext.AppConfig.serverUrl + "cms/doctor/findAll?" + param;
            cmd.ShowOpaqueLayer();
            this.DoWorkAsync(500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    List<DoctorInfoEntity> doctorList = objT["result"].ToObject<List<DoctorInfoEntity>>();
                    //List<DoctorInfoEntity> doctorList = objT["result"]["list"].ToObject<List<DoctorInfoEntity>>();
                    //设置医生列表
                    List<Item> itemList = new List<Item>();
                    foreach (DoctorInfoEntity doctor in doctorList)
                    {
                        Item item = new Item();
                        item.name = doctor.name;
                        item.value = doctor.id;
                        item.tag = doctor.code;
                        itemList.Add(item);
                    }
                    mcDoctor.setDataSource(itemList);
                    cmd.HideOpaqueLayer();
                }
                else
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    return;
                }
            });
        }

        private String workDate;

        private void btnQuery_Click(object sender, EventArgs e)
        {
            setScheduling();
        }

        private void setScheduling()
        {
            //清除排班数据
            panel9.Controls.Clear();

            if (ifQuery)
            {
                if (dateEdit1.Text == null || dateEdit1.Text.ToString().Length == 0)
                {
                    dataController1.ShowError(dateEdit1, "日期不能为空");
                    return;
                }

                workDate = dateEdit1.Text;

                CheckState morning = cbMorning.CheckState;
                CheckState afternoon = cbAfternoon.CheckState;
                CheckState night = cbNight.CheckState;
                CheckState allDay = cbAllAay.CheckState;

                List<String> periodList = new List<String>();
                if (morning != CheckState.Checked && afternoon != CheckState.Checked
                    && night != CheckState.Checked && allDay != CheckState.Checked)
                {
                    dataController1.ShowError(cbAllAay, "至少选一个");
                    return;
                }
                String param = null;
                String url = null;
                String data = null;
                JObject objT = null;
                cmd.ShowOpaqueLayer();
                this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                {
                    if (allDay == CheckState.Checked)
                    {
                        //全天
                        periodList.Clear();
                        periodList.Add("0");
                        periodList.Add("1");
                        periodList.Add("2");
                        periodList.Add("3");
                    }else{
                        if (morning == CheckState.Checked) periodList.Add("0");
                        if (afternoon == CheckState.Checked) periodList.Add("1");
                        if (night == CheckState.Checked) periodList.Add("2");
                        periodList.Add("3");
                    }
                    String ts = "";
                    for (int i = 0; i < periodList.Count; i++)
                    {
                        param = "deptId=" + treeMenuControl1.EditValue + "&doctorId=" + mcDoctor.itemName
                            + "&hospitalId=" + AppContext.Session.hospitalId + "&workDate=" + workDate
                            + "&period=" + periodList[i];
                        url = AppContext.AppConfig.serverUrl + "sch/doctorScheduPlan/isExist?" + param;
                        data = HttpClass.httpPost(url);
                        objT = JObject.Parse(data);
                        if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                        {
                            if (string.Compare(objT["result"].ToString(), "true", true) != 0)
                            {
                                String sd = "";
                                if (periodList[i] == "0") sd = "上午";
                                else if (periodList[i] == "1") sd = "下午";
                                else if (periodList[i] == "2") sd = "晚上";
                                else if (periodList[i] == "3") sd = "全天";
                                ts += sd + ",";
                            }
                        }
                        else
                        {
                            return "2|" + objT["message"].ToString();
                        }
                    }
                    if (ts.Length > 0)
                    {
                        ts = ts.Substring(0, ts.Length-1);
                        return "1|该日期" + ts + "已有排班，请先在【排班列表中停诊】";
                    }
                    return "0|0";

                }, null, (data2) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                {
                    string[] sArray = data2.ToString().Split('|');
                    if (sArray[0] == "1")
                    {
                        labMsg.Text = sArray[1];
                        cmd.HideOpaqueLayer();
                        return;
                    }
                    else if (sArray[0] == "2")
                    {
                        labMsg.Text = "";
                        cmd.HideOpaqueLayer();
                        MessageBoxUtils.Show(sArray[1], MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                        return;
                    }
                    else
                    {
                        labMsg.Text = "";
                    }

                    periodList.Clear();
                    if (morning == CheckState.Checked) periodList.Add("0");
                    if (afternoon == CheckState.Checked) periodList.Add("1");
                    if (night == CheckState.Checked) periodList.Add("2");
                    if (allDay == CheckState.Checked)  periodList.Add("3");

                    List<WorkingDayEntity> wdList = new List<WorkingDayEntity>();//用于存默认出诊时间
                    List<List<WorkingDayEntity>> sList = new List<List<WorkingDayEntity>>();
                    this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                    {
                        String period = "";
                        for (int i = 0; i < periodList.Count; i++)
                        {
                            period += periodList[i] + ",";
                        }
                        if (period.Length > 0)
                            period = period.Substring(0, period.Length-1);

                         //获取默认出诊设置
                        param = "deptId=" + treeMenuControl1.EditValue + "&doctorId=" + mcDoctor.itemName
                            + "&hospitalId=" + AppContext.Session.hospitalId + "&period=" + period;
                        url = AppContext.AppConfig.serverUrl + "cms/doctor/findVisitingTime?"+param;
                        data = HttpClass.httpPost(url);
                        objT = JObject.Parse(data);
                        if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                        {
                            wdList = objT["result"].ToObject<List<WorkingDayEntity>>();

                            //for (int i = periodList.Count - 1; i >= 0; i--)
                            for (int i = 0; i < periodList.Count; i++)
                            {
                                param = "deptId=" + treeMenuControl1.EditValue + "&doctorId=" + mcDoctor.itemName
                                    + "&workDate=" + workDate + "&period=" + periodList[i];
                                url = AppContext.AppConfig.serverUrl + "cms/doctorVisitingTime/findByPropertys?" + param;
                                data = HttpClass.httpPost(url);
                                objT = JObject.Parse(data);
                                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                                {
                                    List<WorkingDayEntity> workingDayList = objT["result"].ToObject<List<WorkingDayEntity>>();
                                    sList.Add(workingDayList);
                                    //setWorkingDay(workingDayList, periodList[i]);
                                }
                                else
                                {
                                    //MessageBox.Show(objT["message"].ToString());
                                    return "2|" + objT["message"].ToString();
                                }
                            }
                        }
                        else
                        {
                            //MessageBox.Show(objT["message"].ToString());
                            return "2|" + objT["message"].ToString();
                        }
                        return "0|0";

                    }, null, (data3) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                    {
                        string[] sArray2 = data3.ToString().Split('|');
                        if (sArray2[0] == "2")
                        {
                            cmd.HideOpaqueLayer();
                            MessageBoxUtils.Show(sArray[1], MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                            return;
                        }
                        for (int i = sList.Count() - 1; i >= 0; i--)
                        {
                            if(wdList.Count>0)
                                setWorkingDay(sList[i], periodList[i], wdList[i]);
                            else
                                setWorkingDay(sList[i], periodList[i], null);
                        }
                        for (int i = 0; i < panel9.Controls.Count; i++)
                        {
                            Panel panel = (Panel)panel9.Controls[i];
                            GroupBorderPanel gb = (GroupBorderPanel)panel.Controls[0];
                            if (gb.Height < panel.Height) gb.Height = panel.Height;
                        }
                        cmd.HideOpaqueLayer();
                    });
                });
            }
        }

        /// <summary>
        /// 生成排班
        /// </summary>
        /// <param name="workingDayList"></param>
        /// <param name="period"></param>
        private void setWorkingDay(List<WorkingDayEntity> workingDayList, String period, WorkingDayEntity wd)
        {
            #region 生成排班
            Panel panelPb = new Panel();
            panelPb.Width = 400;
            panelPb.Dock = DockStyle.Left;
            panelPb.AutoScroll = true;
            GroupBorderPanel groupBox = new GroupBorderPanel();
            groupBox.BorderWidth = 1;
            
            groupBox.AutoSize = true;
            groupBox.Font = new Font("微软雅黑", 10);

            Label labelTips = new Label(); //没有默认出诊时间的提示内容

            TableLayoutPanel tlpTop = new TableLayoutPanel();
            tlpTop.ColumnCount = 7;
            tlpTop.RowCount = 2;
            tlpTop.Size = new System.Drawing.Size(370, 55);

            tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));

            Label label = new Label();
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.BottomCenter;
            label.Font = new Font("微软雅黑", 10);
            label.Text = "开始";
            tlpTop.Controls.Add(label, 0, 0);
            label = new Label();
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.BottomCenter;
            label.Font = new Font("微软雅黑", 10);
            label.Text = "结束";
            tlpTop.Controls.Add(label, 1, 0);
            label = new Label();
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.BottomCenter;
            label.Font = new Font("微软雅黑", 10);
            label.Text = "分段";
            tlpTop.Controls.Add(label, 2, 0);
            label = new Label();
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.BottomCenter;
            label.Font = new Font("微软雅黑", 10);
            label.Text = "现场";
            tlpTop.Controls.Add(label, 3, 0);
            label = new Label();
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.BottomCenter;
            label.Font = new Font("微软雅黑", 10);
            label.Text = "预约";
            tlpTop.Controls.Add(label, 4, 0);
            label = new Label();
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.BottomCenter;
            label.Font = new Font("微软雅黑", 10);
            label.Text = "诊间";
            tlpTop.Controls.Add(label, 5, 0);

            String startTop = "";
            String endTop = "";
            String subsectionTop = "";
            String sceneTop = "";
            String openTop = "";
            String roomTop = "";

            if (wd != null)
            {
                startTop = wd.beginTime;
                endTop = wd.endTime;
                subsectionTop = wd.segmentalDuration;
                sceneTop = wd.numSite;
                openTop = wd.numOpen;
                roomTop = wd.numClinic;
            }
            else
            {
                if (period.Equals("0"))
                {
                    startTop = defaultVisit.mStart;
                    endTop = defaultVisit.mEnd;
                    subsectionTop = defaultVisit.mSubsection;
                    sceneTop = defaultVisit.mScene;
                    openTop = defaultVisit.mOpen;
                    roomTop = defaultVisit.mRoom;
                }
                else if (period.Equals("1"))
                {
                    startTop = defaultVisit.aStart;
                    endTop = defaultVisit.aEnd;
                    subsectionTop = defaultVisit.aSubsection;
                    sceneTop = defaultVisit.aScene;
                    openTop = defaultVisit.aOpen;
                    roomTop = defaultVisit.aRoom;
                }
                else if (period.Equals("2"))
                {
                    startTop = defaultVisit.nStart;
                    endTop = defaultVisit.nEnd;
                    subsectionTop = defaultVisit.nSubsection;
                    sceneTop = defaultVisit.nScene;
                    openTop = defaultVisit.nOpen;
                    roomTop = defaultVisit.nRoom;
                }
                else if (period.Equals("3"))
                {
                    startTop = defaultVisit.allStart;
                    endTop = defaultVisit.allEnd;
                    subsectionTop = defaultVisit.allSubsection;
                    sceneTop = defaultVisit.allScene;
                    openTop = defaultVisit.allOpen;
                    roomTop = defaultVisit.allRoom;
                }
            }

            TimeEdit timeEditTop = new TimeEdit();
            timeEditTop.Properties.AutoHeight = false;
            timeEditTop.Dock = DockStyle.Fill;
            timeEditTop.Font = new Font("微软雅黑", 10);
            timeEditTop.Properties.DisplayFormat.FormatString = "HH:mm";
            timeEditTop.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            timeEditTop.Properties.EditFormat.FormatString = "HH:mm";
            timeEditTop.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            timeEditTop.Properties.EditMask = "HH:mm";
            timeEditTop.Properties.Mask.EditMask = "HH:mm";
            timeEditTop.EditValue = startTop;
            if (period.Equals("0"))
            {
                timeEditTop.EditValueChanged += new EventHandler(SwEditValueChanged);
            }
            else if (period.Equals("1"))
            {
                timeEditTop.EditValueChanged += new EventHandler(XwEditValueChanged);
            }
            else if (period.Equals("2"))
            {
                timeEditTop.EditValueChanged += new EventHandler(WsBeginEditValueChanged);
            }
            //timeEdit.Enabled = teEnabled;
            //timeEdit.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            tlpTop.Controls.Add(timeEditTop, 0, 1);

            //TextEdit textEditTop = new TextEdit();
            //textEditTop.Properties.AutoHeight = false;
            //textEditTop.Dock = DockStyle.Fill;
            //textEditTop.Font = new Font("微软雅黑", 10);
            //textEditTop.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //tlpTop.Controls.Add(textEditTop, 0, 1);

            timeEditTop = new TimeEdit();
            timeEditTop.Properties.AutoHeight = false;
            timeEditTop.Dock = DockStyle.Fill;
            timeEditTop.Font = new Font("微软雅黑", 10);
            timeEditTop.Properties.DisplayFormat.FormatString = "HH:mm";
            timeEditTop.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            timeEditTop.Properties.EditFormat.FormatString = "HH:mm";
            timeEditTop.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            timeEditTop.Properties.EditMask = "HH:mm";
            timeEditTop.Properties.Mask.EditMask = "HH:mm";
            timeEditTop.EditValue = endTop;
            if (period.Equals("0"))
            {
                timeEditTop.EditValueChanged += new EventHandler(SwEditValueChanged);
            }
            else if (period.Equals("1"))
            {
                timeEditTop.EditValueChanged += new EventHandler(XwEditValueChanged);
            }
            else if (period.Equals("2"))
            {
                timeEditTop.EditValueChanged += new EventHandler(WsEndEditValueChanged);
            }
            //timeEdit.Enabled = teEnabled;
            //timeEdit.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            tlpTop.Controls.Add(timeEditTop, 1, 1);

            //textEditTop = new TextEdit();
            //textEditTop.Properties.AutoHeight = false;
            //textEditTop.Dock = DockStyle.Fill;
            //textEditTop.Font = new Font("微软雅黑", 10);
            //textEditTop.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //tlpTop.Controls.Add(textEditTop, 1, 1);

            TextEdit textEditTop = new TextEdit();
            textEditTop.Properties.AutoHeight = false;
            textEditTop.Dock = DockStyle.Fill;
            textEditTop.Font = new Font("微软雅黑", 10);
            textEditTop.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            textEditTop.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            textEditTop.Properties.Mask.UseMaskAsDisplayFormat = true;
            textEditTop.Properties.Mask.EditMask = "[0-9]*";
            textEditTop.EditValue = subsectionTop;
            tlpTop.Controls.Add(textEditTop, 2, 1);

            textEditTop = new TextEdit();
            textEditTop.Properties.AutoHeight = false;
            textEditTop.Dock = DockStyle.Fill;
            textEditTop.Font = new Font("微软雅黑", 10);
            textEditTop.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            textEditTop.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            textEditTop.Properties.Mask.UseMaskAsDisplayFormat = true;
            textEditTop.Properties.Mask.EditMask = "[0-9]*";
            textEditTop.EditValue = sceneTop;
            tlpTop.Controls.Add(textEditTop, 3, 1);

            textEditTop = new TextEdit();
            textEditTop.Properties.AutoHeight = false;
            textEditTop.Dock = DockStyle.Fill;
            textEditTop.Font = new Font("微软雅黑", 10);
            textEditTop.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            textEditTop.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            textEditTop.Properties.Mask.UseMaskAsDisplayFormat = true;
            textEditTop.Properties.Mask.EditMask = "[0-9]*";
            textEditTop.EditValue = openTop;
            tlpTop.Controls.Add(textEditTop, 4, 1);

            textEditTop = new TextEdit();
            textEditTop.Properties.AutoHeight = false;
            textEditTop.Dock = DockStyle.Fill;
            textEditTop.Font = new Font("微软雅黑", 10);
            textEditTop.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            textEditTop.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            textEditTop.Properties.Mask.UseMaskAsDisplayFormat = true;
            textEditTop.Properties.Mask.EditMask = "[0-9]*";
            textEditTop.EditValue = roomTop;
            tlpTop.Controls.Add(textEditTop, 5, 1);

            ButtonControl bc = new ButtonControl();
            bc.Text = "更新";
            bc.Dock = DockStyle.Fill;
            bc.Font = new Font("微软雅黑", 10);
            bc.Style = ButtonStyle.Save;
            bc.Click += new EventHandler(bc_Click); 
            tlpTop.SetRowSpan(bc, 2);
            tlpTop.Controls.Add(bc, 6, 0);


            //当前TableLayoutPanel的数量
            List<WorkingDayEntity> wdwpList = getWorkingDayData(workingDayList, period);
            //多少行数据
            int rowNum = wdwpList.Count;
            //period=0:上午 period=1:下午 period=2:晚上 period=3:全天
            TableLayoutPanel tlpMorning = new TableLayoutPanel();
            if (rowNum == 0) tlpMorning.Enabled = false;
            else labelTips.Visible = false;
            int row = 0;//行数(包括标题)
            String timeInterval = ""; //
            if (rowNum > 3) row = rowNum + 1;
            else row = 4;

            //CheckState checkState = CheckState.Unchecked;
            //CheckState checkAuto = CheckState.Unchecked;
            //if (rowNum == 0)
            //    checkState = CheckState.Unchecked;
            //else if (wdwpList[0].isUse.Equals("0"))
            //    checkState = CheckState.Checked;
            //else
            //    checkState = CheckState.Unchecked;
            ////if (rowNum == 0)
            //    checkAuto = CheckState.Unchecked;
            //else if (wdwpList[0].autoSchedule.Equals("0"))
            //    checkAuto = CheckState.Checked;
            //else
            //    checkAuto = CheckState.Unchecked;
            if (period.Equals("0"))
            {
                timeInterval = "上午";
            }
            else if (period.Equals("1"))
            {
                timeInterval = "下午";
            }
            else if (period.Equals("2"))
            {
                timeInterval = "晚上";
            }
            else if (period.Equals("3"))
            {
                timeInterval = "全天";
            }

            groupBox.GroupText = timeInterval;

            tlpMorning.ColumnCount = 7;
            tlpMorning.RowCount = row;

            tlpMorning.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            tlpMorning.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            tlpMorning.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            tlpMorning.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            tlpMorning.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            tlpMorning.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            tlpMorning.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));

            for (int n = 0; n < row; n++)
            {
                tlpMorning.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            }
            tlpMorning.Size = new System.Drawing.Size(327, row * 30);
            //标题栏
            //Panel panel = new Panel();
            //panel.Dock = DockStyle.Fill;
            //tlpMorning.Controls.Add(panel);
            label = new Label();
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.BottomCenter;
            label.Font = new Font("微软雅黑", 10);
            label.Text = "开始";
            tlpMorning.Controls.Add(label, 1, 0);
            label = new Label();
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.BottomCenter;
            label.Font = new Font("微软雅黑", 10);
            label.Text = "结束";
            tlpMorning.Controls.Add(label, 2, 0);
            label = new Label();
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.BottomCenter;
            label.Font = new Font("微软雅黑", 10);
            label.Text = "现场";
            tlpMorning.Controls.Add(label, 3, 0);
            label = new Label();
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.BottomCenter;
            label.Font = new Font("微软雅黑", 10);
            label.Text = "预约";
            tlpMorning.Controls.Add(label, 4, 0);
            label = new Label();
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.BottomCenter;
            label.Font = new Font("微软雅黑", 10);
            label.Text = "诊间";
            tlpMorning.Controls.Add(label, 5, 0);
            label = new Label();
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.BottomCenter;
            label.Font = new Font("微软雅黑", 10);
            label.Text = "应急";
            label.Visible = false;
            tlpMorning.Controls.Add(label, 6, 0);

            bool teEnabled = true;//当行数小于3的时候，空白文本框需要设为不可选
            String start = "";
            String end = "";
            String scene = "";
            String open = "";
            String room = "";
            String emergency = "";

            for (int r = 1; r < row; r++)
            {
                if (r > rowNum)
                {
                    start = "";
                    end = "";
                    scene = "";
                    open = "";
                    room = "";
                    emergency = "";
                    teEnabled = false;
                }
                else
                {
                    start = wdwpList[r - 1].beginTime;
                    end = wdwpList[r - 1].endTime;
                    scene = wdwpList[r - 1].numSite;
                    open = wdwpList[r - 1].numOpen;
                    room = wdwpList[r - 1].numClinic;
                    emergency = wdwpList[r - 1].numYj;
                    teEnabled = true;
                }
                for (int c = 0; c < 7; c++)
                {
                    if (r == 1 && c == 0)
                    {
                        //第一行第一列
                        CheckBox checkBox = new CheckBox();
                        checkBox.Dock = DockStyle.Fill;
                        checkBox.Font = new Font("微软雅黑", 10);
                        //checkBox.Text = timeInterval;
                        checkBox.Text = "特需";
                        tlpMorning.Controls.Add(checkBox, 0, 1);
                    }
                    //else if (r == 2 && c == 0)
                    //{
                    //    //第二行第一列
                    //    //需跨1行
                    //    CheckBox checkBox = new CheckBox();
                    //    checkBox.Dock = DockStyle.Fill;
                    //    checkBox.Font = new Font("微软雅黑", 10);
                    //    checkBox.ForeColor = Color.FromArgb(255, 153, 102);
                    //    checkBox.Text = "自动排班";
                    //    checkBox.CheckState = checkAuto;
                    //    tlpMorning.SetRowSpan(checkBox, 2);
                    //    tlpMorning.Controls.Add(checkBox, c, r);
                    //}
                    else if (c == 0)
                    {
                        //不做处理
                    }
                    else
                    {
                        if (c == 1)
                        {
                            //第二列
                            TimeEdit timeEdit = new TimeEdit();
                            timeEdit.Properties.AutoHeight = false;
                            timeEdit.Dock = DockStyle.Fill;
                            timeEdit.Font = new Font("微软雅黑", 10);
                            timeEdit.Properties.DisplayFormat.FormatString = "HH:mm";
                            timeEdit.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                            timeEdit.Properties.EditFormat.FormatString = "HH:mm";
                            timeEdit.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                            timeEdit.Properties.EditMask = "HH:mm";
                            timeEdit.Properties.Mask.EditMask = "HH:mm";
                            timeEdit.EditValue = start;
                            timeEdit.Enabled = false;
                            tlpMorning.Controls.Add(timeEdit, c, r);

                            //TextEdit textEdit = new TextEdit();
                            //textEdit.Properties.AutoHeight = false;
                            //textEdit.Dock = DockStyle.Fill;
                            //textEdit.Font = new Font("微软雅黑", 10);
                            //textEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                            //textEdit.Text = start;
                            //textEdit.Enabled = teEnabled;
                            //tlpMorning.Controls.Add(textEdit, c, r);
                        }
                        else if (c == 2)
                        {
                            //第三列
                            TimeEdit timeEdit = new TimeEdit();
                            timeEdit.Properties.AutoHeight = false;
                            timeEdit.Dock = DockStyle.Fill;
                            timeEdit.Font = new Font("微软雅黑", 10);
                            timeEdit.Properties.DisplayFormat.FormatString = "HH:mm";
                            timeEdit.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                            timeEdit.Properties.EditFormat.FormatString = "HH:mm";
                            timeEdit.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                            timeEdit.Properties.EditMask = "HH:mm";
                            timeEdit.Properties.Mask.EditMask = "HH:mm";
                            timeEdit.EditValue = end;
                            timeEdit.Enabled = false;
                            tlpMorning.Controls.Add(timeEdit, c, r);

                            //TextEdit textEdit = new TextEdit();
                            //textEdit.Properties.AutoHeight = false;
                            //textEdit.Dock = DockStyle.Fill;
                            //textEdit.Font = new Font("微软雅黑", 10);
                            //textEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                            //textEdit.Text = end;
                            //textEdit.Enabled = teEnabled;
                            //tlpMorning.Controls.Add(textEdit, c, r);
                        }
                        else if (c == 3)
                        {
                            //第四列 现场
                            TextEdit textEdit = new TextEdit();
                            textEdit.Properties.AutoHeight = false;
                            textEdit.Dock = DockStyle.Fill;
                            textEdit.Font = new Font("微软雅黑", 10);
                            textEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                            textEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
                            textEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
                            textEdit.Properties.Mask.EditMask = "[0-9]*";
                            textEdit.Text = scene;
                            textEdit.Enabled = teEnabled;
                            tlpMorning.Controls.Add(textEdit, c, r);
                        }
                        else if (c == 4)
                        {
                            //第五列 预约
                            TextEdit textEdit = new TextEdit();
                            textEdit.Properties.AutoHeight = false;
                            textEdit.Dock = DockStyle.Fill;
                            textEdit.Font = new Font("微软雅黑", 10);
                            textEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                            textEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
                            textEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
                            textEdit.Properties.Mask.EditMask = "[0-9]*";
                            textEdit.Text = open;
                            textEdit.Enabled = teEnabled;
                            tlpMorning.Controls.Add(textEdit, c, r);
                        }
                        else if (c == 5)
                        {
                            //第六列 诊间
                            TextEdit textEdit = new TextEdit();
                            textEdit.Properties.AutoHeight = false;
                            textEdit.Dock = DockStyle.Fill;
                            textEdit.Font = new Font("微软雅黑", 10);
                            textEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                            textEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
                            textEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
                            textEdit.Properties.Mask.EditMask = "[0-9]*";
                            textEdit.Text = room;
                            textEdit.Enabled = teEnabled;
                            tlpMorning.Controls.Add(textEdit, c, r);
                        }
                        else if (c == 6)
                        {
                            //第七列 应急
                            TextEdit textEdit = new TextEdit();
                            textEdit.Properties.AutoHeight = false;
                            textEdit.Dock = DockStyle.Fill;
                            textEdit.Font = new Font("微软雅黑", 10);
                            textEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                            textEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
                            textEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
                            textEdit.Properties.Mask.EditMask = "[0-9]*";
                            textEdit.Text = emergency;
                            textEdit.Enabled = teEnabled;
                            textEdit.Visible = false; 
                            tlpMorning.Controls.Add(textEdit, c, r);
                        }
                    }
                }
            }
            
            labelTips.Location = new System.Drawing.Point(10, 20);
            labelTips.Text = "没有设置默认出诊时间的，请到医生设置里面设置";
            labelTips.ForeColor = Color.Red;
            labelTips.AutoSize = true;
            groupBox.Controls.Add(labelTips);
            if (row - 1 > rowNum)
            {
                tlpTop.Enabled = false;
            }
            tlpTop.Location = new Point(5, 40);
            groupBox.Controls.Add(tlpTop);
            tlpMorning.Location = new System.Drawing.Point(5, 95);
            groupBox.Controls.Add(tlpMorning);
            panelPb.Controls.Add(groupBox);
            panel9.Controls.Add(panelPb);
            #endregion
        }

        /// <summary>
        /// 获取周几上午||下午||晚上的排班数据
        /// </summary>
        /// <param name="workingDayList">排班数据</param>
        /// <param name="period">0：上午，1：下午，2：晚上</param>
        /// <returns></returns>
        private List<WorkingDayEntity> getWorkingDayData(List<WorkingDayEntity> workingDayList, String period)
        {
            List<WorkingDayEntity> workingDayByWeekList = new List<WorkingDayEntity>();
            foreach (WorkingDayEntity workingDay in workingDayList)
            {
                if (workingDay.period.Equals(period))
                    workingDayByWeekList.Add(workingDay);
            }
            return workingDayByWeekList;
        }

        private void mcDoctor_MenuItemClick(object sender, EventArgs e)
        {
            panel8.Enabled = true;
            if(dateEdit1.EditValue!=null &&(cbMorning.CheckState==CheckState.Checked || 
                cbAfternoon.CheckState==CheckState.Checked || cbNight.CheckState==CheckState.Checked ||
                cbAllAay.CheckState == CheckState.Checked))
            {
                setScheduling();
            }
        }

        protected void bc_Click(object sender, EventArgs e)
        {
            ButtonControl bc = (ButtonControl)sender;
            TableLayoutPanel tlpTop = (TableLayoutPanel)bc.Parent;
            GroupBorderPanel gbp = (GroupBorderPanel)tlpTop.Parent;
            TableLayoutPanel tlp = (TableLayoutPanel)gbp.Controls[2];//排班
            CheckState checkState = CheckState.Unchecked;
            checkState = ((CheckBox)tlp.GetControlFromPosition(0, 1)).CheckState;
            

            TableLayoutPanel newTlp = new TableLayoutPanel();

            String start = tlpTop.GetControlFromPosition(0, 1).Text;
            String end = tlpTop.GetControlFromPosition(1, 1).Text;
            String subsection = tlpTop.GetControlFromPosition(2, 1).Text;
            String scene = tlpTop.GetControlFromPosition(3, 1).Text;
            String open = tlpTop.GetControlFromPosition(4, 1).Text;
            String room = tlpTop.GetControlFromPosition(5, 1).Text;
            String emergency = "0";

            //分段数量
            int rowNum = 0;

            #region 计算分段数量

            if (start.Trim().Length == 0 || end.Trim().Length == 0
                || subsection.Trim().Length == 0)
            {
                cmd.HideOpaqueLayer();
                MessageBoxUtils.Hint(gbp.GroupText + "的设置不能为空", HintMessageBoxIcon.Error, MainForm);
                return;
            }
            String[] startArr = start.Split(new char[] { ':', '：' });
            String[] endArr = end.Split(new char[] { ':', '：' });
            if (startArr.Length != 2)
            {
                cmd.HideOpaqueLayer();
                MessageBoxUtils.Hint(gbp.GroupText + "的开始时间设置有误", HintMessageBoxIcon.Error, MainForm);
                return;
            }
            if (endArr.Length != 2)
            {
                cmd.HideOpaqueLayer();
                MessageBoxUtils.Hint(gbp.GroupText + "的结束时间设置有误", HintMessageBoxIcon.Error, MainForm);
                return;
            }

            //计算早上的分段数量
            if (gbp.GroupText.Equals("上午"))
            {

                DateTime d1 = new DateTime(2004, 1, 1, int.Parse(startArr[0]), int.Parse(startArr[1]), 00);
                DateTime d2 = new DateTime(2004, 1, 1, int.Parse(endArr[0]), int.Parse(endArr[1]), 00);
                TimeSpan d3 = d2.Subtract(d1);
                int minute = d3.Hours * 60 + d3.Minutes;
                if (minute <= 0)
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Hint("上午结束时间不能小于或等于开始时间", HintMessageBoxIcon.Error, MainForm);
                    return;
                }
                if (minute < int.Parse(subsection))
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Hint("上午分段时间大于总时间", HintMessageBoxIcon.Error, MainForm);
                    return;
                }
                if(minute % int.Parse(subsection) == 0)
                    rowNum = minute / int.Parse(subsection);
                else
                    rowNum = (minute / int.Parse(subsection)) + 1;
            }

            //计算下午的分段数量
            if (gbp.GroupText.Equals("下午"))
            {
                DateTime d1 = new DateTime(2004, 1, 1, int.Parse(startArr[0]), int.Parse(startArr[1]), 00);
                DateTime d2 = new DateTime(2004, 1, 1, int.Parse(endArr[0]), int.Parse(endArr[1]), 00);
                TimeSpan d3 = d2.Subtract(d1);
                int minute = d3.Hours * 60 + d3.Minutes;
                if (minute <= 0)
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Hint("下午结束时间不能小于或等于开始时间", HintMessageBoxIcon.Error, MainForm);
                    return;
                }
                if (minute < int.Parse(subsection))
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Hint("下午分段时间大于总时间", HintMessageBoxIcon.Error, MainForm);
                    return;
                }
                if (minute % int.Parse(subsection) == 0)
                    rowNum = minute / int.Parse(subsection);
                else
                    rowNum = (minute / int.Parse(subsection)) + 1;
            }

            //计算晚上的分段数量
            if (gbp.GroupText.Equals("晚上"))
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
                    MessageBoxUtils.Hint("晚上结束时间不能小于或等于开始时间", HintMessageBoxIcon.Error, MainForm);
                    return;
                }
                if (minute < int.Parse(subsection))
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Hint("晚上分段时间大于总时间", HintMessageBoxIcon.Error, MainForm);
                    return;
                }
                if (minute % int.Parse(subsection) == 0)
                    rowNum = minute / int.Parse(subsection);
                else
                    rowNum = (minute / int.Parse(subsection)) + 1;
            }

            //计算全天的分段数量
            if (gbp.GroupText.Equals("全天"))
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
                    MessageBoxUtils.Hint("全天结束时间不能小于或等于开始时间", HintMessageBoxIcon.Error, MainForm);
                    return;
                }
                if (minute < int.Parse(subsection))
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Hint("全天分段时间大于总时间", HintMessageBoxIcon.Error, MainForm);
                    return;
                }
                if (minute % int.Parse(subsection) == 0)
                    rowNum = minute / int.Parse(subsection);
                else
                    rowNum = (minute / int.Parse(subsection)) + 1;
            }
            #endregion

            int row = 0;//行数(包括标题)
            DateTime dt1 = new DateTime();//开始时间
            DateTime dt2 = new DateTime();//结束时间

            if (rowNum > 3) row = rowNum + 1;
            else row = 4;
            dt1 = DateTime.Parse("2008-08-08 " + start + ":00");
            dt2 = dt1.AddMinutes(int.Parse(subsection));

            newTlp.ColumnCount = 7;
            newTlp.RowCount = row;

            newTlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            newTlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            newTlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            newTlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            newTlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            newTlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            newTlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));

            for (int n = 0; n < row; n++)
            {
                newTlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            }
            newTlp.Size = new System.Drawing.Size(297, row * 30);
            //标题栏
            Label label = new Label();
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.BottomCenter;
            label.Font = new Font("微软雅黑", 10);
            label.Text = "开始";
            newTlp.Controls.Add(label, 1, 0);
            label = new Label();
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.BottomCenter;
            label.Font = new Font("微软雅黑", 10);
            label.Text = "结束";
            newTlp.Controls.Add(label, 2, 0);
            label = new Label();
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.BottomCenter;
            label.Font = new Font("微软雅黑", 10);
            label.Text = "现场";
            newTlp.Controls.Add(label, 3, 0);
            label = new Label();
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.BottomCenter;
            label.Font = new Font("微软雅黑", 10);
            label.Text = "预约";
            newTlp.Controls.Add(label, 4, 0);
            label = new Label();
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.BottomCenter;
            label.Font = new Font("微软雅黑", 10);
            label.Text = "诊间";
            newTlp.Controls.Add(label, 5, 0);
            label = new Label();
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.BottomCenter;
            label.Font = new Font("微软雅黑", 10);
            label.Text = "应急";
            label.Visible = false;
            newTlp.Controls.Add(label, 6, 0);

            bool teEnabled = true;//当行数小于3的时候，空白文本框需要设为不可选
            for (int r = 1; r < row; r++)
            {
                start = dt1.ToString("HH:mm");
                if (r == rowNum)
                    end = tlpTop.GetControlFromPosition(1, 1).Text;
                else
                    end = dt2.ToString("HH:mm");
                dt1 = dt2;
                dt2 = dt1.AddMinutes(int.Parse(subsection));
                if (r > rowNum)
                {
                    start = "";
                    end = "";
                    scene = "";
                    open = "";
                    room = "";
                    emergency = "";
                    teEnabled = false;
                }
                for (int c = 0; c < 7; c++)
                {
                    if (r == 1 && c == 0)
                    {
                        //第一行第一列
                        CheckBox checkBox = new CheckBox();
                        checkBox.Dock = DockStyle.Fill;
                        checkBox.Font = new Font("微软雅黑", 10);
                        //checkBox.Text = timeInterval;
                        checkBox.Text = "特需";
                        checkBox.CheckState = checkState;
                        newTlp.Controls.Add(checkBox, 0, 1);
                    }
                    else if (c == 0)
                    {
                        //不做处理
                    }
                    else
                    {
                        if (c == 1)
                        {
                            //第二列
                            TextEdit textEdit = new TextEdit();
                            textEdit.Properties.AutoHeight = false;
                            textEdit.Dock = DockStyle.Fill;
                            textEdit.Font = new Font("微软雅黑", 10);
                            textEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                            textEdit.Text = start;
                            textEdit.Enabled = teEnabled;
                            newTlp.Controls.Add(textEdit, c, r);
                        }
                        else if (c == 2)
                        {
                            //第三列
                            TextEdit textEdit = new TextEdit();
                            textEdit.Properties.AutoHeight = false;
                            textEdit.Dock = DockStyle.Fill;
                            textEdit.Font = new Font("微软雅黑", 10);
                            textEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                            textEdit.Text = end;
                            textEdit.Enabled = teEnabled;
                            newTlp.Controls.Add(textEdit, c, r);
                        }
                        else if (c == 3)
                        {
                            //第四列 现场
                            TextEdit textEdit = new TextEdit();
                            textEdit.Properties.AutoHeight = false;
                            textEdit.Dock = DockStyle.Fill;
                            textEdit.Font = new Font("微软雅黑", 10);
                            textEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                            textEdit.Text = scene;
                            textEdit.Enabled = teEnabled;
                            newTlp.Controls.Add(textEdit, c, r);
                        }
                        else if (c == 4)
                        {
                            //第五列 预约
                            TextEdit textEdit = new TextEdit();
                            textEdit.Properties.AutoHeight = false;
                            textEdit.Dock = DockStyle.Fill;
                            textEdit.Font = new Font("微软雅黑", 10);
                            textEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                            textEdit.Text = open;
                            textEdit.Enabled = teEnabled;
                            newTlp.Controls.Add(textEdit, c, r);
                        }
                        else if (c == 5)
                        {
                            //第六列 诊间
                            TextEdit textEdit = new TextEdit();
                            textEdit.Properties.AutoHeight = false;
                            textEdit.Dock = DockStyle.Fill;
                            textEdit.Font = new Font("微软雅黑", 10);
                            textEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                            textEdit.Text = room;
                            textEdit.Enabled = teEnabled;
                            newTlp.Controls.Add(textEdit, c, r);
                        }
                        else if (c == 6)
                        {
                            //第七列 应急
                            TextEdit textEdit = new TextEdit();
                            textEdit.Properties.AutoHeight = false;
                            textEdit.Dock = DockStyle.Fill;
                            textEdit.Font = new Font("微软雅黑", 10);
                            textEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                            textEdit.Text = emergency;
                            textEdit.Enabled = teEnabled;
                            textEdit.Visible = false;
                            newTlp.Controls.Add(textEdit, c, r);
                        }
                    }
                }
            }

            newTlp.Location = new System.Drawing.Point(5, 95);
            tlp.Dispose();//清除排班
            gbp.Controls.Add(newTlp);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (workDate == null)
            {
                MessageBoxUtils.Hint("请先查询后进行排班再保存", HintMessageBoxIcon.Error, MainForm);
            }
            if (lueIsUse.Text == null || lueIsUse.Text.ToString().Length == 0)
            {
                dataController1.ShowError(lueIsUse, "请选择状态");
                return;
            }

            List<WorkingDayEntity> workingDayList = new List<WorkingDayEntity>();
            //获取排班信息
            int days = panel9.Controls.Count; //排班天数
            for (int i = 0; i < days; i++)
            {
                Panel panel = (Panel)panel9.Controls[i];
                GroupBorderPanel groupBoy = (GroupBorderPanel)panel.Controls[0];
                TableLayoutPanel tlp = (TableLayoutPanel)groupBoy.Controls[2];
                if (tlp.Enabled)
                {
                    CheckBox cbIsUse = (CheckBox)tlp.GetControlFromPosition(0, 1);
                    //CheckBox cbAuto = (CheckBox)tlp.GetControlFromPosition(0, 2);
                    String period = "";
                    if (groupBoy.GroupText.Equals("上午")) period = "0";
                    if (groupBoy.GroupText.Equals("下午")) period = "1";
                    if (groupBoy.GroupText.Equals("晚上")) period = "2";
                    if (groupBoy.GroupText.Equals("全天")) period = "3";
                    //DateTime wsBeginTime = new DateTime();
                    for (int r = 1; r < tlp.RowCount; r++)//行
                    {
                        WorkingDayEntity wordingDay = new WorkingDayEntity();
                        wordingDay.period = period;
                        if (cbIsUse.CheckState == CheckState.Checked)
                        {
                            wordingDay.mzType = "2";
                        }
                        else
                        {
                            wordingDay.mzType = "1";
                        }
                        for (int c = 1; c < tlp.ColumnCount; c++)//列
                        {
                            TextEdit te = (TextEdit)tlp.GetControlFromPosition(c, r);
                            if (c == 1)
                                wordingDay.beginTime = te.Text;
                            else if (c == 2)
                                wordingDay.endTime = te.Text;
                            else if (c == 3)
                                wordingDay.numSite = te.Text;
                            else if (c == 4)
                                wordingDay.numOpen = te.Text;
                            else if (c == 5)
                                wordingDay.numClinic = te.Text;
                            else if (c == 6)
                                wordingDay.numYj = "0";
                            //if (period.Equals("3") && r == 1 && c == 1)
                            //{
                            //    wsBeginTime = DateTime.Parse("2008-08-08 " + wordingDay.beginTime + ":00");
                            //}
                            //if (period.Equals("3"))
                            //{
                            //    DateTime dt2 = DateTime.Parse("2008-08-08 " + wordingDay.beginTime + ":00");
                            //    TimeSpan dt3 = dt2.Subtract(wsBeginTime);
                            //    int minute = dt3.Hours * 60 + dt3.Minutes;
                            //    if (minute <= 0)
                            //    {

                            //    }
                            //}
                        }
                        if(wordingDay.beginTime.Length>0)
                            workingDayList.Add(wordingDay);
                    }
                }
            }
            cmd.ShowOpaqueLayer();
            String scheduSets = Newtonsoft.Json.JsonConvert.SerializeObject(workingDayList);

            String param = "deptId=" + treeMenuControl1.EditValue + "&doctorId=" + mcDoctor.itemName
                + "&hospitalId=" + AppContext.Session.hospitalId + "&workDate=" + workDate
                + "&status=" + lueIsUse.EditValue + "&remarks=" + teRemarks.Text
                + "&scheduSets=" + scheduSets;
            String url = AppContext.AppConfig.serverUrl + "sch/doctorScheduPlan/saveToOne?";
            this.DoWorkAsync(500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url, param);
                return data;

            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                JObject objT = JObject.Parse(data.ToString());
                cmd.HideOpaqueLayer();
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    ifQuery = false;
                    cbMorning.CheckState = CheckState.Unchecked;
                    cbAfternoon.CheckState = CheckState.Unchecked;
                    cbNight.CheckState = CheckState.Unchecked;
                    cbAllAay.CheckState = CheckState.Unchecked;
                    ifQuery = true;
                    MessageBoxUtils.Hint("保存成功!", MainForm);
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    return;
                }
            });
        }

        private void cbMorning_CheckStateChanged(object sender, EventArgs e)
        {
            if (cbAllAay.CheckState == CheckState.Checked)
            {
                cbMorning.CheckState = CheckState.Unchecked;
                dataController1.ShowError(cbAllAay, "选择了全天就不能选择上午、下午、晚上");
                return;
            }
            setScheduling();
            //CheckBox cb = (CheckBox)sender;
            //if (cb.CheckState == CheckState.Checked)
            //{
            //    setScheduling();
            //}
        }

        private void cbAfternoon_CheckStateChanged(object sender, EventArgs e)
        {
            if (cbAllAay.CheckState == CheckState.Checked)
            {
                cbAfternoon.CheckState = CheckState.Unchecked;
                dataController1.ShowError(cbAllAay, "选择了全天就不能选择上午、下午、晚上");
                return;
            }
            setScheduling();
            //CheckBox cb = (CheckBox)sender;
            //if (cb.CheckState == CheckState.Checked)
            //{
            //    setScheduling();
            //}
        }

        private void cbNight_CheckStateChanged(object sender, EventArgs e)
        {
            if (cbAllAay.CheckState == CheckState.Checked)
            {
                cbNight.CheckState = CheckState.Unchecked;
                dataController1.ShowError(cbAllAay, "选择了全天就不能选择上午、下午、晚上");
                return;
            }
            setScheduling();
            //CheckBox cb = (CheckBox)sender;
            //if (cb.CheckState == CheckState.Checked)
            //{
            //    setScheduling();
            //}
        }

        private void cbAllAay_CheckStateChanged(object sender, EventArgs e)
        {
            if (cbMorning.CheckState == CheckState.Checked
                || cbAfternoon.CheckState == CheckState.Checked
                || cbNight.CheckState == CheckState.Checked)
            {
                cbAllAay.CheckState = CheckState.Unchecked;
                dataController1.ShowError(cbAllAay, "选择了上午、下午、晚上就不能选择全天");
                return;
            }
            setScheduling();
            //CheckBox cb = (CheckBox)sender;
            //if (cb.CheckState == CheckState.Checked)
            //{
            //    setScheduling();
            //}
        }

        private void dateEdit1_EditValueChanged(object sender, EventArgs e)
        {
            setScheduling();
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

        private void SingleSchedulingForm_Resize(object sender, EventArgs e)
        {
            if (cmd == null)
                cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            cmd.rectDisplay = this.DisplayRectangle;
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

        /// <summary>
        /// 时间控件限时事件(上午)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwEditValueChanged(object sender, EventArgs e)
        {
            ifTime(sender, "0", true);
        }

        /// <summary>
        /// 时间控件限时事件(下午)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XwEditValueChanged(object sender, EventArgs e)
        {
            ifTime(sender, "1", true);
        }

        /// <summary>
        /// 时间控件限时事件(晚上开始)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WsBeginEditValueChanged(object sender, EventArgs e)
        {
            ifTime(sender, "2", true);
        }

        /// <summary>
        /// 时间控件限时事件(晚上结束)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WsEndEditValueChanged(object sender, EventArgs e)
        {
            ifTime(sender, "2", false);
        }
    }
}
