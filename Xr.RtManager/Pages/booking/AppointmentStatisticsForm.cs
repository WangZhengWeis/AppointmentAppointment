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
using Xr.RtManager.Utils;
using Xr.Common.Controls;
using DevExpress.Utils.Drawing;
using DevExpress.XtraCharts;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraEditors;
using System.Globalization;
using System.Net;
using System.IO;
using System.Xml.Serialization;


namespace Xr.RtManager.Pages.booking
{
    public partial class AppointmentStatisticsForm : UserControl
    {
        private Form MainForm; //主窗体
        Xr.Common.Controls.OpaqueCommand cmd;
        /// <summary>
        /// 查询日期格式类型
        /// </summary>
        String QueryDateType = "day";
        public AppointmentStatisticsForm()
        {
            InitializeComponent();
            
        }
        private void Form_SizeChanged(object sender, EventArgs e)
        {
            if (cmd == null)
                cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            cmd.rectDisplay = this.DisplayRectangle;
        }
        private void UserForm_Load(object sender, EventArgs e)
        {
            MainForm = (Form)this.Parent;
            //Infor();
            cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            //gridBand3.Caption = gridBand15.Caption = gridBand32.Caption = gridBand13.Caption = "开放总额\r\n(预约+现场)";
            //查询科室下拉框数据
            String url = AppContext.AppConfig.serverUrl + "cms/dept/qureyOperateDept";
            String data = HttpClass.httpPost(url);
            JObject objT = JObject.Parse(data);
            if (string.Compare(objT["state"].ToString(), "true", true) == 0)
            {
                List<DeptEntity> deptList = objT["result"]["deptList"].ToObject<List<DeptEntity>>();
                DeptEntity dept = new DeptEntity();
                dept.id = " ";
                dept.name = "全部";
                deptList.Insert(0, dept);
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
            /*//若没配置科室编码让其选择一个
            if (AppContext.AppConfig.deptCode == String.Empty)
            {
                treeDeptId.EditValue = " ";
            }
            else
            {

                treeDeptId.EditValue = AppContext.Session.deptId;
            }
             */
            //配置时间格式
            setDateFomartDefult(true);

            TabpageName = xtraTabControl1.SelectedTabPage.Text;

        }
        /// <summary>
        /// 记录Tab页是否已加载数据
        /// </summary>
        Dictionary<Int32, String> PageInfo = new Dictionary<Int32, String>();
        String TabpageName;
        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            TabpageName = xtraTabControl1.SelectedTabPage.Text;
            if (PageInfo.Count != 0)
            {
                if (VerifyInfo())
                {
                    QueryInfo();
                }
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (VerifyInfo())
            {
                PageInfo.Clear();
                QueryInfo();
            }
        }
        AppointmentStatisticsQueryParam CurrentParam=new AppointmentStatisticsQueryParam();
        private bool VerifyInfo()
        { 
            //deStart.Text,
            //deEnd.Text
            if (treeDeptId.EditValue == null)
            {
                MessageBoxUtils.Hint("请选择科室", HintMessageBoxIcon.Error, MainForm);
                return false;
            }
            if (deStart.EditValue == null)
            {
                MessageBoxUtils.Hint("请选择开始日期", HintMessageBoxIcon.Error, MainForm);
                return false;
            }
            if (deEnd.EditValue == null)
            {
                MessageBoxUtils.Hint("请选择结束日期", HintMessageBoxIcon.Error, MainForm);
                return false;
            }

            DateTime dtStart = new DateTime();
            DateTime dtEnd = new DateTime();
            if (QueryDateType != "day")
            {
                dtStart = DateTime.ParseExact(deStart.Text + "-01", "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
                dtEnd = DateTime.ParseExact(deEnd.Text + "-01", "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
            }
            else
            {
                dtStart = DateTime.ParseExact(deStart.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
                dtEnd = DateTime.ParseExact(deEnd.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
            }
            if (dtEnd < dtStart)
            {
                MessageBoxUtils.Hint("结束日期需大于开始日期", HintMessageBoxIcon.Error, MainForm);
                return false;
            }

            CurrentParam.hospitalId = AppContext.Session.hospitalId;
            CurrentParam.deptId = treeDeptId.EditValue.ToString();
            CurrentParam.deptName = treeDeptId.Text.ToString();
            CurrentParam.reportType = QueryDateType;
            CurrentParam.startDate = deStart.Text;
            CurrentParam.endDate = deEnd.Text;

            return true;
        }
        private void QueryInfo()
        {           
            if (PageInfo.Count < 3)
            {
                // 弹出加载提示框
                //DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitingForm));
                cmd.ShowOpaqueLayer(225, true);

                // 开始异步
                BackgroundWorkerUtil.start_run(bw_DoWork, bw_RunWorkerCompleted, null, false);
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                List<String> Results = new List<String>();//lueDept.EditValue
                //String param = "hospitalId=12&deptId=2&reportType=month&startDate=2019-01&endDate=2019-01";
                String param = "hospitalId={0}&deptId={1}&reportType={2}&startDate={3}&endDate={4}";
                param = String.Format(
                    param, CurrentParam.hospitalId,
                    CurrentParam.deptId,
                    CurrentParam.reportType,
                    CurrentParam.startDate,
                    CurrentParam.endDate);
                String url=String.Empty;
                if (TabpageName == "汇总信息")
                {
                #region 预约途径总数统计[0]
                    if (PageInfo.ContainsKey(0))
                    {
                        e.Result = Results;
                        return;
                    }
                 url = AppContext.AppConfig.serverUrl + "sch/report/bespeakWayReprot?" + param;
                 Results.Add(HttpClass.httpPost(url));
                //Results.Add(@"{""code"":200,""message"":""操作成功"",""result"":[{""bespeakCount"":2,""registerWay"":""分诊台""},{""bespeakCount"":1,""registerWay"":""诊间""},{""bespeakCount"":0,""registerWay"":""自助机""},{""bespeakCount"":0,""registerWay"":""公众号""},{""bespeakCount"":0,""registerWay"":""卫计局平台""},{""bespeakCount"":0,""registerWay"":""官网""},{""bespeakCount"":3,""registerWay"":""预约总数""}],""state"":true}");
                    
                #endregion
                #region 日期预约总数统计（折线图）
                //日期预约总数统计（折线图）
                url = AppContext.AppConfig.serverUrl + "sch/report/dateBespeakReprot?" + param;
                Results.Add(HttpClass.httpPost(url));
                // Results.Add(@"{""code"":200,""message"":""操作成功"",""result"":[{""registerTime"": ""2019-01"",""bespeakCount"": 300},{""registerTime"": ""2019-02"",""bespeakCount"": 500},{""registerTime"": ""2019-03"",""bespeakCount"": 600},{""registerTime"": ""2019-04"",""bespeakCount"": 700},{""registerTime"": ""2019-05"",""bespeakCount"": 800}],""state"":true}");
                #endregion
                #region 科室开放数量预约总数概率统计
                //科室开放数量预约总数概率统计
                 url = AppContext.AppConfig.serverUrl + "sch/report/deptOpenBespeakChanceReprot?" + param;
                 Results.Add(HttpClass.httpPost(url));
                //Results.Add(@"{""code"":200,""message"":""操作成功"",""result"":[{""deptName"":""急诊科"",""breakNum"":""0"",""breakRate"":""0%"",""openNum"":96,""visitNum"":""0"",""visitRate"":""0%"",""bespeakRate"":""3.13%"",""bespeakNum"":3},{""deptName"":""内科"",""breakNum"":""0"",""breakRate"":""0%"",""openNum"":96,""visitNum"":""0"",""visitRate"":""0%"",""bespeakRate"":""3.13%"",""bespeakNum"":3}],""state"":true}");
                #endregion
                }
                else if (TabpageName == "详细信息")
                {
                    #region 详细信息
                    if (PageInfo.ContainsKey(1))
                    {
                        e.Result = Results;
                        return;
                    }
                    //详细信息
                    url = AppContext.AppConfig.serverUrl + "sch/report/deptOpenBespeakReprot?" + param;
                    Results.Add(HttpClass.httpPost(url));
                    //Results.Add(@"{""code"":200,""message"":""操作成功"",""result"":[{""deptName"":""急诊科"",""yyNum"":2,""openNum"":96,""yyFzNum"":1,""xcCzNum"":0,""yyCzNum"":1,""xcFzNum"":1,""xcNum"":1,""deptId"":2},{""deptName"":""急诊科"",""yyNum"":2,""openNum"":96,""yyFzNum"":1,""xcCzNum"":0,""yyCzNum"":1,""xcFzNum"":1,""xcNum"":1,""deptId"":2},{""deptName"":""急诊科"",""yyNum"":2,""openNum"":96,""yyFzNum"":1,""xcCzNum"":0,""yyCzNum"":1,""xcFzNum"":1,""xcNum"":1,""deptId"":2},{""deptName"":""急诊科"",""yyNum"":2,""openNum"":96,""yyFzNum"":1,""xcCzNum"":0,""yyCzNum"":1,""xcFzNum"":1,""xcNum"":1,""deptId"":2},{""deptName"":""急诊科"",""yyNum"":2,""openNum"":96,""yyFzNum"":1,""xcCzNum"":0,""yyCzNum"":1,""xcFzNum"":1,""xcNum"":1,""deptId"":2}],""state"":true}");
                    #endregion
                }
                else
                {
                    if (PageInfo.ContainsKey(2))
                    {
                        e.Result = Results;
                        return;
                    }
                    #region 慢病统计
                    //慢病统计
                    url = AppContext.AppConfig.serverUrl + "sch/report/deptOpenBespeakReprotForNCD?" + param;
                    Results.Add(HttpClass.httpPost(url));
                    //Results.Add(@"{""code"":200,""message"":""操作成功"",""result"":[{""deptName"":""急诊科"",""yyNum"":2,""openNum"":96,""yyFzNum"":1,""xcCzNum"":0,""yyCzNum"":1,""xcFzNum"":1,""xcNum"":1,""deptId"":2},{""deptName"":""急诊科"",""yyNum"":2,""openNum"":96,""yyFzNum"":1,""xcCzNum"":0,""yyCzNum"":1,""xcFzNum"":1,""xcNum"":1,""deptId"":2},{""deptName"":""急诊科"",""yyNum"":2,""openNum"":96,""yyFzNum"":1,""xcCzNum"":0,""yyCzNum"":1,""xcFzNum"":1,""xcNum"":1,""deptId"":2},{""deptName"":""急诊科"",""yyNum"":2,""openNum"":96,""yyFzNum"":1,""xcCzNum"":0,""yyCzNum"":1,""xcFzNum"":1,""xcNum"":1,""deptId"":2},{""deptName"":""急诊科"",""yyNum"":2,""openNum"":96,""yyFzNum"":1,""xcCzNum"":0,""yyCzNum"":1,""xcFzNum"":1,""xcNum"":1,""deptId"":2}],""state"":true}");
                    #endregion
                }

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
                JObject objT=new JObject();
                if (TabpageName == "汇总信息")
                {
                    #region 预约途径总数统计[0]
                     objT = JObject.Parse(datas[0]);
                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        List<RegisterWayEntity> list = objT["result"].ToObject<List<RegisterWayEntity>>();
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i < 7)
                            {
                                Label labName = tPanel_registerWay.GetControlFromPosition(i, 0) as Label;
                                labName.Text = list[i].registerWay;
                                Label labNum = tPanel_registerWay.GetControlFromPosition(i, 1) as Label;
                                labNum.Text = list[i].bespeakCount;
                            }
                        }
                    }
                    else
                    {
                        MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                        return;
                    }
                    #endregion
                    #region 日期预约总数统计（折线图）[1]
                    objT = JObject.Parse(datas[1]);
                    if (string.Compare(objT["state"].ToString(), "true", true) != 0)
                    {
                        MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                        return;
                    }
                    else
                    {
                        List<ChartInfoEntity> chartList = objT["result"].ToObject<List<ChartInfoEntity>>();
                        chartControl1.CreateSeries("统计折线图", ViewType.Line, chartList, "registerTime", "bespeakCount");
                        chartControl1.Series[0].LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                        chartControl1.Series[0].CrosshairLabelVisibility = DevExpress.Utils.DefaultBoolean.True;

                    }
                    #endregion
                    #region 科室开放数量预约总数概率统计[2]
                    objT = JObject.Parse(datas[2]);
                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        List<StatisticsInfoEntity> list = objT["result"].ToObject<List<StatisticsInfoEntity>>();
                        this.gc_statistics.DataSource = list;

                    }
                    else
                    {
                        MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                        return;
                    }
                    #endregion
                    PageInfo.Add(0, TabpageName);
                }
                else if (TabpageName == "详细信息")
                {
                #region 详细信息
                objT = JObject.Parse(datas[0]);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    List < DeptInfoEntity > list= objT["result"].ToObject<List<DeptInfoEntity>>();
                    if (list.Count > 0)
                    {
                        list[0].check = "1";
                        gv_deptInfoCheckRowHandle = 0;
                    }
                    this.gc_deptInfo.DataSource = list;
                    // 开始异步
                    if (list.Count > 0)
                    {
                        BackgroundWorkerUtil.start_run(bw_DoWorkGetDocList, bw_RunWorkerGetDocListCompleted, null, false);
                    }
                    else
                    {
                        this.gc_docInfo.DataSource = null;
                    }
                    PageInfo.Add(1, TabpageName);
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    return;
                }
                #endregion
                }
                else{
                #region 慢病统计
                objT = JObject.Parse(datas[0]);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    List<DeptInfoEntity> list = objT["result"].ToObject<List<DeptInfoEntity>>();
                    if (list.Count > 0)
                    {
                        list[0].check = "1";
                        gv_deptNCDInfoCheckRowHandle = 0;
                    }
                    this.gc_deptInfoNCD.DataSource = list;
                    // 开始异步
                    if (list.Count > 0)
                    {
                        BackgroundWorkerUtil.start_run(bw_DoWorkGetNCDDocList, bw_RunWorkerGetNCDDocListCompleted, null, false);
                    }
                    else
                    {
                        this.gc_docInfoNCD.DataSource = null;
                    }
                    PageInfo.Add(2, TabpageName);
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    return;
                }
                #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
            }
            finally
            {
                // 关闭加载提示框
                //DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
                cmd.HideOpaqueLayer();
            }
        }


        int gv_deptInfoCheckRowHandle;
        
        private void gv_deptInfo_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            cmd.ShowOpaqueLayer();
            this.DoWorkAsync(100, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                return null;
            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
               GridView gv = sender as GridView;
               //gv.SetRowCellValue(0, gv.Columns["check"], "1");
               //当前行 置为 选中状态
               if (gv_deptInfoCheckRowHandle != e.RowHandle)
               {
                   //将当前点击的选中
                   gv.SetRowCellValue(e.RowHandle, gv.Columns["check"], "1");
                   //将前面选中的取消 0表示 未选中
                   gv.SetRowCellValue(gv_deptInfoCheckRowHandle, gv.Columns["check"], "0");

                   gv_deptInfoCheckRowHandle = e.RowHandle;

                   String param = "hospitalId={0}&deptId={1}&reportType={2}&startDate={3}&endDate={4}";
                   param = String.Format(param, CurrentParam.hospitalId,
                       gv_deptInfo.GetRowCellValue(gv_deptInfoCheckRowHandle, gv_deptInfo.Columns["deptId"]).ToString(),
                       CurrentParam.reportType,CurrentParam.startDate,CurrentParam.endDate);
                   String url = AppContext.AppConfig.serverUrl + "sch/report/doctorBespeakReprot?" + param;
                   this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                   {
                       data = HttpClass.httpPost(url);
                       return data;

                   }, null, (data2) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                   {
                       JObject objT = JObject.Parse(data2.ToString());
                       if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                       {
                           List<DocInfoEntity> list = objT["result"].ToObject<List<DocInfoEntity>>();
                           this.gc_docInfo.DataSource = list;
                           cmd.HideOpaqueLayer();
                       }
                       else
                       {
                           cmd.HideOpaqueLayer();
                           MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                       }
                   });
               }
               else
               {
                   cmd.HideOpaqueLayer();
               }
            });



            //Log4net.LogHelper.Info("1");
            //GridView gv = sender as GridView;
            ////gv.SetRowCellValue(0, gv.Columns["check"], "1");
            ////当前行 置为 选中状态
            //if (gv_deptInfoCheckRowHandle != e.RowHandle)
            //{
            //    cmd.ShowOpaqueLayer(225, true);
            //    //将当前点击的选中
            //    gv.SetRowCellValue(e.RowHandle, gv.Columns["check"], "1");
            //    //将前面选中的取消 0表示 未选中
            //    gv.SetRowCellValue(gv_deptInfoCheckRowHandle, gv.Columns["check"], "0");

            //    gv_deptInfoCheckRowHandle = e.RowHandle;
            //    // 弹出加载提示框
            //    //DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitingForm));
            //    Log4net.LogHelper.Info("2");
            //    // 开始异步
            //    BackgroundWorkerUtil.start_run(bw_DoWorkGetDocList, bw_RunWorkerGetDocListCompleted, null, false);

                
            //}
        }
    
        private void bw_DoWorkGetDocList(object sender, DoWorkEventArgs e)
        {
            try
            {
                Log4net.LogHelper.Info("3");
                //String param = "hospitalId=12&deptId=2&reportType=day&startDate=2019-01-01&endDate=2019-01-20";
                String param = "hospitalId={0}&deptId={1}&reportType={2}&startDate={3}&endDate={4}";
                param = String.Format(
                    param, CurrentParam.hospitalId,
                     gv_deptInfo.GetRowCellValue(gv_deptInfoCheckRowHandle, gv_deptInfo.Columns["deptId"]).ToString(),
                    CurrentParam.reportType,
                    CurrentParam.startDate,
                    CurrentParam.endDate);
                String url = AppContext.AppConfig.serverUrl + "sch/report/doctorBespeakReprot?" + param;
                e.Result = HttpClass.httpPost(url);
                //e.Result = @"{""code"":200,""message"":""操作成功"",""result"":[{""czNum"":1,""yyNum"":3,""openNum"":84,""doctorId"":1,""fzNum"":2,""doctorName"":""张医生""},{""czNum"":""0"",""yyNum"":""0"",""openNum"":""0"",""doctorId"":10,""fzNum"":""0"",""doctorName"":""1232""},{""czNum"":""0"",""yyNum"":""0"",""openNum"":12,""doctorId"":15,""fzNum"":""0"",""doctorName"":""杰大哥""},{""czNum"":""0"",""yyNum"":""0"",""openNum"":""0"",""doctorId"":13,""fzNum"":""0"",""doctorName"":""21321""}],""state"":true}";
            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
            }
        }
        private void bw_RunWorkerGetDocListCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                String data = e.Result as String;
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    List<DocInfoEntity> list = objT["result"].ToObject<List<DocInfoEntity>>();
                    this.gc_docInfo.DataSource = list;

                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            }
            catch (Exception ex)
            {
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
            }
            finally
            {
                try
                {
                    // 关闭加载提示框
                    //DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
                    Log4net.LogHelper.Info("4");
                    cmd.HideOpaqueLayer();
                }
                catch
                { }
            }
        }
        int gv_deptNCDInfoCheckRowHandle;
        private void gv_deptNCDInfo_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            cmd.ShowOpaqueLayer();
            this.DoWorkAsync(100, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
            {
                return null;
            }, null, (data) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
            {
                GridView gv = sender as GridView;
                //gv.SetRowCellValue(0, gv.Columns["check"], "1");
                //当前行 置为 选中状态
                if (gv_deptInfoCheckRowHandle != e.RowHandle)
                {
                    //将当前点击的选中
                    gv.SetRowCellValue(e.RowHandle, gv.Columns["check"], "1");
                    //将前面选中的取消 0表示 未选中
                    gv.SetRowCellValue(gv_deptNCDInfoCheckRowHandle, gv.Columns["check"], "0");

                    gv_deptNCDInfoCheckRowHandle = e.RowHandle;

                    String param = "hospitalId={0}&deptId={1}&reportType={2}&startDate={3}&endDate={4}";
                    param = String.Format(
                        param, CurrentParam.hospitalId,
                         gv_deptInfoNCD.GetRowCellValue(gv_deptNCDInfoCheckRowHandle, gv_deptInfoNCD.Columns["deptId"]).ToString(),
                        CurrentParam.reportType,
                        CurrentParam.startDate,
                        CurrentParam.endDate);
                    String url = AppContext.AppConfig.serverUrl + "sch/report/doctorBespeakReprotForNCD?" + param;
                    this.DoWorkAsync(0, (o) => //耗时逻辑处理(此处不能操作UI控件，因为是在异步中)
                    {
                        data = HttpClass.httpPost(url);
                        return data;

                    }, null, (data2) => //显示结果（此处用于对上面结果的处理，比如显示到界面上）
                    {
                        JObject objT = JObject.Parse(data2.ToString());
                        if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                        {
                            List<DocInfoEntity> list = objT["result"].ToObject<List<DocInfoEntity>>();
                            this.gc_docInfoNCD.DataSource = list;
                            cmd.HideOpaqueLayer();
                        }
                        else
                        {
                            cmd.HideOpaqueLayer();
                            MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                        }
                    });
                }
                else
                {
                    cmd.HideOpaqueLayer();
                }
            });


            //GridView gv = sender as GridView;
            ////gv.SetRowCellValue(0, gv.Columns["check"], "1");
            ////当前行 置为 选中状态
            //if (gv_deptNCDInfoCheckRowHandle != e.RowHandle)
            //{
            //    //将当前点击的选中
            //    gv.SetRowCellValue(e.RowHandle, gv.Columns["check"], "1");
            //    //将前面选中的取消 0表示 未选中
            //    gv.SetRowCellValue(gv_deptNCDInfoCheckRowHandle, gv.Columns["check"], "0");

            //    gv_deptNCDInfoCheckRowHandle = e.RowHandle;
            //    // 弹出加载提示框
            //    //DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitingForm));
            //    cmd.ShowOpaqueLayer(225, true);
            //    // 开始异步
            //    BackgroundWorkerUtil.start_run(bw_DoWorkGetNCDDocList, bw_RunWorkerGetNCDDocListCompleted, null, false);

                
            //}fffffffffff
        }
        private void bw_DoWorkGetNCDDocList(object sender, DoWorkEventArgs e)
        {
            try
            {
                //String param = "hospitalId=12&deptId=2&reportType=day&startDate=2019-01-01&endDate=2019-01-20";
                String param = "hospitalId={0}&deptId={1}&reportType={2}&startDate={3}&endDate={4}";
                param = String.Format(
                    param, CurrentParam.hospitalId,
                     gv_deptInfoNCD.GetRowCellValue(gv_deptNCDInfoCheckRowHandle, gv_deptInfoNCD.Columns["deptId"]).ToString(),
                    CurrentParam.reportType,
                    CurrentParam.startDate,
                    CurrentParam.endDate);
                String url = AppContext.AppConfig.serverUrl + "sch/report/doctorBespeakReprotForNCD?" + param;
                e.Result = HttpClass.httpPost(url);
                //e.Result = @"{""code"":200,""message"":""操作成功"",""result"":[{""czNum"":1,""yyNum"":3,""openNum"":84,""doctorId"":1,""fzNum"":2,""doctorName"":""张医生""},{""czNum"":""0"",""yyNum"":""0"",""openNum"":""0"",""doctorId"":10,""fzNum"":""0"",""doctorName"":""1232""},{""czNum"":""0"",""yyNum"":""0"",""openNum"":12,""doctorId"":15,""fzNum"":""0"",""doctorName"":""杰大哥""},{""czNum"":""0"",""yyNum"":""0"",""openNum"":""0"",""doctorId"":13,""fzNum"":""0"",""doctorName"":""21321""}],""state"":true}";
            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
            }
        }
        private void bw_RunWorkerGetNCDDocListCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                String data = e.Result as String;
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    List<DocInfoEntity> list = objT["result"].ToObject<List<DocInfoEntity>>();
                    this.gc_docInfoNCD.DataSource = list;

                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            }
            catch (Exception ex)
            {
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
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
        #region 表格样式实现
        /// <summary>
        /// 自定义表头颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bandedGridView1_CustomDrawBandHeader(object sender, BandHeaderCustomDrawEventArgs e)
        {
            //背景颜色没有设置且为空，则默认
            if (e.Band.AppearanceHeader == null || (e.Band.AppearanceHeader.BackColor == Color.Empty && !e.Band.AppearanceHeader.Options.UseBackColor))
                return;
            Rectangle rect = e.Bounds;
            //rect.Inflate(-1, -1);

            // 填充标题颜色.
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(180, e.Band.AppearanceHeader.BackColor)), rect);
            e.Appearance.DrawString(e.Cache, e.Info.Caption, e.Info.CaptionRect);
            Pen pen = new Pen(Color.LightGray, 0.1f);
            e.Graphics.DrawRectangle(pen, rect);
            // 绘制过滤和排序按钮.
            foreach (DrawElementInfo info in e.Info.InnerElements)
            {
                if (!info.Visible) continue;
                ObjectPainter.DrawObject(e.Cache, info.ElementPainter, info.ElementInfo);
            }
            e.Handled = true;
        }


        private void OnCustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            //背景颜色没有设置且为空，则默认
            if (e.Column == null || (e.Column.AppearanceHeader.BackColor == Color.Empty && !e.Column.AppearanceHeader.Options.UseBackColor))
                return;
            Rectangle rect = e.Bounds;
            //rect.Inflate(-1, -1);

            // 填充标题颜色.
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(180, e.Column.AppearanceHeader.BackColor)), rect);
            e.Appearance.DrawString(e.Cache, e.Info.Caption, e.Info.CaptionRect);
            Pen pen = new Pen(Color.LightGray, 0.1f);
            e.Graphics.DrawRectangle(pen, rect);
            // 绘制过滤和排序按钮.
            foreach (DrawElementInfo info in e.Info.InnerElements)
            {
                if (!info.Visible) continue;
                ObjectPainter.DrawObject(e.Cache, info.ElementPainter, info.ElementInfo);
            }
            e.Handled = true;
        }


        bool firstfootcell = true;
        private void gv_deptInfo_CustomDrawFooter(object sender, DevExpress.XtraGrid.Views.Base.RowObjectCustomDrawEventArgs e)
        {
            Rectangle r = e.Bounds;
            Brush brush = e.Cache.GetGradientBrush(e.Bounds, Color.Transparent, Color.Transparent, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
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
            Pen p = new Pen(Color.LightGray);
            Brush brush = e.Cache.GetGradientBrush(e.Bounds, Color.WhiteSmoke, Color.WhiteSmoke, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
            e.Graphics.FillRectangle(brush, r);
            e.Graphics.DrawRectangle(p, r.X-1 , r.Y, r.Width, r.Height);
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
        private void gc_deptInfo_Paint(object sender, PaintEventArgs e)
        {
            firstfootcell = true;
        }
        #endregion
        
        private void rBtn_Click(object sender, EventArgs e)
        {
            if (rBtn_day.IsCheck)
            {
                QueryDateType = "day"; 
            }
            else
            {
                QueryDateType = "month";
            }
            setDateFomartDefult(rBtn_day.IsCheck);
        }
        private void deStart_EditValueChanged(object sender, EventArgs e)
        {
            //deEnd.EditValue = deStart.EditValue;
        }
        /// <summary>
        /// //配置时间格式
        /// </summary>
        /// <param name="flag">true为日期查询 false为月份查询</param>
        private  void setDateFomartDefult(bool flag)
        {
            if (flag)
            {
                this.deStart.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
                this.deStart.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                this.deStart.Properties.EditFormat.FormatString = "yyyy-MM-dd";
                this.deStart.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                this.deStart.Properties.Mask.EditMask = "yyyy-MM-dd";
                this.deStart.Properties.VistaCalendarInitialViewStyle = VistaCalendarInitialViewStyle.MonthView;
                this.deStart.Properties.VistaCalendarViewStyle = ((DevExpress.XtraEditors.VistaCalendarViewStyle)((DevExpress.XtraEditors.VistaCalendarViewStyle.MonthView | DevExpress.XtraEditors.VistaCalendarViewStyle.YearView)));
                this.deStart.EditValue = System.DateTime.Now;// new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, 1);

                this.deEnd.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
                this.deEnd.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                this.deEnd.Properties.EditFormat.FormatString = "yyyy-MM-dd";
                this.deEnd.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                this.deEnd.Properties.Mask.EditMask = "yyyy-MM-dd";
                this.deEnd.Properties.VistaCalendarInitialViewStyle = VistaCalendarInitialViewStyle.MonthView;
                this.deEnd.Properties.VistaCalendarViewStyle = ((DevExpress.XtraEditors.VistaCalendarViewStyle)((DevExpress.XtraEditors.VistaCalendarViewStyle.MonthView | DevExpress.XtraEditors.VistaCalendarViewStyle.YearView)));
                this.deEnd.EditValue = System.DateTime.Now.AddDays(7);//new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day);
            }
            else
            {
                this.deStart.Properties.DisplayFormat.FormatString = "yyyy-MM";
                this.deStart.Properties.EditFormat.FormatString = "yyyy-MM";
                this.deStart.Properties.Mask.EditMask = "yyyy-MM";
                this.deStart.Properties.VistaCalendarInitialViewStyle = VistaCalendarInitialViewStyle.YearView;
                this.deStart.Properties.VistaCalendarViewStyle = ((DevExpress.XtraEditors.VistaCalendarViewStyle)((DevExpress.XtraEditors.VistaCalendarViewStyle.YearView)));
                this.deStart.EditValue = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, 1);

                this.deEnd.Properties.DisplayFormat.FormatString = "yyyy-MM";
                this.deEnd.Properties.EditFormat.FormatString = "yyyy-MM";
                this.deEnd.Properties.Mask.EditMask = "yyyy-MM";
                this.deEnd.Properties.VistaCalendarInitialViewStyle = VistaCalendarInitialViewStyle.YearView;
                this.deEnd.Properties.VistaCalendarViewStyle = ((DevExpress.XtraEditors.VistaCalendarViewStyle)((DevExpress.XtraEditors.VistaCalendarViewStyle.YearView)));
                this.deEnd.EditValue = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day);
            }
        }
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonControl2_Click(object sender, EventArgs e)
        {
            if (VerifyInfo())
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Execl files(*.xls)|*.xls";
                saveFileDialog.FilterIndex = 0;
                saveFileDialog.RestoreDirectory = true; //保存对话框是否记忆上次打开的目录
                //saveFileDialog.CreatePrompt = true;
                saveFileDialog.Title = "导出Excel文件到";
                DateTime now = DateTime.Now;
                saveFileDialog.FileName = TabpageName + "表";// + now.Year.ToString().PadLeft(2) + now.Month.ToString().PadLeft(2, '0') + now.Day.ToString().PadLeft(2, '0') + "-" + now.Hour.ToString().PadLeft(2, '0') + now.Minute.ToString().PadLeft(2, '0') + now.Second.ToString().PadLeft(2, '0');
                //点了保存按钮进入
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (saveFileDialog.FileName.Trim() == "")
                    {
                        MessageBoxUtils.Hint("请输入要保存的文件名", HintMessageBoxIcon.Error, MainForm);
                        return;
                    }

                    Thread.Sleep(500);
                    cmd.ShowOpaqueLayer(225, true);
                    String param = @"type={0}&hospitalId={1}&deptId={2}&reportType={3}&startDate={4}&endDate={5}";

                    if (TabpageName == "汇总信息")
                    {
                        param = String.Format(
                           param, "3",
                           CurrentParam.hospitalId,
                           CurrentParam.deptId,
                           CurrentParam.reportType,
                           CurrentParam.startDate,
                           CurrentParam.endDate);
                    }
                    else if (TabpageName == "详细信息")
                    {
                        param = String.Format(
                           param, "1",
                           CurrentParam.hospitalId,
                           CurrentParam.deptId,
                           CurrentParam.reportType,
                           CurrentParam.startDate,
                           CurrentParam.endDate);
                    }
                    else
                    {
                        param = String.Format(
                           param, "2",
                           CurrentParam.hospitalId,
                           CurrentParam.deptId,
                           CurrentParam.reportType,
                           CurrentParam.startDate,
                           CurrentParam.endDate);
                    }
                    String url = AppContext.AppConfig.serverUrl + "sch/report/exportExcel?" + param;
                    String[] args = { url, saveFileDialog.FileName };
                    // 开始异步
                    //BackgroundWorkerUtil.start_run(bw_DoWorkExcel, bw_RunWorkerExcelCompleted, null, false);
                    Thread t = new Thread(DownLoadFile);//创建线程
                    t.Start(args);//用来给函数传递参数，开启线程
                    
                }
            }

        }
        //异步下载
        void DownLoadFile(object Args)
        {

            // 远程获取目标页面源码
            try
            {
                string strTargetHtml = string.Empty;
                String[] args = Args as String[];
                WebClient wc = new WebClient();
                wc.DownloadFile(args[0], args[1]);
                wc.Dispose();

            }
            catch (Exception e)
            {
                String Msg = e.Message;
                if(e.InnerException!=null)
                {
                    Msg = e.InnerException.Message;
                }
                Action action = () =>
                {
                    MessageBoxUtils.Hint(Msg, HintMessageBoxIcon.Error, this);
                };
                Invoke(action);
            }
            finally
            {
                Action action = () =>
                {
                    cmd.HideOpaqueLayer();
                };
                Invoke(action);
            }
        }
            
		
        /// <summary>
        /// 导出平均候诊时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_exportWaitTime_Click(object sender, EventArgs e)
        {
            if (VerifyInfo())
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Execl files(*.xls)|*.xls";
                saveFileDialog.FilterIndex = 0;
                saveFileDialog.RestoreDirectory = true; //保存对话框是否记忆上次打开的目录
                //saveFileDialog.CreatePrompt = true;
                saveFileDialog.Title = "导出Excel文件到";
                DateTime now = DateTime.Now;
                saveFileDialog.FileName = CurrentParam.deptName+"平均候诊时间表";// + now.Year.ToString().PadLeft(2) + now.Month.ToString().PadLeft(2, '0') + now.Day.ToString().PadLeft(2, '0') + "-" + now.Hour.ToString().PadLeft(2, '0') + now.Minute.ToString().PadLeft(2, '0') + now.Second.ToString().PadLeft(2, '0');
                //点了保存按钮进入
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (saveFileDialog.FileName.Trim() == "")
                    {
                        MessageBoxUtils.Hint("请输入要保存的文件名", HintMessageBoxIcon.Error, MainForm);
                        return;
                    }
                    
                    Thread.Sleep(500);
                    cmd.ShowOpaqueLayer(225, true);
                    String param = @"hospitalId={0}&deptId={1}&reportType={2}&startDate={3}&endDate={4}";


                    param = String.Format(
                       param, CurrentParam.hospitalId,
                       CurrentParam.deptId,
                       CurrentParam.reportType,
                       CurrentParam.startDate,
                       CurrentParam.endDate);

                    String url = AppContext.AppConfig.serverUrl + "sch/report/exportWaitTime?" + param;
                    String[] args = { url, saveFileDialog.FileName };
                    // 开始异步
                    //BackgroundWorkerUtil.start_run(bw_DoWorkExcel, bw_RunWorkerExcelCompleted, null, false);
                    Thread t = new Thread(DownLoadFile);//创建线程
                    t.Start(args);//用来给函数传递参数，开启线程
                }
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

        private void AppointmentStatisticsForm_Resize(object sender, EventArgs e)
        {
            float width = (panel4.Width - 20) / 2;
            
            if (width > 650)
            {
                gv_docInfo.OptionsView.ColumnAutoWidth = true;
                gv_docInfoNCD.OptionsView.ColumnAutoWidth = true;
            }
            else
            {
                gv_docInfo.OptionsView.ColumnAutoWidth = false;
                gv_docInfo.BestFitColumns();
                gv_docInfoNCD.OptionsView.ColumnAutoWidth = false;
                gv_docInfoNCD.BestFitColumns();
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



    }
    #region 实体类

    /// <summary>
    /// 查询参数实体
    /// </summary>
    public class AppointmentStatisticsQueryParam
    { 
           /*String param = "hospitalId={0}&deptId={1}&reportType={2}&startDate={3}&endDate={4}";
                param = String.Format(
                    param, AppContext.Session.hospitalId, 
                    lueDept.EditValue.ToString(),
                    QueryDateType,
                    deStart.Text,
                    deEnd.Text);
            */
        /// <summary>
        /// 医院ID
        /// </summary>
        public String hospitalId { get; set; }
        /// <summary>
        /// 科室ID
        /// </summary>
        public String deptId { get; set; }
        /// <summary>
        /// 科室名称
        /// </summary>
        public String deptName { get; set; }
        /// <summary>
        /// 日期类型
        /// </summary>
        public String reportType { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public String startDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public String endDate { get; set; }
    }
    /// <summary>
    /// 预约途径实体类
    /// </summary>
    public class RegisterWayEntity
    {
        /// <summary>
        /// 预约途径
        /// </summary>
        public String registerWay { get; set; }

        /// <summary>
        /// 人数
        /// </summary>
        public String bespeakCount { get; set; }
    }

    /// <summary>
    /// 统计折线图信息实体类
    /// </summary>
    public class ChartInfoEntity
    {
        /// <summary>
        /// 时间X轴
        /// </summary>
        public String registerTime { get; set; }

        /// <summary>
        /// 人数Y轴
        /// </summary>
        public Int32 bespeakCount { get; set; }
    }

    /// <summary>
    /// 统计表格信息实体类
    /// </summary>
    public class StatisticsInfoEntity
    {
        /// <summary>
        /// 科室名称
        /// </summary>
        public String deptName { get; set; }
        /// <summary>
        /// 开放名额
        /// </summary>
        public String openNum { get; set; }
        /// <summary>
        /// 预约人数
        /// </summary>
        public String bespeakNum { get; set; }
        /// <summary>
        /// 预约率
        /// </summary>
        public String bespeakRate { get; set; }
        /// <summary>
        /// 就诊人数
        /// </summary>
        public String visitNum { get; set; }
        /// <summary>
        /// 就诊率
        /// </summary>
        public String visitRate { get; set; }
        /// <summary>
        /// 爽约人数
        /// </summary>
        public String breakNum { get; set; }
        /// <summary>
        /// 爽约率
        /// </summary>
        public String breakRate { get; set; }
    }
    /// <summary>
    /// 统计科室信息实体类
    /// </summary>
    public class DeptInfoEntity
    {
        private String _check = "0";

        public String check
        {
            get { return _check; }
            set { _check = value; }
        }
        /// <summary>
        /// 科室名称
        /// </summary>
        public String deptName { get; set; }
        /// <summary>
        /// 开放总额
        /// </summary>
        public Int32 openNum { get; set; }
        /// <summary>
        /// 预约人数
        /// </summary>
        public Int32 yyNum { get; set; }
        /// <summary>
        /// 预约初诊
        /// </summary>
        public Int32 yyCzNum { get; set; }
        /// <summary>
        /// 预约复诊
        /// </summary>
        public Int32 yyFzNum { get; set; }
        /// <summary>
        /// 现场人数
        /// </summary>
        public Int32 xcNum { get; set; }
        /// <summary>
        /// 现场初诊
        /// </summary>
        public Int32 xcCzNum { get; set; }
        /// <summary>
        /// 现场复诊
        /// </summary>
        public Int32 xcFzNum { get; set; }
        /// <summary>
        /// 科室ID
        /// </summary>
        public Int32 deptId { get; set; }
    }
    /// <summary>
    /// 统计医生信息实体类
    /// </summary>
    public class DocInfoEntity
    {
        /// <summary>
        /// 医生姓名
        /// </summary>
        public String doctorName { get; set; }
        /// <summary>
        /// 开放总额
        /// </summary>
        public Int32 openNum { get; set; }
        /// <summary>
        /// 现场总额
        /// </summary>
        public Int32 siteNum { get; set; }
        /// <summary>
        /// 预约人数
        /// </summary>
        public Int32 yyNum { get; set; }
        /// <summary>
        ///预约复诊人数
        /// </summary>
        public Int32 yyFzNum { get; set; }
        /// <summary>
        /// 预约初诊人数
        /// </summary>
        public Int32 yyCzNum { get; set; }
        /// <summary>
        /// 预约率
        /// </summary>
        public String yyRate { get; set; }
        /// <summary>
        ///现场人数
        /// </summary>
        public Int32 xcNum { get; set; }
        /// <summary>
        /// 现场初诊人数
        /// </summary>
        public Int32 xcCzNum { get; set; }
        /// <summary>
        /// 现场复诊人数
        /// </summary>
        public Int32 xcFzNum { get; set; }
        /// <summary>
        /// 现场率
        /// </summary>
        public String xcRate { get; set; }
        /// <summary>
        /// 医生ID
        /// </summary>
        public Int32 doctorId { get; set; }
    }
#endregion
}
