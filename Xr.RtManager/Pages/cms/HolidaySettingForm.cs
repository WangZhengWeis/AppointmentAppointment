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
using Xr.Common;

namespace Xr.RtManager.Pages.cms
{
    public partial class HolidaySettingForm : UserControl
    {
        private Form MainForm; //主窗体
        Xr.Common.Controls.OpaqueCommand cmd;
        public HolidaySettingForm()
        {
            InitializeComponent();
            MainForm = (Form)this.Parent;
            pageControl1.MainForm = MainForm;
            pageControl1.PageSize = Convert.ToInt32(AppContext.AppConfig.pagesize);
            cmd = new Xr.Common.Controls.OpaqueCommand(AppContext.Session.waitControl);
            cmd.ShowOpaqueLayer(225, false);
            HolidaySettingList(1,pageControl1.PageSize);
            #region 
            //string StartYear = (DateTime.Now.Year-2).ToString();//获取当前年份
            //string SeparatedYear = (DateTime.Now.Year -(Convert.ToInt32(StartYear)-5)).ToString();
            //cmbYear.DataSource = Enumerable.Range(Convert.ToInt32(StartYear), Convert.ToInt32(SeparatedYear)).ToList();
            //cmbYear.SelectedIndex = cmbYear.Items.IndexOf(DateTime.Now.Year);
            #endregion 
        }
        public List<HolidayInfoEntity> Data = new List<HolidayInfoEntity>();
        #region 节假日列表
        /// <summary>
        /// 节假日列表
        /// </summary>
        /// <param name="pageNo">页码</param>
        /// <param name="pageSize">页数</param>
        public void HolidaySettingList(int pageNo, int pageSize)
        {
            try
            {
                String url = AppContext.AppConfig.serverUrl + "cms/holiday/list?pageNo=" + pageNo + "&pageSize=" + pageSize +"&hospital.id=" + AppContext.Session.hospitalId;
                String data = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    this.gc_Holiday.DataSource = objT["result"]["list"].ToObject<List<HolidayInfoEntity>>();
                    pageControl1.setData(int.Parse(objT["result"]["count"].ToString()),
                    int.Parse(objT["result"]["pageSize"].ToString()),
                    int.Parse(objT["result"]["pageNo"].ToString()));
                    cmd.HideOpaqueLayer();
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    cmd.HideOpaqueLayer();
                }
            }
            catch (Exception ex)
            {
                cmd.HideOpaqueLayer();
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                Log4net.LogHelper.Error("获取节假日列表错误信息：" + ex.Message);
            }
        }
        #endregion
        #region 查询节假日详情
        public void CheckHolidayDetails(string id)
        {
            try
            {
                String url = AppContext.AppConfig.serverUrl + "api/cms/holiday/findById?id=" + id;
                String data = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            }
            catch (Exception ex)
            {
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                Log4net.LogHelper.Error("查询节假日详情错误信息：" + ex.Message);
            }
        }
        #endregion 
        #region 查询指定医院下节假日列表
       /// <summary>
        /// 查询指定医院下节假日列表
       /// </summary>
       /// <param name="id">医院ID</param>
        public void QueryDesignatedHospital(string id)
        {
            try
            {
                String url = AppContext.AppConfig.serverUrl + "api/cms/holiday/findAll?hospital.id=" +id;
                String data = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            }
            catch (Exception ex)
            {
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                Log4net.LogHelper.Error("查询指定医院下节假日列表错误信息：" + ex.Message);
            }
        }
        #endregion
        #region 查询时间段内属于节假日的日期
        /// <summary>
        /// 查询时间段内属于节假日的日期
        /// </summary>
        /// <param name="hospitalId">医院ID</param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        public void TheDateHolidayPeriod(string hospitalId, string beginDate, string endDate)
        {
            try
            {
               String url = AppContext.AppConfig.serverUrl + "api/cms/holiday/checkHolidayForDate?hospitalId=" + hospitalId + "&beginDate=" + beginDate + "&endDate=" + endDate;
                String data = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            }
            catch (Exception ex)
            {
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                Log4net.LogHelper.Error("查询时间段内属于节假日的日期错误信息：" + ex.Message);
            }
        }
        #endregion 
        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                groupBox1.Enabled = true;
                dcHodily.ClearValue();
                holidayInfoEntity = new HolidayInfoEntity();
                holidayInfoEntity.isUse = "0";
                dcHodily.SetValue(holidayInfoEntity);
               // radioGroup2.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
               //radioGroup2.SelectedIndex = 0;
                Log4net.LogHelper.Error("节假日新增错误信息：" + ex.Message);
            }
        }
        #endregion
        #region 修改
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedRow = gv_Holiday.GetFocusedRow() as HolidayInfoEntity;
                if (selectedRow == null)
                    return;
                dcHodily.SetValue(selectedRow);
                groupBox1.Enabled = true ;
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("节假日修改错误信息：" + ex.Message);
            }
        }
        #endregion
        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedRow = gv_Holiday.GetFocusedRow() as HolidayInfoEntity;
                if (selectedRow == null)
                    return;
                //MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBoxUtils.Show("确定要删除吗?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MainForm);
                if (dr == DialogResult.OK)
                {
                    String param = "?id=" + selectedRow.id;
                    String url = AppContext.AppConfig.serverUrl + "cms/holiday/delete" + param;
                    String data = HttpClass.httpPost(url);
                    JObject objT = JObject.Parse(data);
                    if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                    {
                        Xr.Common.MessageBoxUtils.Hint("删除成功", MainForm);
                        HolidaySettingList(1, pageControl1.PageSize);
                        this.dcHodily.ClearValue();
                    }
                    else
                    {
                        MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                Log4net.LogHelper.Error("节假日修改错误信息：" + ex.Message);
            }
        }
        #endregion
        #region 控制编辑
        /// <summary>
        /// 控制编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gv_Holiday_ShowingEditor(object sender, CancelEventArgs e)
        {
            int selectRow = gv_Holiday.GetSelectedRows()[0];
            string name = this.gv_Holiday.GetRowCellValue(selectRow, "name").ToString();
            if (name != "")
            {
                //if (obj["code"].ToString() != "")
                //{
                e.Cancel = true;
                //  }
            }
        }
        #endregion
        #region 保存按钮
        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butContronl_Click(object sender, EventArgs e)
        {
            try
            {
                if (!dcHodily.Validate())
                {
                    return;
                }
                dcHodily.GetValue(holidayInfoEntity);
                String param = "?" + PackReflectionEntity<HolidayInfoEntity>.GetEntityToRequestParameters(holidayInfoEntity, true);
                String url = AppContext.AppConfig.serverUrl + "cms/holiday/save" + param;
                String data = HttpClass.httpPost(url);
                JObject objT = JObject.Parse(data);
                if (string.Compare(objT["state"].ToString(), "true", true) == 0)
                {
                    MessageBoxUtils.Hint("保存成功!", MainForm);
                    dcHodily.ClearValue();
                    groupBox1.Enabled = false ;
                    HolidaySettingList(1, pageControl1.PageSize);
                }
                else
                {
                    MessageBoxUtils.Show(objT["message"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                }
            }
            catch (Exception ex)
            {
                MessageBoxUtils.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MainForm);
                Log4net.LogHelper.Error("节假日保存错误信息：" + ex.Message);
            }
        }
        #endregion 
        #region 窗体Load事件
        public HolidayInfoEntity holidayInfoEntity { get; set; }
        private void HolidaySettingForm_Load(object sender, EventArgs e)
        {
            dcHodily.DataType = typeof(HolidayInfoEntity);
            if (holidayInfoEntity != null)
            {
                dcHodily.SetValue(holidayInfoEntity);
            }
            else
            {
                holidayInfoEntity = new HolidayInfoEntity();
            }
        }
        #endregion
        #region 加粗线条颜色
        /// <summary>
        /// 加粗线条颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void groupBox1_Paint(object sender, PaintEventArgs e)
        {
            //GroupBox gBox = (GroupBox)sender;
            ////e.Graphics.Clear(gBox.BackColor);
            ////e.Graphics.DrawString(gBox.Text, gBox.Font, Brushes.Gray, 10, 1);
            //var vSize = e.Graphics.MeasureString(gBox.Text, gBox.Font);
            //e.Graphics.DrawLine(Pens.Gray, 1, vSize.Height / 2, 8, vSize.Height / 2);
            //e.Graphics.DrawLine(Pens.Gray, vSize.Width + 8, vSize.Height / 2, gBox.Width - 2, vSize.Height / 2);
            //e.Graphics.DrawLine(Pens.Gray, 1, vSize.Height / 2, 1, gBox.Height - 2);
            //e.Graphics.DrawLine(Pens.Gray, 1, gBox.Height - 2, gBox.Width - 2, gBox.Height - 2);
            //e.Graphics.DrawLine(Pens.Gray, gBox.Width - 2, vSize.Height / 2, gBox.Width - 2, gBox.Height - 2);
        }
        #endregion 
        #region 分页跳转事件
        private void pageControl1_Query(int CurrentPage, int PageSize)
        {
            cmd.ShowOpaqueLayer(225, false);
            HolidaySettingList(CurrentPage, PageSize);
        }
        #endregion 

        private void gv_Holiday_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                var selectedRow = gv_Holiday.GetFocusedRow() as HolidayInfoEntity;
                if (selectedRow == null)
                    return;
                dcHodily.SetValue(selectedRow);
                groupBox1.Enabled = true;
            }
            catch (Exception ex)
            {
                Log4net.LogHelper.Error("节假日修改错误信息：" + ex.Message);
            }
        }
    }
}
