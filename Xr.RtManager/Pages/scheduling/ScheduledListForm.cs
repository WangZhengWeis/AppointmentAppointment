using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Xr.Http;
using Newtonsoft.Json.Linq;
using Xr.Common;
using System.Threading;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using System.Drawing;

namespace Xr.RtManager.Pages.scheduling
{
    public partial class ScheduledListForm : UserControl
    {
        private Form MainForm; //主窗体
        Xr.Common.Controls.OpaqueCommand cmd;

        public ScheduledListForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗口加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScheduledListForm_Load(object sender, EventArgs e)
        {
            MainForm = (Form)this.Parent;
            deBegin.EditValue = DateTime.Now.ToString("yyyy-MM-dd");
            deEnd.EditValue = DateTime.Now.AddDays(11).ToString("yyyy-MM-dd");//修改为当前日期+11之后的日期。
            cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);

            bool hzFlag = false;
            foreach (FunctionEntity function in AppContext.Session.functionList)
            {
                if (function.name.Equals("已排班列表门诊特需互转"))
                {
                    hzFlag = true;
                    break;
                }
            }
            if (hzFlag)
            {
                buttonControl2.Visible = hzFlag;
            }
            else
            {
                buttonControl4.Location = new Point(25, 47);
            }

            bool fhFlag = false;
            foreach (FunctionEntity function in AppContext.Session.functionList)
            {
                if (function.name.Equals("已排班列表放号审核"))
                {
                    fhFlag = true;
                    break;
                }
            }
            if (hzFlag && fhFlag)
            {
                buttonControl3.Visible = fhFlag;
            }
            else if (!hzFlag && fhFlag)
            {
                buttonControl3.Visible = fhFlag;
                buttonControl3.Location = new Point(131, 47);
            }


            bool addFlag = false;
            foreach (FunctionEntity function in AppContext.Session.functionList)
            {
                if (function.name.Equals("已排班列表增加排班"))
                {
                    addFlag = true;
                    break;
                }
            }

            buttonControl1.Visible = addFlag;
            if (hzFlag && !fhFlag){
                buttonControl1.Location = new Point(239, 47);
            }
            if (!hzFlag && fhFlag)
            {
                buttonControl1.Location = new Point(217, 47);
            }
            else if (!hzFlag && !fhFlag)
            {
                buttonControl1.Location = new Point(131, 47);
            }
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

            lookUpEdit1.Properties.DataSource = dictList;
            lookUpEdit1.Properties.DisplayMember = "label";
            lookUpEdit1.Properties.ValueMember = "value";
            lookUpEdit1.ItemIndex = 0;

            List<DictEntity> dictList2 = new List<DictEntity>();
            dict = new DictEntity();
            dict.value = "`";
            dict.label = "全部";
            dictList2.Add(dict);
            dict = new DictEntity();
            dict.value = "0";
            dict.label = "放号";
            dictList2.Add(dict);
            dict = new DictEntity();
            dict.value = "1";
            dict.label = "未放号";
            dictList2.Add(dict);

            lookUpEdit2.Properties.DataSource = dictList2;
            lookUpEdit2.Properties.DisplayMember = "label";
            lookUpEdit2.Properties.ValueMember = "value";
            lookUpEdit2.ItemIndex = 0;

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
                    DeptEntity dept = new DeptEntity();
                    dept.id = "`";
                    dept.name = "全部";
                    //deptList.Insert(0, dept);
                    treeDept.Properties.DataSource = deptList;
                    treeDept.Properties.TreeList.KeyFieldName = "id";
                    treeDept.Properties.TreeList.ParentFieldName = "parentId";
                    treeDept.Properties.DisplayMember = "name";
                    treeDept.Properties.ValueMember = "id";
                    treeDept.EditValue = AppContext.Session.deptList[0].id;

                    SearchData();
                }
                else
                {
                    cmd.HideOpaqueLayer();
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
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
            Object deptId = treeDept.EditValue;
            if (deptId.Equals("`"))
                deptId = "";
            String param = "hospital.id=" + AppContext.Session.hospitalId + "&dept.id=" + deptId;
            //String param = "pageNo=1&pageSize=10000&hospital.id=" + AppContext.Session.hospitalId + "&dept.id=" + deptId;
            String url = AppContext.AppConfig.serverUrl + "cms/doctor/findAll?" + param;
            String data = HttpClass.httpPost(url);
            JObject objT = JObject.Parse(data);
            if (string.Compare(objT["state"].ToString(), "true", true) == 0)
            {
                List<DoctorInfoEntity> doctorList = objT["result"].ToObject<List<DoctorInfoEntity>>();
                DoctorInfoEntity doctor = new DoctorInfoEntity();
                doctor.id = "";
                doctor.name = "全部医生";
                doctorList.Insert(0, doctor);
                lueDoctor.Properties.DataSource = doctorList;
                lueDoctor.Properties.DisplayMember = "name";
                lueDoctor.Properties.ValueMember = "id";
                lueDoctor.ItemIndex = 0;
            }
            else
            {
                MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                return;
            }
        }

        /// <summary>
        /// 查询按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            cmd.ShowOpaqueLayer();
            SearchData();
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        private void SearchData()
        {
            Object deptId = treeDept.EditValue;
            if (deptId.Equals("`"))
                deptId = "";
            Object isOpen = lookUpEdit2.EditValue;
            if (isOpen.Equals("`"))
                isOpen = "";
            String param = "beginDate=" + deBegin.Text + "&endDate=" + deEnd.Text
    + "&hospitalId=" + AppContext.Session.hospitalId + "&deptId=" + deptId
    + "&doctorId=" + lueDoctor.EditValue + "&status=" + lookUpEdit1.EditValue
    + "&isOpen=" + isOpen;
            String url = AppContext.AppConfig.serverUrl + "sch/doctorScheduPlan/findByPropertys?" + param;
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
                    List<ScheduledEntity> dataSource = new List<ScheduledEntity>();
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
                    btnHz.DataSource = dataSource;
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

        /// <summary>
        /// 表格单元格合并事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_CellMerge(object sender, DevExpress.XtraGrid.Views.Grid.CellMergeEventArgs e)
        {
            int rowHandle1 = e.RowHandle1;
            int rowHandle2 = e.RowHandle2;
            string deptName1 = gridView1.GetRowCellValue(rowHandle1, gridView1.Columns["deptName"]).ToString(); //获取科室列值
            string deptName2 = gridView1.GetRowCellValue(rowHandle2, gridView1.Columns["deptName"]).ToString();
            string workDate1 = gridView1.GetRowCellValue(rowHandle1, gridView1.Columns["workDate"]).ToString(); //获取日期列值
            string workDate2 = gridView1.GetRowCellValue(rowHandle2, gridView1.Columns["workDate"]).ToString();
            if (e.Column.FieldName == "deptName") 
            {
                if (deptName1 != deptName2)
                {
                    e.Merge = false; //值相同的2个单元格是否要合并在一起
                    e.Handled = true; //合并单元格是否已经处理过，无需再次进行省缺处理
                }
            }
            else if (e.Column.FieldName == "workDate")
            {
                if (!(deptName1 == deptName2 && workDate1 == workDate2))
                {
                    e.Merge = false; 
                    e.Handled = true; 
                }
            }
            else
            {
                e.Merge = false; 
                e.Handled = true; 
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var edit = new RemarksEdit();
            edit.deptId = treeDept.EditValue.ToString();
            if (edit.deptId.Equals("`"))
                edit.deptId = "";
            edit.doctorId = lueDoctor.EditValue.ToString();
            edit.beginDate = deBegin.Text;
            edit.endDate = deEnd.Text;
            if (edit.ShowDialog() == DialogResult.OK)
            {
                Thread.Sleep(300);
                SearchData();
                MessageBoxUtils.Hint("修改成功!", MainForm);
            }
        }

        private void repositoryItemLookUpEdit1_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            int selectRow = gridView1.GetSelectedRows()[0];

            if (e.OldValue.Equals("0"))
            {
                //停诊要显示特定的界面
                var selectedRow = gridView1.GetFocusedRow() as ScheduledEntity;
                if (selectedRow == null)
                    return;
                var edit = new TingZhenEdit();
                edit.scheduled = selectedRow;
                if (edit.ShowDialog() == DialogResult.OK)
                {
                    MessageBoxUtils.Hint("停诊成功!", MainForm);
                }
            }
            else
            {
                if (MessageBoxUtils.Show("确定要修改状态吗?", MessageBoxButtons.OKCancel,
     MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MainForm) == DialogResult.OK)
                {
                    var selectedRow = gridView1.GetFocusedRow() as ScheduledEntity;
                    String period = "";
                    if (selectedRow.am != null && (selectedRow.am.Equals("√") || selectedRow.am.Equals("特√")))
                        period = "0";
                    else if (selectedRow.pm != null && (selectedRow.pm.Equals("√") || selectedRow.pm.Equals("特√")))
                        period = "1";
                    else if (selectedRow.night != null && (selectedRow.night.Equals("√") || selectedRow.night.Equals("特√")))
                        period = "2";
                    else if (selectedRow.allday != null && (selectedRow.allday.Equals("√") || selectedRow.allday.Equals("特√")))
                        period = "3";
                    String param = "deptId=" + selectedRow.deptId + "&doctorId=" + selectedRow.doctorId
                        + "&period=" + period + "&workDate=" + selectedRow.workDate
                        + "&status=" + e.NewValue + "&hospitalId=" + AppContext.Session.hospitalId;
                    String url = AppContext.AppConfig.serverUrl + "sch/doctorScheduPlan/updatestatus?" + param;
                    String data = HttpClass.httpPost(url);
                    JObject objT = JObject.Parse(data);
                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        MessageBoxUtils.Hint("修改成功!", MainForm);
                    }
                    else
                    {
                        this.gridView1.SetRowCellValue(selectRow, gridView1.Columns["status"], e.OldValue);
                        MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    }
                }
                else
                {
                    this.gridView1.SetRowCellValue(selectRow, gridView1.Columns["status"], e.OldValue);
                }
            }
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

        private void ScheduledListForm_Resize(object sender, EventArgs e)
        {
            if (cmd == null)
                cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            cmd.rectDisplay = this.DisplayRectangle;
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            var selectedRow = gridView1.GetFocusedRow() as ScheduledEntity;
            if (selectedRow == null)
                return;
            var edit = new ModifyNumberSourceEdit();
            edit.scheduled = selectedRow;
            if (edit.ShowDialog() == DialogResult.OK)
            {
                MessageBoxUtils.Hint("修改成功!", MainForm);
                //this.DoWorkAsync(500, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                //{
                //    Thread.Sleep(2700);
                //    return null;

                //}, null, (r) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                //{
                //    cmd.ShowOpaqueLayer(255, true);
                //    SearchData(true, pageControl1.CurrentPage, pageControl1.PageSize);
                //});
            }
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

        /// <summary>
        /// 放号审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFhsh_Click(object sender, EventArgs e)
        {
            var edit = new NotOpenListEdit();
            edit.deptId = treeDept.EditValue.ToString();
            edit.doctorId = lueDoctor.EditValue.ToString();
            edit.beginDate = deBegin.Text;
            edit.endDate = deEnd.Text;
            edit.doctorList = lueDoctor.Properties.DataSource as List<DoctorInfoEntity>;
            if (edit.ShowDialog() == DialogResult.OK)
            {
                Thread.Sleep(300);
                SearchData();
                MessageBoxUtils.Hint("修改成功!", MainForm);
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

        private void buttonControl1_Click(object sender, EventArgs e)
        {
            var selectedRow = gridView1.GetFocusedRow() as ScheduledEntity;
            if (selectedRow == null)
                return;
            var edit = new MenZhenTeXuHuZhuanEdit();
            edit.scheduled = selectedRow;
            if (edit.ShowDialog() == DialogResult.OK)
            {
                Thread.Sleep(300);
                SearchData();
                MessageBoxUtils.Hint("修改成功!", MainForm);
            }
        }
        bool firstfootcell = true;
        private void btnHz_CustomDrawFooter(object sender, DevExpress.XtraGrid.Views.Base.RowObjectCustomDrawEventArgs e)
        {
            Rectangle r = e.Bounds;
            Brush brush = e.Cache.GetGradientBrush(e.Bounds, Color.LightGray, Color.LightGray, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
            e.Graphics.FillRectangle(brush, r);
            e.Handled = true;
        }
        /// <summary>
        /// 自定义表格尾部数据统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            int dx = e.Bounds.Height;
            //Brush brush = e.Cache.GetGradientBrush(e.Bounds, Color.Transparent, Color.Transparent, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
            Rectangle r = e.Bounds;
            r.Inflate(1, -1);
            //r.Inflate(0, -1);
            Pen p = new Pen(Color.Gray);
            Brush brush = e.Cache.GetGradientBrush(e.Bounds, Color.Transparent, Color.Transparent, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
            e.Graphics.FillRectangle(brush, r);
            e.Graphics.DrawRectangle(p, r.X - 1, r.Y, r.Width, r.Height);
            //e.Graphics.DrawRectangle(p, r.X - 1, r.Y, r.Width+1, r.Height);
            //ControlPaint.DrawBorder(e.Graphics, r, Color.Gray, ButtonBorderStyle.Solid);
            //r.Inflate(-1, -1);
            //e.Graphics.FillRectangle(brush, r);
            //r.Inflate(-2, 0);
            e.Appearance.DrawString(e.Cache, e.Info.DisplayText, r);
            if (firstfootcell)
            {
                //e.Graphics.DrawRectangle(p, r.X-1, r.Y, r.Width, r.Height);
                e.Appearance.DrawString(e.Cache, "总计", r);

                firstfootcell = false;
            }
            e.Handled = true;
        }
        private void btnHz_Paint(object sender, PaintEventArgs e)
        {
            firstfootcell = true;
        }

        private void buttonControl1_Click_1(object sender, EventArgs e)
        {
            var selectedRow = gridView1.GetFocusedRow() as ScheduledEntity;
            if (selectedRow == null)
                return;
            if (selectedRow.isOpen.Equals("未放号"))
            {
                MessageBoxUtils.Show("只能添加已放号的排班", MessageBoxButtons.OK, MainForm);
                return;
            }
            var edit = new AddRowNumberEdit();
            edit.scheduled = selectedRow;
            if (edit.ShowDialog() == DialogResult.OK)
            {
                MessageBoxUtils.Hint("修改成功!", MainForm);
            }
        }
    }
}
