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
using System.Xml.Serialization;

namespace Xr.RtManager.Pages.scheduling
{
    public partial class NotOpenListEdit : Form
    {
        public NotOpenListEdit()
        {
            InitializeComponent();
        }

        Xr.Common.Controls.OpaqueCommand cmd;

        public String deptId;
        public String doctorId;
        public String beginDate;
        public String endDate;
        public List<DoctorInfoEntity> doctorList;

        private bool firstQuery = true;

        public ScheduledEntity scheduled { get; set; }

        private void ModifyNumberSourceEdit_Load(object sender, EventArgs e)
        {
            cmd = new Xr.Common.Controls.OpaqueCommand(this);
            cmd.ShowOpaqueLayer(0f);
            deBegin.EditValue = beginDate;
            deEnd.EditValue = endDate;

            //获取可操作科室
            String url = AppContext.AppConfig.serverUrl + "cms/dept/qureyOperateDept";
            this.DoWorkAsync(500, (o) =>
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) =>
            {
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    List<DeptEntity> deptList = objT["result"]["deptList"].ToObject<List<DeptEntity>>();
                    treeDept.Properties.DataSource = deptList;
                    treeDept.Properties.TreeList.KeyFieldName = "id";
                    treeDept.Properties.TreeList.ParentFieldName = "parentId";
                    treeDept.Properties.DisplayMember = "name";
                    treeDept.Properties.ValueMember = "id";
                    treeDept.EditValue = deptId;

                    lueDoctor.Properties.DataSource = doctorList;
                    lueDoctor.Properties.DisplayMember = "name";
                    lueDoctor.Properties.ValueMember = "id";
                    lueDoctor.EditValue = doctorId;

                    //合并值相同的单元格
                    gridView1.OptionsView.AllowCellMerge = true;
                    //设置表格中状态下拉框的数据
                    List<DictEntity> dictList = new List<DictEntity>();
                    DictEntity dict = new DictEntity();
                    dict.value = "0";
                    dict.label = "正常";
                    dictList.Add(dict);
                    dict = new DictEntity();
                    dict.value = "1";
                    dict.label = "停诊";
                    dictList.Add(dict);
                    repositoryItemLookUpEdit1.DataSource = dictList;
                    repositoryItemLookUpEdit1.DisplayMember = "label";
                    repositoryItemLookUpEdit1.ValueMember = "value";
                    repositoryItemLookUpEdit1.ShowHeader = false;
                    repositoryItemLookUpEdit1.ShowFooter = false;
                    //SearchData();
                    cmd.HideOpaqueLayer();
                }
                else
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                    return;
                }
            });
        }

        /// <summary>
        /// 科室选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeDept_EditValueChanged(object sender, EventArgs e)
        {
            if (!firstQuery)
            {
                Object deptId = treeDept.EditValue;
                if (deptId.Equals("`"))
                    deptId = "";
                String param = "pageNo=1&pageSize=10000&hospital.id=" + AppContext.Session.hospitalId + "&dept.id=" + deptId;
                String url = AppContext.AppConfig.serverUrl + "cms/doctor/list?" + param;
                this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                {
                    String data = HttpClass.httpPost(url);
                    return data;

                }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                {
                    JObject objT = JObject.Parse(data.ToString());
                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        List<DoctorInfoEntity> doctorList = objT["result"]["list"].ToObject<List<DoctorInfoEntity>>();
                        DoctorInfoEntity doctor = new DoctorInfoEntity();
                        doctor.id = "";
                        doctor.name = "全部医生";
                        doctorList.Insert(0, doctor);
                        lueDoctor.Properties.DataSource = doctorList;
                        lueDoctor.Properties.DisplayMember = "name";
                        lueDoctor.Properties.ValueMember = "id";
                        lueDoctor.ItemIndex = 0;
                        cmd.HideOpaqueLayer();
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
                firstQuery = false;
            }
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        private void SearchData()
        {
            Object deptId = treeDept.EditValue;
            if (deptId.Equals("`"))
                deptId = "";
            String param = "beginDate=" + deBegin.Text + "&endDate=" + deEnd.Text
    + "&hospitalId=" + AppContext.Session.hospitalId + "&deptId=" + deptId
    + "&doctorId=" + lueDoctor.EditValue;
            String url = AppContext.AppConfig.serverUrl + "sch/doctorScheduPlan/findNotOpen?" + param;
            this.DoWorkAsync(500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    List<ScheduledEntity> scheduledList = objT["result"].ToObject<List<ScheduledEntity>>();
                    List<ScheduledEntity > dataSource = new List<ScheduledEntity>();
                    for (int i = 0; i < scheduledList.Count; i++)
                    {
                        ScheduledEntity scheduled = scheduledList[i];
                        scheduled.num = (i + 1).ToString();
                        String gou = "√";
                        if (scheduled.mzType.Equals("2"))
                            gou = "特√";
                        if (scheduled.period.Equals("0"))
                            scheduled.am = gou;
                        else if (scheduled.period.Equals("1"))
                            scheduled.pm = gou;
                        else if (scheduled.period.Equals("2"))
                            scheduled.night = gou;
                        else if (scheduled.period.Equals("3"))
                            scheduled.allday = gou;
                        dataSource.Add(scheduled);
                    }
                    gcScheduled.DataSource = dataSource;
                    cmd.HideOpaqueLayer();
                }
                else
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                    return;
                }
            });
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            cmd.ShowOpaqueLayer();
            SearchData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<ScheduledEntity> dataSource = gcScheduled.DataSource as List<ScheduledEntity>;
            if (dataSource==null || dataSource.Count == 0)
            {
                MessageBoxUtils.Show("请先查询到数据再放号", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, this);
                return;
            }

            if (MessageBoxUtils.Show("确定放号吗?", MessageBoxButtons.OKCancel,
MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, this) == DialogResult.OK)
            {
                Object deptId = treeDept.EditValue;
                if (deptId.Equals("`"))
                    deptId = "";
                String param = "beginDate=" + deBegin.Text + "&endDate=" + deEnd.Text
                + "&hospitalId=" + AppContext.Session.hospitalId + "&deptId=" + deptId
                + "&doctorId=" + lueDoctor.EditValue;
                String url = AppContext.AppConfig.serverUrl + "sch/doctorScheduPlan/updateIsOpen?" + param;
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

        /// <summary>
        /// 深克隆方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="RealObject"></param>
        /// <returns></returns>
        public static T Clone<T>(T RealObject)
        {
            using (Stream stream = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stream, RealObject);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)serializer.Deserialize(stream);
            }
        }

        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "am" || e.Column.FieldName == "pm"
                || e.Column.FieldName == "night" || e.Column.FieldName == "allday") //指定列
            {
                if ((string)e.CellValue == "特√")  //条件  e.CellValue 为object类型
                    e.Appearance.BackColor = Color.FromArgb(255, 34, 167);
            }      
        }
    }
}
