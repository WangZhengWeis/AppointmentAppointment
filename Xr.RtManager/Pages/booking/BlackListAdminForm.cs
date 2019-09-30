using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Configuration;
using Newtonsoft.Json.Linq;
using Xr.Common;
using Xr.Http;
using Xr.Common.Controls;
using Xr.RtManager.Utils;
using DevExpress.XtraGrid.Views.Grid;

namespace Xr.RtManager.Pages.booking
{
    public partial class BlackListAdminForm : UserControl
    {
        private Form MainForm; //主窗体
        Xr.Common.Controls.OpaqueCommand cmd;
        public BlackListAdminForm()
        {
            InitializeComponent();
            timer1.Interval = Int32.Parse(ConfigurationManager.AppSettings["AutoRefreshTimeSpan"]) * 1000;
            //cmd = new Xr.Common.Controls.OpaqueCommand(this);
            //cmd.ShowOpaqueLayer(225, true);
        }

        private JObject obj { get; set; }

        private void UserForm_Load(object sender, EventArgs e)
        {
            MainForm = (Form)this.Parent;
            pageControl1.MainForm = MainForm;
            pageControl1.PageSize = Convert.ToInt32(AppContext.AppConfig.pagesize);
            cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            bool jdFlag = false;
            foreach(FunctionEntity function in AppContext.Session.functionList){
                if(function.name.Equals("黑名单解冻")){
                    jdFlag = true;
                    break;
                }
            }
            buttonControl2.Visible = jdFlag;

            //查询科室下拉框数据
            String url = AppContext.AppConfig.serverUrl + "cms/dept/qureyOperateDept";
            String data = HttpClass.httpPost(url);
            JObject objT = JObject.Parse(data);
            if (string.Compare(objT["state"].ToString(), "true", true) == 0)
            {
                List<DeptEntity> deptList = objT["result"]["deptList"].ToObject<List<DeptEntity>>();
                treeDeptId.Properties.DataSource = deptList;
                treeDeptId.Properties.TreeList.KeyFieldName = "id";
                treeDeptId.Properties.TreeList.ParentFieldName = "parentId";
                treeDeptId.Properties.DisplayMember = "name";
                treeDeptId.Properties.ValueMember = "id";
                //默认选择选择第一个
                if (deptList.Count > 0)
                    treeDeptId.EditValue = deptList[0].id;
            }
            else
            {
                MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                return;
            }

            QueryInfo(1,10);

        }
        int currPageNo = 1;
        int pageSize = 10;
        private void QueryInfo(int pageNo, int pageSize)
        {
            currPageNo = pageNo;
            // 弹出加载提示框
            //DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitingForm));
            cmd.ShowOpaqueLayer(225, true);
            // 开始异步
            BackgroundWorkerUtil.start_run(bw_DoWork, bw_RunWorkerCompleted, null, false);
            if (!cb_AutoRefresh.Checked)
            {
                this.gc_PatientList.DataSource = null;
            }

        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                List<String> Results = new List<String>();//lueDept.EditValue
                //String param = "hospitalId=12&deptId=2&reportType=month&startDate=2019-01&endDate=2019-01";
                String param = @"isUse=1&pageNo={0}&pageSize={1}";
                param = String.Format(
                    param, currPageNo,
                    pageSize);
                String url = String.Empty;

                //获取黑名单
                url = AppContext.AppConfig.serverUrl + "sch/blackList/list?" + param;
                Results.Add(HttpClass.httpPost(url));
                //Results.Add(@"{""code"":200,""message"":""操作成功"",""result"":[{""deptName"":""急诊科"",""yyNum"":2,""openNum"":96,""yyFzNum"":1,""xcCzNum"":0,""yyCzNum"":1,""xcFzNum"":1,""xcNum"":1,""deptId"":2},{""deptName"":""急诊科"",""yyNum"":2,""openNum"":96,""yyFzNum"":1,""xcCzNum"":0,""yyCzNum"":1,""xcFzNum"":1,""xcNum"":1,""deptId"":2},{""deptName"":""急诊科"",""yyNum"":2,""openNum"":96,""yyFzNum"":1,""xcCzNum"":0,""yyCzNum"":1,""xcFzNum"":1,""xcNum"":1,""deptId"":2},{""deptName"":""急诊科"",""yyNum"":2,""openNum"":96,""yyFzNum"":1,""xcCzNum"":0,""yyCzNum"":1,""xcFzNum"":1,""xcNum"":1,""deptId"":2},{""deptName"":""急诊科"",""yyNum"":2,""openNum"":96,""yyFzNum"":1,""xcCzNum"":0,""yyCzNum"":1,""xcFzNum"":1,""xcNum"":1,""deptId"":2}],""state"":true}");


                e.Result = Results;
            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
            }
        }
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                List<String> datas = e.Result as List<String>;
                if (datas.Count == 0)
                {
                    return;
                }
                JObject objT = new JObject();
                    objT = JObject.Parse(datas[0]);
                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        List<BlackListEntity> list = objT["result"]["list"].ToObject<List<BlackListEntity>>();//0-启用
                        /*for (int i = list.Count - 1; i >= 0; i--)
                        {
                            if (list[i].isUse != "0")
                            {
                                list.Remove(list[i]);
                            }
                        }
                         */
                        this.gc_BlackList.DataSource = list;
                        pageControl1.setData(int.Parse(objT["result"]["count"].ToString()),
                        int.Parse(objT["result"]["pageSize"].ToString()),
                        int.Parse(objT["result"]["pageNo"].ToString()));
                    }
                    else
                    {
                        MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MainForm);
                        //MessageBox.Show(objT["message"].ToString());
                        return;
                    }

            }
            catch (Exception ex)
            {
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MainForm);
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                // 关闭加载提示框
                //DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
                cmd.HideOpaqueLayer();
            }
        }
        BlackListEntity CurrentrowItem;
        private void gv_deptInfo_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            GridView gv = sender as GridView;

            CurrentrowItem = gv_BlackList.GetRow(e.RowHandle) as BlackListEntity;

            // 弹出加载提示框
            //DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitingForm));
            cmd.ShowOpaqueLayer(225, true);
            // 开始异步
            BackgroundWorkerUtil.start_run(bw_DoWorkGetPatientList, bw_RunWorkerGetPatientListCompleted, null, false);


        }

        private void bw_DoWorkGetPatientList(object sender, DoWorkEventArgs e)
        {
            try
            {
                //String param = "hospitalId=12&deptId=2&reportType=day&startDate=2019-01-01&endDate=2019-01-20";
                String param = "blackListId={0}";
                param = String.Format(
                    param, CurrentrowItem.id);
                String url = AppContext.AppConfig.serverUrl + "sch/blacklistDetail/list?" + param;
                e.Result = HttpClass.httpPost(url);
                //e.Result = @"{""code"":200,""message"":""操作成功"",""result"":[{""czNum"":1,""yyNum"":3,""openNum"":84,""doctorId"":1,""fzNum"":2,""doctorName"":""张医生""},{""czNum"":""0"",""yyNum"":""0"",""openNum"":""0"",""doctorId"":10,""fzNum"":""0"",""doctorName"":""1232""},{""czNum"":""0"",""yyNum"":""0"",""openNum"":12,""doctorId"":15,""fzNum"":""0"",""doctorName"":""杰大哥""},{""czNum"":""0"",""yyNum"":""0"",""openNum"":""0"",""doctorId"":13,""fzNum"":""0"",""doctorName"":""21321""}],""state"":true}";
            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
            }
        }
        private void bw_RunWorkerGetPatientListCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                String data = e.Result as String;
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    List<BlackListPatientEntity> list = objT["result"]["list"].ToObject<List<BlackListPatientEntity>>();
                    this.gc_PatientList.DataSource = list;
                }
                else
                {
                    MessageBoxUtils.Hint(objT["message"].ToString(), HintMessageBoxIcon.Error, MainForm);
                    //MessageBox.Show(objT["message"].ToString());
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                throw new Exception(ex.InnerException.Message);
            }
            finally
            {
                try
                {
                    // 关闭加载提示框
                    //DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
                    cmd.HideOpaqueLayer();
                }
                catch
                { }
            }
        }

        private void gv_BlackList_MouseDown(object sender, MouseEventArgs e)
        {
            //鼠标左键点击
            System.Threading.Thread.Sleep(10);
            if (e.Button == MouseButtons.Left)
            {

                //GridHitInfo gridHitInfo = gridView.CalcHitInfo(e.X, e.Y);
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo gridHitInfo = gv_BlackList.CalcHitInfo(e.X, e.Y);

                //在列标题栏内且列标题name是"colName"
                if (gridHitInfo.Column != null)
                {
                    if (gridHitInfo.InColumnPanel && gridHitInfo.Column.Name == "select")
                    {

                        //获取该列右边线的x坐标

                        DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo gridViewInfo = (DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo)this.gv_BlackList.GetViewInfo();

                        int x = gridViewInfo.GetColumnLeftCoord(gridHitInfo.Column) + gridHitInfo.Column.Width;

                        //右边线向左移动3个像素位置不弹出对话框（实验证明3个像素是正好的）

                        if (e.X < x - 3)
                        {

                            //XtraMessageBox.Show("点击select列标题！");

                            for (int i = 0; i < gv_BlackList.RowCount; i++)
                            {
                                //鼠标的那个按钮按下 
                                string dr = gv_BlackList.GetRowCellValue(i, "check").ToString();
                                if (dr == "1")
                                    gv_BlackList.SetRowCellValue(i, gv_BlackList.Columns["check"], "0");
                                else//(dr == "0")
                                    gv_BlackList.SetRowCellValue(i, gv_BlackList.Columns["check"], "1");
                            }
                        }

                    }
                    /*else if (!gridHitInfo.InColumnPanel)
                    {

                        int i = gridHitInfo.RowHandle;
                        string dr = gv_BlackList.GetRowCellValue(i, "check").ToString();
                        if (dr == "1")
                            gv_BlackList.SetRowCellValue(i, gv_BlackList.Columns["check"], "0");
                        else//(dr == "0")
                            gv_BlackList.SetRowCellValue(i, gv_BlackList.Columns["check"], "1");

                    }
                     */
                }

            }

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {


            //string value = "";
            //string strSelected = "";
            List<BlackListEntity> itemList = new List<BlackListEntity>();
            for (int i = 0; i < gv_BlackList.RowCount; i++)
            {   //   获取选中行的check的值   
                string dr = gv_BlackList.GetRowCellValue(i, "check").ToString();
                if (dr != String.Empty)
                {
                    if (dr == "1")
                    {
                        BlackListEntity rowItem = gv_BlackList.GetRow(i) as BlackListEntity;
                        itemList.Add(rowItem);
                        //bandedGvList.Columns["euDrugtype"].FilterInfo = new ColumnFilterInfo("[euDrugtype] LIKE '0'");

                    }
                }
            }
            if (itemList.Count > 0)
            {
                if (MessageBoxUtils.Show("确认解冻选中的黑名单吗?", MessageBoxButtons.OKCancel,
             MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MainForm) == DialogResult.OK)
                {
                    foreach (var item in itemList)
                    {
                        String param = "id={0}";
                        param = String.Format(
                            param, item.id);
                        String url = AppContext.AppConfig.serverUrl + "sch/blackList/unlock?" + param;
                        string res = HttpClass.httpPost(url);
                    }
                    QueryInfo(1, pageControl1.PageSize);
                }
            }
            else
            {
                //MessageBox.Show("请选择要解冻的数据行");
                MessageBoxUtils.Show("请选择要解冻的数据行", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MainForm);
            }

        }

        private void cb_AutoRefresh_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_AutoRefresh.Checked&&!timer1.Enabled)
            {
                timer1.Start();
            }
            else
            { 
                timer1.Stop(); 
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            QueryInfo(1, pageControl1.PageSize);
        }

        private void buttonControl3_Click(object sender, EventArgs e)
        {
            var Frm = new BlackListAddFrm();
            if (Frm.ShowDialog() == DialogResult.OK)
            {
                MessageBoxUtils.Hint("保存成功!", MainForm);
                QueryInfo(1, pageControl1.PageSize);
            }
        }

        private void buttonControl3_Click_1(object sender, EventArgs e)
        {
            QueryInfo(1, pageControl1.PageSize);
        }

        private void BlackListAdminForm_SizeChanged(object sender, EventArgs e)
        {
            if (cmd == null)
                cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            cmd.rectDisplay = this.DisplayRectangle;
        }

        private void pageControl1_Query(int CurrentPage, int PageSize)
        {
            cmd.ShowOpaqueLayer(225, true);
            QueryInfo(CurrentPage, PageSize);
        }

        private void treeDeptId_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                List<HospitalInfoEntity> doctorInfoEntity = new List<HospitalInfoEntity>();
                // 查询医生下拉框数据
                string url = AppContext.AppConfig.serverUrl + "cms/doctor/findAll?hospital.id=" + AppContext.Session.hospitalId + "&dept.id=" + treeDeptId.EditValue;
                String data = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    doctorInfoEntity = objT["result"].ToObject<List<HospitalInfoEntity>>();
                    doctorInfoEntity.Insert(0, new HospitalInfoEntity { id = "", name = "全部" });
                    lueDoctorId.Properties.DataSource = doctorInfoEntity;
                    lueDoctorId.Properties.DisplayMember = "name";
                    lueDoctorId.Properties.ValueMember = "id";
                    lueDoctorId.ItemIndex = 0;
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                Log4net.LogHelper.Error("获取医生错误信息：" + ex.Message);
            }
        }

        private void buttonControl4_Click(object sender, EventArgs e)
        {
            String param = "patientId=" + tePatientId.EditValue + "&&patientName=" + tePatientName.EditValue
                +"&&deptId="+treeDeptId.EditValue + "&&doctorId=" + lueDoctorId.EditValue
                +"&&dateBegin="+deBegin.Text + "&&dateEnd=" + deEnd.Text;
            String url = AppContext.AppConfig.serverUrl + "sch/blacklistDetail/queryList?"+param;

            cmd.ShowOpaqueLayer();
            this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                String data = HttpClass.httpPost(url);
                return data;

            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                cmd.HideOpaqueLayer();
                JObject objT = JObject.Parse(data.ToString());
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    List<BlackListPatientEntity> list = objT["result"]["list"].ToObject<List<BlackListPatientEntity>>();
                    this.gc_PatientList.DataSource = list;
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
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
    /// <summary>
    /// 黑名单
    /// </summary>
    public class BlackListEntity
    {
        private string _check = "0";

        public string check
        {
            get { return _check; }
            set { _check = value; }
        }
        /// <summary>
        /// 黑名单序列号
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 医院id
        /// </summary>
        public string hospitalId { get; set; }
        /// <summary>
        /// 患者id
        /// </summary>
        public string patientId { get; set; }
        /// <summary>
        /// 患者姓名
        /// </summary>
        public string patientName { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string sex { get; set; }
        /// <summary>
        /// 收录时间
        /// </summary>
        public string createDate { get; set; }
        /// <summary>
        /// 最后爽约时间
        /// </summary>
        public string updateDate { get; set; }  
        /// <summary>
        /// 有效标志
        /// </summary>
        public string isUse { get; set; }
        /// <summary>
        /// 爽约次数
        /// </summary>
        public string breakTimes { get; set; }

    }

    /// <summary>
    /// 黑名单患者详细信息
    /// </summary>
    public class BlackListPatientEntity
    {
        /// <summary>
        /// 序列号
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 黑名单序列号
        /// </summary>
        public string blackListId { get; set; }
        /// <summary>
        /// 医院id
        /// </summary>
        public string hospitalId { get; set; }
        /// <summary>
        /// 科室id
        /// </summary>
        public string deptId { get; set; }
        /// <summary>
        /// 科室名称
        /// </summary>
        public string deptName { get; set; }
        /// <summary>
        /// 医生id
        /// </summary>
        public string doctorId { get; set; }
        /// <summary>
        /// 医生姓名
        /// </summary>
        public string doctorName { get; set; }
        /// <summary>
        /// 患者id
        /// </summary>
        public string patientId { get; set; }
        /// <summary>
        /// 患者姓名
        /// </summary>
        public string patientName { get; set; }
        /// <summary>
        /// 预约时间
        /// </summary>
        public string regVisitTime { get; set; }
        /// <summary>
        /// 爽约记录时间
        /// </summary>
        public string createDate { get; set; }
        /// <summary>
        /// 有效标志
        /// </summary>
        public string isUse { get; set; }
        /// <summary>
        /// 排班id
        /// </summary>
        public string scheduRegisterId { get; set; }

    }
}
